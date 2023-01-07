using System;

class Program
{
    static void Main(string[] args)
    {
        // get grade percentage form the user
        Console.WriteLine("What is your grade percentage?");
        string gradePercentStr = Console.ReadLine();

        //convert the string to a float
        float gradePercent = float.Parse(gradePercentStr);
        
        string letter = "";
        string passFail = "";

        if (gradePercent >= 90){
            letter = "A";
            passFail = "Pass";
        }

        else if (gradePercent >= 80){
           letter = ("B");
            passFail = ("Pass");
        }

        else if (gradePercent >= 70){
            letter = ("C");
            passFail = "Pass";
        }

        else if (gradePercent >= 60){
            letter = ("D");
            passFail = "Fail";
        }

        else if (gradePercent < 60){
            letter = ("F");
            passFail = ("Fail");
        }

        Console.WriteLine(letter);
        Console.WriteLine(passFail);


    }
}