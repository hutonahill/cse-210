using System;
using System.Text.Json;

class Program{
    public static bool running;
    
    public static char[] bannedChars =" &%$#@*(){}[]'=+_".ToCharArray();

    private static Dictionary<string, functionClass> registeredFunctions 
    = new Dictionary<string, functionClass>();

    private static List<functionClass> functionObjects;

    public static string scriptrurePath {
        get;
        private set;
    } = @"scripures.json";

    static void Main(string[] args){

        print($"\n== Welcome to Scriptures.cs ==\n");

        running = true;

        functionObjects = new List<functionClass>{
            new Quit(), new StartTest(), new Display(), 
            new Delete(), new NewScripture()
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

    public static Dictionary<string, string> loadScriptures(){
        if (File.Exists(scriptrurePath) == true){
            string jsonString = File.ReadAllText(Program.scriptrurePath);

            if (jsonString != ""){
                ScriptureJson ScriptureJson =
                JsonSerializer.Deserialize<ScriptureJson>(jsonString)!;

                return ScriptureJson.dict;
            }
            else{
                return new Dictionary<string, string>();
            }
            
        }
        else{
            File.Create(scriptrurePath);
            return new Dictionary<string, string>();
        }
        
        
    }
    
    public static void saveScriptures(Dictionary<string, string> scrputreDict){
        if (File.Exists(scriptrurePath) == false){
            File.Create(scriptrurePath);
        }
        
        ScriptureJson scriptureJson = new ScriptureJson(){
            dict = scrputreDict
        };

        string jsonString = JsonSerializer.Serialize<ScriptureJson>(scriptureJson);
        File.WriteAllText(scriptrurePath, jsonString);
    }
    
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

    public static void clear(){
        Console.Clear();
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
        };

        FlagRegistry = new Dictionary<string, flagClass>();

        constructFlagRegistry();
    }

    protected override void runNoFlag(){
        Program.running = false;
    }
}


// new scripture 
/// <summary>
/// Function Class <c>NewScripture</c> allows the user to add a new scripture.
/// </summary>
class NewScripture:functionClass{
    public NewScripture(){
        Title = "New Scripture";
        
        Discription = "Adds a new scriptrue to the programs database.";

        CommandInputs = new List<string>{
            "new", "newScripture", "ns"
        };

        FlagObjects = new List<flagClass>{
            new overwrite()
        };

        FlagRegistry = new Dictionary<string, flagClass>();

        constructFlagRegistry();
    }

    protected override void runNoFlag(){
        string refInput = Program.input(
            "Input the verse refrance (such as John 3:16): "
        );

        Dictionary<string, string> scriptureDict = Program.loadScriptures();

        if (scriptureDict.ContainsKey(refInput)){
            Program.print($"The verse {refInput} is already registered");
        }
        else if((refInput.ToLower() == "help") || (refInput.ToLower() == "back")){
            Program.print("you cannot store a scripture with the refrance " +
            "'help' or 'back");
        }
        else{
            string verseInput = Program.input(
                "Input the verse with no numbers, in one line: "
            );

            scriptureDict[refInput] = verseInput;

            Program.saveScriptures(scriptureDict);
        }

    }
}

/// <summary>
/// Flag Class <c>overwrite</c> allows the users new scripture to overwrite an old function.
/// </summary>
class overwrite:flagClass{
    public overwrite(){
        Title = "Overwrite";
        Discription = "allows you to overwrite a verce";
        Flags = new List<string>{
            "-o", "overwrite", "o"
        };
        Paramiters = "";
    }

    public override void run(string peramiter){
        if (peramiter != ""){
            Program.print($"The '{Title}' flag does not accept any paramiters.");
        }

        else{
            string refInput = Program.input(
                "Input the verse refrance (such as John 3:16): "
            );

            if((refInput.ToLower() == "help") || (refInput.ToLower() == "back")){
                Program.print("you cannot store a scripture with the refrance " +
                "'help' or 'back");
            }
            else{
                Dictionary<string, string> scriptureDict = Program.loadScriptures();

                string verseInput = Program.input(
                    "Input the verse with no numbers, in one line: "
                );

                scriptureDict[refInput] = verseInput;

                Program.saveScriptures(scriptureDict);
            }

            
        }
    }
}

/// <summary>
/// Flag Class <c>refrance</c> allows the user to pass a refrance into the function when they run it.
/// currently broken
/// </summary>
class newRefrance:flagClass{
    public newRefrance(){
        Title = "Refrance";
        Discription = "allows you to pass the refrance in the command";
        Flags = new List<string>{
            "-r", "refrance", "r"
        };
        Paramiters = "ref - the refrance to the verce (such as John 3:16)";
    }

    public override void run(string peramiter){
        string refInput = peramiter;
        
        Dictionary<string, string> scriptureDict = Program.loadScriptures();

        if (scriptureDict.ContainsKey(refInput)){
            Program.print($"The verse {refInput} is already registered");
        }
        else if((refInput.ToLower() == "help") || (refInput.ToLower() == "back")){
            Program.print("you cannot store a scripture with the refrance " +
            "'help' or 'back");
        }
        else{
            string verseInput = Program.input(
                "Input the verse with no numbers, in one line: "
            );

            scriptureDict[refInput] = verseInput;

            Program.saveScriptures(scriptureDict);
        }
    }
}


// delete scriptures
/// <summary>
/// Function Class <c>Delete</c> allows the user to delete a scripture
/// </summary>
class Delete:functionClass{
    public Delete(){
        Title = "Delete Scripture";

        Discription = "Deletes a scripture";

        CommandInputs = new List<string>{
            "del", "delete", "ds"
        };

        FlagObjects = new List<flagClass>{
        };

        FlagRegistry = new Dictionary<string, flagClass>();

        constructFlagRegistry();
    }

    protected override void runNoFlag(){
        bool validInput = false;
        
        while (validInput == false){
            string userInput = Program.input("Input the verse refrance to delete: ");

            Dictionary<string, string> dict = Program.loadScriptures();

            
            if (userInput.ToLower() == "back"){
                validInput = true;
            }

            else if (userInput.ToLower() == "help"){
                Program.print("Stored scriptures: ");
                
                string output = "";
                
                foreach (string s in dict.Keys){
                    output = output + $"'{s}', ";
                }
                
                //remove the last two chariters
                output = output.Remove(output.Length - 2, 2);

                Program.print(output);
                validInput = false;
            }

            else if (dict.ContainsKey(userInput) == false){
                Program.print("No scriputre with that refrance");
                validInput = false;
            }

            else{
                dict.Remove(userInput);
                Program.print("Scriputre deleted.");
                validInput = true;
            }

            Program.saveScriptures(dict);
        }//while
        
    }
}

/// <summary>
/// Flag Class <c>refrance</c> allows the user to pass a refrance into the function when they run it.
/// currently broken
/// </summary>
class delRefrance:flagClass{
    public delRefrance(){
        Title = "Refrance";
        Discription = "allows you to pass the refrance in the command";
        Flags = new List<string>{
            "-r", "refrance", "r"
        };
        Paramiters = "ref - the refrance to the verce (such as John 3:16)";
    }

    public override void run(string peramiter){
        
        string userInput = peramiter;

        Dictionary<string, string> dict = Program.loadScriptures();

        if (userInput.ToLower() == "help"){
            Program.print("Stored scriptures: ");
            
            string output = "";
            
            foreach (string s in dict.Keys){
                output = output + $"'{s}', ";
            }
            
            //remove the last two chariters
            output = output.Remove(output.Length - 2, 2);

            Program.print(output);
        }

        else if (dict.ContainsKey(userInput) == false){
            Program.print("No scriputre with that refrance");
        }

        else{
            dict.Remove(userInput);
            Program.print("Scriputre deleted.");
        }

        Program.saveScriptures(dict);
        
    }
}


// display scriptures
/// <summary>
/// Function Class <c>Display</c> displays a scriptur to the user.
/// </summary>
class Display:functionClass{
    public Display(){
        Title = "Dsiplay  Scripture";

        Discription = "Displays a full scripture to the user";

        CommandInputs = new List<string>{
            "dis", "display"
        };

        FlagObjects = new List<flagClass>{
        };

        FlagRegistry = new Dictionary<string, flagClass>();

        constructFlagRegistry();
    }

    protected override void runNoFlag(){
        bool validInput = false;
        
        while (validInput == false){
            string userInput = Program.input("Input the verse refrance to display: ");

            Dictionary<string, string> dict = Program.loadScriptures();

            
            if (userInput.ToLower() == "back"){
                validInput = true;
            }

            else if (userInput.ToLower() == "help"){
                Program.print("Stored scriptures: ");
                
                string output = "";
                
                foreach (string s in dict.Keys){
                    output = output + $"'{s}', ";
                }
                
                //remove the last two chariters
                output = output.Remove(output.Length - 2, 2);

                Program.print(output);
                validInput = false;
            }

            else if (dict.ContainsKey(userInput) == false){
                Program.print("No scriputre with that refrance");
                validInput = false;
            }

            else{
                Program.print();
                Program.print(dict[userInput]);
                validInput = true;
            }


        }//while
    }
}

/// <summary>
/// Flag Class <c>refrance</c> allows the user to pass a refrance into the function when they run it.
/// currently broken
/// </summary>
class disRefrance:flagClass{
   
    public disRefrance(){
        Title = "Refrance";
        Discription = "allows you to pass the refrance in the command";
        Flags = new List<string>{
            "-r", "refrance", "r"
        };
        Paramiters = "ref - the refrance to the verce (such as John 3:16)";
    }

    public override void run(string peramiter){
        string userInput = peramiter;

        Dictionary<string, string> dict = Program.loadScriptures();

        
        if (userInput.ToLower() == "help"){
            Program.print("Stored scriptures: ");
            
            string output = "";
            
            foreach (string s in dict.Keys){
                output = output + $"'{s}', ";
            }
            
            //remove the last two chariters
            output = output.Remove(output.Length - 2, 2);

            Program.print(output);
        }

        else if (dict.ContainsKey(userInput) == false){
            Program.print("No scriputre with that refrance");
        }

        else{
            Program.print(dict[userInput]);
        }
    }
}


// start test
/// <summary>
/// Fucntion Class <c>StartTest</c> tests the user, this is the the primary funciton that contains the requiremtns
/// </summary>
class StartTest:functionClass{
    public StartTest(){
        Title = "Start Test";

        Discription = "Tests the users memorization of the scriputre";

        CommandInputs = new List<string>{
            "start", "test", "starttest"
        };

        FlagObjects = new List<flagClass>{
            new dificulty()
        };

        FlagRegistry = new Dictionary<string, flagClass>();

        constructFlagRegistry();
    }

    protected override void runNoFlag(){
        string scripture = getScriputreInline();

        if (scripture != "back"){
            runTest(scripture, 5);
        }
        
    }

    public void runTest(string scripture, int removalsPerIteration){
        
        // split the scripture into a list
        List<string> splitScript = scripture.Split(" ").ToList();
        List<int> visibleIndexes = new List<int>();

        Random random = new Random();

        for (int i = 0; i < splitScript.Count; i++){
            visibleIndexes.Add(i);
        }

        bool end = false;

        int lvl = 0;

        Program.clear();
        Program.print($"Level {lvl}:");
        displayScript(splitScript);

        while (end == false){

            // display the scripture to the user
            

            string userInput = Program.input("scriptureTest.cs/StartTest> ").ToLower();

            if (userInput == "back"){
                end = true;
            }

            else if (userInput == "help"){
                Program.print("Input '' to advnace to the next stage.");
                Program.print("Input 'back' to leave the test.");
            }

            else{

                // remove a number of words equil to removalsPerIteration
                for (int i = 0; i < removalsPerIteration; i++){
                    if (visibleIndexes.Count > 0 ){
                        // get a random index from the list of indexes of visible words
                        int removeValueIndex = random.Next(visibleIndexes.Count);

                        // conver the visibleIndexes index to a splitScript index
                        int removeValue = visibleIndexes[removeValueIndex];
                        
                        // blank the selected word
                        splitScript[removeValue] = convertToBlank(splitScript[removeValue]);

                        // remove the now blank index from the list of visible indexes.
                        visibleIndexes.RemoveAt(removeValueIndex);

                        if (visibleIndexes.Count == 0){
                            end = true;
                        }

                        lvl = lvl + 1;
                    }

                    
                    
                }

                Program.clear();
                Program.print($"Level {lvl}:");
                displayScript(splitScript);
                
            }

            

        }//while
    }

    public string getScriputreInline(){
        bool validInput = false;
        
        while (validInput == false){
            string userInput = Program.input("Input the verse refrance to display: ");

            Dictionary<string, string> dict = Program.loadScriptures();

            
            if (userInput.ToLower() == "back"){
                validInput = true;
                return "back";
            }

            else if (userInput.ToLower() == "help"){
                Program.print("Stored scriptures: ");
                
                string output = "";
                
                foreach (string s in dict.Keys){
                    output = output + $"'{s}', ";
                }
                
                //remove the last two chariters
                output = output.Remove(output.Length - 2, 2);

                Program.print(output);
                validInput = false;
            }

            else if (dict.ContainsKey(userInput) == false){
                Program.print("No scriputre with that refrance");
                validInput = false;
            }

            else{
                validInput = true;
                return dict[userInput];
            }


        }//while

        return "ERROR";
    }

    private void displayScript(List<string> splitScript){
        string output = "";

        foreach (string s in splitScript){
            output = output + s + " ";
        }
        Program.print();
        Program.print(output);
    }

    private string convertToBlank(string visible){
        
        string output = "";
        List<char> nonConverts = new List<char>{
            '(',')', '-', ';', ',','?', '.', '&', '!','"'
        };

        for (int i = 0; i < visible.Length; i++){

            if (nonConverts.Contains(visible[i]) == true){
                output = output + visible[i];
            }
            else{
                output = output + '_';
            }
        }

        return output;
    }

}

/// <summary>
/// Flag Class <c>refrance</c> allows the user to pass a refrance into the function when they run it.
/// currently broken
/// </summary>
class testRefrance:flagClass{
    public testRefrance(){
        Title = "Refrance";
        Discription = "allows you to pass the refrance in the command";
        Flags = new List<string>{
           "-r", "refrance", "r"
        };
        Paramiters = "ref - the refrance to the verce (such as John 3:16)";
    }

    public override void run(string peramiter){

        
        string userInput = Program.input("Input the verse refrance to display: ");

        string script = "";

        Dictionary<string, string> dict = Program.loadScriptures();

        
        if (userInput.ToLower() == "back"){
            // do nothing, leave the function
        }

        else if (userInput.ToLower() == "help"){
            Program.print("Stored scriptures: ");
            
            string output = "";
            
            foreach (string s in dict.Keys){
                output = output + $"'{s}', ";
            }
            
            //remove the last two chariters
            output = output.Remove(output.Length - 2, 2);

            Program.print(output);
        }

        else if (dict.ContainsKey(userInput) == false){
            Program.print("No scriputre with that refrance");
        }

        else{
            script = dict[userInput];
            StartTest startTest = new StartTest();
            startTest.runTest(script, 5);
        }
    }
}

class dificulty:flagClass{
    public dificulty(){
        Title = "Dificulty";
        Discription = "allows you to change the nber of words that are blanked every round";
        Flags = new List<string>{
            "-d", "dificulty", "dif"
        };
        Paramiters = "dif - the number of words to be removed each round";
    }

    public override void run(string peramiter){
        if (peramiter == ""){
            Program.print($"Flag {Title} requires a peramiter.");
        }
        else{
            int dif = 0;

            if (int.TryParse(peramiter, out dif) == false){
                Program.print($"The peramiter for flag {Title} but be an intiger");
            }
            else if(dif <= 0){
                Program.print("Dificulty must be greater than 0");
            }
            else{
                StartTest startTest = new StartTest();
                string scripture = startTest.getScriputreInline();

                if (scripture != "back"){
                    int words = scripture.Split("").Length;
                    
                    if (dif > words){
                        Program.print("the dificulty may not excede the number of words.");
                    }
                    else{
                        startTest.runTest(scripture, dif);
                    }
                }
            }
        }

        
    }
}

