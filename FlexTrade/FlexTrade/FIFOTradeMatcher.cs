using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexTrade
{
    public class FIFOTradeMatcher : TradeMatcher
    {
        public List<Trade> match(List<Fill> unmatchedOrders, Dictionary<Order, List<Fill>> orderToFillMap)
        {
             List<Trade> trades = new List<Trade>();

            lock(unmatchedOrders)
            {
                List<Product> products = getListOfProducts(unmatchedOrders);
            
                //For each product try to find matches
                foreach(Product currProd in products)
                {
                    //Create query to find buys for a product
                    IEnumerable<Fill> buyFillQuery =
                    from fill in unmatchedOrders
                    where fill.originalOrder.side == Order.Side.BUY &&
                          fill.originalOrder.product == currProd
                    select fill;

                    //Query to find sells for a product
                    IEnumerable<Fill> sellFillQuery =
                    from fill in unmatchedOrders
                    where fill.originalOrder.side == Order.Side.SELL &&
                          fill.originalOrder.product == currProd
                    select fill;

                    trades = matchForCurrentProduct(unmatchedOrders, buyFillQuery, sellFillQuery);
                }
            }

            //calculate the P/L for each trade
            calculatePNL(trades);

            return trades;
        }

        private List<Trade> matchForCurrentProduct(List<Fill> unmatchedOrders, IEnumerable < Fill > buyFillQuery, IEnumerable < Fill > sellFillQuery)
        {
            List<Trade> trades = new List<Trade>();
            Trade currentTrade = new Trade();
            Fill openingFill = null, closingFill = null;

            //state variables
            Boolean buyOpened = true;
            Boolean partialMatch = false;
            int openingQtyNeeded = 0;

            //while we have both buys and sells for this product, keep matching
            while (buyFillQuery.Count() > 0 && sellFillQuery.Count() > 0)
            {
                //take the top buy and match with the top sell
                Fill buyFill = buyFillQuery.First();
                Fill sellFill = sellFillQuery.First();

                //If we have already started to match a trade, continue by match the current fill
                partialMatch = (currentTrade.openingOrders != null && currentTrade.openingOrders.Count > 0);
                if (partialMatch && buyOpened)
                    closingFill = sellFill;
                else if(partialMatch && !buyOpened)
                    closingFill = buyFill;

                //We haven't already determined an opening order, determine which will be
                //considered the opening order based on whether the buy or sell came in first
                else
                {
                    //if the buy fill was first
                    if (sellFill.time >= buyFill.time)
                    {
                        openingFill = buyFill;
                        closingFill = sellFill;
                        currentTrade.openingOrders.Add(openingFill,openingFill.qty - openingFill.matchedQuantity);
                        openingQtyNeeded = openingFill.qty - openingFill.matchedQuantity;
                        buyOpened = true;
                    }
                    else
                    {
                        openingFill = sellFill;
                        closingFill = buyFill;
                        buyOpened = false;
                    }
                }

                //int qtyNeeded = openingFill.qty - openingFill.matchedQuantity;
                int qtyAvail = closingFill.qty - closingFill.matchedQuantity;

                if (openingQtyNeeded >= qtyAvail)
                {
                    currentTrade.closingOrders.Add(closingFill, qtyAvail);
                    openingQtyNeeded = openingQtyNeeded - qtyAvail;
                    //openingFill.matchedQuantity += qtyAvail;
                    //closingFill.matchedQuantity = qtyAvail;
                }
                else
                {
                   currentTrade.closingOrders.Add(closingFill, openingQtyNeeded);
                   openingQtyNeeded = 0;
                   //openingFill.matchedQuantity = qtyNeeded;
                   //closingFill.matchedQuantity += qtyNeeded;
                }

                //If the Trade object is full matched, add it to the list of trades
                if (currentTrade.openingOrders.Values.Sum() == currentTrade.closingOrders.Values.Sum())
                {
                    trades.Add(currentTrade);
                    currentTrade = new Trade();
                    partialMatch = false;

                    //remove the opening orders now that it is matched
                    foreach(Fill f in currentTrade.openingOrders.Keys)
                    {
                        //update the fills and remove them from the umatched list
                        openingFill.matchedQuantity = f.qty;
                        unmatchedOrders.Remove(f);
                    } 
                    foreach (Fill f in currentTrade.closingOrders.Keys)
                    {
                        if (f.matchedQuantity == f.qty)
                        {
                            f.matchedQuantity = f.qty;
                            unmatchedOrders.Remove(f);
                        }
                        else
                        {
                            f.matchedQuantity += currentTrade.closingOrders[f];
                        }
                    }

                }
            }
            return trades;
        }

        //Get all products represented by these orders
        private List<Product> getListOfProducts(List<Fill> orders)
        {
            List<Product> p = new List<Product>();

            IEnumerable<Product> ProductQuery =
            from fill in orders
            select fill.originalOrder.product;

            p.AddRange(ProductQuery.Distinct());

            return p;
        }

        private void calculatePNL(List<Trade> trades)
        {
            
            foreach (Trade t in trades)
            {
                double totalBought = 0.0;
                double totalSold = 0.0;

                
                    
            }
        }

        private double calcTotalForMapOfOrders(Dictionary<Order,int> orders)
        {
            double total = 0.0;

            foreach (Order key in orders.Keys)
            {
                int qty = orders[key];
                //orderToFillMap
            }

            return total;
        }
    }
}
