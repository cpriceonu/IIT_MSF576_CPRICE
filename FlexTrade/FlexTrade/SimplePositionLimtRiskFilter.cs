using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexTrade
{
    //A simple risk filter that ensures that no more then a predefined maximum number of positions
    //is held in an one product
    class SimplePositionLimtRiskFilter : RiskFilter 
    {
        private PositionManager positionManager { get; set; }
        private int maxPositionSize { get; set; }

        public SimplePositionLimtRiskFilter(PositionManager p) 
        {         
            positionManager = p;
            maxPositionSize = 1000;
        }

        public bool isAcceptable(Order o)
        {
            int size = positionManager.getPositionSizeByProduct(o.product);

            if (size > maxPositionSize)
                return false;
            else
                return true;
        }
    }
}
