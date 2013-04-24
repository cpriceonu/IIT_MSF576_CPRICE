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
        private Order orderQuantity { get; set; }
        private double BidPrice { get; set; }
        private double AskPrice { get; set; }

        public MyConstructor(List<BrokerManager> brokers)
        	//register to receive events from the broker
        	foreach(BrokerManager brk in brokers)
        	{
            		brk.BidUpdate += new BidUpdateEventHandler(bidUpdate);
            		brk.AskUpdate += new AskUpdateEventHandler(askUpdate);
                   brk.OrderConfirmed += new OrderConfirmedEventHandler(orderConfirm); 
        	} 

        public ComplexRiskFilter(PositionManager p) 
        {         
            positionManager = p;
            maxPositionSize = 1000;
        }

        public bool isAcceptable(Order o)
        {
            int size = positionManager.getPositionSizeByProduct(o.product);
            int orderSize = orderQuantity.Order(o.product);

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

            if (BidPrice > AskPrice)
               return false;
            else
               return true;

            if (orderSize + size > maxPositionSize)
               return false
            else
               return true;
        }

        public void bidUpdate(Product p)
        {
            BidPrice = p.bidUpdate;
        } 

        public void askUpdate(Product p)
        {
            AskPrice = p.askUpdate;  
        } 
           
        public void bidQtyUpdate(Product p) 
        {
            //do nothing
        }
           
        public void askQtyUpdate(Product p) 
        {   
            //do nothing
        }
        
        public void lastUpdate(Product p) 
        {
            //do nothing
        }
        
        public void lastQtyUpdate(Product p) 
        {
            //do nothing
        }

        public void orderConfirmed(Order ord)
        {
            //do nothing
        }

    }
}
