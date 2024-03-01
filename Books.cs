using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BackEnd_Project_1
{
    [Serializable]
    internal class Book
    {
        public string Title { get; }
        public string Author { get; }
        public string YearPublished { get; }

        public Book(string title, string author, string yearPublished)
        {
            Title = title;
            Author = author;
            YearPublished = yearPublished;
        }

        public string ToString()
        {
            return $"{Title} by {Author} : {YearPublished};";
        }
    }

    [Serializable]
    internal class BookManager
    {
        public static string CreateBooksFile()
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string filePath = Path.Combine(desktopPath, "Books.txt");

            try
            {
                if (!File.Exists(filePath))
                {
                    File.Create(filePath).Close();
                }
                return filePath;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating file: {ex.Message}");
                return null;
            }
        }


        public LinkedList<Book> Books { get; set; } 
        public static string basePath { get; } = CreateBooksFile();



        public void CheckForList()
        {
                try
                {
                    string list = File.ReadAllText(basePath);
                    if (string.IsNullOrEmpty(list))
                    {
                        Books = new LinkedList<Book>();
                    }
                    else
                    {
                    DeserializeList();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error reading file: {ex.Message}");
                }
        }


        public void SerializeList()
        {
            try
            {
                string list = JsonConvert.SerializeObject(Books);
                File.WriteAllText(basePath, list);
            } catch (Exception ex)
            {
                Console.WriteLine($"Error serializing account: {ex.Message}");
            }
        }

        public void DeserializeList()
        {
            try
            {
                string list = File.ReadAllText(basePath);
                Books = JsonConvert.DeserializeObject<LinkedList<Book>>(list);
            } catch (Exception ex)
            {
                Console.WriteLine($"Error deserializing account: {ex.Message}");
            }
        }

        public void AddBook(Book book)
        {
            Books.AddLast(book);
        }

        public void RemoveBook(Book book)
        {
            Books.Remove(book);
        }

        public void ShowAllBooks()
        {
            foreach (Book book in Books)
            {
                Console.WriteLine("\n");
                Console.WriteLine(book.ToString());
            }
        }

        public Book FindBook(string title)
        {
            
            return Books.FirstOrDefault(book => book.Title.Equals(title));
        }
    }

    internal class UserInterface
    {
        public void Start()
        {
            bool logIn = true;
            BookManager bookManager = new BookManager();
            bookManager.CheckForList();
            Console.WriteLine("Welcome!");
            while (logIn)
            {
                switch (AskForOptions())
                {
                    case "1":
                        AddBook(bookManager);
                        break;
                    case "2":
                        bookManager.ShowAllBooks();
                        break;
                    case "3":
                        FindBook(bookManager);
                        break;
                    case "4":
                        logIn = false;
                        break;
                }
            }
            bookManager.SerializeList();
        }

        public void AddBook(BookManager bookManager)
        {
            string title = null;
            string author = null;
            string year = null;
            Console.WriteLine("Please enter book title:");
            do
            {
                title = Console.ReadLine();
                if (title == "") Console.WriteLine("Invalid title!");
            } while (title == "");
            title = ConvertFirstLetterToUpper(title);
            Console.WriteLine("Please enter book author:");
            do
            {
                author = Console.ReadLine();
                if (author == "") Console.WriteLine("Invalid author!");
            } while (author == "");
            author = ConvertFirstLetterToUpper(author);
            Console.WriteLine("Please enter the year when the book was published:");
            string input = null;
            string pattern = @"^[0-9]+$";
            do
            {
                input = Console.ReadLine();
                if (Regex.IsMatch(input, pattern) == false) Console.WriteLine("Invalid input!");
            } while (Regex.IsMatch(input, pattern) == false);
            year = input;
            bookManager.AddBook(new Book(title, author, year));
            Console.WriteLine("The book was addded successfully");
        }

        public void FindBook(BookManager bookManager)
        {
            string title = null;
            Console.WriteLine("Please enter the title:");
            do
            {
                title = Console.ReadLine();
                if (title == "") Console.WriteLine("Invalid input!");
            } while (title == "");
            Book book = bookManager.FindBook(ConvertFirstLetterToUpper(title));
            if (book == null) Console.WriteLine("Could not find a book with such title.");
            else Console.WriteLine("\n" + book.ToString());
        }

        public string AskForOptions()
        {
            Console.WriteLine("\n\nPlease choose:\n\n1) Add a new book\n2) Show all books\n3) Find a book using title\n4) Log out");
            string pattern = @"^[1-4]$";
            string input = null;
            do
            {
                input = Console.ReadLine();
                if (Regex.IsMatch(input, pattern) == false) Console.WriteLine("Invalid input!");
            } while (Regex.IsMatch(input, pattern) == false);
            return input;
        }

        public static string ConvertFirstLetterToUpper(string input)
        {
            string[] words = input.ToLower().Split(new char[] { ' ', '-' }, StringSplitOptions.RemoveEmptyEntries);
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            for (int i = 0; i < words.Length; i++)
            {
                words[i] = textInfo.ToTitleCase(words[i]);
            }
            string result = string.Join(" ", words);
            return result;
        }
    }
}
