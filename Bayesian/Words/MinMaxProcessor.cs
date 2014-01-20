namespace Bayesian.Words
{
    /// <summary>
    /// Limits the length of the string.
    /// </summary>
    public class MinMaxProcessor : iWordProcessor
    {
        /// <summary>
        /// Max length
        /// </summary>
        private readonly int _max;

        /// <summary>
        /// Min length
        /// </summary>
        private readonly int _min;

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
        public string Process(string pStr)
        {
            return (pStr.Length > _min && pStr.Length < _max) ? pStr : null;
        }
    }
}