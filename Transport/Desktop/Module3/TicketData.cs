using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module3
{
    public class TicketData
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public DateTime BirthDate { get; set; }
        public int CabinTypeID { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Phone { get; set; }
        public string PassportNumber { get; set; }
        public int PassportCountryID { get; set; }
        public string PassportCountry { get; set; }

        public int CabinTypeId;
        public decimal CabinPrice = 0.0m;
        public List<int> schedulesId = new List<int>();
    }
}
