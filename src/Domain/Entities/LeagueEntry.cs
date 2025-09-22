using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class LeagueEntry
    {
        public Guid LeagueEntryId { get; set; }   

        public Guid LeagueId { get; set; }        
        public Guid TeamId { get; set; }         

        public DateTime JoinedAt { get; set; }    

        
        public League? League { get; set; }
        public Team? Team { get; set; }
    }
}
