using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//remove this comment
namespace FlexTrade
{
    class Order
    {
        public int internalId { get; set; }
        public int exchangeId { get; set; }
        public DateTime timeCreated { get; set; }
        public DateTime lastModified { get; set; }
        public List<Double> fillPrices { get; set; }
        public List<Int32> fillQuantities { get; set; }
        public Side side { get; set; }
        public Int32 orderQuantity { get; set; }
        public Product product { get; set; }
        public Int32 matchedQuantity { get; set; }
        public String currency { get; set; }
        public OrderStatus status { get; set; }

        public Order(Product p, int quantity, Side s)
        {
            product = p;
            orderQuantity = quantity;
            side = s;
            timeCreated = DateTime.UtcNow;
        }

        public enum Side
        {
            BUY,
            SELL
        };

        public enum OrderStatus
        {
            INITIAL, 
            SENT, 
            CONFIRM, 
            OPEN, 
            PARTIAL_FILL, 
            FILL, 
            PARTIAL_MATCH, 
            MATCHED, 
            CANCELLED
        };
    }
}
