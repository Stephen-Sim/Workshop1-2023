using System;
using System.Collections.Generic;
using System.Text;

namespace Session5_Mobile.Models
{
    public class AddonServiceDetail
    {
        public long? ID { get; set; }
        public System.Guid GUID { get; set; }
        public long AddonServiceID { get; set; }
        public long ServiceID { get; set; }
        public decimal Price { get; set; }
        public System.DateTime FromDate { get; set; }
        public string Notes { get; set; }
        public long NumberOfPeople { get; set; }
        public bool isRefund { get; set; }
        public string FromDateString { get; set; } = string.Empty;
        public string ServiceName { get; set; } = string.Empty;
        public string IconName { get; set; } = string.Empty;
    }
}
