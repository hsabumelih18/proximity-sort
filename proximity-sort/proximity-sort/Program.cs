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

        static int levenshteinDistance(string wordA, string wordB)
        {
            int countChanges=0;
            //Console.WriteLine(wordA);
            for (int i = 0; i < Math.Min(wordA.Length, wordB.Length); i++)
            {
                if (wordA[i] != wordB[i])
                {
                    //Console.WriteLine(wordA[i] + "->" + wordB[i]);
                    countChanges++;
                }
            }
            if (wordA.Length > wordB.Length)
            {
                //Console.WriteLine("remove " + wordA.Substring(wordB.Length));
                countChanges++;
            }

            else if (wordB.Length > wordA.Length)
            {
                //Console.WriteLine("add " + wordA.Substring(wordB.Length));
                countChanges++;
            }
            //Console.WriteLine(wordB);
            return countChanges;
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
            int matchingChar = 0;
            Dictionary<char, int>.KeyCollection Keys1 = letterCount1.Keys;

            foreach (char key in Keys1)
            {
                if (letterCount2.ContainsKey(key))
                {
                    // adds the lesser amount of times a character has been seen in both words
                    matchingChar += Math.Min(letterCount1[key], letterCount2[key]);
                }
            }
            return matchingChar;
        }

        static float jaroWinkler(string wordA, string wordB)
        {
            float result = 0;
            int sz1 = wordA.Length;
            int sz2 = wordA.Length;
            int matchChar = getSameLettersCount(wordA, wordB);
            Dictionary<char, int> letterCount1 = getLetters(wordA);
            Dictionary<char, int> letterCount2 = getLetters(wordB);
            Dictionary<char, int>.KeyCollection Keys1 = letterCount1.Keys;
            Dictionary<char, int>.KeyCollection Keys2 = letterCount2.Keys;
            char[] sameLetters1 = new char[25];
            char[] sameLetters2 = new char[25];

            char[] matchingLetters2 = new char[25];
            int count = 0, sameCount;
            foreach (char key in Keys1)
            {
                if (letterCount2.ContainsKey(key))
                {
                    sameLetters1[count++] = key;
                }
            }

            sameCount = 0;
            foreach (char key in Keys2)
            {
                if (letterCount1.ContainsKey(key))
                {
                    sameLetters2[sameCount++] = key;
                }
            }
            count = 0;
            foreach (char letter in wordB)
            {
                foreach (char sameLetter in sameLetters2)
                {
                    if (letter == sameLetter)
                    {
                        if (letterCount2[letter] > 0)
                        {
                            //Console.WriteLine(/*letterCount1[letter] + " " + */letter);
                            matchingLetters2[count++] = letter;
                            letterCount2[letter]--;

                        }

                    }
                }

            }
            for (int i = 0; i < sameLetters1.Length; i++)
            {
                Console.WriteLine(sameLetters1[i]);
            }

            int nonMatchChar = 0, transpositions;
            for (int i = 0; i < sameCount; i++)
            {
                Console.WriteLine(matchingLetters2[i] + " " + sameLetters1[i]);
                if (matchingLetters2[i] != sameLetters1[i]) nonMatchChar++;
            }
            transpositions = nonMatchChar / 2;
            Console.WriteLine(matchChar + " " + nonMatchChar + " " + transpositions);

            result = ((float)matchChar / (float)Math.Abs(sz1) + (float)matchChar / (float)Math.Abs(sz1)
                + ((float)matchChar - (float)transpositions) / (float)matchChar) / (float)3;

            return result;
        }


        static void Main(string[] args)
        {
            string a = "winkler";
            string b = "welfare";


            //Console.WriteLine(jaroWinkler(a, b));
            //Console.WriteLine(getSameLettersCount(a, b));
            //Console.WriteLine(hammingDistance(a, b));
            Console.WriteLine(levenshteinDistance(a, b));
            Console.WriteLine("---------------");
            damerauLevenshtein(a, b);
            //Dictionary<char, int> letterCount = getLetters(a);
            //foreach (KeyValuePair<char, int> letterCountPair in letterCount)
            //{
            //    Console.WriteLine(letterCountPair.Key + " " + letterCountPair.Value);
            //}
            //damerauLevenshtein(a, b);
        }

    }
}
