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
            int countChanges = 0;
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
                countChanges += wordA.Substring(wordB.Length).Length;
            }

            else if (wordB.Length > wordA.Length)
            {
                //Console.WriteLine("add " + wordA.Substring(wordB.Length));
                countChanges += wordA.Substring(wordB.Length).Length;
            }
            //Console.WriteLine(wordB);
            return countChanges;
        }



        static int damerauLevenshtein(string wordA, string wordB)
        {
            int minSize = Math.Min(wordA.Length, wordB.Length), countChanges = 0;
            //Console.WriteLine(wordA);
            for (int i = 0; i < minSize; i++)
            {
                if (wordA[i] != wordB[i])
                {
                    if (i != minSize - 1 && wordA[i] == wordB[i + 1] && wordA[i + 1] == wordB[i])
                    {
                        //Console.WriteLine("swap " + wordA[i] + "," + wordB[i]);
                        //Console.WriteLine(i);
                        countChanges++;
                        i++;
                    }

                    else
                    {
                        //Console.WriteLine(wordA[i] + "->" + wordB[i]);
                        //Console.WriteLine(i);
                        countChanges++;
                    }
                }
            }
            if (wordA.Length > wordB.Length)
            {
                //Console.WriteLine("remove " + wordA.Substring(wordB.Length));
                countChanges += wordA.Substring(wordB.Length).Length;
            }

            else if (wordB.Length > wordA.Length)
            {
                //Console.WriteLine("add " + wordA.Substring(wordB.Length));
                countChanges += wordA.Substring(wordB.Length).Length;
            }
            // Console.WriteLine(wordB);
            return countChanges;
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

        static void showWords(string[] words)
        {
            for(int i=0;i<words.Length;i++)
            {
                Console.WriteLine(i+1+"."+words[i]);
            }
            Console.WriteLine("Press any key to continue:");
            Console.ReadKey();
        }

        static void printOptions()
        {
            Console.WriteLine("----Choose an option----");
            Console.WriteLine("1.View data");
            Console.WriteLine("2.Search for a word");
            Console.WriteLine("3.Leave the program");
        }

        static int chooseOption()
        {
            printOptions();
            string line = Console.ReadLine();
            int option = int.Parse(line);
            while (option != 1 && option != 2 && option != 3)
            {
                Console.WriteLine("Enter a valid option!");
                line = Console.ReadLine();
                option = int.Parse(line);
            }
            return option;
        }

        static bool Menu(string[] wordList)
        {
            Console.Clear();
            int option = chooseOption();
            Console.Clear();
            if(option==1)
            {
                showWords(wordList);
            }
            if(option==2)
            {
                Console.WriteLine("Input a word to search:");
                string line = Console.ReadLine();
            }
            else if(option==3)
            {
                return false;
            }
            


            return true;
        }



        static void Main(string[] args)
        {
            //string a = "winkler";
            //string b = "welfare";
            string[] wordList;

            wordList = new string[10] {"gradushka", "gradiven", "grad", "grub", "grozen", "greshka", "georgi", "gramaden", "greben", "grah"};
            while(Menu(wordList))
            {
                ;
            }
            
            // Initialization of array
            //str1 = new string[5] { "Element 1", "Element 2", "a","b","c"};
            //str1[1] = "asd";
            //Console.WriteLine(str1[1]);
            //Console.WriteLine(jaroWinkler(a, b));
            //Console.WriteLine(getSameLettersCount(a, b));
            //Console.WriteLine(hammingDistance(a, b));
            //Console.WriteLine(levenshteinDistance(a, b));
            //Console.WriteLine("---------------");
            //Console.WriteLine(damerauLevenshtein(a, b));
            //Dictionary<char, int> letterCount = getLetters(a);
            //foreach (KeyValuePair<char, int> letterCountPair in letterCount)
            //{
            //    Console.WriteLine(letterCountPair.Key + " " + letterCountPair.Value);
            //}
            //damerauLevenshtein(a, b);
        }

    }
}
