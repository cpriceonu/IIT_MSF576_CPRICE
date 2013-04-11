using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexTrade
{
    public class PositionGridData : IEquatable<PositionGridData>
    {
        public String symbol { get; set; }
        public int position { get; set; }
        public Double value { get; set; }
        public Double last { get; set; }

        public bool Equals(PositionGridData other)
        {
            if (symbol.Equals(other.symbol))
                return true;
            else
                return false;
        }
    }
}
