using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello Sandbox World!");
    }
}

class entryDataType{
    
    private string prompt;
    private string entry;

    private DateDataType date;

    // setters
    public void setPrompt(string promptValue){
        prompt = promptValue;
    }

    public void setEntry(string entryValue){
        entry = entryValue;
    }

    public Boolean setDate(DateDataType dateValue){
        
        bool validInput = false;

        // check to make sure all three values of DateDataType are
        // are included.

        return validInput;
    }

    // Getters

    public string getPrompt(){
        return prompt;
    }

    public string getEntry(){
        return entry;
    }

    public DateDataType GetDateDataType(){
        return date;
    }

    // fucntions
    public string display(){

        string output = "";

        return output;

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
            yearError.raiseError("You must input a year greater than 0.")
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
        return($"{month}/{day}/{year}")
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