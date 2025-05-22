// Program.cs
using System;

public class Program
{
    public static void Main(string[] args)
    {
        // Test constructors
        Fraction fraction1 = new Fraction();       // Should be 1/1
        Fraction fraction2 = new Fraction(6);      // Should be 6/1
        Fraction fraction3 = new Fraction(6, 7);   // Should be 6/7

        Console.WriteLine("Initial Fractions:");
        Console.WriteLine($"Fraction 1: {fraction1.TopNumber}/{fraction1.BottomNumber}");
        Console.WriteLine($"Fraction 2: {fraction2.TopNumber}/{fraction2.BottomNumber}");
        Console.WriteLine($"Fraction 3: {fraction3.TopNumber}/{fraction3.BottomNumber}");

        // Test getters and setters
        Console.WriteLine("\nTesting setters and getters:");
        Fraction testFraction = new Fraction(2, 3);
        Console.WriteLine($"Original testFraction: {testFraction.TopNumber}/{testFraction.BottomNumber}");

        testFraction.TopNumber = 5;
        testFraction.BottomNumber = 8;
        Console.WriteLine($"Modified testFraction: {testFraction.TopNumber}/{testFraction.BottomNumber}");

        // Test methods
        Console.WriteLine("\nTesting GetFractionString and GetDecimalValue:");

        Console.WriteLine($"\nFraction 1: {fraction1.GetFractionString()}");
        Console.WriteLine($"Decimal Value: {fraction1.GetDecimalValue()}");

        Console.WriteLine($"\nFraction 3 (6/7): {fraction3.GetFractionString()}");
        Console.WriteLine($"Decimal Value: {fraction3.GetDecimalValue()}"); // Should be 0.857... if it were 6/7
                                                                        // Oh, fraction 3 was 6/7, let's make a 3/4 for the common 0.75 example
        Fraction anotherFraction = new Fraction(3, 4);
        Console.WriteLine($"\nFraction (3/4): {anotherFraction.GetFractionString()}");
        Console.WriteLine($"Decimal Value: {anotherFraction.GetDecimalValue()}"); // Should be 0.75

        Console.WriteLine($"\nModified testFraction (5/8): {testFraction.GetFractionString()}");
        Console.WriteLine($"Decimal Value: {testFraction.GetDecimalValue()}"); // Should be 0.625


        // Test zero denominator in constructor
        Console.WriteLine("\nTesting zero denominator in constructor:");
        try
        {
            Fraction errorFraction = new Fraction(5, 0);
            Console.WriteLine($"Error fraction: {errorFraction.GetFractionString()}");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        // Test zero denominator in setter
        Console.WriteLine("\nTesting zero denominator in setter:");
        try
        {
            testFraction.BottomNumber = 0;
            Console.WriteLine($"Test fraction after bad set: {testFraction.GetFractionString()}");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        Console.WriteLine($"Test fraction final state: {testFraction.TopNumber}/{testFraction.BottomNumber}");


        Console.WriteLine("\nTests complete.");
    }
}