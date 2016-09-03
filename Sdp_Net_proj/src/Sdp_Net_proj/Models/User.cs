using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sdp_Net_proj.Models
{
    public class User
    {
        public string ID { get; set; } // Primary key
        public string User_Name { get; set; } // Unique filed 
        public string Email { get; set; }
        public string Passowrd { get; set; }
        public int User_score { get; set; }

        public ICollection<Game> Games { get; set; }
    }
}
