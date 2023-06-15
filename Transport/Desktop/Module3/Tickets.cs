using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module3
{
    public class Tickets
    {
        public int? Id;
        public string From;
        public string To;
        public string Date;
        public string Time;
        public string FlightNumber = string.Empty;
        public int CabinTypeId;
        public string CabinType;
        public decimal CabinPrice = 0.0m;
        public int NumberOfStops = 0;

        public List<int> schedulesId = new List<int>();
    }
}
