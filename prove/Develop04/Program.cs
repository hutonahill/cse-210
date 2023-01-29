// Author, Evan Riker

using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Globalization;

// main class, runs when you run the project.
class Program{


    //create function objects
    private static newEntry newEntryTemp = new newEntry();

    private static viewEntries viewEntriesTemp = new viewEntries();

    private static newPrompt newPromptTemp = new newPrompt();

    private static wipeEntries wipeTemp = new wipeEntries();

    private static Quit quitTemp = new Quit();

    private static removeEntry removeEntryTemp = new removeEntry();

    public static bool running = true;

    public static string entryPath{
        get;
        private set;
    } = @"entries.json";

    public static string promptsPath{
        get;
        private set;
    } = @"prompts.json";

    static void Main(string[] args){

         print($"\n== Welcome to Journal.cs ==\n");

        // keep the program running until the user tells it to stop.
        while (running == true){
            
            string userInput = "";

            //create a list with used fucntion objects
            List<functionClass> commandObjects = new List<functionClass>{
                newEntryTemp, viewEntriesTemp, quitTemp, newPromptTemp, 
                removeEntryTemp, wipeTemp
            };

            // create a list of commands
            List<string> validCommands = new List<string>{"help"};

            // create a dictionary that stores function objects
            Dictionary<string, functionClass> registeredFunctions = new Dictionary<string, functionClass>();

            // loop though every function object
            for (int i = 0; i < commandObjects.Count; i++){
                List<string> targetCommandList = new List<string>(
                    commandObjects[i].CommandInputs
                );

                // for every command in the fucntion add a key to the 
                // dictionary that points to that fucntion.

                for (int j = 0; j < targetCommandList.Count; j++){

                    // make sure there are no duplicate commands
                    if (validCommands.Contains(targetCommandList[j]) == false){
                        registeredFunctions.Add(targetCommandList[j], 
                        commandObjects[i]);
                    }

                    // if there is duplicate warn the user and stop the program.
                    else{
                        print(
                            $"Duplicate command '{targetCommandList[j]}' in " + 
                            $"registered fucntion '{commandObjects[i].Title}'."
                        );
                        running = false;
                    }
                }
            }
            

            // get input from the user to determin what to do.
            if (running == true){
                userInput = input("journal.cs> ").ToLower();
            }
            
            // if the user asked for help and the program is still running
            // run the help function
            if (userInput == "help" && running == true){
                help(commandObjects);
            }

            // if the program is still running run the input command
            else if (running == true){
                try{
                    registeredFunctions[userInput].run();
                }

                // if the command is not a key in the dictionary, inform the 
                // the user that the command they input is invalid
                catch (KeyNotFoundException){
                    print("Invalid command. Please try agran or run 'help' to " + 
                    "a list of valid commands");
                }
            }
            
            print("");

        }
        
        //tell the user the program has stoped.
        print($"Stoped. \n");

    }

    private static void help(List<functionClass> commands){
        
        if (commands.Count != 0){

            string output = "Commands";

            for (int i = 0; i < commands.Count; i++){
                functionClass targetCommand = commands[i];
                output = output + ($"\n{i+1} {targetCommand.Title}" + 
                        $"\n    Valid Inputs: {targetCommand.displayCommandInputs()}" + 
                        $"\n    Discription: {targetCommand.Discription}" + 
                        $"\n");
            }
            
            print(output);
        }
        else{
            print("No registered commands");
        }
    }

    public static EntriesJson loadEntries(){

        string jsonString = File.ReadAllText(Program.entryPath);
        EntriesJson getEntries = JsonSerializer.Deserialize<EntriesJson>(jsonString)!;

        return getEntries;
    }

    public static void saveEntries(List<string> datesWithEntires, 
        List<string> codedEntreis){
        EntriesJson entriesJson = new EntriesJson{
            datesWithEntires = datesWithEntires,
            encodedEntries = codedEntreis
        };

        // parce the format object into a sting
        string jsonString = JsonSerializer.Serialize<EntriesJson>(entriesJson);

        // save the string to a file
        File.WriteAllText(Program.entryPath, jsonString);
    }

    public static PromptJson loadPrompts(){
        string rawPrompt =  File.ReadAllText(Program.promptsPath);
        PromptJson getPrompts = JsonSerializer.Deserialize<PromptJson>(rawPrompt)!;

        return getPrompts;
    }

    public static void savePrompts(List<string> promptList){
        PromptJson savePrompts = new PromptJson{
            prompts = promptList
        };

        // parce the object into a json string
        string jsonString = JsonSerializer.Serialize<PromptJson>(savePrompts);

        // save the parced string to a file.
        File.WriteAllText(Program.promptsPath, jsonString);
    }


    // input and output functions.
    // this allows the input and output of the program to be flexable
    // allowing me to more easily divert output to a website or other form of UI.
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

// a class define the structure of prompt.json
class PromptJson{
    public List<string> prompts { get; set; }
}

// a class to define the structure of entries.json
class EntriesJson{
    public List<string> datesWithEntires { get; set; }
    public List<string> encodedEntries { get; set; }
}

class EntryDataType{
    
    public string prompt{
        get;
        private set;
    } = "";
    public string responce{
        get;
        private set;
    } = "";

    public DateDataType date{
        get;
        private set;
    } = new DateDataType();

    public EntryDataType(){

    }

    public EntryDataType(string promptValue, string responceValue, 
    DateDataType dateValue){
        setPrompt(promptValue);
        setResponce(responceValue);
        setDate(dateValue);
    }

    // setters
    public void setPrompt(string promptValue){
        prompt = promptValue;
    }

    public void setResponce(string entryValue){
        responce = entryValue;
    }

    public Boolean setDate(DateDataType dateValue){
        
        bool validInput = false;

        // check to make sure all three values of DateDataType are
        // are included.

        if (dateValue.isComplete() == true){
            date = dateValue;
        }
        else{
            throw new ArgumentException("Input dataValue is incomplete.");
        }

        return validInput;
    }

    // fucntions
    public string display(){

        string displayDate = date.display();

        return ($"{displayDate}" + 
            $"\n    Prompt: {prompt}" + 
            $"\n    Responce: {responce}");

    }

    public string encode(){
        return $"{prompt}"+"\\" + $"{responce}" + "\\" + $"{date.display()}";
    }

    public void decode(string code){
        
        string[] parts1 = code.Replace("\\","-").Split('-');

        prompt = parts1[0];
        responce = parts1[1];

        string[] dateParce = parts1[2].Split("/");


        int month = Int32.Parse(dateParce[0]);

        int day = Int32.Parse(dateParce[1]);

        int year = Int32.Parse(dateParce[2]);

        date = new DateDataType(day, month, year);
    }
}

class DateDataType{
    // set variables to -1 by default
    public int day{
        get;
        private set;
    } = -1;

    public int month{
        get;
        private set;
    } = -1;

    public int year{
        get;
        private set;
    } = -1;

    public bool validDate{
        get;
        private set;
    } = true;

    public string errorMsg{
        get;
        private set;
    } = "";
    

    private bool isLeapYear;

    private List<int> daysInMonths = new List<int>{
        31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31
    };

    public DateDataType(){
    }

    public DateDataType(int dayValue, int monthValue, int yearValue){

        myError yearError = checkYear(yearValue);
        if (yearError.checkError() == false){
            year = yearValue;

            myError monthError = checkMonth(monthValue);

            if (monthError.checkError() == false){
                month = monthValue;

                myError dayError = checkDay(dayValue, month, year);

                if (dayError.checkError() == false){
                    day = dayValue;
                }
                else{
                    validDate = false;
                    errorMsg = dayError.readMsg();
                }
            }
            else{
                validDate = false;
                errorMsg = monthError.readMsg();
            }
        }
        else{
            validDate = false;
            errorMsg = yearError.readMsg();
        }

        
    }

    // Setters
    public myError setDay(int dayValue){
        
        myError dayError;

        if (month > -1){
            if (year > -1){
                dayError = checkDay(dayValue, month, year);
            }
            else{
                dayError = checkDay(dayValue, month);
            }
        }
        else{
            dayError = checkDay(dayValue);
        }

        if (dayError.checkError() == false){
            day = dayValue;
        }

        return dayError;
    }

    public myError setMonth(int monthValue){
        myError monthError = checkMonth(monthValue);

        if (monthError.checkError() == false){
            month = monthValue;
        }

        return monthError;
    }

    public myError setYear(int yearValue){
        myError yearError = checkYear(yearValue);

        if (yearError.checkError() == false){
            year = yearValue;

            isLeapYear = isYearLeap(year);
        }


        return yearError;
    }

    // Error checkers

    private myError checkDay(int dayValue){
        myError dayError = new myError();

        if (((1 <= dayValue) || (dayValue <= 31)) == false){
            dayError.raiseError("You must input a day between 1 and 31");
        }

        return dayError;
    }

    private myError checkDay(int dayValue, int monthValue){
        myError dayError = new myError();

        int monthIndex = monthValue - 1;

        int daysInInputMonth = daysInMonths[monthIndex];

        if (monthValue == 2){
            daysInInputMonth = daysInInputMonth + 1;
        }

        if ((monthValue > -1) && 
            ((1 <= dayValue) && (dayValue <= daysInInputMonth)) == false){
            
            dayError.raiseError(
                $"There are {daysInInputMonth} days in month {monthValue}. " +
                $"You must input a dayValue between 1 and {daysInInputMonth}.");
        }
        else{
            if (((1 <= dayValue) || (dayValue <= 31)) == false){
                dayError.raiseError("You must input a day between 1 and 31");
            }
        }

        return dayError;
    }

    private myError checkDay(int dayValue, int monthValue, int yearValue){
        myError dayError = new myError();

        int monthIndex = monthValue - 1;

        int daysInInputMonth = daysInMonths[monthIndex];

        // if year is defined, chedk if its a leap year 
        if (yearValue > -1){

            // if its a leap year and febuary, add a day for the 29th
            if ((isLeapYear == true) && (monthValue == 2)){
                daysInInputMonth = daysInInputMonth + 1;
            }
        }

        // if year hasent been defined yet, and its feb, add a day just in case.
        else{
            if (monthValue == 2){
                daysInInputMonth = daysInInputMonth + 1;
            }
        }
        
        // make sure the 
        if ((monthValue > -1) && 
            ((1 <= dayValue) && (dayValue <= daysInInputMonth)) == false){
            
            dayError.raiseError(
                $"There are {daysInInputMonth} days in month {monthValue}. " +
                $"You must input a dayValue between 1 and {daysInInputMonth}.");
        }
        else{
            if (((1 <= dayValue) || (dayValue <= 31)) == false){
                dayError.raiseError("You must input a day between 1 and 31");
            }
        }

        return dayError;


    }

    private myError checkMonth(int monthValue){
        myError monthError = new myError();
        
        if ((1 <= monthValue) && (monthValue <= 12) == false){
            monthError.raiseError("You must input a month between 1 and 12");
        }

        return monthError;
    }
    
    private myError checkYear(int yearValue){
        myError yearError = new myError();
        
        if (yearValue < 0){
            yearError.raiseError("You must input a year greater than 0.");
        }

        return yearError;
    }
    
    private bool isYearLeap(int yearValue){
        if (yearValue % 100 == 0){
            if (yearValue % 400 == 0){
                return true;
            }

            else{
                return false;
            }
        
        }
        else{
            if (year % 4 == 0){
                return true;
            }

            else{
                return false;
            }
        }
    }

    // Functions
    public string display(){
        return($"{month}/{day}/{year}");
    }

    public bool isComplete(){
        if ((day > -1) && (month > -1) && (year > -1)){
            return true;
        }
        else{
            return false;
        }
    }
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

// Author: Riker, Evan

// command class framework
abstract class functionClass{

    public abstract string Title{
        get;
        protected set;
    }

    public abstract string Discription{
        get;
        protected set;
    }

    public abstract List<string> CommandInputs {
        get;
        protected set;
    }

    public string displayCommandInputs(){
        string output = "";
        string copyData;

        // format each command
        for (int i = 0; i< CommandInputs.Count; i++){
            copyData = $"'{CommandInputs[i]}',";
            output = output + copyData;
        }
        
        // remove the ',' from the end of teh string
        output = output.Remove(output.Length - 1, 1);

        return output;
    }

    public abstract void run();

}

//defalut commands
class Quit:functionClass{


    public override string Title{
        get;
        protected set;
    } = "Quit";

    public override string Discription{
        get;
        protected set;
    } = "exits the application.";

    public override List<string> CommandInputs{
        get;
        protected set;
    } = new List<string>{
        "quit", "exit", "3"
    };

    public override void run(){
        Program.running = false;
    }
}

//custom commands
class newEntry:functionClass{


    public override string Title{
        get;
        protected set;
    } = "New Entry";

    public override string Discription{
        get;
        protected set;
    } = "Offers the user a prompt and lets them input.";

    
    public override List<string> CommandInputs{
        get;
        protected set;
    } = new List<string>{
        "entry", "new", "new entry", "1"
    };

    public override void run(){

        // read a list of dates with posts and the posts
        EntriesJson getEntries = Program.loadEntries();

        List<string> datesWithEntiresExtracted = getEntries.datesWithEntires;

        List<string> entriesExtracted = getEntries.encodedEntries;

        // get the date and put it into a date object
        DateTime whatsToday = DateTime.Now;

        DateDataType today = new DateDataType(whatsToday.Day, whatsToday.Month, 
        whatsToday.Year);

        // check to an entry has been made today
        if (datesWithEntiresExtracted == null){
            datesWithEntiresExtracted = new List<string>();
            entriesExtracted = new List<string>();
        }

        if (datesWithEntiresExtracted.Contains(today.display())){
            Program.print("You have already made an entry today, " + 
            "Please come back tomarrow.");
        }

        else{
            //read the prompts into a list
            PromptJson getPrompts = Program.loadPrompts();

            List<string> prompts = getPrompts.prompts;

            // pick a random prompt from the prompts list
            Random randomGen = new Random();

            string displayPrompt = prompts[randomGen.Next(prompts.Count)];

            // display the prompt to the user
            Program.print(displayPrompt);

            // Get the users responce.
            string userInput = Program.input();


            EntryDataType todaysEntry = new EntryDataType(displayPrompt, 
            userInput, today);

            // save the new entry to the lists
            datesWithEntiresExtracted.Add(today.display());
            entriesExtracted.Add(todaysEntry.encode());

            // save the modified lists to a format object
            Program.saveEntries(datesWithEntiresExtracted, entriesExtracted);
        }

    }

}

class viewEntries:functionClass{

    public override string Title{
        get;
        protected set;
    } = "View Entries";


    public override string Discription{
        get;
        protected set;
    } = "Prints all posts in order of input.";

    public override List<string> CommandInputs{
        get;
        protected set;
    } = new List<string>{
        "old", "view", "view entries", "2"
    };

    public override void run(){
        // read the contance of the file to a string.
        string jsonString = File.ReadAllText(Program.entryPath);
        // parce the string into the format object
        EntriesJson getEntries = JsonSerializer.Deserialize<EntriesJson>(jsonString)!;

        // get the data from the format object in a form we can easily read.
        List<EntryDataType> entryList = new List<EntryDataType>();
        List<string> entriesExtracted = getEntries.encodedEntries;

        // check to make sure there are entreis to read
        if (entriesExtracted == null){
            Program.print("there are no posts");
        }
        else{
            // for each item add the data to a new entryDataType object and display
            // and display it to the user.
            for (int i = 0; i < entriesExtracted.Count; i++){
                entryList.Add(new EntryDataType());

                entryList[i].decode(getEntries.encodedEntries[i]);

                Program.print();
                Program.print(entryList[i].display());
                
            }
        }
    }

}

class newPrompt:functionClass{
    
    public override string Title{
        get;
        protected set;
    } = "New Prompt";

    public override string Discription{
        get;
        protected set;
    } = "allows the user to add a new Journal prompt.";

    public override List<string> CommandInputs{
        get;
        protected set;
    } = new List<string>{
        "new prompt", "4"
    };

    public override void run(){


        PromptJson getPrompts = Program.loadPrompts();

        // get the data from the format object in a form we can easily read.
        List<string> promptList = getPrompts.prompts;

        string newPrompt = Program.input("Input the new prompt: ");

        promptList.Add(newPrompt);

        // save the modified list to a new instance of the format object
        Program.savePrompts(promptList);
    }

}

class wipeEntries:functionClass{

    public override string Title{
        get;
        protected set;
    } = "Wipe Entries";


    public override string Discription{
        get;
        protected set;
    } = "deletes all journal entreies. IMPOSIPLE TO REVERCE.";

    public override List<string> CommandInputs{
        get;
        protected set;
    } = new List<string>{
        "wipe entries"
    };

    public override void run(){
        Program.saveEntries(null, null);

        Program.print("All entreis wiped.");
    }

}


class removeEntry:functionClass{
    public override string Title{
        get;
        protected set;
    } = "Remove Entry";

    public override string Discription{
        get;
        protected set;
    } = "Removes a spesific entry by date";

    public override List<string> CommandInputs{
        get;
        protected set;
    } = new List<string>{
        "remove", "remove entry", "5"
    };

    public override void run(){

        DateDataType inputDate = getDate();

        if (inputDate.validDate == false){
            throw new Exception($"Date invalid. Check getDate function.");
        }

        EntriesJson entryJson = Program.loadEntries();

        List<string> dateList = entryJson.datesWithEntires;
        List<string> entryList = entryJson.encodedEntries;

        if (dateList.Contains(inputDate.display()) == false){
            Program.print("There is no entry with that date.");
        }
        else{
            int index = dateList.IndexOf(inputDate.display());

            dateList.RemoveAt(index);
            entryList.RemoveAt(index);

            Program.saveEntries(dateList,entryList);

            Program.print("entry removed");
        }



    }

    private DateDataType getDate(){
        bool validDate = false;
        DateDataType inputDate = new DateDataType();

        while (validDate == false){
            string userInput = Program.input(
                "Input a date in the format MM/DD/YYYY: "
            );

            // check that the /s are in the right places
            string char2 = char.ToString(userInput[2]);
            string char5 = char.ToString(userInput[5]);

            if ((char2.Equals("/") == false) || (char5.Equals("/") == false)){
                
                validDate = false;
                Program.print("You must input a date in the format MM/DD/YYYY. " + 
                $"charicters 3 and 6 must be '/'. you input them as " + 
                $"'{userInput[2]}' and '{userInput[5]}'. Please Try again.\n");
            }
            else{
                string[] splitInput = userInput.Split("/");

                // check that there were only 3 slashes
                if (splitInput.Length != 3){
                    validDate = false;
                    Program.print("You must input a date in the format MM/DD/YYYY. " + 
                    $" As such there must be 2 '/' charicters. You input " + 
                    $"{splitInput.Length-1}. Please try again.\n");
                }
                else {
                    string splitMonth = splitInput[0];
                    int month;

                    string splitDay = splitInput[1];
                    int day;

                    string splitYear = splitInput[2];
                    int year;

                    // check the length of each value
                    if (splitMonth.Length != 2){
                        validDate = false;
                        Program.print("You must input a date in the format MM/DD/YYYY. " + 
                        $" As such there the month values must consist of 2 charicters. You input " + 
                        $"{splitMonth.Length}. Please try again.\n");
                    }

                    else if (splitDay.Length != 2){
                        validDate = false;
                        Program.print("You must input a date in the format MM/DD/YYYY. " + 
                        $" As such there the day values must consist of 2 charicters. You input " + 
                        $"{splitDay.Length}. Please try again.\n");
                    }
                    else if (splitYear.Length != 4){
                        validDate = false;
                        Program.print("You must input a date in the format MM/DD/YYYY. " + 
                        $" As such there the year values must consist of 4 charicters. You input " + 
                        $"{splitYear.Length}. Please try again.\n");
                    }

                    // chec that each value can be parced to an int.
                    else if (int.TryParse(splitMonth, out month) == false){
                        validDate = false;
                        Program.print("The month value must be a number. " + 
                        "Please try again.\n");
                    }
                    else if(int.TryParse(splitDay, out day) == false){
                        validDate = false;
                        Program.print("The day value must be a number. " + 
                        "Please try again.\n");
                    }
                    else if(int.TryParse(splitYear, out year) == false){
                        validDate = false;
                        Program.print("The year value must be a number. " + 
                        "Please try again.\n");
                    }
                    else{
                        inputDate = new DateDataType(
                            day, month, year
                        );

                        if (inputDate.validDate == false){
                            Program.print(inputDate.errorMsg + $"\n");
                        }
                        else{
                            validDate = true;
                        }
                    }
                }

                
            }

        }
        // END WHILE LOOP

        return inputDate;
    }
}

// Author: Evan Riker