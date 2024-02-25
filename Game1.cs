using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BackEnd_Project_1
{
    internal class Game1
    {
        public static bool CheckNumber(string number)
        {
            string pattern = @"^-?\d+(\.\d+)?$";
            if(Regex.IsMatch(number, pattern)) return true;
            return false;
        }
        public static void Execute(int lowerLimit, int upperLimit)
        {
            int number = new Random().Next(lowerLimit, upperLimit + 1);
            string input;
            int guess = number + 1;
            int tries = 0;
            do
            {
                Console.WriteLine("Please enter the number:");
                input = Console.ReadLine();
                if (CheckNumber(input)) guess = int.Parse(input);
                else continue;
                if (guess > number) Console.WriteLine("Lower.");
                if (guess < number) Console.WriteLine("Higher.");
                tries++;
            } while(guess != number);
            Console.WriteLine($"Correct! You needed {tries} tries.");
        }

        public static void Start()
        {
            int lowerLimit = 0;
            int upperLimit = 0;
            string input;
            do
            {
                Console.WriteLine("Please write lower limit:");
                input = Console.ReadLine();
                if (CheckNumber(input)) lowerLimit = int.Parse(input);
            } while (!CheckNumber(input));
            do
            {
                Console.WriteLine("Please write upper limit:");
                input = Console.ReadLine();
                if (CheckNumber(input)) upperLimit = int.Parse(input);
            } while (!CheckNumber(input));
            Execute(lowerLimit, upperLimit);
        }
    }
}
