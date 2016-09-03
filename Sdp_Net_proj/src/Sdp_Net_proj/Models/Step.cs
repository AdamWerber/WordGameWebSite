using System.Collections.Generic;

namespace Sdp_Net_proj.Models
{
    public class Step
    {
        public string ID { get; set; }
        public int Step_Number { get; set; }
        public string Word { get; set; }
        public int Word_Score { get; set; }
        public string Letters_Indexs { get; set; } // it will be in a structure of a number and then ',' - we will parse it as needed.
        public int Player1_Score { get; set; }
        public int Player2_Score { get; set; }
    }
}