using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        List<Shape> shapes = new List<Shape>();

        shapes.Add(new Square("Red", 4));
        shapes.Add(new Rectangle("Blue", 4, 5));
        shapes.Add(new Circle("Green", 3));
        shapes.Add(new Rectangle("Yellow", 6, 2.5));

        Console.WriteLine("Calculating Areas of Shapes:");
        Console.WriteLine("-----------------------------");

        foreach (Shape shape in shapes)
        {
            double area = shape.GetArea();
            string color = shape.GetColor();
            string type = shape.GetType().Name;

            Console.WriteLine($"Shape: {type}, Color: {color}, Area: {area:F2}");
        }
    }
}
