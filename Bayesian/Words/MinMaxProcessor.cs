
namespace Bayesian.Words
{
    /// <summary>
    /// Limits the length of the string.
    /// </summary>
    public class MinMaxProcessor : iWordProcessor
    {
        /// <summary>
        /// Min length
        /// </summary>
        private readonly int _min;

        /// <summary>
        /// Max length
        /// </summary>
        private readonly int _max;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pMin"></param>
        /// <param name="pMax"></param>
        public MinMaxProcessor(int pMin, int pMax)
        {
            _min = pMin;
            _max = pMax;
        }

        /// <summary>
        /// Check the length.
        /// </summary>
        public string process(string pStr)
        {
            return (pStr.Length > _min && pStr.Length < _max) ? pStr : null;
        }
    }
}
