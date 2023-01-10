using System;

class Program
{
    static void Main(string[] args)
    {
        print("Welcome to list creator!");

        // Define the govening variable
        Boolean running = true;

        // create a list we will add to 
        List<int> numbers = new List<int>();

        // start the loop
        while (running == true){
            int number = intInput("Enter an intiger: ");

            if (number == 0){
                running = false;
            }

            numbers.Add(number);
        }

        int maxNum = numbers[0];
        int sum = 0;
        foreach (int number in numbers){

            // add the number to sum
            sum = sum + number;

            // check if number is greater than maxNum
            if (number > maxNum){
                maxNum = number;
            }
        }

        print($"Sum: {sum}");
        print($"Average: {sum/numbers.Count}");
        print($"Largest number: {maxNum}");
    }

    static int intInput(string prompt){
        Boolean validInput = false;
        int output = 1;

        while(validInput == false){
            Console.WriteLine(prompt);
            
            string rawInput = Console.ReadLine();

            int number1 = 0;
            bool canConvert = int.TryParse(rawInput, out number1);
            if (canConvert == true){
                output = Int32.Parse(rawInput);
                validInput = true;
            }
            else{
                Console.WriteLine("You must input an intiger. " + 
                "Please try agean.");
            }

        }
        
        return output;
    }

    static void print(string ourput){
        Console.WriteLine(ourput);
    }
}