using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Globalization;

class Program{
    
    private static List<string> newEntryCmds = new List<string>{
        "entry", "new", "new entry", "1"
    };

    private static List<string> viewEntriesCmds = new List<string>{
        "old", "view", "view entries", "2"
    };

    private static List<string> quitCmds = new List<string>{
        "quit", "exit", "3"
    };

    private static List<string> newPromptCmds = new List<string>{
        "new prompt", "4"
    };

    private static List<string> helpCmds = new List<string>{
        "help", "5"
    };

    private static List<string> wipeCmds = new List<string>{
        "wipe entries"
    };

    static void Main(string[] args){

        bool running = true;
        
        while (running == true){
            

            print($"\nWelcome to Journal.cs\n");

            bool validInput = false;

            string path = "";
            string userInput;
            // create lists that contain valid commands
            

            // get input from the user to determin what to do.
            while (validInput == false){
                userInput = input("Would you like to make " +
                "a new entry, view entries or quit? (1/2/3): ").ToLower();

                // entry command
                if (newEntryCmds.Contains(userInput)){
                        path = "new";
                        validInput = true;
                }

                // 
                else if (viewEntriesCmds.Contains(userInput)){
                        path = "old";
                        validInput = true;
                }

                else if (quitCmds.Contains(userInput)){
                    path = "quit";
                    validInput = true;
                }

                else if (newPromptCmds.Contains(userInput)){
                    path = "prompt";
                    validInput = true;
                }

                else if (helpCmds.Contains(userInput)){
                    path = "help";
                    validInput = true;
                }

                else if (wipeCmds.Contains(userInput)){
                    path = "wipe";
                    validInput = true;
                }

                else{
                    print("that command is not recognised. Please try agean, or " + 
                    "type 'help'");
                }

            }

            if (path == "new"){
                newEntry();
            }

            else if(path == "old"){
                viewEntries();
            }

            else if(path == "quit"){
                running = false;
            }

            else if(path == "prompt"){
                newPrompt();
            }

            else if (path == "help"){
                help();
            }

            else if (path == "wipe"){
                wipe();
            }

            else{
                throw new Exception($"Invalid Path: {path}");
            }

            print("");

        }
        
        print($"Stoped. \n");


        
    }


    static void newEntry(){

        // read a list of dates with posts and the posts
        string jsonString = File.ReadAllText(@"entries.json");
        EntriesJson getEntries = JsonSerializer.Deserialize<EntriesJson>(jsonString)!;

        List<string> datesWithEntiresExtracted = getEntries.datesWithEntires;

        List<string> entriesExtracted = getEntries.entries;

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
            print("You have already made an entry today, " + 
            "Please come back tomarrow.");
        }

        else{
            //read the prompts into a list
            string rawPrompt =  File.ReadAllText(@"prompts.json");
            promptJson getPrompts = JsonSerializer.Deserialize<promptJson>(rawPrompt)!;

            List<string> prompts = getPrompts.prompts;

            // pick a random prompt from the prompts list
            Random randomGen = new Random();

            string displayPrompt = prompts[randomGen.Next(prompts.Count)];

            // display the prompt to the user
            print(displayPrompt);

            // Get the users responce.
            string userInput = input();


            EntryDataType todaysEntry = new EntryDataType(displayPrompt, 
            userInput, today);

            // save the new entry to the lists
            datesWithEntiresExtracted.Add(today.display());
            entriesExtracted.Add(todaysEntry.encode());

            // save the modified lists to a format object
            EntriesJson entriesJson = new EntriesJson{
                datesWithEntires = datesWithEntiresExtracted,
                entries = entriesExtracted
            };

            // parce the format object into a sting
            jsonString = JsonSerializer.Serialize<EntriesJson>(entriesJson);

            // save the string to a file
            File.WriteAllText("entries.json", jsonString);
        }

    }

    static void viewEntries(){

        // read the contance of the file to a string.
        string jsonString = File.ReadAllText(@"entries.json");

        // parce the string into the format object
        EntriesJson getEntries = JsonSerializer.Deserialize<EntriesJson>(jsonString)!;

        // get the data from the format object in a form we can easily read.
        List<EntryDataType> entryList = new List<EntryDataType>();
        List<string> entriesExtracted = getEntries.entries;

        // check to make sure there are entreis to read
        if (entriesExtracted == null){
            print("there are no posts");
        }
        else{
            // for each item add the data to a new entryDataType object and display
            // and display it to the user.
            for (int i = 0; i < entriesExtracted.Count; i++){
                entryList.Add(new EntryDataType());

                entryList[i].decode(getEntries.entries[i]);

                print();
                print(entryList[i].display());
                
            }
        }
        
        
    }

    static void help(){

        //create input string
        string newEntryList = cmdListDisplay(newEntryCmds);

        string viewEntriesList = cmdListDisplay(viewEntriesCmds);

        string quitList = cmdListDisplay(quitCmds);

        string newPromptList = cmdListDisplay(newPromptCmds);

        string wipeList = cmdListDisplay(wipeCmds);

        string output = ($"Commands:\n1 New Entry:" + 
            $"\n    Valid inputs: {newEntryList}" + 
            $"\n    Discription: Offers the user a prompt and lets them input " + 
            " a jorunal entry." + 
            $"\n" +
            $"\n2 View Entries:" + 
            $"\n    Valid inputs: {viewEntriesList}" + 
            $"\n    Discription: Prints all posts in order of input." + 
            $"\n" + 
            $"\n3 Quit:" + 
            $"\n    Valid inputs: {quitList}" + 
            $"\n    Discription: exits the application." + 
            $"\n" + 
            $"\n4 New Prompt:" + 
            $"\n    Valid inputs: {newPromptList}" + 
            $"\n    Discripiton: allows the user to add a new Journal prompt." +
            $"\n" + 
            $"\n5 Wipe Entries: " + 
            $"\n    Valid inputs: {wipeList} " + 
            $"\n    Discription: deletes all journal entreies. " +
            "IMPOSIPLE TO REVERCE.");
        
        print(output);
    }

    static string cmdListDisplay(List<string> cmdList){
        string output = "";
        string copyData;

        // format each command
        for (int i = 0; i< cmdList.Count; i++){
            copyData = $"'{cmdList[i]}',";
            output = output + copyData;
        }
        
        // remove the ',' from the end of teh string
        output = output.Remove(output.Length - 1, 1);

        return output;
    }

    static void newPrompt(){
        // read the contance of the file to a string.
        string jsonString = File.ReadAllText(@"prompts.json");

        // parce the string into the format object
        promptJson getPrompts = JsonSerializer.Deserialize<promptJson>(jsonString)!;

        // get the data from the format object in a form we can easily read.
        List<string> promptList = getPrompts.prompts;

        string newPrompt = input("Input the new prompt: ");

        promptList.Add(newPrompt);

        // save the modified list to a new instance of the format object
        promptJson savePrompts = new promptJson{
            prompts = promptList
        };

        // parce the object into a json string
        jsonString = JsonSerializer.Serialize<promptJson>(savePrompts);

        // save the parced string to a file.
        File.WriteAllText(@"prompts.json", jsonString);




    }

    static void wipe(){
        

        EntriesJson blankEntries = new EntriesJson();

        string jsonString = JsonSerializer.Serialize<EntriesJson>(blankEntries);
        
        File.WriteAllText(@"entries.json", jsonString);

        print("All entreis wiped.");
    }


    static void print(string msg){
        Console.WriteLine(msg);
    }

    static void print(){
        Console.WriteLine();
    }

    static string input(string prompt){
        Console.Write(prompt);
        return Console.ReadLine();
    }

    static string input(){
        return Console.ReadLine();
    }
}

// a class define the structure of prompt.json
class promptJson{
    public List<string> prompts { get; set; }
}

// a class to define the structure of entries.json
class EntriesJson{
    public List<string> datesWithEntires { get; set; }
    public List<string> entries { get; set; }
}

class EntryDataType{
    
    private string prompt;
    private string responce;

    private DateDataType date;

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

    // Getters

    public string getPrompt(){
        return prompt;
    }

    public string getResponce(){
        return responce;
    }

    public DateDataType GetDateDataType(){
        return date;
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
    private int day = -1;
    private int month = -1;
    private int year = -1;

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
                    throw new ArgumentException(dayError.readMsg());
                }
            }
            else{
                throw new ArgumentException(monthError.readMsg());
            }
        }
        else{
            throw new ArgumentException(yearError.readMsg());
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

    // Getters
    public int getDay(){
        return day;
    }

    public int getMonth(){
        return month;
    }

    public int getYear(){
        return year;
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

