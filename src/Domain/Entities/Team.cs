using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Domain.Entities
{
    public class Team
    {
        public Guid TeamId { get; set; }
        public string TeamName { get; set; } = null!;

        public Guid UserId { get; set; }

        public int BudgetRemaining { get; set; }
        public int TotalPoints { get; set; }
        public DateTime CreatedAt { get; set; }

        public User? User { get; set; }
        public ICollection<LeagueEntry>? LeagueEntries { get; set; }

    }
}
