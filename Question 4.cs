using System;
using System.Linq;
using System.Text.Json;
using System.Collections.Generic;

public record Student(string Name, int Score);

public class Solution
{
    public string GetStudentsJson(string[] items, int minScore)
    {
        List<Student> students = new List<Student>();

        foreach (string item in items)
        {
            string[] parts = item.Split(':');

            students.Add(new Student(
                parts[0],
                int.Parse(parts[1])
            ));
        }

        var result = students
            .Where(s => s.Score >= minScore)
            .OrderByDescending(s => s.Score)
            .ThenBy(s => s.Name)
            .ToList();

        return JsonSerializer.Serialize(result);
    }
}