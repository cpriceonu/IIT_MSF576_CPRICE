﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexTrade
{
       //This is the product class
    abstract class Product
    {
        public String name { get; set; }
        public String symbol { get; set; }
        public Exchange.Exchanges exchange { get; set; }
        public Double bid { get; set; }
        public Double ask { get; set; }
        public Int32 askQty { get; set; }
        public Int32 bidQty { get; set; }
        public Double last { get; set; }
        public Int32 lastQty { get; set; }
        public DateTime asOf { get; set; }

        public Product(String n, String sym)
        {
            bid = 0;
            ask = 0;
            askQty = 0;
            bidQty = 0;
            last = 0;
            lastQty = 0;
            asOf = DateTime.UtcNow;
            symbol = sym;
            name = n;
        }
    }
}
