using System;

class Program
{
    static void Main(string[] args)
    {

        // get first name
        Console.WriteLine("What is your first name? ");
        string firstName = Console.ReadLine();
        
        // get last name
        Console.WriteLine("What is your last name? ");
        string lastName = Console.ReadLine();

        // display result
        Console.WriteLine($"Your name is {lastName}, {firstName} {lastName}.");
    }
}