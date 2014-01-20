using System.Collections.Generic;

namespace Bayesian.Words
{
    /// <summary>
    /// Holds a collection of processors.
    /// </summary>
    public class Processors : List<iWordProcessor>, iWordProcessor
    {
        /// <summary>
        /// Processes the collection.
        /// </summary>
        public string process(string pStr)
        {
            foreach (iWordProcessor proc in this)
            {
                pStr = proc.process(pStr);
                if (pStr == null)
                {
                    return null;
                }
            }
            return pStr;
        }
    }
}
