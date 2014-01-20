using Bayesian.Words;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BayesianTest.Words
{
    [TestClass]
    public class LowerCaseProcessorTest
    {
        [TestMethod]
        public void Test_LowerCase()
        {
            LowerCaseProcessor proc = new LowerCaseProcessor();

            Assert.AreEqual("something", proc.Process("SomeThing"));
            Assert.AreEqual("something", proc.Process("SOMETHING"));
            Assert.AreEqual("something", proc.Process("something"));
        }
    }
}