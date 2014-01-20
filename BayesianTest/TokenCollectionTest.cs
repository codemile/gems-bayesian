using System;
using System.Collections.Generic;
using Bayesian;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BayesianTest
{
    [TestClass]
    public class TokenCollectionTest : BayesianTest
    {
        private const string _JSON = "{\"hello\":3,\"chicken\":1}";

        [TestMethod]
        public void Test_Add()
        {
            TokenCollection a = Create(new[] {"mouse", "mouse", "mouse"});
            TokenCollection b = Create(new[] {"mouse", "chicken", "chicken"});

            TokenCollection add = TokenCollection.Add(a, b);

            Assert.AreEqual(4, add.Sum);
            Assert.AreEqual(4, add.get("mouse"));
            Assert.IsFalse(add.Contains("chicken"));
            Assert.AreEqual(0, add.get("house"));
        }

        [TestMethod]
        public void Test_Count()
        {
            TokenCollection col = TokenCollection.Deserialize(_JSON);

            Assert.AreEqual(0, col.get("superman"));
            Assert.AreEqual(3, col.get("hello"));
            Assert.AreEqual(1, col.get("chicken"));
        }

        [TestMethod]
        public void Test_From_JSON()
        {
            TokenCollection col = TokenCollection.Deserialize(_JSON);

            Assert.AreEqual(4, col.Sum);
            Assert.AreEqual(2, col.Count);
            Assert.AreEqual(3, col.get("hello"));
            Assert.AreEqual(1, col.get("chicken"));
        }

        [TestMethod]
        public void Test_Merge()
        {
            TokenCollection a = Create(new[] {"mouse", "mouse", "mouse"});
            TokenCollection b = Create(new[] {"mouse", "chicken", "chicken"});

            TokenCollection merge = TokenCollection.Merge(a, b);

            Assert.AreEqual(6, merge.Sum);
            Assert.AreEqual(4, merge.get("mouse"));
            Assert.AreEqual(2, merge.get("chicken"));
        }

        [TestMethod]
        public void Test_Subtract()
        {
            TokenCollection a = Create(new[] {"mouse", "mouse", "mouse"});
            TokenCollection b = Create(new[] {"mouse", "chicken", "chicken"});

            TokenCollection sub = TokenCollection.Subtract(a, b);

            Assert.AreEqual(2, sub.Sum);
            Assert.AreEqual(2, sub.get("mouse"));
            Assert.AreEqual(1, sub.Count);
            Assert.IsFalse(sub.Contains("chicken"));
            Assert.AreEqual(0, sub.get("chicken"));
        }

        [TestMethod]
        public void Test_Subtract_Empty()
        {
            TokenCollection a = Create(new[] {"mouse", "mouse", "mouse"});
            TokenCollection b = Create(new[] {"mouse", "mouse", "mouse"});

            TokenCollection sub = TokenCollection.Subtract(a, b);

            Assert.AreEqual(0, sub.Sum);
            Assert.AreEqual(0, sub.get("mouse"));
            Assert.AreEqual(0, sub.Count);
        }

        [TestMethod]
        public void Test_Subtract_Rule()
        {
            TokenCollection a = new TokenCollection();
            a.Add("mouse");
            a.Add("mouse");
            a.Add("mouse");

            TokenCollection b = new TokenCollection();
            b.Add("house");
            b.Add("house");

            try
            {
                TokenCollection.Subtract(b, a);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
            }
        }

        [TestMethod]
        public void Test_To_JSON()
        {
            Dictionary<string, int> tokens = new Dictionary<string, int> {{"hello", 3}, {"chicken", 1}};

            TokenCollection col = new TokenCollection(tokens);
            string str = TokenCollection.Serialize(col);

            Assert.AreEqual(_JSON, str);
        }
    }
}