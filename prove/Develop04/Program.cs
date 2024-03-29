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

    public static string Title = "Mindfulness.cs";

    private static Dictionary<string, functionClass> registeredFunctions 
    = new Dictionary<string, functionClass>();

    private static List<functionClass> functionObjects;

    static void Main(string[] args){


        print($"\n== Welcome to {Title} ==\n");

        running = true;

        functionObjects = new List<functionClass>{
            new Quit(), new BreathingFunction(), new ReflectionFunction(),
            new ListingFunction()
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
            

            List<string> copyData = new List<string>(ExtractCommand(rawInput));

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

    /// <summary>
    /// Prints the input text to the console, wrapping it to fit within the 
    /// console window width without cutting words, and optionally indents new lines by the specified number of tabs.
    /// </summary>
    /// <param name="text">The text to print.</param>
    /// <param name="tabLevel">The number of tabs to use for indenting new lines (default 0).</param>
    /// <param name="tabWidth">The width of each tab character in spaces (default 4).</param>
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

    public static void delLine(){
        // Move the cursor to the beginning of the last line
        Console.SetCursorPosition(0, Console.CursorTop);

        // Clear the last line
        Console.Write(new string(' ', Console.WindowWidth));

        // Move the cursor back to the beginning of the line
        Console.SetCursorPosition(0, Console.CursorTop);
    }

    public static void clearTerminal(){
        Console.Clear();
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
        output = output.Remove(output.Length - 2, 2);

        return output;
    }

    /// <summary>
    /// String <c>displayFlagInputs</c> compiles a list of flags and there
    /// registerd commands (found in flagClass.Flags)
    /// </summary>
    public List<(string, int)> displayFlagInputs(){

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

        List<(string,int)> output = new List<(string, int)>();


        if (FlagObjects.Count != 0){

            // loop through each object
            for (int i = 0; i < FlagObjects.Count; i++){
                flagClass targetObject = FlagObjects[i];

                string copyData = "";

                // construct roman numeral and gap
                string numeral = ToRoman(i+1);
                int extraChar = numeral.Length - 1;

                //two tabs deep
                output.Add(($"{numeral} {targetObject.Title}", 2));

                

                // three tabs deep
                output.Add(
                    ($"Discription: {targetObject.Discription}", 3)
                );

                // loop thorugh each command in the flag dictionary.
                if (targetObject.Flags.Count != 1){
                    
                    // threee tabs deep
                    copyData = $"Flags: ";
                    for (int j = 0; j < targetObject.Flags.Count; j++){
                        
                        copyData = copyData + $"'{targetObject.Flags[j]}', ";
                        
                    }
                    // remove the ',' from the end of the string
                    copyData = copyData.Remove(copyData.Length - 2, 2);

                    // add the string to output. Note its 3 tabs deep.
                    output.Add((copyData, 3));

                    if (targetObject.Paramiters != null){
                        copyData = $"Peramiters: ";
                        copyData = copyData + $"{targetObject.Paramiters}";

                        output.Add((copyData, 3));
                    }
                    
                    

                }
                else{
                    throw new Exception("Flag Classes must have atleast one flag");
                }
            }
        }
        else{
            output =new List<(string, int)>{
                ("        No Supported Flags.", 2)
            };
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

                    //make sure the flag stats with -
                    else if (targetFlag[0] != '-'){
                        Program.print($"ERROR: flag '{targetFlag}' for function " + 
                        $"'{Title}' does not start with the '-' char.");

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
        };

        FlagRegistry = new Dictionary<string, flagClass>();

        constructFlagRegistry();
    }

    protected override void runNoFlag(){
        Program.running = false;
    }
}


// activity structure
abstract class activityClass{
    protected string IntroMsg {
        get;
        set;
    }

    private string EndMsg {
        get;
        set;
    } = $"Good Job!";

    protected int minForActivity {
        get;
        set;
    }

    protected string activetyDiscription {
        get;
        set;
    }

    protected string activetyTitle{
        get;
        set;
    } = "";

    protected abstract int activety();

    public void runActvity(int min = -1){
        Program.print(IntroMsg);

        spin(1);
        
        Program.print(activetyTitle);
        Program.print(activetyDiscription);
        if (min == -1){
            minForActivity = IntInput(
                "How many minates would you like to spend on this activity? "
            );
        }
        else{
            minForActivity = min;
        }
        
        Program.print("Prepare to begain.");
        spin(1);
        
        Program.clearTerminal();

        int timeMS = activety();
        double timeM = (double) (timeMS / 1000)/60;
        timeM = Math.Round(timeM, 2);

        Program.print(EndMsg);
        spin(1);
        Program.print($"The actvity lasted {timeM} minute(s).");
        spin(2);
    }

    /// <summary>
    /// Spins a console spinner for a given number of seconds with a configurable pause time.
    /// </summary>
    /// <param name="seconds">The number of seconds to spin the console spinner.</param>
    /// <param name="pauseTime">The pause time between each spinner sequence. Defaults to 100 milliseconds if not provided.</param>
    protected void spin(int seconds, int pauseTime = 100){
        List<string> sequance = new List<string>{
            "\\", "|", "/", "-"
        };

        

        int miliSecs = seconds * 1000;

        int timeCount = 0;
        int charCounter = 1;

        while (timeCount <miliSecs){
            // print the char
            Program.printNoLine(sequance[charCounter % sequance.Count]);
            charCounter ++;

            System.Threading.Thread.Sleep(pauseTime);
            timeCount = timeCount + pauseTime;

            // remove the old char
            Program.printNoLine($"\b \b \b");
        }
        
    }

    /// <summary>
    /// Prompts the user to enter an integer and loops until a valid integer is entered.
    /// </summary>
    /// <param name="prompt"> a sring that will be displied to the user.</peram>
    /// <returns>The integer entered by the user.</returns>
    private int IntInput(string prompt){
        bool validInput = false;
        int output = 0;
        while (validInput == false){
            string input = Program.input(prompt);

            int result;
            if (int.TryParse(input, out result) == false){
                Program.print("Invalid input. Please enter an integer.");
            }
            else if(result < 1){
                Program.print("Invalid input. Please enter a number greater than 0.");
            }
            else{
                output = result;
                validInput = true;
            }

            
        }

        return output;
    }

    /// <summary>
    /// Converts a string to an integer. If the conversion fails, it informs the user and returns null.
    /// </summary>
    /// <param name="str">The string to convert to an integer.</param>
    /// <returns>The integer value of the string, or null if the conversion fails.</returns>
    public int? StringToInt(string str, int min){
        int result;

        // Try to parse the string as an integer
        if (int.TryParse(str, out result) == false){
            // The string could not be parsed as an integer, so inform the user and return null
            Program.print($"The peramiter for the flag {activetyTitle} must be an intiger.");
            return null;
        }

        else if(result < min){
            Program.print($"The peramiter for the flag {activetyTitle} must 0 or greater.");
            return null;
        }

        else{
            

            // The string was successfully parsed as an integer, so return the result
            return result;
        }
    }
}


// Breathing Activity
class BreathingFunction:functionClass{
    
    

    public BreathingFunction(){
        Title = "Breathing";

        Discription = "Runs the breathing activity";

        CommandInputs = new List<string>{
            "breath", "breathing"
        };

        FlagObjects = new List<flagClass>{
            new editHold(), new editPause(), new breathActivetyTime()
        };

        FlagRegistry = new Dictionary<string, flagClass>();

        constructFlagRegistry();
    }
    
    public BreathingFunction(bool functions){
        
    }
    protected override void runNoFlag(){
        BreathingActivity breathingActivity = new BreathingActivity();
        breathingActivity.runActvity();
    }

    
}

class BreathingActivity:activityClass{

    public int holdTime = 3;

    public int pauseTime = 1500;

    public BreathingActivity(){
        IntroMsg = "Welcome to the Breathing Activity";

	    minForActivity = 0;

	    activetyDiscription = (
            "This activity will help you relax by walking your through breathing " + 
            "in and out slowly. Clear your mind and focus on your breathing."
        );

	    activetyTitle = "Breathing Activity";
    }

    protected override int activety(){

        // in mili seconds
        int timeCounter = 0;

        string inMsg = "Breathe in...";

        string outMsg = "Breathe out...";

        List<string> msgs = new List<string>{
            inMsg, outMsg
        };

        int currentMsg = 0;

        // min * 60 sec in min * 1000 mili seconds in second.
        while (timeCounter < minForActivity*60*1000){
            Program.printNoLine(msgs[currentMsg]);

            wait(pauseTime+500);
            timeCounter = timeCounter + pauseTime;

            // delete the old line
            Program.delLine();

            // preform count down 
            for (int i = holdTime; i > 0; i--){
                string iString = i.ToString();
                Program.printNoLine(iString);
                wait(1000);
                timeCounter = timeCounter + 1000;
                
                Program.delLine();
            }

            msgs.Reverse();
        }

        return timeCounter;
    }

    private void wait(int milliseconds){
        System.Threading.Thread.Sleep(milliseconds);
    }
}

class editHold:flagClass{
    
    public editHold(){
        Title = "Edit Hold Time";
        
        Discription = "Allows the user to change to the " + 
        "ammount of time breath is held.";
        
        Flags = new List<string>{
            "-ht", "-holdTime"
        };
        
        Paramiters = "int seconds - the number of seconds breath is to be held.";
    }

    public override void run(string peramiter){
        
        BreathingActivity breathingActivity = new BreathingActivity();

        int? seconds = breathingActivity.StringToInt(peramiter, 0);

        if (seconds != null){
            
            breathingActivity.holdTime = seconds ?? 0;
            breathingActivity.runActvity();
        }
        
    }

    
}

class editPause:flagClass{
    public editPause(){
        Title = "Edit Pause Time";
        
        Discription = "Allows the user to change to the " + 
        "ammount of time the breath message is displayed.";
        
        Flags = new List<string>{
            "-pt", "-pauseTime"
        };
        
        Paramiters = "int miliSeconds - the number of miliseconds the " +
        "breathing message is displayed.";
    }

    public override void run(string peramiter){
        
        BreathingActivity breathingActivity = new BreathingActivity();

        int? miliSeconds = breathingActivity.StringToInt(peramiter, 0);

        if (miliSeconds != null){
            
            breathingActivity.pauseTime = miliSeconds ?? 0;
            breathingActivity.runActvity();
        }
        
    }
}

class breathActivetyTime:flagClass{
    public breathActivetyTime(){
        Title = "Activity Time";
        
        Discription = "Allows the user to pass to the " + 
        "length of the actvity in the function call.";
        
        Flags = new List<string>{
            "-at", "-activityTime"
        };
        
        Paramiters = "int minutes - the number of minutes the actvity will " + 
        "last.";
    }

    public override void run(string peramiter){
        
        BreathingActivity breathingActivity = new BreathingActivity();

        int? mins = breathingActivity.StringToInt(peramiter, 1);

        if (mins != null){
            breathingActivity.runActvity(mins ?? 0);
        }
        
    }

}


// Reflection Activity
class ReflectionFunction:functionClass{
    
    private string reflectionPath = "ReflectionData.json";
    
    public ReflectionFunction(){
        Title = "Reflection";

        Discription = "Runs the reflection activity";

        CommandInputs = new List<string>{
            "reflect", "reflection", "Reflect", "Reflection"
        };

        FlagObjects = new List<flagClass>{
            new reflectActivetyTime(), new reflectTime()
        };

        FlagRegistry = new Dictionary<string, flagClass>();

        constructFlagRegistry();
    }

    // allows a new object to be created without generating all the values.
    public ReflectionFunction(bool gettingMethods){

    }

    protected override void runNoFlag(){
        ReflectionActivity reflectionActivity = new ReflectionActivity();
        reflectionActivity.runActvity();
    }

    public Dictionary<string, List<string>> loadReflectionData(){
        if (File.Exists(reflectionPath) == true){
            string jsonString = File.ReadAllText(reflectionPath);

            if (jsonString != ""){
                ReflectionJson reflectionJson =
                JsonSerializer.Deserialize<ReflectionJson>(jsonString)!;

                Dictionary<string, List<string>> dict = 
                new Dictionary<string, List<string>>{
                    {"times", reflectionJson.times},
                    {"questions", reflectionJson.questions}
                };
                return dict;
            }
            else{
                return new Dictionary<string, List<string>>();
            }
            
        }
        else{
            File.Create(reflectionPath);
            return new Dictionary<string, List<string>>();
        }
        
        
    }

    public void saveReflectionData(Dictionary<string, List<string>> dict){
        if (File.Exists(reflectionPath) == false){
            File.Create(reflectionPath);

            ReflectionJson scriptureJson = new ReflectionJson(){
                times = new List<string>(),
                questions = new List<string>()
            };

            string jsonString = JsonSerializer.Serialize<ReflectionJson>(scriptureJson);
            File.WriteAllText(reflectionPath, jsonString);
        }
        
        if(dict.Count != 2){
            throw new FormatException(
                "dict must only contain the keys 'times' and 'questions'"
            );
        }

        else if (dict.ContainsKey("times") == false){
            throw new FormatException(
                "dict must  contain the key 'times'"
            );
        }

        else if(dict.ContainsKey("questions") == false){
            throw new FormatException(
                "dict must contain the key 'questions'"
            );
        }
        else{
            ReflectionJson scriptureJson = new ReflectionJson(){
                times = dict["times"],
                questions = dict["questions"]
            };

            string jsonString = JsonSerializer.Serialize<ReflectionJson>(scriptureJson);
            File.WriteAllText(reflectionPath, jsonString);
        }
        
        
    }

}

class ReflectionActivity:activityClass{
    
    public int reflectSeconds = 5;

    public ReflectionActivity(){
        IntroMsg = "Welcome to the Reflection Activity";

	    minForActivity = 0;

	    activetyDiscription = (
            "This activity will help you reflect on times in your life " +
            "when you have shown strength and resilience. " + 
            "This will help you recognize the power you have and how you " + 
            "can use it in other aspects of your life."
        );

	    activetyTitle = "Reflection Activity";
    }

    protected override int activety(){
        
        // load in the quetions and times
        ReflectionFunction reflectionFunction = new ReflectionFunction(true);

        Dictionary <string, List<string>> dict = reflectionFunction.loadReflectionData();

        List<string> times = dict["times"];
        List<string> questions = dict["questions"];

        int timeCount = 0;

        //clear the terminal
        Program.clearTerminal();

        // pick a random time prompt
        Random rand = new Random();

        int randIndex = rand.Next(times.Count);

        // display it to the user.
        Program.print(times[randIndex]);
        
        // wait 1 second
        spin(1);
        timeCount = timeCount + 1000;

        while (timeCount < minForActivity*60*1000){
            // pick a random question
            randIndex = rand.Next(questions.Count);

            // display it to the user
            Program.print();
            Program.print(questions[randIndex]);
            

            // spin for the alloted ammount of time
            spin(reflectSeconds);
            timeCount = timeCount + (reflectSeconds * 1000);

            // delete the question in preperation for the next loop
            Program.delLine();
        }

        return timeCount;

    }
}

class ReflectionJson{
    public List<string> times {get; set;}

    public List<string> questions {get; set;}
}

class reflectActivetyTime:flagClass{
    public reflectActivetyTime(){
        Title = "Activity Time";
        
        Discription = "Allows the user to pass to the " + 
        "length of the actvity in the function call.";
        
        Flags = new List<string>{
            "-at", "-activityTime"
        };
        
        Paramiters = "int minutes - the number of minutes the actvity will " + 
        "last.";
    }

    public override void run(string peramiter){
        
        ReflectionActivity reflectionActivity = new ReflectionActivity
        ();

        int? mins = reflectionActivity.StringToInt(peramiter, 1);

        if (mins != null){
            reflectionActivity.runActvity(mins ?? 0);
        }
        
    }

}

class reflectTime:flagClass{
    
    public reflectTime(){
        Title = "Reflection Time";
        
        Discription = "Allows the user to change the ammount of time they " + 
        "ponder each question";
        
        Flags = new List<string>{
            "-rt", "-reflectTime", "-reflectionTime", "-reflecttime",
            "-reflectiontime"
        };
        
        Paramiters = "int seconds - the number of sedonds each question " + 
        "will be pondered";
    }

    public override void run(string peramiter){
        
        ReflectionActivity reflectionActivity = new ReflectionActivity
        ();

        int? secs = reflectionActivity.StringToInt(peramiter, 1);

        if (secs != null){
            reflectionActivity.reflectSeconds = secs ?? 0;
            reflectionActivity.runActvity();
        }
        
    }

}


//  Listeneing Actvity
class ListingFunction:functionClass{
    
    private string listingPath = "ListingData.json";

    public ListingFunction(){
        Title = "Listing";

        Discription = "Runs the listing activity";

        CommandInputs = new List<string>{
            "list", "listing"
        };

        FlagObjects = new List<flagClass>{
            new listActivetyTime(), new countDown()
        };

        FlagRegistry = new Dictionary<string, flagClass>();

        constructFlagRegistry();
    }

    public ListingFunction(bool GetingFunctions){}

    protected override void runNoFlag(){
        ListingActivty listingActivty = new ListingActivty();
         listingActivty.runActvity();

    }

    public List<string> loadListingData(){
        if (File.Exists(listingPath) == true){
            string jsonString = File.ReadAllText(listingPath);

            if (jsonString != ""){
                ListingJson listingJson =
                JsonSerializer.Deserialize<ListingJson>(jsonString)!;

                
                return listingJson.questions;
            }
            else{
                return new List<string>();
            }
            
        }
        else{
            File.Create(listingPath);
            return new List<string>();
        }
        
        
    }

    public void saveListingData(List<string> questionsList){
        ListingJson listingJson;
        string jsonString;
        
        if (File.Exists(listingPath) == false){
            
            File.Create(listingPath);

            listingJson = new ListingJson(){
                questions = new List<string>()
            };

            jsonString = JsonSerializer.Serialize<ListingJson>(listingJson);
            File.WriteAllText(listingPath, jsonString);
        }
        


        listingJson = new ListingJson(){
            questions = questionsList
        };

        jsonString = JsonSerializer.Serialize<ListingJson>(listingJson);
        File.WriteAllText(listingPath, jsonString);
        
        
        
    }

    
}

class ListingJson{
    public List<string> questions {get; set;}
}

class ListingActivty:activityClass{
    public int countDown = 5;
    
    public ListingActivty(){
        IntroMsg = "Welcome to the Listing Activity";

	    minForActivity = 0;

	    activetyDiscription = (
            "This activity will help you reflect on the good things in your " + 
            "life by having you list as many things as you can in a certain area."
        );

	    activetyTitle = "Listing Activity";
    }

    protected override int activety(){

        //get the start time
        DateTime startTime = DateTime.UtcNow;
        double MSElapsed = 0;
        
        // load in functions from the function class
        ListingFunction listingFunction = new ListingFunction(true);

        // load the list of questions
        List<string> questions = listingFunction.loadListingData();

        Random rand = new Random();

        // pick a question to ask the user
        int randIndex = rand.Next(questions.Count);

        //ask the question
        Program.print(questions[randIndex]);

        for (int i = countDown; i > 0; i--){
            Program.print($"{i}");
            wait(1000);
        }
        Program.print();
        int counter = 0;

        int maxTime = minForActivity*1000*60;

        // determin how much time has elapsed.
        MSElapsed = DateTime.UtcNow.Subtract(startTime).TotalMilliseconds;

        while (MSElapsed < maxTime){
            
            // get a reasponce from the user
            Program.input();

            // count the responce
            counter ++;
            // determin how much time has elapsed.
            MSElapsed = DateTime.UtcNow.Subtract(startTime).TotalMilliseconds;
        }
        
        Program.print($"You listed {counter} item(s)");

        // determin how much time has elapsed.
        MSElapsed = DateTime.UtcNow.Subtract(startTime).TotalMilliseconds;

        return Convert.ToInt32(MSElapsed);
    }

    private void wait(int milliseconds){
        System.Threading.Thread.Sleep(milliseconds);
    }
}

class listActivetyTime:flagClass{
    
    public listActivetyTime(){
        Title = "Activity Time";
        
        Discription = "Allows the user to pass to the " + 
        "length of the actvity in the function call.";
        
        Flags = new List<string>{
            "-at", "-activityTime"
        };
        
        Paramiters = "int minutes - the number of minutes the actvity will " + 
        "last.";
    }

    public override void run(string peramiter){
        
        ListingActivty listingActivty = new ListingActivty();

        int? mins = listingActivty.StringToInt(peramiter, 1);

        if (mins != null){
            listingActivty.runActvity(mins ?? 0);
        }
        
    }

}

class countDown:flagClass{
    public countDown(){
        Title = "Count Down";
        
        Discription = "Allows the user to change to the " + 
        "length of the count down before the user can respond";
        
        Flags = new List<string>{
            "-cd", "-countDown", "-countdown"
        };
        
        Paramiters = "int seconds - the number of seconds the program will " + 
        "count down for before the user can start listing";
    }

    public override void run(string peramiter){
        
        ListingActivty listingActivty = new ListingActivty();

        int? sec = listingActivty.StringToInt(peramiter, 1);

        if (sec != null){
            listingActivty.countDown = sec ?? 0;

            listingActivty.runActvity();
        }
        
    }
}

