using System;
using System.Collections.Generic;
using System.Text;

namespace Session5_Mobile.Models
{
    public class User
    {
        public long ID { get; set; }
        public string FullName { get; set; }
        public int FamilyCount { get; set; }
        public int AddOnServiceCount { get; set; }
        public long AddOnServiceId { get; set; }
    }
}
