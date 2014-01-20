
namespace Bayesian.Words
{
    /// <summary>
    /// Converts to lower case.
    /// </summary>
    public class LowerCaseProcessor : iWordProcessor
    {
        public string process(string pStr)
        {
            return pStr.ToLower();
        }
    }
}
