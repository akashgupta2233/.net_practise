using System;

class Program
{
    static int[] MultiplicationTable(int n, int upto)
    {
        int[] row = new int[upto];

        for (int i = 1; i <= upto; i++)
        {
            row[i - 1] = n * i;
        }

        return row;
    }

    static void Main()
    {
        int n = 3;
        int upto = 5;

        int[] result = MultiplicationTable(n, upto);

        Console.WriteLine("[" + string.Join(", ", result) + "]");
    }
}