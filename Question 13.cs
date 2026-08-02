using System;

class Program
{
    static double FeetToCentimeters(int feet)
    {
        return Math.Round(feet * 30.48, 2, MidpointRounding.AwayFromZero);
    }

    static void Main()
    {
        int feet = int.Parse(Console.ReadLine());
        Console.WriteLine(FeetToCentimeters(feet));
    }
}