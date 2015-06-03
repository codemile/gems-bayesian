using System;
using System.Collections.Generic;

namespace Bayesian
{
    /// <summary>
    /// The main class that performs the bayesian filtering calculations.
    /// </summary>
    public static class Analyzer
    {
        /// <summary>
        /// Calculates a probability value based on statistics from two categories
        /// </summary>
        private static float CalcProbability(Probability pProb, float pFirstCount, float pFirstTotal, float pSecondCount,
                                             float pSecondTotal)
        {
            const float s = 1f;
            const float x = .5f;

            float bw = pFirstCount / pFirstTotal;
            float gw = pSecondCount / pSecondTotal;
            float pw = ((bw) / ((bw) + (gw)));
            float n = pFirstCount + pSecondCount;
            float fw = ((s * x) + (n * pw)) / (s + n);

            pProb.Log(fw);

            return fw;
        }

        /// <summary>
        /// </summary>
        private static float getPrediction(IEnumerable<string> pTokens, TokenCollection pFirst, TokenCollection pSecond)
        {
            Probability prob = new Probability();

            foreach (string token in pTokens)
            {
                int firstCount = pFirst.get(token);
                int secondCount = pSecond.get(token);

                if (firstCount == 0 && secondCount == 0)
                {
                    continue;
                }

                float probability = CalcProbability(prob, firstCount, pFirst.Sum, secondCount, pSecond.Sum);

                Console.WriteLine(@"{0}: [{1}] ({2}-{3}), ({4}-{5})",
                    token,
                    probability,
                    firstCount,
                    pFirst.Sum,
                    secondCount,
                    pSecond.Sum);
            }

            return prob.Combine();
        }

        /// <summary>
        /// Calculates the probability that the pTokens belong to the pGood collection, when
        /// compared to the pBad collection.
        /// The return value will be from 0 to 1, where 1 is 100% belongs to the pGood and 0 is
        /// it does not belong.
        /// </summary>
        public static float Score(IEnumerable<string> pTokens, TokenCollection pGood, TokenCollection pBad)
        {
            float prediction = getPrediction(pTokens, pGood, pBad);
            if (float.IsNaN(prediction))
            {
                return 0.0f;
            }
            return Math.Max(0.0f, prediction - 0.5f) * 2;
        }

        /// <summary>
        /// Used to store the probability as tokens are analyzed.
        /// </summary>
        private class Probability
        {
            /// <summary>
            /// </summary>
            private float _i;

            /// <summary>
            /// </summary>
            private float _invI;

            /// <summary>
            /// Combines the probabilities from the first and second lists.
            /// </summary>
            /// <returns></returns>
            public float Combine()
            {
                return _i / (_i + _invI);
            }

            /// <summary>
            /// Records the probability of a token.
            /// </summary>
            /// <param name="pProb"></param>
            public void Log(float pProb)
            {
                if (float.IsNaN(pProb))
                {
                    return;
                }

                _i = Math.Abs(_i) < Single.Epsilon ? pProb : _i * pProb;
                _invI = Math.Abs(_invI) < Single.Epsilon ? (1 - pProb) : _invI * (1 - pProb);
            }
        }
    }
}