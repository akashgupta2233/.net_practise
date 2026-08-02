using System;
using System.Text;
using System.Collections.Generic;

public class Solution
{
    static bool IsVowel(char c)
    {
        c = char.ToLower(c);
        return c == 'a' || c == 'e' || c == 'i' || c == 'o' || c == 'u';
    }

    public static void Main(string[] args)
    {
        string firstWord = Console.ReadLine();
        string secondWord = Console.ReadLine();

        // Store characters of second word in lowercase for case-insensitive lookup
        HashSet<char> secondChars = new HashSet<char>();
        foreach (char ch in secondWord)
        {
            secondChars.Add(char.ToLower(ch));
        }

        // Task 1: Remove common consonants from first word
        StringBuilder filtered = new StringBuilder();

        foreach (char ch in firstWord)
        {
            char lower = char.ToLower(ch);

            if (!IsVowel(ch) && secondChars.Contains(lower))
            {
                continue; // remove common consonant
            }

            filtered.Append(ch);
        }

        // Task 2: Remove consecutive duplicate characters
        StringBuilder result = new StringBuilder();

        foreach (char ch in filtered.ToString())
        {
            if (result.Length == 0 || result[result.Length - 1] != ch)
            {
                result.Append(ch);
            }
        }

        Console.WriteLine(result.ToString());
    }
}