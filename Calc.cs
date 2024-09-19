using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Calc
{
    public class Calculator
    {
        public double input(string equation)
        {
            // Handle parentheses recursively
            while (equation.Contains("("))
            {
                // Find innermost parentheses using regex
                Match match = Regex.Match(equation, @"\(([^()]+)\)");
                if (match.Success)
                {
                    // Get the expression inside the parentheses
                    string innerExpression = match.Groups[1].Value;
                    // Recursively evaluate the expression inside
                    double result = input(innerExpression);
                    // Replace the parentheses with the result in the original equation
                    equation = equation.Replace($"({innerExpression})", result.ToString());
                }
            }
            // Now process the equation without parentheses
            return ProcessEquation(equation);
        }

        private double ProcessEquation(string equation)
        {
            string[] expandEquation = equation.Split(' ');
            List<string> processedEquation = new List<string>(expandEquation);

            // Initialize calculator operations
            Dictionary<string, Calculator> calculator = new Dictionary<string, Calculator>
            {
                { "^", new Pow() },
                { "*", new Multiply() },
                { "/", new Divide() },
                { "+", new Add() },
                { "-", new Subtract() }
            };

            // Handle powers
            for (int i = 0; i < processedEquation.Count; i++)
            {
                string current = processedEquation[i];
                if (current == "^")
                {
                    double left = double.Parse(processedEquation[i - 1]);
                    double right = double.Parse(processedEquation[i + 1]);
                    double result = calculator[current].Opp(left, right);

                    processedEquation[i - 1] = result.ToString();
                    processedEquation.RemoveAt(i);
                    processedEquation.RemoveAt(i);
                    i--;
                }
            }

            // Handle multiplication and division
            for (int i = 0; i < processedEquation.Count; i++)
            {
                string current = processedEquation[i];
                if (current == "*" || current == "/")
                {
                    double left = double.Parse(processedEquation[i - 1]);
                    double right = double.Parse(processedEquation[i + 1]);
                    double result = calculator[current].Opp(left, right);

                    processedEquation[i - 1] = result.ToString();
                    processedEquation.RemoveAt(i);
                    processedEquation.RemoveAt(i);
                    i--;
                }
            }

            // Handle addition and subtraction
            double finalResult = double.Parse(processedEquation[0]);
            for (int i = 1; i < processedEquation.Count; i += 2)
            {
                string current = processedEquation[i];
                double right = double.Parse(processedEquation[i + 1]);
                finalResult = calculator[current].Opp(finalResult, right);
            }

            return finalResult;
        }

        protected virtual double Opp(double x, double y)
        {
            return 0.0;
        }
    }

    internal class Add : Calculator
    {
        protected override double Opp(double x, double y)
        {
            return x + y;
        }
    }

    internal class Subtract : Calculator
    {
        protected override double Opp(double x, double y)
        {
            return x - y;
        }
    }

    internal class Multiply : Calculator
    {
        protected override double Opp(double x, double y)
        {
            return x * y;
        }
    }

    internal class Divide : Calculator
    {
        protected override double Opp(double x, double y)
        {
            return x / y;
        }
    }

    internal class Pow : Calculator
    {
        protected override double Opp(double x, double y)
        {
            return Math.Pow(x, y);
        }
    }
}
