﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cats.Models
{
    public partial class DistibtionStatus
    {
        public string FDPID { get; set; }
        public string PlanName { get; set; }
        public string WoredaName { get; set; }
        public bool Status { get; set; }
        public int RegionID { get; set; }
        public string RegionName { get; set; }
        public int WoredaID { get; set; }

    }
}
