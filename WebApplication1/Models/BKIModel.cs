using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class BKIModel
    {
        [Key]
        public int RecordId { get; set; }

        public int DeviceID { get; set; }

        public int Lamp { get; set; }

        public string Object { get; set; }

        public string OSType { get; set; }

        public string CommonCondition { get; set; }

        public string PKNumber { get; set; }

        public string SHNumber { get; set; }

        public string Deposit_Withdraw_code { get; set; }

        public string BKI_Commentary { get; set; }

        public string First_Inscription { get; set; }

        public string Second_Incription { get; set; }

        public string Section_Number { get; set; }
    }
}
