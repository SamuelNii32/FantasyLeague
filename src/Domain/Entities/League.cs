using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities


{
    public enum LeagueType : byte
    {
        Classic = 0,
        HeadToHead = 1
    }

    public class League
    {
        public Guid LeagueId { get; set; }          

        public string LeagueName { get; set; } = null!;  

        
        public Guid CreatedBy { get; set; }

        // Only required/meaningful for Head-to-Head leagues; null for Classic.
        public int? MaxTeams { get; set; }

        public int StartGameweekId { get; set; }     
        public string JoinCode { get; set; } = null!;

        public LeagueType Type { get; set; } = LeagueType.Classic;

        public DateTime CreatedAt { get; set; }  

       
        public User? CreatedByUser { get; set; }            
        public ICollection<LeagueEntry>? Entries { get; set; }  
    }
}
