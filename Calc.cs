using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Calc
{
    public class Calculator
    {
        public decimal input(string equation)
        {
           try
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
                            decimal result = Decimal.Parse(input(innerExpression));
                            // Replace the parentheses with the result in the original equation
                            equation = equation.Replace($"({innerExpression})", result.ToString());
                        }
                    }
                    // Now process the equation without parentheses
                    return ProcessEquation(equation).ToString("G");
                }
                catch (Exception ex)
                {
                    return "syntx err";
                }    
        }

        private decimal ProcessEquation(string equation)
        {
            // Regex to match numbers (including negatives), operators, and handle parentheses
            var tokens = Regex.Matches(equation, @"-?\d+(\.\d+)?|[-+*/^()]")
                              .Cast<Match>()
                              .Select(m => m.Value)
                              .ToArray();

            List<string> processedEquation = new List<string>(tokens);

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
                    decimal left = decimal.Parse(processedEquation[i - 1]);
                    decimal right = decimal.Parse(processedEquation[i + 1]);
                    decimal result = calculator[current].Opp(left, right);

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
                    decimal left = decimal.Parse(processedEquation[i - 1]);
                    decimal right = decimal.Parse(processedEquation[i + 1]);
                    decimal result = calculator[current].Opp(left, right);

                    processedEquation[i - 1] = result.ToString();
                    processedEquation.RemoveAt(i);
                    processedEquation.RemoveAt(i);
                    i--;
                }
            }

            // Handle addition and subtraction
            decimal finalResult = decimal.Parse(processedEquation[0]);
            for (int i = 1; i < processedEquation.Count; i += 2)
            {
                string current = processedEquation[i];
                decimal right = decimal.Parse(processedEquation[i + 1]);
                finalResult = calculator[current].Opp(finalResult, right);
            }

            return finalResult;
        }


        protected virtual decimal Opp(decimal x, decimal y)
        {
            return 0.0m;
        }
    }

    internal class Add : Calculator
    {
        protected override decimal Opp(decimal x, decimal y)
        {
            return x + y;
        }
    }

    internal class Subtract : Calculator
    {
        protected override decimal Opp(decimal x, decimal y)
        {
            return x - y;
        }
    }

    internal class Multiply : Calculator
    {
        protected override decimal Opp(decimal x, decimal y)
        {
            return x * y;
        }
    }

    internal class Divide : Calculator
    {
        protected override decimal Opp(decimal x, decimal y)
        {
            return x / y;
        }
    }

    internal class Pow : Calculator
    {
        protected override decimal Opp(decimal x, decimal y)
        {   
            
            return (decimal)Math.Pow((double)x, (double)y);
        }
    }
}
