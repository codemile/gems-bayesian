using Bayesian.Words;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BayesianTest.Words
{
    [TestClass]
    public class MinMaxProcessorTest
    {
        public string[] to_short = {
                                       "a",
                                       "aa",
                                       "aaa"
                                   };

        public string[] to_long = {
                                      "1234567890",
                                      "12345678901",
                                      "12345678902"
                                  };

        public string[] just_right = {
                                         "12345",
                                         "123456",
                                         "1234567"
                                     };

        [TestMethod]
        public void Test_Min_Max()
        {
            MinMaxProcessor proc = new MinMaxProcessor(3, 10);

            foreach (string str in to_short)
            {
                Assert.IsNull(proc.process(str));
            }

            foreach (string str in to_long)
            {
                Assert.IsNull(proc.process(str));
            }

            foreach (string str in just_right)
            {
                Assert.AreEqual(str, proc.process(str));
            }
        }
    }
}
