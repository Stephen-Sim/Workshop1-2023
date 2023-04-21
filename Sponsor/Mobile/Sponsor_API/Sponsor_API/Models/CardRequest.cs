using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sponsor_API.Models
{
    public class CardRequest
    {
        public string CardNo { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public string CVV { get; set; }
    }
}