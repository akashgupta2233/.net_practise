using System;

class Program
{
    static int LargestInteger(int a, int b, int c)
    {
        int largest = a;

        if (b > largest)
            largest = b;

        if (c > largest)
            largest = c;

        return largest;
    }

    static void Main()
    {
        int a = int.Parse(Console.ReadLine());
        int b = int.Parse(Console.ReadLine());
        int c = int.Parse(Console.ReadLine());

        Console.WriteLine(LargestInteger(a, b, c));
    }
}