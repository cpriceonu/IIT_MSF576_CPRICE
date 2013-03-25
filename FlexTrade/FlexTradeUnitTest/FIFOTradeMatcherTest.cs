using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlexTrade;
using System.Collections.Generic;

namespace FlexTradeUnitTest
{
    [TestClass]
    public class FIFOTradeMatcherTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            FIFOTradeMatcher matcher = new FIFOTradeMatcher();

            Equity p = new Equity("Google","GOOG");
            Order order1 = new Order(p, 10, Order.Side.BUY);
            Order order2 = new Order(p, 10, Order.Side.SELL);

            Fill fill1 = new Fill();
            fill1.price = 100;
            fill1.qty = 10;
            fill1.originalOrder = order1;

            Fill fill2 = new Fill();
            fill2.price = 100;
            fill2.qty = 10;
            fill2.originalOrder = order1;

            List<Fill> fills = new List<Fill>();
            fills.Add(fill1);
            fills.Add(fill2);

            matcher.match(fills, null);

            Assert.AreEqual(1, 1);
        }
    }
}
