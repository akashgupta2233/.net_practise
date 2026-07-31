using System;

class Program
{
    // Method 1: Using ref
    static void SwapRef(ref int a, ref int b)
    {
        int temp = a;
        a = b;
        b = temp;
    }

    // Method 2: Using out
    static void SwapOut(int x, int y, out int a, out int b)
    {
        a = y;
        b = x;
    }

    static void Main()
    {
        int num1 = 10;
        int num2 = 20;

        Console.WriteLine("Before Swap (ref):");
        Console.WriteLine("num1 = " + num1 + ", num2 = " + num2);

        SwapRef(ref num1, ref num2);

        Console.WriteLine("After Swap (ref):");
        Console.WriteLine("num1 = " + num1 + ", num2 = " + num2);

        int a = 30;
        int b = 40;

        Console.WriteLine("\nBefore Swap (out):");
        Console.WriteLine("a = " + a + ", b = " + b);

        SwapOut(a, b, out a, out b);

        Console.WriteLine("After Swap (out):");
        Console.WriteLine("a = " + a + ", b = " + b);
    }
}