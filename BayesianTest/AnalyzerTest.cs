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
            TokenCollection good = Create(new[] {"ikea", "kitchen", "mouse"});
            TokenCollection bad = Create(new[] {"house", "stock", "chicken"});

            Processors proc = new Processors();
            const string document = "ikea kitchen mouse ikea kitchen mouse ikea kitchen mouse ikea kitchen mouse";
            Tokens tokens = new Tokens(document, proc);

            Analyzer a = new Analyzer();
            float score = Analyzer.Score(tokens, good, bad);

            //Assert.AreEqual(0.0f, score);
        }
    }
}