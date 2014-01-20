using System.Linq;
using Bayesian;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BayesianTest
{
    public class BayesianTest
    {
        protected static TokenCollection Create(string[] pTokens)
        {
            TokenCollection a = new TokenCollection();
            foreach (string token in pTokens)
            {
                a.Add(token);
            }

            Assert.AreEqual(pTokens.Length, a.Sum);
            Assert.AreEqual(pTokens.Distinct().Count(), a.Count);
            foreach (string token in pTokens)
            {
                Assert.AreNotEqual(0, a.get(token));
            }

            return a;
        }
    }
}