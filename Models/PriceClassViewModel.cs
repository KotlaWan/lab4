using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace laba2.Models
{
    public class PriceClassViewModel
    {
        public List<PriceClass> PriceClass { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public string Type { get; set; }
    }
}
