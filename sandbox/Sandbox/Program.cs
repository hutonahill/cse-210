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

    public static string Title = "Program.cs";

    private static Dictionary<string, functionClass> registeredFunctions 
    = new Dictionary<string, functionClass>();

    private static List<functionClass> functionObjects;

    static void Main(string[] args){


        print($"\n== Welcome to {Title} ==\n");

        running = true;

        functionObjects = new List<functionClass>{
            new Quit()
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
                help(functionObjects, flagInput);
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
    private static void help(List<functionClass> functions, List<string> flags){
        
        if ((functions.Count != 0) && (flags.Count == 0)){

            string output = "Commands:";

            for (int i = 0; i < functions.Count; i++){
                functionClass targetCommand = functions[i];
                string copyData = targetCommand.displayFlagInputs();

                output = output + ($"\n{i+1} {targetCommand.Title}" + 
                        $"\n    Valid Inputs: {targetCommand.displayCommandInputs()}" + 
                        $"\n    Discription: {targetCommand.Discription}" + 
                        $"\n    Function Flags: {copyData}" +
                        $"\n");
            }
            
            print(output);
        }
        else if (functions.Count != 0){
            if (flags.Count == 1){
                if (registeredFunctions.ContainsKey(flags[0]) == true){
                    
                    functionClass targetCommand = registeredFunctions[flags[0]];
                    
                    string copyData = targetCommand.displayFlagInputs();

                    print($"{registeredFunctions[flags[0]].Title}" + 
                        $"\n    Valid Inputs: {targetCommand.displayCommandInputs()}" + 
                        $"\n    Discription: {targetCommand.Discription}" + 
                        $"\n    Function Flags: {copyData}" +
                        $"\n");
                }
                else{
                    print($"The flag {flags[0]} is not a valid command.");
                }
                
            }
            else{
                print($"help only accepts one flag, you input {flags.Count}.  Please try agean.");
            }
        }

        else{
            print("No registered commands");
        }
    }


    // inputs and outputs
    
    public static void print(string msg){
        Console.WriteLine(msg);
    }

    public static void print(){
        Console.WriteLine();
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

class ScriptureJson {
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

//structure classes

/// <summary>
/// Abstract Class <c>functionClass</c> the structure of functions.
/// </summary>
public abstract class functionClass{

    /// <summary>
    /// String <c>Title</c> The tile of the function, displayed when the user runs
    /// the help command
    /// </summary>
    public string Title{
        get;
        protected set;
    }

    /// <summary>
    /// String <c>Discription</c> A discription of what the fuction does.
    /// </summary>
    public string Discription{
        get;
        protected set;
    }

    /// <summary>
    /// List<String> <c>CommandInputs</c> a list of strings that the system will
    /// recignize as calling this function.
    /// </summary>
    public List<string> CommandInputs {
        get;
        protected set;
    }

    /// <summary>
    /// List<flagClass> <c>FlagObjects</c> a list of flags intigrated with 
    /// this function.
    /// </summary>
    public List<flagClass> FlagObjects {
        get;
        protected set;
    }

    /// <summary>
    /// Dictionary<string, flagClass> <c>FlagRegistry</c> a dict of flags with
    /// there inputs (found in flagClass.Flags) as the key
    /// </summary>
    public Dictionary<string, flagClass> FlagRegistry{
        get;
        protected set;
    }

    /// <summary>
    /// String <c>displayCommandInputs</c> compiles a list of commands from 
    /// CommandInputs and returns a string for the help function
    /// </summary>
    public string displayCommandInputs(){
        
        string output = "";

        // format each command
        for (int i = 0; i< CommandInputs.Count; i++){
            output = output + $"'{CommandInputs[i]}', ";;
        }
        
        // remove the ',' from the end of the string
        output = output.Remove(output.Length - 1, 1);

        return output;
    }

    /// <summary>
    /// String <c>displayFlagInputs</c> compiles a list of flags and there
    /// registerd commands (found in flagClass.Flags)
    /// </summary>
    public string displayFlagInputs(){

        // convert an int into a roman numeral (sorce: https://stackoverflow.com/a/11749642/13091622)
        string ToRoman(int number){
            if ((number < 0) || (number > 3999)) 
            throw new ArgumentOutOfRangeException("insert value betwheen 1 and 3999");
            if (number < 1) return string.Empty;            
            if (number >= 1000) return "m" + ToRoman(number - 1000);
            if (number >= 900) return "cm" + ToRoman(number - 900); 
            if (number >= 500) return "d" + ToRoman(number - 500);
            if (number >= 400) return "cd" + ToRoman(number - 400);
            if (number >= 100) return "c" + ToRoman(number - 100);            
            if (number >= 90) return "xc" + ToRoman(number - 90);
            if (number >= 50) return "l" + ToRoman(number - 50);
            if (number >= 40) return "xl" + ToRoman(number - 40);
            if (number >= 10) return "x" + ToRoman(number - 10);
            if (number >= 9) return "ix" + ToRoman(number - 9);
            if (number >= 5) return "v" + ToRoman(number - 5);
            if (number >= 4) return "iv" + ToRoman(number - 4);
            if (number >= 1) return "i" + ToRoman(number - 1);
            throw new Exception("Impossible state reached");
        }

        string output = "";


        if (FlagObjects.Count != 0){

            // loop through each object
            for (int i = 0; i < FlagObjects.Count; i++){
                flagClass targetObject = FlagObjects[i];

                

                // two tabs deep
                output = output + (
                    $"\n          {ToRoman(i)} {targetObject.Title}:"
                );

                // three tabs deep
                output = output + (
                    $"\n              Discription: {targetObject.Discription}"
                );

                // loop thorugh each command in the flag dictionary.
                if (targetObject.Flags.Count != 1){
                    output = output + ($"\n              Flags: ");
                    for (int j = 0; j < targetObject.Flags.Count; j++){
                        
                        output = output + $"'{targetObject.Flags[j]}', ";
                        
                    }
                    // remove the ',' from the end of the string
                    output = output.Remove(output.Length - 1, 1);

                    if (targetObject.Paramiters != null){
                        output = output + $"\n              Peramiters: ";
                        output = output + $"{targetObject.Paramiters}";
                    }
                    
                    

                }
                else{
                    throw new Exception("Flag Classes must have atleast one flag");
                }
            }
        }
        else{
            output = "No Supported Flags.";
        }

        return output;
    }

    /// <summary>
    /// void <c>constructFlagRegistry</c> constructs FlagRegistry using 
    /// the flag objects in FlagObjects. MUST be run in the constructer.
    /// </summary>
    protected void constructFlagRegistry(){
        // if there are flags register.

        FlagRegistry = new Dictionary<string, flagClass>();
        
        if (FlagObjects.Count > 0){
            //register flags

            //list of flags
            List<string> validFlags = new List<string>();

            // loop through all flag objects
            for (int i = 0; i < FlagObjects.Count; i++){
                flagClass targetFlagObject = FlagObjects[i];

                // loop through each flag
                for (int j = 0; j < targetFlagObject.Flags.Count; j++){
                    
                    string targetFlag = targetFlagObject.Flags[j];

                    int charCheck = targetFlag.IndexOfAny(Program.bannedChars);

                    // Check that there are no duplicate flags
                    if (validFlags.Contains(targetFlag) == true){
                        Program.print($"ERROR: funtion '{Title}' contains duplice flag " + 
                        $"in flag '{targetFlag}'.");
                        
                        Program.running = false;
                        
                    }
                    
                    // Check for banned chars
                    else if (charCheck != -1){
                        Program.print($"ERROR: flag '{targetFlag}' for function " + 
                        $"'{Title}' contains banned char " +
                        $"'{Program.bannedChars[charCheck]}'.");
                        
                        Program.running = false;
                    }

                    else{
                        // add flag and corisponding object to the registery
                        FlagRegistry[targetFlag] = targetFlagObject;

                        // add flag to list of valid flags
                        validFlags.Add(targetFlag);
                    }
                }
            }
        }

        // make sure there is at least one command input
        if (CommandInputs == null){
            Program.print($"ERROR: funciton '{Title}' has no command Inputs");
            Program.running = false;
        }
        else if(CommandInputs.Count == 0){
            Program.print($"ERROR: funciton '{Title}' has no command Inputs");
            Program.running = false;
        }
    }

    /// <summary>
    /// void <c>run</c> if there are any flags run figures out which one and 
    /// runs it. if not it runs runNoFlag()
    /// </summary>
    /// <param name="flags"> a list of flags the user input.</peram>
    public void run(List<string> flags){
        
        if (flags.Count == 0){
            runNoFlag();
        }
        else if(flags.Count == 1){
            if (FlagRegistry != null){

                string flag = flags[0];

                string peram = "";

                
                // if the flag contains a ( assume the user is passing a paramiter and extract it.
                peram = ExtractParameter(flag);
                
                if (peram != null){
                    flag = flag.Replace(("(" + peram + ")"), "");
                    try{
                        FlagRegistry[flag].run(peram);
                    }
                    catch(KeyNotFoundException){
                        Program.print($"flag '{flag}' is not registered in " + 
                        $"function '{Title}'.  Please try again");
                    }
                }
                
            }
            else {
                Program.print("No flags registered for this command.");
            }
            
        }
        else{
            Program.print("Only one flag is supported at this time.  Please try again.");
        }
        
    }

    /// <summary>
    /// Extracts parameter from a string.
    /// </summary>
    /// <param name="input">A string</param>
    /// <returns>A parameter (words surrounded with parentheses)</returns>
    /// <author>This code snippet was written with the help of ChatGPT.</author>
    private static string ExtractParameter(string input){
        input = input + " ";
        int openParentheses = 0;
        int closeParentheses = 0;
        string result = "";

        // loop through every character in the input
        for (int i = 0; i < input.Length; i++){
            
            char currentChar = input[i];

            // if the current char is '('
            if (currentChar == '('){

                // If a nested parenthesis is detected,
                // inform the user and return null
                if (openParentheses > 0){
                    Program.print(
                        "The command you input includes nested opening parentheses. " +
                        "Please try again."
                    );
                    return null;
                }

                // add one to the count of '('.
                openParentheses++;
            }

            // if the current char is ')'
            else if (currentChar == ')'){

                // If a nested ')' is detected,
                // inform the user and return null
                if (closeParentheses > 0){
                    Program.print(
                        "The command you input includes nested closing parentheses.  " +
                        "Please try again."
                    );
                    return null;
                }
                // if not, add the detected ')' to the count
                else{
                    closeParentheses++;

                    // make sure tehre arnt multiple perams
                    if (closeParentheses == 2){
                        Program.print(
                            "the command you input includes two peramiters attached to one flag.  " + 
                            "Please try again."
                        );
                        return null;
                    }
                }

                // if the number of '(' and ')' are the same, add what
                // between them to the output string.
                if (closeParentheses == openParentheses){
                    
                    // add one to the starting index to not include the '('
                    int startIndex = input.LastIndexOf('(', i - 1) + 1;

                    // subtract 1 form the length so the ')' is not included
                    int length = i - startIndex;

                    // extract the enclosed string
                    result = input.Substring(startIndex, length);
                }

                // if there are more ')' than '(' inform the user and return null
                else if (closeParentheses > openParentheses){
                    Program.print(
                        "The command you input has a closing parentheses " +
                        "that comes before an opening parentheses. " +
                        "Please try again."
                    );
                    return null;
                }
            }
        }

        // If there are any open or close parentheses remaining, inform the user and return null
        if (openParentheses > 1 || closeParentheses > 1){
            Program.print(
                "The command you input has an unclosed parentheses. " +
                "Please try again."
            );
            return null;
        }

        // Return the extracted parameter
        return result;
    }

    /// <summary>
    /// void <c>runNoFlag</c> the default code that runs when there are no flags.
    /// </summary>
    protected abstract void runNoFlag();

}

/// <summary>
/// Abstract Class <c>flagClass</c> the structure of flags for functions.
/// </summary>
public abstract class flagClass{

    /// <summary>
    /// String <c>Title</c> The tile of the flag, displayed when the user runs
    /// the help command
    /// </summary>
    public string Title{
        get;
        protected set;
    }

    /// <summary>
    /// String <c>Discription</c> A discription of what the flag does.
    /// </summary>
    public string Discription{
        get;
        protected set;
    }

    /// <summary>
    /// List<String> <c>Flags</c> A list of strings that the system recignises
    /// as this flag.
    /// </summary>
    public List<string> Flags {
        get;
        protected set;
    }

    /// <summary>
    /// String <c>Paramiter</c> A discription of the peramiter of the flag
    /// if it has one.
    /// </summary>
    public string Paramiters{
        get;
        protected set;
    }

    /// <summary>
    /// void <c>run</c> Code that runs when the the flag is triggered.
    /// if it has one.
    /// <param name="peramiter"> the flags peramiter.  If flag has no peramiter input "".</peram>
    /// </summary>
    public abstract void run(string peramiter);

}


// functions

/// <summary>
/// Function Class <c>quit</c> stops the program.
/// </summary>
class Quit:functionClass{

    public Quit(){
        Title = "Quit";

        Discription = "Quits the program";

        CommandInputs = new List<string>{
            "quit", "exit", "stop"
        };

        FlagObjects = new List<flagClass>{
            new testFlag()
        };

        FlagRegistry = new Dictionary<string, flagClass>();

        constructFlagRegistry();
    }

    protected override void runNoFlag(){
        Program.running = false;
    }
}


class testFlag:flagClass{

    public testFlag(){
        Title = "Testing Flag";
        Discription = "Makes sure the flag system is working properly";
        Flags = new List<string>{
            "-t", "test", "t"
        };
        Paramiters = "string peram - sting to output";
    }
    

    public override void run(string peramiter){
        if (peramiter == ""){
            Program.print($"{Title} requires a peramiter");
        }
        else{
            Program.print(peramiter);
        }
    }
}

