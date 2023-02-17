using System;
using System.Text.Json;


// === PROGRAM FRAMEWORK === 
// the below is a framework to run funcitons with little modification to 
// the program class. You should primarily only have to add function classes,
// flag classes, and add the function classes to the functionObjects list
// on line 27

class Program{
    public static bool running;
    
    public static char[] bannedChars =" &%$#@*(){}[]'=+_".ToCharArray();

    private static Dictionary<string, functionClass> registeredFunctions 
    = new Dictionary<string, functionClass>();

    private static List<functionClass> functionObjects;

    static void Main(string[] args){

        print($"\n== Welcome to Scriptures.cs ==\n");

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
            string rawInput = input("scriptures.cs> ").ToLower();

            string[] splitInput = rawInput.Split(" ");

            commandInput = splitInput[0];

            flagInput = splitInput.Skip(1).ToArray().ToList();
            
            
            // if the user asked for help and the program is still running
            // run the help function
            if (commandInput == "help" ){
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


    public string Title{
        get;
        protected set;
    }

    public string Discription{
        get;
        protected set;
    }

    public List<string> CommandInputs {
        get;
        protected set;
    }

    public List<flagClass> FlagObjects {
        get;
        protected set;
    }

    public Dictionary<string, flagClass> FlagRegistry{
        get;
        protected set;
    }

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

    public void constructFlagRegistry(){
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

    public void run(List<string> flags){
        
        if (flags.Count == 0){
            runNoFlag();
        }
        else if(flags.Count == 1){
            if (FlagRegistry != null){

                string flag = flags[0];

                string peram = "";

                bool isValidPeram = true;
                
                // if the flag contains a ( assume the user is passing a paramiter and extract it.
                if (flag.Contains('(')){
                    if (flag.Contains(')') == false){
                        Program.print($"flag '{flag}' contains a '(' but not ')'.  " +
                        "Please try again.");
                        isValidPeram = false;
                    }
                    else if (flag[flag.Length-1] != ')'){
                        Program.print("The last char of a flag with a peramiter " + 
                        "must be ')'.  Please try again.");
                        isValidPeram = false;
                    }
                    else{

                        // split the peramiter off the flag
                        string[] splitFLag = flag.Split('(');
                        
                        // make sure there was only 1 ( char
                        if (splitFLag.Count() != 2){
                            Program.print("Flags with peramiters may only " + 
                            "contain one '(' charicter.  Please try again.");
                            isValidPeram = false;
                        }

                        else{
                            flag = splitFLag[0];
                            peram = splitFLag[1];

                            // remvoe the ) from the end of peram
                            peram = peram.Remove(peram.Length -1);
                        }
                        
                    }
                }
                
                if (isValidPeram == true){
                    try{
                        FlagRegistry[flag].run(peram);
                    }
                    catch(KeyNotFoundException){
                        Program.print($"flag '{flag}' is not registered in " + 
                        $"function '{Title}'.  Please try again");
                    }
                }
                

                
            }
            else Program.print("No flags registered for this command.");
            
        }
        else{
            Program.print("Only one flag is supported at this time.  Please try again.");
        }
        
    }

    protected abstract void runNoFlag();

}

/// <summary>
/// Abstract Class <c>flagClass</c> the structure of flags for functions.
/// </summary>
public abstract class flagClass{

    public string Title{
        get;
        protected set;
    }

    public string Discription{
        get;
        protected set;
    }

    public List<string> Flags {
        get;
        protected set;
    }

    public string Paramiters{
        get;
        protected set;
    }

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

