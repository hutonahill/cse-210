using System;
using System.Text.Json;
using System.Text.Json.Serialization;

class Program
{
    private List<DateDataType> dates;

    static void Main(string[] args){

        bool running = true;
        
        while (running == true){
            //read the prompts into a list
            string text =  File.ReadAllText(@"prompts.json");
            var getPrompts = JsonSerializer.Deserialize<promptJson>(text);

            List<string> prompts = getPrompts.prompts;

            // read a list of dates with posts and the posts

            text = File.ReadAllText(@"entries.json");
            var getEntries = JsonSerializer.Deserialize<entriesJson>(text);

            List<string> datesWithEntires = getEntries.datesWithEntires;

            List<entryDataType> entries = getEntries.entries;


            print("Welcome to Journal.cs");

            print("Would you like to make a new entry, view entries or quit? (1/2/3)");

            bool validInput = false;

            string path = "";
            string userInput;

            // get input from the user to determin what to do.
            while (validInput == false){
                userInput = input().ToLower();

                if (userInput == "entry" || userInput == "new" || 
                    userInput == "new entry"){
                        path = "new";
                        validInput = true;
                }

                else if (userInput == "old" || userInput == "view" || 
                    userInput == "view entries" || userInput == "2"){
                        path = "old";
                        validInput = true;
                }

                else if (userInput == "quit" || userInput == "3"){
                    path = "quit";
                    validInput = true;
                }

                else if (userInput == "new prompt" || userInput == "4"){
                    path = "prompt";
                    validInput = true;
                }

                else if (userInput == "help" || userInput == "5"){
                    path = "help";
                    validInput = true;
                }

                else if (userInput == "wipe entries"){
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



            // write data to the file when done
            List<entriesJson> writeEntries = new List<entriesJson>();

            writeEntries.Add(new entriesJson(){
                // datesWithEntires = 
                // entries = 
            });
            
            string json = JsonSerializer.Serialize(entries);
            File.WriteAllText(@"entries.json", json);
        }
    }

    static void newEntry(){
        print("entry");
    }

    static void viewEntries(){
        print("view");
    }

    static void help(){
        string output = ($"1 New Entry:" + 
            $"\n    Valid inputs: 'new', 'new entry', '1'" + 
            $"\n    Discription: Offers the user a prompt and lets them input " + 
            " a jorunal entry." + 
            $"\n" +
            $"\n2 View Entries:" + 
            $"\n    Valid inputs: 'old', 'view', 'view entries', '2'" + 
            $"\n    Discription: Prints all posts in order of input." + 
            $"\n" + 
            $"\n3 Quit:" + 
            $"\n    Valid inputs: 'quit', '3'" + 
            $"\n    Discription: exits the application." + 
            $"\n" + 
            $"\n4 New Prompt:" + 
            $"\n    Valid inputs: 'new prompt', '4'" + 
            $"\n    Discripiton: allows the user to add a new Journal prompt." +
            $"\n" + 
            $"\n5 Wipe Entries: " + 
            $"\n    Valid inputs: 'wipe entries' " + 
            $"\n    Discription: deletes all journal entreies. " +
            "IMPOSIPLE TO REVERCE.");
        
        print(output);
    }

    static void newPrompt(){
        print("new");
    }

    static void wipe(){
        print("wipe");
    }


    static void print(string msg){
        Console.WriteLine(msg);
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
class entriesJson{
    public List<string> datesWithEntires { get; set; }
    public List<entryDataType> entries { get; set; }
}

class entryDataType{
    
    private string prompt;
    private string responce;

    private DateDataType date;

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

        error yearError = checkYear(yearValue);
        if (yearError.checkError() == false){
            year = yearValue;

            error monthError = checkMonth(monthValue);

            if (monthError.checkError() == false){
                month = monthValue;

                error dayError = checkDay(dayValue, month, year);

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
    public error setDay(int dayValue){
        
        error dayError;

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

    public error setMonth(int monthValue){
        error monthError = checkMonth(monthValue);

        if (monthError.checkError() == false){
            month = monthValue;
        }

        return monthError;
    }

    public error setYear(int yearValue){
        error yearError = checkYear(yearValue);

        if (yearError.checkError() == false){
            year = yearValue;

            isLeapYear = isYearLeap(year);
        }


        return yearError;
    }

    // Error checkers

    private error checkDay(int dayValue){
        error dayError = new error();

        if (((1 <= dayValue) || (dayValue <= 31)) == false){
            dayError.raiseError("You must input a day between 1 and 31");
        }

        return dayError;
    }

    private error checkDay(int dayValue, int monthValue){
        error dayError = new error();

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

    private error checkDay(int dayValue, int monthValue, int yearValue){
        error dayError = new error();

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

    private error checkMonth(int monthValue){
        error monthError = new error();
        
        if ((1 <= monthValue) && (monthValue <= 12) == false){
            monthError.raiseError("You must input a month between 1 and 12");
        }

        return monthError;
    }
    
    private error checkYear(int yearValue){
        error yearError = new error();
        
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

class error{
    private bool isError = false;

    private string errorMsg = "";

    public error(){

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