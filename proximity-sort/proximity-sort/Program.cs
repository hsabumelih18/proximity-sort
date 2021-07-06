using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proximity_sort
{
    class Program
    {
        //returns the amount of different characters between two strings with the same size
        static int hammingDistance(string wordA, string wordB)
        {
            //int similarities =Math.Abs( wordA.Length - wordB.Length);
            int similarities = 0;
            for (int i = 0; i < wordA.Length; i++)
            {
                if (wordA[i] != wordB[i]) similarities++;
            }

            return similarities;
        }



        static void Main(string[] args)
        {
            string a = "kitten";
            string b = "miffen";

            Console.WriteLine(hammingDistance(a, b));
        }

    }
}
