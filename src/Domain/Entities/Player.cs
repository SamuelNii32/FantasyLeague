using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{

    public enum PlayerPosition : short {GK = 0, DEF = 1, MID = 2, FWD = 3 }

    public enum PlayerStatus : short {Available = 0, Doubtful = 1, Injured = 2, Suspended = 3}
    public class Player
    {
        // PK (we'll configure ValueGeneratedOnAdd / DB default in Step 2)
        public Guid Id { get; set; }

        // Required FK → Clubs.Id
        public Guid ClubId { get; set; }

        // Required (max length configured in Step 2)
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;

        // Required
        public PlayerPosition Position { get; set; }

        // Required: price in tenths (e.g., 45 = £4.5m)
        public int Cost { get; set; }

        // Optional-to-set; defaults to Available via enum default
        public PlayerStatus Status { get; set; } = PlayerStatus.Available;

        // Set by DB default (UTC) in Step 2 mapping
        public DateTime CreatedAt { get; set; }

        public Club? Club { get; set; }



    }
}

