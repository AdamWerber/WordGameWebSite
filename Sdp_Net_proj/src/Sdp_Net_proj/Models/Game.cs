using System.Collections.Generic;

namespace Sdp_Net_proj.Models
{
    public class Game
    {
        public string ID { get; set; } // Primary key
        public string player1 { get; set; }
        public int player1_score { get; set; }

        public string player2 { get; set; }
        public int player2_score { get; set; }

        public ICollection<Step> Game_Steps { get; set; }
    }
}