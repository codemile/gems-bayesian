using System.Linq;
using Bayesian;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BayesianTest
{
    public class BayesianTest
    {
        public TokenCollection create(string[] pTokens)
        {
            TokenCollection a = new TokenCollection();
            foreach (string token in pTokens)
            {
                a.add(token);
            }

            Assert.AreEqual(pTokens.Length, a.sum);
            Assert.AreEqual(pTokens.Distinct().Count(), a.count);
            foreach (string token in pTokens)
            {
                Assert.AreNotEqual(0, a.get(token));
            }

            return a;
        }
    }
}
