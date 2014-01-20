using Bayesian;
using Bayesian.Words;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BayesianTest
{
    [TestClass]
    public class AnalyzerTest : BayesianTest
    {
        [TestMethod]
        public void Test_Score()
        {
            TokenCollection good = create(new string[] { "ikea", "kitchen", "mouse" });
            TokenCollection bad = create(new string[] { "house", "stock", "chicken" });

            Processors proc = new Processors();
            string document = "ikea kitchen mouse ikea kitchen mouse ikea kitchen mouse ikea kitchen mouse";
            Tokens tokens = new Tokens(document, proc);

            Analyzer a = new Analyzer();
            float score = a.score(tokens, good, bad);

            //Assert.AreEqual(0.0f, score);
        }
    }
}
