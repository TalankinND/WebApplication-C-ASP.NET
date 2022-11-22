using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class DeviceListModel
    {
        [Key]
        public int DeviceID { get; set; }

        public string DeviceName { get; set; }

        public string DeviceShortName { get; set; }

        public string DeviceType { get; set; }
    }
}
