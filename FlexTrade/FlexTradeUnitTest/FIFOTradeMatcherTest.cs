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
        public void PerfectMatchTest()
        {
            FIFOTradeMatcher matcher = new FIFOTradeMatcher();
            List<Trade> matches = new List<Trade>();

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
            fill2.originalOrder = order2;

            List<Fill> fills = new List<Fill>();
            fills.Add(fill1);
            fills.Add(fill2);

            matches = matcher.match(fills, null);

            Assert.IsTrue(matches.Count == 1 && fills.Count == 0);
        }

        [TestMethod]
        public void LopsidedMatchTest()
        {
            FIFOTradeMatcher matcher = new FIFOTradeMatcher();
            List<Trade> matches = new List<Trade>();

            Equity p = new Equity("Google", "GOOG");
            Order order1 = new Order(p, 10, Order.Side.BUY);
            Order order2 = new Order(p, 5, Order.Side.SELL);
            Order order3 = new Order(p, 25, Order.Side.BUY);
            Order order4 = new Order(p, 7, Order.Side.SELL);
            Order order5 = new Order(p, 12, Order.Side.SELL);

            Fill fill1 = new Fill();
            fill1.price = 100;
            fill1.qty = 10;
            fill1.originalOrder = order1;

            Fill fill2 = new Fill();
            fill2.price = 100;
            fill2.qty = 5;
            fill2.originalOrder = order2;

            Fill fill3 = new Fill();
            fill3.price = 100;
            fill3.qty = 20;
            fill3.originalOrder = order3;

            Fill fill4 = new Fill();
            fill4.price = 100;
            fill4.qty = 6;
            fill4.originalOrder = order4;

            Fill fill5 = new Fill();
            fill5.price = 100;
            fill5.qty = 12;
            fill5.originalOrder = order5;

            List<Fill> fills = new List<Fill>();
            fills.Add(fill1);
            fills.Add(fill2);
            fills.Add(fill3);
            fills.Add(fill4);
            fills.Add(fill5); 

            matches = matcher.match(fills, null);

            Assert.IsTrue(matches.Count == 1 && fills.Count == 3);
        }
        [TestMethod]
        public void LopsidedMatchTest2()
        {
            FIFOTradeMatcher matcher = new FIFOTradeMatcher();
            List<Trade> matches = new List<Trade>();

            Equity p = new Equity("Google", "GOOG");
            Order order1 = new Order(p, 10, Order.Side.SELL);
            Order order2 = new Order(p, 5, Order.Side.SELL);
            Order order3 = new Order(p, 25, Order.Side.SELL);
            Order order4 = new Order(p, 7, Order.Side.BUY);
            Order order5 = new Order(p, 12, Order.Side.BUY);
            Order order6 = new Order(p, 2, Order.Side.BUY);

            Fill fill1 = new Fill();
            fill1.price = 100;
            fill1.qty = 7;
            fill1.originalOrder = order1;

            Fill fill2 = new Fill();
            fill2.price = 100;
            fill2.qty = 5;
            fill2.originalOrder = order2;

            Fill fill3 = new Fill();
            fill3.price = 100;
            fill3.qty = 21;
            fill3.originalOrder = order3;

            Fill fill4 = new Fill();
            fill4.price = 100;
            fill4.qty = 7;
            fill4.originalOrder = order4;

            Fill fill5 = new Fill();
            fill5.price = 100;
            fill5.qty = 12;
            fill5.originalOrder = order5;

            Fill fill6 = new Fill();
            fill6.price = 100;
            fill6.qty = 1;
            fill6.originalOrder = order6;

            List<Fill> fills = new List<Fill>();
            fills.Add(fill1);
            fills.Add(fill2);
            fills.Add(fill3);
            fills.Add(fill4);
            fills.Add(fill5);
            fills.Add(fill6); 

            matches = matcher.match(fills, null);

            Assert.IsTrue(matches.Count == 2 && fills.Count == 3);
        }
    }
}
