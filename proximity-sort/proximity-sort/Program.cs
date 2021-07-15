using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ReverseComparer : IComparer
{
    // Call CaseInsensitiveComparer.Compare with the parameters reversed.
    public int Compare(Object x, Object y)
    {
        return (new CaseInsensitiveComparer()).Compare(y, x);
    }
}

namespace proximity_sort
{

    class Program
    {
        //returns the amount of different characters between two strings with the same size
        static int hammingDistance(string wordA, string wordB)
        {
            int similarities = 0;

            for (int i = 0; i < Math.Min(wordA.Length, wordB.Length); i++)
            {
                if (wordA[i] != wordB[i]) similarities++;
            }

            return similarities;
        }

        //returns the distance between two words by deleting adding letters
        //and changing one letter into another
        static int levenshteinDistance(string wordA, string wordB)
        {
            int countChanges = 0;

            for (int i = 0; i < Math.Min(wordA.Length, wordB.Length); i++)
            {
                if (wordA[i] != wordB[i])
                {
                    countChanges++;
                }
            }

            if (wordA.Length > wordB.Length)
            {
                countChanges += wordA.Substring(wordB.Length).Length;
            }

            else if (wordB.Length > wordA.Length)
            {
                countChanges += wordB.Substring(wordA.Length).Length;
            }

            return countChanges;
        }


        //adds the option of transpostions to the standart levenshtein metric
        static int damerauLevenshtein(string wordA, string wordB)
        {
            int minSize = Math.Min(wordA.Length, wordB.Length), countChanges = 0;
            for (int i = 0; i < minSize; i++)
            {
                if (wordA[i] != wordB[i])
                {
                    if (i != minSize - 1 && wordA[i] == wordB[i + 1] && wordA[i + 1] == wordB[i])
                    {
                        countChanges++;
                        i++;
                    }

                    else
                    {
                        countChanges++;
                    }
                }
            }

            if (wordA.Length > wordB.Length)
            {
                countChanges += wordA.Substring(wordB.Length).Length;
            }

            else if (wordB.Length > wordA.Length)
            {
                countChanges += wordB.Substring(wordA.Length).Length;
            }

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


        //Calculates the distance between two strings by
        //a formula which results in a fractional (0-1) number
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
            //stores the keys that appear in both dictionaries
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
                            matchingLetters2[count++] = letter;
                            letterCount2[letter]--;
                        }

                    }
                }

            }
            //calculates the non mathcing characters at the same position
            int nonMatchChar = 0, transpositions;
            for (int i = 0; i < sameCount; i++)
            {
                if (matchingLetters2[i] != sameLetters1[i]) nonMatchChar++;
            }
            transpositions = nonMatchChar / 2;

            result = ((float)matchChar / (float)Math.Abs(sz1) + (float)matchChar / (float)Math.Abs(sz1)
                + ((float)matchChar - (float)transpositions) / (float)matchChar) / (float)3;

            return result;
        }

        //prints the words in an array of strings
        static void showWords(string[] words)
        {
            for (int i = 0; i < words.Length; i++)
            {
                Console.WriteLine(i + 1 + "." + words[i]);
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

        static string askForWord()
        {
            Console.WriteLine("Input a word to search:");
            string word = Console.ReadLine();
            Console.Clear();
            return word;
        }

        //makes the user enter a number until it's
        //either 1,2 or 3. That's later returned
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

        static void showMetricOptions()
        {
            Console.WriteLine("Choose a string similarity metric:");
            Console.WriteLine("1.Hamming");
            Console.WriteLine("2.Levenshtein");
            Console.WriteLine("3.Damerau-Levenshtein");
            Console.WriteLine("4.Jaro-Winkler");
        }
        //
        static int chooseMetric()
        {
            showMetricOptions();
            string line = Console.ReadLine();
            int option = int.Parse(line);
            while (option != 1 && option != 2 && option != 3 && option != 4)
            {
                Console.WriteLine("Enter a valid option!");
                line = Console.ReadLine();
                option = int.Parse(line);
            }
            return option;
        }

        //sorts the words by the distance from the searched
        //word via the Levenshtein string metric
        static void sortByLevenshtein(string wordSearch, string[] wordList)
        {
            int[] metricResults = new int[15];
            int resultCount = 0, result;
            bool[] used = new bool[30];
            foreach (string word in wordList)
            {
                result = levenshteinDistance(word, wordSearch);
                if (used[result] == false)
                {
                    metricResults[resultCount++] = result;
                    used[result] = true;
                }
            }
            //sorts the array in ascending order
            Array.Sort(metricResults, 0, resultCount);
            Console.Clear();
            int wordsOutputed = 1;
            for (int i = 0; i < resultCount; i++)
            {
                foreach (string word in wordList)
                {
                    result = levenshteinDistance(word, wordSearch);
                    if (metricResults[i] == result)
                    {
                        Console.WriteLine(wordsOutputed++ + "." + word);
                    }
                }
            }


            Console.WriteLine("Press any key to continue:");
            Console.ReadKey();
        }

        static void sortByDamerau(string wordSearch, string[] wordList)
        {
            int[] metricResults = new int[15];
            int resultCount = 0, result;
            bool[] used = new bool[30];
            foreach (string word in wordList)
            {
                result = damerauLevenshtein(word, wordSearch);
                if (used[result] == false)
                {
                    metricResults[resultCount++] = result;
                    used[result] = true;
                }
            }
            //sorts the array in ascending order
            Array.Sort(metricResults, 0, resultCount);
            Console.Clear();
            int wordsOutputed = 1;
            for (int i = 0; i < resultCount; i++)
            {
                foreach (string word in wordList)
                {
                    result = damerauLevenshtein(word, wordSearch);
                    if (metricResults[i] == result)
                    {
                        Console.WriteLine(wordsOutputed++ + "." + word);
                    }
                }
            }


            Console.WriteLine("Press any key to continue:");
            Console.ReadKey();
        }

        static bool isUsed(float[] usedResults, float number)
        {
            bool used = false;
            foreach (float result in usedResults)
            {
                if (result == number)
                {
                    used = true;
                    break;
                }
            }
            return used;
        }

        static void sortByJaro(string wordSearch, string[] wordList)
        {
            float[] metricResults = new float[15];
            float result;
            int resultCount = 0;

            foreach (string word in wordList)
            {
                result = jaroWinkler(word, wordSearch);
                if (isUsed(metricResults, result) == false)
                {
                    metricResults[resultCount++] = result;
                }

            }


            //sorts the array in descending order
            IComparer revComparer = new ReverseComparer();
            Array.Sort(metricResults, 0, resultCount, revComparer);
            Console.Clear();
            int wordsOutputed = 1;
            for (int i = 0; i < resultCount; i++)
            {
                foreach (string word in wordList)
                {
                    result = jaroWinkler(word, wordSearch);
                    if (metricResults[i] == result)
                    {
                        Console.WriteLine(wordsOutputed++ + "." + word);
                    }
                }
            }


            Console.WriteLine("Press any key to continue:");
            Console.ReadKey();
        }

        //asks the user which method they would like to
        //sort the words by and sorts them
        static void sortByDistance(string[] wordList)
        {
            string wordSearch = askForWord();
            int metricChoice = chooseMetric();

            if (metricChoice == 1)
            {
                //we use the levenshtein function call for the hamming distance because the hamming distance
                //can only be used with words of the same length which is not possible in our cause
                sortByLevenshtein(wordSearch, wordList);
            }
            else if (metricChoice == 2)
            {
                sortByLevenshtein(wordSearch, wordList);
            }
            else if (metricChoice == 3)
            {
                sortByDamerau(wordSearch, wordList);
            }
            else
            {
                sortByJaro(wordSearch, wordList);
            }

        }

        static bool Menu(string[] wordList)
        {
            Console.Clear();
            int option = chooseOption();
            Console.Clear();
            if (option == 1)
            {
                showWords(wordList);
            }
            if (option == 2)
            {
                sortByDistance(wordList);
            }
            else if (option == 3)
            {
                return false;
            }



            return true;
        }





        static void Main(string[] args)
        {

            string[] wordList;

            wordList = new string[10] { "gradushka", "gradiven", "grad", "grub", "grozen", "greshka", "georgi", "gramaden", "greben", "grah" };
            //calls the Menu function until false is returned
            //which happens only when the user inputs 3
            while (Menu(wordList))
            {
                ;
            }
        }

    }
}
