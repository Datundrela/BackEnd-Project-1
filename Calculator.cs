using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BackEnd_Project_1
{
    internal class Calculator
    {
        public interface IOperator
        {
            public double Invoke(double x, double y);
        }

        public class Addition : IOperator
        {
            public double Invoke(double x, double y)
            {
                return x + y;
            }
        }

        public class Subtraction : IOperator
        {
            public double Invoke(double x, double y)
            {
                return x - y;
            }
        }

        public class Multiplication : IOperator
        {
            public double Invoke(double x, double y)
            {
                return x * y;
            }
        }

        public class Division : IOperator
        {
            public double Invoke(double x, double y)
            {
                if (y == 0)
                {
                    Console.WriteLine("Cannot divide by zero!");
                    Environment.Exit(0);
                }
                return x / y;
            }
        }

        public class Operators
        {
            public static Dictionary<int, IOperator> operators = new Dictionary<int, IOperator>();
            static Operators()
            {
                operators.Add(1, new Addition());
                operators.Add(2, new Subtraction());
                operators.Add(3, new Multiplication());
                operators.Add(4, new Division());
            }
        }
        public static int AskForOperator()
        {
            string pattern = "^[1-4]$";
            bool correct = true;
            string input;
            do
            {
                Console.WriteLine("Please, choose the operator\n\n1) +\n2) -\n3) *\n4) /");
                input = Console.ReadLine();
                try
                {
                    correct = Regex.IsMatch(input, pattern);
                } catch(NullReferenceException ex) { 
                    Console.WriteLine("No Input!"); 
                }
                if (!correct) Console.WriteLine("Incorrect input. Please write only the listed numbers.");
            } while (!correct);
            int key = int.Parse(input);
            return key;
        }

        public static double AskForOperand()
        {
            string pattern = @"^-?\d+(\.\d+)?$";
            bool correct = true;
            string input;
            do
            {
                Console.WriteLine("\n\nPlease, enter the operand:");
                input = Console.ReadLine();
                try
                {
                    correct = Regex.IsMatch(input, pattern);
                }
                catch (NullReferenceException ex)
                {
                    Console.WriteLine("No Input!");
                }
                if (!correct) Console.WriteLine("Incorrect input. Please write only numbers.");
            } while(!correct);
            return double.Parse(input);
        }

        public static void Execute()
        {
            int operation = AskForOperator();
            double operand1 = AskForOperand();
            double operand2 = AskForOperand();
            double result = Operators.operators[operation].Invoke(operand1, operand2);
            Console.WriteLine($"\nResult: {result}");
        }

        public static void Main(string[] args)
        {
            Execute();
        }
    }
}
