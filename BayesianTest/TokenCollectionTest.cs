using System;
using System.Collections.Generic;
using Bayesian;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BayesianTest
{
    [TestClass]
    public class TokenCollectionTest : BayesianTest
    {
        private const string JSON = "{\"hello\":3,\"chicken\":1}";

        [TestMethod]
        public void Test_To_JSON()
        {
            Dictionary<string, int> tokens = new Dictionary<string, int>();
            tokens.Add("hello", 3);
            tokens.Add("chicken", 1);

            TokenCollection col = new TokenCollection(tokens);
            string str = TokenCollection.serialize(col);

            Assert.AreEqual(JSON, str);
        }

        [TestMethod]
        public void Test_From_JSON()
        {
            TokenCollection col = TokenCollection.deserialize(JSON);

            Assert.AreEqual(4, col.sum);
            Assert.AreEqual(2, col.count);
            Assert.AreEqual(3, col.get("hello"));
            Assert.AreEqual(1, col.get("chicken"));
        }

        [TestMethod]
        public void Test_Count()
        {
            TokenCollection col = TokenCollection.deserialize(JSON);

            Assert.AreEqual(0, col.get("superman"));
            Assert.AreEqual(3, col.get("hello"));
            Assert.AreEqual(1, col.get("chicken"));
        }

        [TestMethod]
        public void Test_Subtract_Rule()
        {
            TokenCollection a = new TokenCollection();
            a.add("mouse");
            a.add("mouse");
            a.add("mouse");

            TokenCollection b = new TokenCollection();
            b.add("house");
            b.add("house");

            try
            {
                TokenCollection.subtract(b, a);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
            }
        }

        [TestMethod]
        public void Test_Subtract()
        {
            TokenCollection a = create(new string[] { "mouse", "mouse", "mouse" });
            TokenCollection b = create(new string[] { "mouse", "chicken", "chicken" });

            TokenCollection sub = TokenCollection.subtract(a, b);

            Assert.AreEqual(2, sub.sum);
            Assert.AreEqual(2, sub.get("mouse"));
            Assert.AreEqual(1, sub.count);
            Assert.IsFalse(sub.contains("chicken"));
            Assert.AreEqual(0, sub.get("chicken"));
        }

        [TestMethod]
        public void Test_Subtract_Empty()
        {
            TokenCollection a = create(new string[] { "mouse", "mouse", "mouse" });
            TokenCollection b = create(new string[] { "mouse", "mouse", "mouse" });

            TokenCollection sub = TokenCollection.subtract(a, b);

            Assert.AreEqual(0, sub.sum);
            Assert.AreEqual(0, sub.get("mouse"));
            Assert.AreEqual(0, sub.count);
        }

        [TestMethod]
        public void Test_Add()
        {
            TokenCollection a = create(new string[] { "mouse", "mouse", "mouse" });
            TokenCollection b = create(new string[] { "mouse", "chicken", "chicken" });

            TokenCollection add = TokenCollection.add(a, b);

            Assert.AreEqual(4, add.sum);
            Assert.AreEqual(4, add.get("mouse"));
            Assert.IsFalse(add.contains("chicken"));
            Assert.AreEqual(0, add.get("house"));
        }

        [TestMethod]
        public void Test_Merge()
        {
            TokenCollection a = create(new string[] { "mouse", "mouse", "mouse" });
            TokenCollection b = create(new string[] { "mouse", "chicken", "chicken" });

            TokenCollection merge = TokenCollection.merge(a, b);

            Assert.AreEqual(6, merge.sum);
            Assert.AreEqual(4, merge.get("mouse"));
            Assert.AreEqual(2, merge.get("chicken"));
        }

    }
}
