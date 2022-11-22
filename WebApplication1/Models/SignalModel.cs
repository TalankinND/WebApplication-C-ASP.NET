using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class SignalModel
    {
        [Key]
        public int RecordID { get; set; }

        public int DeviceID { get; set; }

        public int HS { get; set; }

        public string Сabinet { get; set; }

        public string SignalType { get; set; }

        public string SensorName { get; set; }

        public string Section { get; set; }

        public string CodeCombination { get; set; }

        public string BI_Number_Lamp { get; set; }

        public string Signature_BI { get; set; }

        public string Comment_BI { get; set; }

        public string Signature_Security { get; set; }

        public string Bonus { get; set; }
    }
}
