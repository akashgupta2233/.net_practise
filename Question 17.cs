using System;
using System.Collections.Generic;
using System.Linq;

class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string Category { get; set; }

    public Book(int id, string title, string author, string category)
    {
        Id = id;
        Title = title;
        Author = author;
        Category = category;
    }
}

class Program
{
    static List<Book> books = new List<Book>();

    static void Main()
    {
        int choice;

        do
        {
            Console.WriteLine("\n===== BOOK LIBRARY MANAGEMENT =====");
            Console.WriteLine("1. Add Book");
            Console.WriteLine("2. View Books");
            Console.WriteLine("3. Update Book");
            Console.WriteLine("4. Delete Book");
            Console.WriteLine("5. Search Book");
            Console.WriteLine("6. Dynamic Demo");
            Console.WriteLine("0. Exit");
            Console.Write("Enter choice: ");

            choice = Convert.ToInt32(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    AddBook();
                    break;
                case 2:
                    ViewBooks();
                    break;
                case 3:
                    UpdateBook();
                    break;
                case 4:
                    DeleteBook();
                    break;
                case 5:
                    SearchBook();
                    break;
                case 6:
                    DynamicDemo();
                    break;
                case 0:
                    Console.WriteLine("Exiting...");
                    break;
                default:
                    Console.WriteLine("Invalid Choice");
                    break;
            }

        } while (choice != 0);
    }

    static void AddBook()
    {
        Console.Write("Book Id: ");
        int id = Convert.ToInt32(Console.ReadLine());

        Console.Write("Title: ");
        string title = Console.ReadLine();

        Console.Write("Author: ");
        string author = Console.ReadLine();

        Console.Write("Category: ");
        string category = Console.ReadLine();

        books.Add(new Book(id, title, author, category));

        Console.WriteLine("Book Added Successfully.");
    }

    static void ViewBooks()
    {
        if (books.Count == 0)
        {
            Console.WriteLine("No books available.");
            return;
        }

        foreach (var book in books)
        {
            Console.WriteLine($"{book.Id} | {book.Title} | {book.Author} | {book.Category}");
        }
    }

    static void UpdateBook()
    {
        Console.Write("Enter Book Id to Update: ");
        int id = Convert.ToInt32(Console.ReadLine());

        Book book = books.Find(b => b.Id == id);

        if (book != null)
        {
            Console.Write("New Title: ");
            book.Title = Console.ReadLine();

            Console.Write("New Author: ");
            book.Author = Console.ReadLine();

            Console.Write("New Category: ");
            book.Category = Console.ReadLine();

            Console.WriteLine("Book Updated Successfully.");
        }
        else
        {
            Console.WriteLine("Book Not Found.");
        }
    }

    static void DeleteBook()
    {
        Console.Write("Enter Book Id to Delete: ");
        int id = Convert.ToInt32(Console.ReadLine());

        Book book = books.Find(b => b.Id == id);

        if (book != null)
        {
            books.Remove(book);
            Console.WriteLine("Book Deleted Successfully.");
        }
        else
        {
            Console.WriteLine("Book Not Found.");
        }
    }

    static void SearchBook()
    {
        Console.Write("Enter Title to Search: ");
        string title = Console.ReadLine();

        var result = books.Where(b =>
            b.Title.Contains(title, StringComparison.OrdinalIgnoreCase));

        foreach (var book in result)
        {
            Console.WriteLine($"{book.Id} | {book.Title} | {book.Author} | {book.Category}");
        }
    }

    static void DynamicDemo()
    {
        dynamic bookInfo = new System.Dynamic.ExpandoObject();

        bookInfo.Title = "C# Programming";
        bookInfo.Author = "John";

        Console.WriteLine("\nDynamic Object Details:");
        Console.WriteLine("Title : " + bookInfo.Title);
        Console.WriteLine("Author: " + bookInfo.Author);
    }
}