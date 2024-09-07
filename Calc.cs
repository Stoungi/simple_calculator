using System;

namespace Calc
{
    public class Calculator
    {
        public double input(string equation)
        {
            // Initialize calculator operations
            Dictionary<string, Calculator> calculator = new Dictionary<string, Calculator>
            {
                { "^", new Pow() },
                { "*", new Multiply() },
                { "/", new Divide() },
                { "+", new Add() },
                { "-", new Subtract() }
            };
            
            string[] expandEquation = equation.Split(' ');
            List<string> processedEquation = new List<string>(expandEquation);

            // First pass: handle powers
            for (int i = 0; i < processedEquation.Count; i++)
            {
                string current = processedEquation[i];
                if (current == "^")
                {
                    double left = double.Parse(processedEquation[i - 1]);
                    double right = double.Parse(processedEquation[i + 1]);
                    double result = calculator[current].opp(left, right);

                    // Replace the left operand, operator, and right operand with the result
                    processedEquation[i - 1] = result.ToString();
                    processedEquation.RemoveAt(i);   // Remove the operator
                    processedEquation.RemoveAt(i);   // Remove the right operand
                    i--;  // Adjust index after removing items
                }
            }
            // Second pass: handle multiplication and division
            for (int i = 0; i < processedEquation.Count; i++)
            {
                string current = processedEquation[i];
                if (current == "*" || current == "/")
                {
                    double left = double.Parse(processedEquation[i - 1]);
                    double right = double.Parse(processedEquation[i + 1]);
                    double result = calculator[current].opp(left, right);

                    // Replace the left operand, operator, and right operand with the result
                    processedEquation[i - 1] = result.ToString();
                    processedEquation.RemoveAt(i);   // Remove the operator
                    processedEquation.RemoveAt(i);   // Remove the right operand
                    i--;  // Adjust index after removing items
                }
            }

            // Third pass: handle addition and subtraction
            double finalResult = double.Parse(processedEquation[0]);
            for (int i = 1; i < processedEquation.Count; i += 2)
            {
                string current = processedEquation[i];
                double right = double.Parse(processedEquation[i + 1]);
                finalResult = calculator[current].opp(finalResult, right);
            }

            return finalResult;
        }

        protected virtual double opp(double x, double y)
        {
            return 0.0;
        }

    }

    internal class Add : Calculator
    {
        protected override double opp(double x, double y)
        {
            return x + y;
        }

    }

    internal class Subtract : Calculator
    {
        protected override double opp(double x, double y)
        {
            return x - y;
        }

    }
    internal class Multiply : Calculator
    {
        protected override double opp(double x, double y)
        {
            return x * y;
        }

    }

    internal class Divide : Calculator
    {
        protected override double opp(double x, double y)
        {
            return x / y;
        }

    }
    internal class Pow : Calculator
    {
        protected override double opp(double x, double y)
        {
            return Math.Pow(x, y);
        }

    }

}
