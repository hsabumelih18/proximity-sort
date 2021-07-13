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



        static void damerauLevenshtein(string wordA, string wordB)
        {
            int minSize = Math.Min(wordA.Length, wordB.Length);
            Console.WriteLine(wordA);
            for (int i = 0; i < minSize; i++)
            {
                if (wordA[i] != wordB[i])
                {
                    if (i != minSize - 1 && wordA[i] == wordB[i + 1] && wordA[i + 1] == wordB[i])
                    {
                        Console.WriteLine("swap " + wordA[i] + "," + wordB[i]);
                        Console.WriteLine(i);
                        i++;
                    }

                    else
                    {
                        Console.WriteLine(wordA[i] + "->" + wordB[i]);
                        Console.WriteLine(i);
                    }
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

        //returns a dictionary with the letter a string has
        static Dictionary<char, int> getLetters(string str)
        {
            Dictionary<char, int> letterCount = new Dictionary<char, int>();
            foreach (char letter in str)
            {
                if (letterCount.ContainsKey(letter))
                {
                    letterCount[letter]++;
                }
                else
                {
                    letterCount.Add(letter, 1);
                }

            }
            return letterCount;
        }

        //returns the amount of letters that appear in two strings at the same time 
        static int getSameLettersCount(string s1, string s2)
        {
            Dictionary<char, int> letterCount1 = getLetters(s1);
            Dictionary<char, int> letterCount2 = getLetters(s2);
            int matchingChar=0;
            Dictionary<char, int>.KeyCollection Keys1 = letterCount1.Keys;

            Console.WriteLine();
            foreach (char key in Keys1)
            {
                if (letterCount2.ContainsKey(key))
                {
                    // adds the lesser amount of times a character has been seen in both words
                    matchingChar+= Math.Min(letterCount1[key], letterCount2[key]);
                }
            }
            return matchingChar;
        }

        static void Main(string[] args)
        {
            string a = "mamka";
            string b = "amkan";


            Console.WriteLine(getSameLettersCount(a, b));
            //Console.WriteLine(hammingDistance(a, b));
            //levenshteinDistance(a, b);
            //Dictionary<char, int> letterCount = getLetters(a);
            //foreach (KeyValuePair<char, int> letterCountPair in letterCount)
            //{
            //    Console.WriteLine(letterCountPair.Key + " " + letterCountPair.Value);
            //}
            //damerauLevenshtein(a, b);
        }

    }
}
