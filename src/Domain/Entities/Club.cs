using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Club
    {
        public Guid Id { get; set; }                 
        public string Name { get; set; } = null!;    
        public string ShortName { get; set; } = null!; 
        public DateTime CreatedAt { get; set; }

    }
}
