using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BackEnd_Project_1
{
    internal class Game2
    {
        public static string ReadEmbeddedResource(string resourceName)
        {
            try
            {
                // Load the current assembly
                Assembly assembly = Assembly.GetExecutingAssembly();

                // Open the embedded resource stream
                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                {
                    if (stream != null)
                    {
                        // Read the content of the resource
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            // Read the entire content of the resource into a string
                            string resourceContent = reader.ReadToEnd();
                            return resourceContent;
                        }
                    }
                    else
                    {
                        throw new ArgumentException("Resource not found.");
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Console.WriteLine($"Error reading embedded resource: {ex.Message}");
                return null;
            }
        }

        static string GetRandomWord(string[] words)
        {
            Random rand = new Random();
            int index = rand.Next(0, words.Length);
            return words[index];
        }
        public static string ChooseWord()
        {
            string file = ReadEmbeddedResource("BackEnd_Project_1.Resources.Nouns.txt");
            string[] words = file.Split(new char[] { '\n' });
            return GetRandomWord(words);
        }

        public static void WriteWord(char[] word, bool[] table)
        {
            Console.WriteLine("\n");
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < word.Length; i++)
            {
                if (table[i]) sb.Append(word[i] + " ");
                else sb.Append('_' + " ");
            }
            int falseCount = 0;
            for(int i = 0; i < table.Length; i++)
            {
                if (table[i] == false) falseCount++;
            }
            sb.Append(" " + falseCount);
            Console.WriteLine(sb.ToString());
        }

        public static char AskForChar()
        {
            string pattern = @"^[a-zA-Z]$";
            string input;
            char guess = ' ';
            do
            {
                Console.WriteLine("\nPlease enter one letter:");
                input = Console.ReadLine();
                if (Regex.IsMatch(input, pattern)) guess = Char.Parse(input);
            } while (!Regex.IsMatch(input, pattern));
            return guess;
        }


        public static void Execute(int amount)
        {
            Dictionary<char, bool> allLetters = new Dictionary<char, bool>();
            for (char c = 'A'; c <= 'Z'; c++)
            {
                allLetters[c] = false;
            }
            int count = 0;
            int lives = amount;
            char guess;
            char[] characters = ChooseWord().ToCharArray();
            bool[] table = new bool[characters.Length];
            while(count < characters.Length && lives > 0)
            {
                WriteWord(characters, table);
                Console.WriteLine($"\nLives left: {lives}");
                guess = char.ToUpper(AskForChar());
                if (allLetters[guess])
                {
                    Console.WriteLine("Letter already guessed.");
                    continue;
                }
                if (characters.Contains(guess))
                {
                    Console.WriteLine("Correct!");
                    allLetters[guess] = true;
                    for (int i = 0; i < characters.Length; i++)
                    {
                        if (characters[i] == guess)
                        {
                            table[i] = true;
                            count++;
                        }
                        }
                } else
                {
                    Console.WriteLine("Incorrect!");
                    allLetters[guess] = true;
                    lives--;
                }
            }
            Console.WriteLine("\nThe word was: " + new string(characters));
        }
    }
}
