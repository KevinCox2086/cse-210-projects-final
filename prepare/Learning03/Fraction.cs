// Fraction.cs
using System;

public class Fraction
{
    // Attributes
    private int topNumber;
    private int bottomNumber;

    // Constructor - no parameters
    public Fraction()
    {
        this.topNumber = 1;
        this.bottomNumber = 1;
    }

    // Constructor - one parameter (for top)
    public Fraction(int top)
    {
        this.topNumber = top;
        this.bottomNumber = 1;
    }

    // Constructor - two parameters
    public Fraction(int top, int bottom)
    {
        if (bottom == 0)
        {
            // Denominator can't be zero
            throw new ArgumentException("Denominator cannot be zero.");
        }
        this.topNumber = top;
        this.bottomNumber = bottom;
    }

    // Getter and Setter for top number
    public int TopNumber
    {
        get { return this.topNumber; }
        set { this.topNumber = value; }
    }

    // Getter and Setter for bottom number
    public int BottomNumber
    {
        get { return this.bottomNumber; }
        set
        {
            if (value == 0)
            {
                // Denominator can't be zero
                throw new ArgumentException("Denominator cannot be zero.");
            }
            this.bottomNumber = value;
        }
    }

    // Method to get fraction as a string like "3/4"
    public string GetFractionString()
    {
        return $"{this.topNumber}/{this.bottomNumber}";
    }

    // Method to get the decimal value
    public double GetDecimalValue()
    {
        return (double)this.topNumber / this.bottomNumber;
    }
}