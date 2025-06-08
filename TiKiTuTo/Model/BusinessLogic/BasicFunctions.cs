
using TiKiTuTo.Controller;
using TiKiTuTo.Model.DataObjects;
using TiKiTuTo.Model.BusinessLogic.GameLogic;


namespace TiKiTuTo.Model.BusinessLogic
{
    public class BasicFunctions
    {
        /// <summary>
        /// For a given integer n, calculates the largest number p where p <= n and p = 2^x, where x is a natural number.
        /// </summary>
        /// <param name="n">The integer in question</param>
        /// <returns>The largest number smaller or equal to n which is a power of 2.</returns>
        public static int HighestPowerOf2(int n)
        {
            int p = (int)(Math.Log(n) /
                           Math.Log(2));
            return (int)Math.Pow(2, p);
        }


        
    }
}