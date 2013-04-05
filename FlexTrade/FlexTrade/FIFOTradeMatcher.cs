using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexTrade
{
    public class FIFOTradeMatcher : TradeMatcher
    {
        public List<Trade> match(List<Fill> unmatchedOrders)
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
                          fill.originalOrder.product == currProd &&
                          fill.matchedQuantity < fill.qty
                    select fill;

                    //Query to find sells for a product
                    IEnumerable<Fill> sellFillQuery =
                    from fill in unmatchedOrders
                    where fill.originalOrder.side == Order.Side.SELL &&
                          fill.originalOrder.product == currProd &&
                          fill.matchedQuantity < fill.qty
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
                    if (unmatchedOrders.IndexOf(sellFill) >= unmatchedOrders.IndexOf(buyFill))
                    {
                        openingFill = buyFill;
                        closingFill = sellFill;
                        buyOpened = true;
                    }
                    else
                    {
                        openingFill = sellFill;
                        closingFill = buyFill;
                        buyOpened = false;
                    }
                    currentTrade.openingOrders.Add(openingFill, openingFill.qty - openingFill.matchedQuantity);
                    openingQtyNeeded = openingFill.qty - openingFill.matchedQuantity;
                }

                int qtyAvail = closingFill.qty - closingFill.matchedQuantity;

                if (openingQtyNeeded >= qtyAvail)
                {
                    currentTrade.closingOrders.Add(closingFill, qtyAvail);
                    openingQtyNeeded = openingQtyNeeded - qtyAvail;
                    openingFill.matchedQuantity += qtyAvail;
                    closingFill.matchedQuantity += qtyAvail;
                }
                else
                {
                   currentTrade.closingOrders.Add(closingFill, openingQtyNeeded);
                   openingFill.matchedQuantity += openingQtyNeeded;
                   closingFill.matchedQuantity += openingQtyNeeded;
                   openingQtyNeeded = 0;
                }

                //If the Trade object is full matched, add it to the list of trades
                if (currentTrade.openingOrders.Values.Sum() == currentTrade.closingOrders.Values.Sum())
                {
                    trades.Add(currentTrade);
                    
                    //remove the opening orders now that it is matched
                    foreach(Fill f in currentTrade.openingOrders.Keys)
                    {
                        //update the fills and remove them from the umatched list
                        unmatchedOrders.Remove(f);
                    } 
                    foreach (Fill f in currentTrade.closingOrders.Keys)
                    {
                        if (f.matchedQuantity == f.qty)
                            unmatchedOrders.Remove(f);
                    }
                    currentTrade = new Trade();
                    partialMatch = false;
                }
            }

            //unwind partial matches
            if (currentTrade.openingOrders.Count() > 0)
            {
                currentTrade.openingOrders.Keys.First().matchedQuantity -= currentTrade.closingOrders.Values.Sum();
                foreach (Fill f in currentTrade.closingOrders.Keys)
                    f.matchedQuantity -= currentTrade.closingOrders[f];
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

                if (t.openingOrders.Count() > 0 && t.openingOrders.First().Key.originalOrder.side == Order.Side.BUY)
                {
                    totalBought = calcTotalForMapOfOrders(t.openingOrders);
                    totalSold = calcTotalForMapOfOrders(t.closingOrders);
                }
                else
                {
                    totalBought = calcTotalForMapOfOrders(t.closingOrders);
                    totalSold = calcTotalForMapOfOrders(t.openingOrders);
                }

                t.profitloss = totalSold - totalBought;
            }
        }

        private double calcTotalForMapOfOrders(Dictionary<Fill,int> orders)
        {
            double total = 0.0;

            foreach (Fill f in orders.Keys)
                total += f.price * orders[f];

            return total;
        }
    }
}
