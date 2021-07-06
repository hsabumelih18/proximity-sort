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

        static void levenshteinDistance(string wordA, string wordB)
        {
            Console.WriteLine(wordA);
            for (int i = 0; i < Math.Min(wordA.Length, wordB.Length); i++)
            {
                if (wordA[i] != wordB[i])
                {
                    Console.WriteLine(wordA[i] + "->" + wordB[i]);
                }
            }
            if (wordA.Length > wordB.Length)
            {
                Console.WriteLine("remove " + wordA.Substring(wordB.Length));
            }

            else if (wordB.Length > wordA.Length)
            {
                Console.WriteLine("add " + wordA.Substring(wordB.Length));
            }
            Console.WriteLine(wordB);
        }

        static void Main(string[] args)
        {
            string a = "sittingasd";
            string b = "kitten";

            //Console.WriteLine(hammingDistance(a, b));
            levenshteinDistance(a, b);
        }

    }
}