using System;
using System.Text.Json;


// === PROGRAM FRAMEWORK === 
// the below is a framework to run funcitons with little modification to 
// the program class. You should primarily only have to add function classes,
// flag classes, and add the function classes to the functionObjects list
// on line 27

/// <summary>
/// a framework that gets commands from users. To add a command create a 
/// functionClass and add a new object to the function object list
/// This code snippet was written with the help of ChatGPT.
/// </summary>
public static class Program{
    public static bool running;
    
    public static char[] bannedChars =" &%$#@*(){}[]'=+_".ToCharArray();

    public static string Title = "Eternal Quest.cs";

    private static Dictionary<string, functionClass> registeredFunctions 
    = new Dictionary<string, functionClass>();

    private static List<functionClass> functionObjects;

    static void Main(string[] args){


        print($"\n== Welcome to {Title} ==\n");

        running = true;

        functionObjects = new List<functionClass>{
            new QuitFunction(), new newGoal(), new compliteGoal(), new diplayGoals(),
            new getPoints()
        };
        
        // set up the command framework
        if (running == true){
            constructFramework();
        }
        
        //define some variables so compiler wont give us errors about undefined functions.
        string commandInput = "";
        List<string> flagInput = new List<string>();

        // keep the program running until the user tells it to stop.   
        while (running == true){
            // get input from the user to determin what to do.
            string rawInput = input($"{Title}> ");

            List<string> copyData = ExtractCommand(rawInput);

            commandInput = copyData[0];

            flagInput = ExtractFlagsAndParameters(copyData[1]);

            if (flagInput == null){
                // do nothing, ExtractFlagsAndParameters handels informing 
                // the user of errors.
            }
            // if the user asked for help and the program is still running
            // run the help function
            else if (commandInput == "help" ){
                help(functionObjects, copyData[1]);
            }

            // if the program is still running run the input command
            else{
                try{
                    registeredFunctions[commandInput].run(flagInput);
                }

                // if the command is not a key in the dictionary, inform the 
                // the user that the command they input is invalid
                catch (KeyNotFoundException){
                    print("Invalid command.  Please try agran or run 'help' to " + 
                    "a list of valid commands");
                }
            }
            
            print("");

        }
        
        //tell the user the program has stoped.
        print($"Stoped.  \n");

    }

    /// <summary>
    /// extract the command from a string.
    /// This code snippet was written with the help of ChatGPT.
    /// </summary>
    /// <param name="input">a string</param>
    /// <returns>a list where the first index is the command and the second is the rest of the string</returns>
    /// <author>This code snippet was written with the help of ChatGPT.</author>
    private static List<string> ExtractCommand(string input){
        List<string> output = new List<string>();
        int firstSpace = input.IndexOf(' ');
        if (firstSpace == -1){
            // There is only one word in the input
            output.Add(input);
            output.Add("");
        }
        else{
            output.Add(input.Substring(0, firstSpace));
            output.Add(input.Substring(firstSpace + 1));
        }
        return output;
    }
    
    /// <summary>
    /// Extracts a flag and parameter as a single string from a given input string.
    /// A flag is defined as a word that starts with the '-' character. 
    /// The parameter is at the end of the flag and is surrounded by parentheses. 
    /// </summary>
    /// <param name="input">The input string to extract flags and parameters from</param>
    /// <returns>A List of strings containing the flags and their corresponding parameters (if any)</returns>
    /// <author>This code snippet was written with the help of ChatGPT.</author>
    private static List<string> ExtractFlagsAndParameters(string input){
        List<string> flags = new List<string>();
        int index = 0;

        bool running = true;

        // Continue looping until all flags are found
        while ((index < input.Length) && (running == true)){
            
            // Find the next flag
            int flagStart = input.IndexOf('-', index);
            if (flagStart == -1){
                // No more flags found, exit the loop

                running = false;
                return flags;
            }

            else{
                // Check if the flag is inside parentheses
                bool insideParentheses = false;
                for (int i = flagStart; i >= 0; i--){
                    if (input[i] == '('){
                        insideParentheses = true;
                        i = -1;
                    }
                    else if (input[i] == ')'){
                        insideParentheses = false;
                        i = -1;
                    }
                }

                if (insideParentheses == false){

                    // Find the end of the flag

                    // the end of the flag can ether be a ' ' or 
                    // a ')'
                    int nextOpen = input.IndexOf('(', flagStart);
                    int nextClose = input.IndexOf(')', flagStart);
                    int nextSpace = input.IndexOf(' ', flagStart);

                    int flagEnd;
                    
                    // if the next '(' is closer than the next ' ',
                    // then the flag is ended with a ')'
                    if (nextOpen == -1){
                        nextOpen = input.Length;
                    }

                    if (nextSpace == -1){
                        nextSpace = input.Length;
                    }

                    // the only way next open and nextspace can be equal 
                    // is if they are both set the the lenght of input.
                    if (nextOpen  == nextSpace){
                        flagEnd = input.Length - 1;
                    }

                    else if (nextOpen < nextSpace){
                        flagEnd = nextClose;
                    }
                    
                    // if not its ended with a space.
                    else{
                        flagEnd = nextSpace;
                    }
                    

                    if ((flagEnd == -1)){
                        
                        // No ')' found, exit the loop
                        print(
                            "The command you input contains an unclosed parentheses.  " + 
                            "Please try again."
                        );
                        running = true;
                        return null;
                    }

                    else{
                        // Check for nested parentheses
                        int nestedStart = input.IndexOf('(', flagStart + 1);

                        while (nestedStart != -1 && nestedStart < flagEnd){
                            int nestedEnd = input.IndexOf(')', nestedStart);
                            if (nestedEnd == -1){
                                // No closing parenthesis found, exit the loop
                                return null;
                            }
                            nestedStart = input.IndexOf('(', nestedStart + 1);
                        }

                        // Add the flag with parameter to the list
                        flags.Add(input.Substring(flagStart, flagEnd - flagStart + 1));
                        index = flagEnd + 1;
                    }
                }
                else{
                    // Flag is inside parentheses, skip it
                    index = flagStart + 1;
                }
            }

        } // while index < input.Length) && (running == true)

        // Return the list of flags
        return flags;
    }

    /// <summary>
    /// construct a registry of fucntions that the framework can run
    /// </summary>
    /// <returns>a dictionary containing command and the funcitons they run</returns>
    private static void constructFramework(){
        // create a list of commands
        // "help" is preloaded into this to account for a help function
        List<string> validCommands = new List<string>{"help"};

        

        // create a dictionary that stores function objects

        // loop though every function object
        for (int i = 0; i < functionObjects.Count; i++){
            List<string> targetCommandList = functionObjects[i].CommandInputs;
            

            // for every command in the fucntion add a key to the 
            // dictionary that points to that fucntion.

            for (int j = 0; j < targetCommandList.Count; j++){

                // make sure there are no duplicate commands
                if ((validCommands.Contains(targetCommandList[j]) == false) 
                && (running == true)){
                    
                    //check to make sure commands dont contains spaces.
                    int checkValue = targetCommandList[j].IndexOfAny(bannedChars);
                    if (checkValue == -1){
                        
                        // if the command is not a duplicate add the command 
                        // and corisponding function to the dictionary
                        registeredFunctions.Add(targetCommandList[j], 
                        functionObjects[i]);

                        // add the command to the list of valid commands
                        validCommands.Add(targetCommandList[j]);
                    }
                    else{
                        
                        // inform the user which commands contain banned charicters
                        // so the probem can be fixed.
                        print($"The command '{targetCommandList[j]}' " + 
                        $"of function '{functionObjects[i].Title}' contains a " + 
                        "banned charicter.");
                    }
                    
                }

                // if there is duplicate warn the user and stop the program.
                else if (running == true){
                    print(
                        $"Duplicate command '{targetCommandList[j]}' in " + 
                        $"registered fucntion '{functionObjects[i].Title}'."
                    );
                    running = false;
                }
            }
        }
    }

    /// <summary>
    /// a command that displays a list of funcitons, the commands that run them
    /// and any flags or peramiters
    /// </summary>
    /// <param name="functions">a list of funciton objects</param>
    /// <param name="flags"> a list of flags the user input.</peram>
    private static void help(List<functionClass> functions, string flags){
        
        if ((functions.Count != 0) && (flags == "")){

            printf("Commands:");

            for (int i = 0; i < functions.Count; i++){
                functionClass targetCommand = functions[i];
                
                List<(string, int)> flagOutput = targetCommand.displayFlagInputs();

                    printf($"{targetCommand.Title}", 1);
                    printf($"Valid Inputs: {targetCommand.displayCommandInputs()}", 2); 
                    printf($"Discription: {targetCommand.Discription}", 2);
                    printf($"Function Flags:", 2);
                    for (int j = 0; j < flagOutput.Count; j++){
                        printf(flagOutput[j].Item1, (flagOutput[j].Item2));
                    }
                    print();
            }
    
        }
        else if ((functions.Count != 0) && (flags != "")){
            
            if (registeredFunctions.ContainsKey(flags) == true){
                
                functionClass targetCommand = registeredFunctions[flags];
                
                List<(string, int)> flagOutput = targetCommand.displayFlagInputs();

                printf($"{targetCommand.Title}");
                printf($"Valid Inputs: {targetCommand.displayCommandInputs()}", 1); 
                printf($"Discription: {targetCommand.Discription}", 1);
                printf($"Function Flags:", 1);
                for (int j = 0; j < flagOutput.Count; j++){
                    printf(flagOutput[j].Item1, flagOutput[j].Item2);
                }
                print();
            }
            else{
                print($"The flag {flags} is not a valid command.");
            }
        }

        else{
            print("No registered commands");
        }
    }

    public static void printf(string text, int tabLevel = 0, int extraTab = 1, int tabWidth = 4){
        // Get the width of the console window in characters.
        int consoleWidth = Console.WindowWidth - 7;

        string gap = "";
        for (int i = 0; i < tabLevel; i++){
            gap = gap + new string(' ', tabWidth);
            
        }
        
        if ((gap.Length + 10) > consoleWidth){
            print(text);
        }
        else if ((text.Length + gap.Length) > consoleWidth){
            
            // Split the input text into an array of words.
            string[] words = text.Split(' ');

            // Initialize the current line length and indentation level to 0.
            int currentLineLength = 0;

            printNoLine(gap);
            currentLineLength = gap.Length;
            for (int i = 0; i < extraTab; i++){
                gap = gap + new string(' ', tabWidth);
                
            }

            // Iterate over each word in the array.
            foreach (string word in words){
                // Check if adding the word to the current line would exceed the console window width.
                if (currentLineLength + word.Length + 1 > consoleWidth){
                    // If it does, start a new line and 
                    print();

                    // indent the new line by the current indentation level.
                    
                    printNoLine(gap);

                    // Reset the current line length to 0.
                    currentLineLength = gap.Length;
                }

                // Write the word to the console with a trailing space character.
                printNoLine(word + " ");

                // Update the current line length to include the length of the word and the space character.
                currentLineLength += word.Length + 1;

            }
            print();
        }
        else{
            // If the text fits within the console window, print it as is.
            print($"{gap}{text}");
        }
    }


    // inputs and outputs
    
    public static void print(string msg){
        Console.WriteLine(msg);
    }

    public static void print(){
        Console.WriteLine();
    }

    public static void printNoLine(string msg){
        Console.Write(msg);
    }

    public static string input(string prompt){
        Console.Write(prompt);
        return Console.ReadLine();
    }

    public static string input(){
        return Console.ReadLine();
        
    }


}

// Data Types

class DataJson {
    public Dictionary<string, string> dict { get; set; }
}

class myError{
    private bool isError = false;

    private string errorMsg = "";

    public myError(){

    }

    // funcitons
    public void raiseError(string msg){
        isError = true;

        errorMsg = msg;
    }
    
    public bool checkError(){
        return isError;
    }

    public string readMsg(){
        return errorMsg;
    }
}

// Functions

