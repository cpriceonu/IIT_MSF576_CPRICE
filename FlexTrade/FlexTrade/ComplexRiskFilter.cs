using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexTrade
{
    //A simple risk filter that ensures that no more then a predefined maximum number of positions
    //is held in an one product
    class ComplexRiskFilter : RiskFilter, BrokerListener
    {
        private PositionManager positionManager { get; set; }
        private int maxPositionSize { get; set; }

        public ComplexRiskFilter(PositionManager p) 
        {         
            positionManager = p;
            maxPositionSize = 1000;
        }

        public bool isAcceptable(Order o)
        {
            int size = positionManager.getPositionSizeByProduct(o.product);

            if (o.side == Order.Side.BUY)
                size += o.orderQuantity;
            else
                size -= o.orderQuantity;

            if (Math.Abs(size) > Math.Abs(maxPositionSize))
            {
                List<String> messages = new List<String>();
                messages.Add("Too many positions in " + o.product.symbol + ". Would have " + size + " open positions if executed.");
                throw new UnacceptableRiskException(this, messages);
            }
            else
                return true;
        }
    }
}
