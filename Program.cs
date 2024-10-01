
using Calc;

class Program
{

    static void Main(String[] args)
    {
        Calculator calc = new Calculator();

        Console.WriteLine(calc.input("1/0"));
        Console.WriteLine(calc.input("2 + 2 * 2.5 / 2"));
        Console.WriteLine(calc.input("2 * 3 ^ 2"));
        Console.WriteLine(calc.input("(2 + 2) ^ 0 / 2"));
        
        
        

       
    }
}
