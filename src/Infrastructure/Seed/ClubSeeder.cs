// Infrastructure/Seed/ClubSeeder.cs

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.Entities;          
// Adjust these namespaces/types to your project:
using Infrastructure.Data; // AppDbContext
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Seed
{
    public static class ClubSeeder
    {
        // STEP 1: Tiny internal DTO representing one JSON row (Name + ShortName)
        private sealed record ClubSeedDto(string Name, string ShortName);

        // Validation constants (keep aligned with your EF config)
        private const int ShortNameMin = 2;
        private const int ShortNameMax = 12;
        private const int NameMax = 100;

        /// <summary>
        /// Seeds Clubs from a JSON file.
        /// STEP 1: Define shell (class + method + parameters + TODO).
        /// STEP 2: Resolve JSON path with config + fallback and basic existence checks.
        /// STEP 3: Read + parse + validate JSON, compute change plan (no DB writes yet).
        /// STEP 4: Apply plan in a single transaction (upsert).
        /// </summary>
        public static async Task SeedAsync(
            AppDbContext db,
            ILogger logger,
            IConfiguration config,
            IHostEnvironment env)
        {
            // STEP 1: TODO map for future steps
            // read JSON → validate → transaction → upsert → log

            // STEP 2: Resolve the JSON file path (config first, then sensible default)
            var configuredPath = config["Seed:ClubsPath"];
            var defaultPath = Path.Combine(env.ContentRootPath, "Seed", "clubs.full.json");

            string path = string.IsNullOrWhiteSpace(configuredPath)
                ? defaultPath
                : Path.GetFullPath(configuredPath);

            logger.LogInformation("Club seeder: using file: {Path} (configured: {Configured})",
                                  path,
                                  configuredPath ?? "<null>");

            if (!File.Exists(path))
            {
                logger.LogWarning("Club seeding skipped: file not found at {Path}", path);
                return;
            }

            // =========================
            // STEP 3: Read + parse JSON
            // =========================

            string json;
            try
            {
                json = await File.ReadAllTextAsync(path);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Club seeding aborted: failed to read seed file at {Path}", path);
                return;
            }

            List<ClubSeedDto>? rawInput;
            try
            {
                rawInput = JsonSerializer.Deserialize<List<ClubSeedDto>>(
                    json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Club seeding aborted: invalid JSON in {Path}", path);
                return;
            }

            if (rawInput is null || rawInput.Count == 0)
            {
                logger.LogWarning("Club seeding skipped: seed file is empty: {Path}", path);
                return;
            }

            // Normalize inputs (trim; upper-case ShortName)
            var input = rawInput.Select(c => new ClubSeedDto(
                NormalizeName(c.Name),
                NormalizeShort(c.ShortName)
            )).ToList();

            // Per-item validation (blank, length)
            var invalids = new List<string>();

            foreach (var c in input)
            {
                if (string.IsNullOrWhiteSpace(c.Name))
                    invalids.Add($"ShortName '{c.ShortName}': Name is required");

                if (string.IsNullOrWhiteSpace(c.ShortName))
                    invalids.Add("ShortName is required");

                if (c.ShortName.Length < ShortNameMin || c.ShortName.Length > ShortNameMax)
                    invalids.Add($"ShortName '{c.ShortName}': length must be {ShortNameMin}–{ShortNameMax}");

                if (c.Name.Length > NameMax)
                    invalids.Add($"ShortName '{c.ShortName}': Name too long (>{NameMax})");
            }

            if (invalids.Count > 0)
            {
                logger.LogError("Club seeding aborted: {Count} validation error(s):{NewLine}- {Errors}",
                    invalids.Count,
                    Environment.NewLine,
                    string.Join($"{Environment.NewLine}- ", invalids));
                return;
            }

            // Duplicate checks inside the file
            var dupShorts = input.GroupBy(c => c.ShortName)
                                 .Where(g => g.Count() > 1)
                                 .Select(g => g.Key)
                                 .ToList();
            if (dupShorts.Count > 0)
            {
                logger.LogError("Club seeding aborted: duplicate ShortName(s) in file: {Dups}",
                    string.Join(", ", dupShorts));
                return;
            }

            var dupNames = input.GroupBy(c => c.Name)
                                .Where(g => g.Count() > 1)
                                .Select(g => g.Key)
                                .ToList();
            if (dupNames.Count > 0)
            {
                logger.LogError("Club seeding aborted: duplicate Name(s) in file: {Dups}",
                    string.Join(", ", dupNames));
                return;
            }

            // Load existing clubs from DB (read-only) to compute change plan
            List<(string ShortName, string Name)> existing;
            try
            {
                existing = await db.Clubs
                    .AsNoTracking()
                    .Select(x => new ValueTuple<string, string>(x.ShortName, x.Name))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Club seeding aborted: failed to query existing Clubs from DB");
                return;
            }

            var existingByShort = existing
                .GroupBy(e => NormalizeShort(e.ShortName))
                .ToDictionary(g => g.Key, g => g.First()); // Unique by constraint

            // Compute change plan: Inserts / Updates / No-ops
            var toInsert = new List<ClubSeedDto>();
            var toUpdate = new List<(string ShortName, string OldName, string NewName)>();
            var noOps = 0;

            foreach (var c in input)
            {
                if (!existingByShort.TryGetValue(c.ShortName, out var existingRow))
                {
                    toInsert.Add(c);
                }
                else if (!string.Equals(existingRow.Name, c.Name, StringComparison.Ordinal))
                {
                    toUpdate.Add((c.ShortName, existingRow.Name, c.Name));
                }
                else
                {
                    noOps++;
                }
            }

            logger.LogInformation("Club seed plan from {Path}: Total {Total}. Insert {Insert}, Update {Update}, No-Op {NoOp}.",
                path, input.Count, toInsert.Count, toUpdate.Count, noOps);

            if (toInsert.Count > 0)
            {
                foreach (var c in toInsert)
                    logger.LogInformation("  + Insert: {Short} — {Name}", c.ShortName, c.Name);
            }
            if (toUpdate.Count > 0)
            {
                foreach (var u in toUpdate)
                    logger.LogInformation("  ~ Update: {Short} — \"{Old}\" → \"{New}\"", u.ShortName, u.OldName, u.NewName);
            }

            // =========================
            // STEP 4: Apply the changes
            // =========================

            // Optional dry-run toggle (set Seed:DryRun=true to preview in CI)
            var dryRun = config.GetValue<bool?>("Seed:DryRun") ?? false;
            if (dryRun)
            {
                logger.LogInformation("DryRun enabled (Seed:DryRun=true). No database writes performed.");
                return;
            }

            if (toInsert.Count == 0 && toUpdate.Count == 0)
            {
                logger.LogInformation("Club seeding: nothing to do (idempotent).");
                return;
            }

            using var tx = await db.Database.BeginTransactionAsync();
            try
            {
                // Inserts
                if (toInsert.Count > 0)
                {
                    foreach (var c in toInsert)
                    {
                        // NOTE: Adjust the entity type/namespace if needed
                        var entity = new Club
                        {
                            Name = c.Name,
                            ShortName = c.ShortName
                            // CreatedAt is DB default (GETUTCDATE())
                        };
                        db.Clubs.Add(entity);
                    }
                }

                // Updates
                if (toUpdate.Count > 0)
                {
                    var updateKeys = new HashSet<string>(toUpdate.Select(u => u.ShortName), StringComparer.Ordinal);
                    var byNewName = toUpdate.ToDictionary(u => u.ShortName, u => u.NewName, StringComparer.Ordinal);

                    var entitiesToUpdate = await db.Clubs
                        .Where(x => updateKeys.Contains(x.ShortName))
                        .ToListAsync();

                    foreach (var e in entitiesToUpdate)
                    {
                        // Only update the Name; keep ShortName/Id/timestamps intact
                        e.Name = byNewName[e.ShortName];
                        // EF will track the change and update on SaveChanges
                    }
                }

                var affected = await db.SaveChangesAsync();
                await tx.CommitAsync();

                logger.LogInformation("Club seeding applied: Inserted {Insert}, Updated {Update}, Skipped {NoOp}. Rows affected: {Affected}.",
                    toInsert.Count, toUpdate.Count, noOps, affected);
            }
            catch (DbUpdateException ex)
            {
                await tx.RollbackAsync();
                logger.LogError(ex, "Club seeding failed due to a database update error (likely a uniqueness conflict on Name or ShortName).");
            }
            catch (Exception ex)
            {
                await tx.RollbackAsync();
                logger.LogError(ex, "Club seeding failed unexpectedly. Rolled back transaction.");
            }
        }

        private static string NormalizeShort(string? s) =>
            (s ?? string.Empty).Trim().ToUpperInvariant();

        private static string NormalizeName(string? s) =>
            (s ?? string.Empty).Trim();
    }
}
