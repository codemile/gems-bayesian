using Bayesian.Words;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BayesianTest.Words
{
    [TestClass]
    public class MinMaxProcessorTests
    {
        private readonly string[] _justRight =
        {
            "12345",
            "123456",
            "1234567"
        };

        private readonly string[] _toLong =
        {
            "1234567890",
            "12345678901",
            "12345678902"
        };

        private readonly string[] _toShort =
        {
            "a",
            "aa",
            "aaa"
        };

        [TestMethod]
        public void Test_Min_Max()
        {
            MinMaxProcessor proc = new MinMaxProcessor(3, 10);

            foreach (string str in _toShort)
            {
                Assert.IsNull(proc.Process(str));
            }

            foreach (string str in _toLong)
            {
                Assert.IsNull(proc.Process(str));
            }

            foreach (string str in _justRight)
            {
                Assert.AreEqual(str, proc.Process(str));
            }
        }
    }
}