using System;

class Program
{
    static string DisplayHeight(int heightCm)
    {
        if (heightCm < 150)
            return "Short";
        else if (heightCm < 180)
            return "Average";
        else
            return "Tall";
    }

    static void Main()
    {
        int heightCm = int.Parse(Console.ReadLine());
        Console.WriteLine(DisplayHeight(heightCm));
    }
}