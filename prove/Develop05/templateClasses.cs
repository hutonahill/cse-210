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
                int copyInt = targetObject.Flags.Count;
                if (copyInt != 0){
                    
                    // threee tabs deep
                    copyData = $"Flags: ";
                    for (int j = 0; j < targetObject.Flags.Count; j++){
                        
                        copyData = copyData + $"'{targetObject.Flags[j]}', ";
                        
                    }
                    // remove the ', ' from the end of the string
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
        else if(flags.Count >= 1){
            
            // check that this function has flags
            if (FlagRegistry != null){
                
                // loop thorugh every flag function
                foreach (string flag in flags){
                    string peram = "";
                    peram = ExtractParameter(flag);

                    if (peram != null){
                        string flagOnly = flag.Replace(("(" + peram + ")"), "");
                        try{
                            FlagRegistry[flagOnly].run(peram);
                        }
                        catch(KeyNotFoundException){
                            Program.print($"flag '{flagOnly}' is not registered in " + 
                            $"function '{Title}'.  Please try again");
                        }
                    }
                }

                runNoFlag();
            }
            else {
                Program.print("No flags registered for this command.");
            }
            
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

public abstract class goalClass{
    public string Title {
        get;
        protected set;
    }= "";

    public string Discription {
        get;
        protected set;
    }= "";

    public int? ScoreOnComplete {
        get;
        protected set;
    } = null;

    public bool IsComplite {
        get;
        protected set;
    } = false;

    public abstract string display();

    public abstract void onCompletion();

    public abstract string endcode();

    public abstract void decode(string encodedClass);
}