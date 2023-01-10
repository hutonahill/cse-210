using System;

class Program
{
    static void Main(string[] args)
    {
        DisplayWelcome();

        DisplayResult(PromptUserName(), SquareNumber(PromptUserNumber()));
    }

    static void DisplayWelcome(){
        string ouput = ($"Welcome to the program!");

        Console.WriteLine(ouput);
    }

    static string PromptUserName(){
        print("what is your name? ");
        return Console.ReadLine();
    }

    static int PromptUserNumber(){
        Boolean validInput = false;
        int output = 1;

        while(validInput == false){
            Console.WriteLine("Please enter your favorite number: ");
            
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

    static float SquareNumber(int num){
        return num * num;
    }
    
    static void DisplayResult(string name, float squared){
        print($"{name}, the square of your number is {squared}");
    }

    static void print(string ourput){
        Console.WriteLine(ourput);
    }

    

    
}
