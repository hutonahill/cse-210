using System;

class Program
{
    static void Main(string[] args)
    {
        // collect the informatino from the user
        Console.WriteLine("Welcome to Number Guesser!");
        
        Boolean validInput = false;

        int maxGuess = 0;

        while (validInput == false){
            
            maxGuess = intInput(
                "What is the hightest number you would like to guess?");
            
            if (maxGuess > 0){
                validInput = true;
            }
            else{
                Console.WriteLine("You must input a number greater than 0. " +
                "Please try agean.");
            }
        }
        


        Random randomGenerator = new Random();
        int randomNumber = randomGenerator.Next(0, maxGuess);

        Console.WriteLine("Number Generated. ");

        Boolean found = false;

        while (found == false){
            int guess = intInput("Enter your guess: ");

            if (randomNumber > guess){
                Console.WriteLine("Higher.");
            }
            else if (randomNumber < guess){
                Console.WriteLine("Lower.");
            }
            else{
                Console.WriteLine("Correct! ");
                found = true;
            }

        }

        Console.WriteLine("END PROGRAM");
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
}