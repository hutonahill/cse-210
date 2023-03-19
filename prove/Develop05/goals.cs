using System.Globalization;
using System.Text;
using System.Text.Json;

public static class GoalRegistry{

    static GoalRegistry(){
        loadGoals();
    }

    public static string goalType = "";
    public static void newGoal(string CommandPath){
        
        
        CommandPath = CommandPath + "/newGoal";

        bool validInput = false;

        while (validInput == false){
            string inputType;
            if (goalType == ""){
                inputType = Program.input("What type of goal would you like to set?").ToLower();
            }
            else{
                inputType = goalType.ToLower();
            }
            
            if (inputType == "back"){
                validInput = true;
            }

            else if (inputType == "simple"){
                validInput = true;
                newSimpleGoal(CommandPath);
            }
            else if (inputType == "eternal"){
                validInput = true;
                newEternalGoal(CommandPath);
            }
            else if ((inputType == "checklist") || (inputType == "list")){
                validInput = true;
                newChecklistGoal(CommandPath);
            }
            else {
                Program.print("You must input 'simple', 'eternal', or 'checklist'.");
                
                // if the goal type was passed in via the command, instead as 
                // part of the newGoal() method, dont loop.
                if (goalType != ""){
                    validInput = true;
                }
            }
        }

        goalType = "";
        
    }


    public static string goalTitle = "";

    private static void newSimpleGoal(string CommandPath){
        string title = "";

        bool validInput = false;
        bool error = false;

        List<string> keys = Goals.Keys.ToList();

        while (validInput == false){
            if (goalTitle == ""){
                title = Program.input("What should this simple goal be called: ");
            }
            else{
                title = goalTitle;
            }
            

            if (title == "back"){
                validInput = true;
            }
            else if (keys.Contains(title.ToLower())){
                Program.print("That title already exists. Please try again.");

                if (goalTitle != ""){
                    validInput = true;
                    error = true;
                }
            }
            else{
                validInput = true;
            }
        }

        if (title.ToLower() != "back" && error == false){

            
            string discription = Program.input($"Please discribe {title} goal: ");

            if (discription.ToLower() != "back"){
                

                int points = 0;
                validInput = false;

                string rawPts = "";

                while(validInput == false){
                    rawPts = Program.input($"How many points should you earn when you complete {title}: ");
                    
                    if (rawPts.ToLower() == "back"){
                        validInput = true;
                    }
                    else if (int.TryParse(rawPts, out points) == false){
                        Program.print("You must input an intiger");
                    }
                    else{
                        validInput = true;
                    }
                }

                if (rawPts.ToLower() != "back"){
                    

                    Simple simple = new Simple(title, discription, points);

                    Goals[title.ToLower()] = simple;
                }
            }
        }

        goalTitle = "";
        saveGoals();
    }

    private static void newEternalGoal(string CommandPath){
        
        string title = "";

        bool validInput = false;
        bool error = false;

        List<string> keys = Goals.Keys.ToList();

        while (validInput == false){
            if (goalTitle == ""){
                title = Program.input("What should this eternal goal be called: ");
            }
            else{
                title = goalTitle;
            }
            

            if (title == "back"){
                validInput = true;
            }
            else if (keys.Contains(title.ToLower())){
                Program.print("That title already exists. Please try again.");

                if (goalTitle != ""){
                    validInput = true;
                    error = true;
                }
            }
            else{
                validInput = true;
            }
        }

        if (title.ToLower() != "back" && error == false){

            
            string discription = Program.input($"Please discribe {title} goal: ");

            if (discription.ToLower() != "back"){
                

                int points = 0;
                validInput = false;

                string rawPts = "";

                while(validInput == false){
                    rawPts = Program.input($"How many points should you earn when you complete {title}: ");
                    
                    if (rawPts.ToLower() == "back"){
                        validInput = true;
                    }
                    else if (int.TryParse(rawPts, out points) == false){
                        Program.print("You must input an intiger");
                    }
                    else{
                        validInput = true;
                    }
                }

                if (rawPts.ToLower() != "back"){
                    

                    Eternal eternal = new Eternal(title, discription, points);

                    Goals[title.ToLower()] = eternal;
                }
            }
        }

        goalTitle = "";
        saveGoals();
    }

    private static void newChecklistGoal(string CommandPath){

        string title = "";

        bool validInput = false;
        bool error = false;

        List<string> keys = Goals.Keys.ToList();

        while (validInput == false){
            if (goalTitle == ""){
                title = Program.input("What should this checklist goal be called: ");
            }
            else{
                title = goalTitle;
            }
            

            if (title == "back"){
                validInput = true;
            }
            else if (keys.Contains(title.ToLower())){
                Program.print("That title already exists. Please try again.");

                if (goalTitle != ""){
                    validInput = true;
                    error = true;
                }
            }
            else{
                validInput = true;
            }
        }

        if (title.ToLower() != "back" && error == false){

            
            string discription = Program.input($"Please discribe {title} goal: ");

            if (discription.ToLower() != "back"){
                

                int points = 0;
                validInput = false;

                string rawPts = "";

                while(validInput == false){
                    rawPts = Program.input($"How many points should you earn when you complete {title}: ");
                    
                    if (rawPts.ToLower() == "back"){
                        validInput = true;
                    }
                    else if (int.TryParse(rawPts, out points) == false){
                        Program.print("You must input an intiger");
                    }
                    else{
                        validInput = true;
                    }
                }

                if (rawPts.ToLower() != "back"){

                    int iderationPoints = 0;
                    validInput = false;

                    rawPts = "";

                    while(validInput == false){
                        rawPts = Program.input($"How many points should you earn when you complete one ideration {title}: ");
                        
                        if (rawPts.ToLower() == "back"){
                            validInput = true;
                        }
                        else if (int.TryParse(rawPts, out iderationPoints) == false){
                            Program.print("You must input an intiger");
                        }
                        else{
                            validInput = true;
                        }
                    }

                    if (rawPts.ToLower() != "back"){

                        int numIderations = 0;
                        validInput = false;

                        rawPts = "";

                        while(validInput == false){
                            rawPts = Program.input($"How many times would you like to complite {title}: ");
                            
                            if (rawPts.ToLower() == "back"){
                                validInput = true;
                            }
                            else if (int.TryParse(rawPts, out numIderations) == false){
                                Program.print("You must input an intiger");
                            }
                            else{
                                validInput = true;
                            }
                        }
                        if (rawPts.ToLower() != "back"){
                            Checklist checklist = new Checklist(title, discription, points, numIderations, iderationPoints );

                            Goals[title.ToLower()] = checklist;
                        }
                    }
                }
            }
        }

        goalTitle = "";
        saveGoals();
    }


    public static string TitleToComplite = "";

    public static void completeGoal(string CommandPath){

        CommandPath = CommandPath + "/CompleteGoal";
        string inputTitle = "";

        List<string> keys = Goals.Keys.ToList();

        bool validInput = false;
        while (validInput == false){

            if (TitleToComplite != ""){
                inputTitle = TitleToComplite;
            }
            else{
                inputTitle = Program.input("What goal would you like to complete: ");
            }
            

            if (inputTitle.ToLower() == "back"){
                validInput = true;
            }
            else if (inputTitle.ToLower() == "help"){
                displayGoals(CommandPath);
            }
            else if (keys.Contains(inputTitle) == false){
                Program.print($"There is no stored goal with the tile: {inputTitle}");
            }
            else{
                validInput = true;
                Goals[inputTitle].onCompletion();
                TitleToComplite = "";
                saveGoals();
            }
        }
    }

    public static string TitleToDisplay = "";

    public static void displayGoals(string CommandPath){
        List<string> keys = Goals.Keys.ToList();

        if (TitleToDisplay == ""){
            foreach(string key in keys){
                Program.print(Goals[key].display());
            }

            if (keys.Count == 0){
                Program.print("No Goals");
            }
        }
        else{
            if (keys.Contains(TitleToDisplay) == false){
                Program.print($"Threre is no such goal '{TitleToDisplay}'.");
            }
            else{
                Program.print(Goals[TitleToDisplay].display());
            }

            TitleToDisplay = "";
        }
        
    }


    private static string goalPath = "goals.json";

    private static void saveGoals(){
        if (File.Exists(goalPath) == false){
            File.Create(goalPath);
        }

        List<string> lst = new List<string>();

        // convert a list of objects to a list of strings
        foreach(string gck in Goals.Keys){
            lst.Add(Goals[gck].endcode());
        }

        lst.Insert(0, $"{UserScore}");

        GoalJson goalJson = new GoalJson(){
            goalList = lst
        };

        string jsonString = JsonSerializer.Serialize<GoalJson>(goalJson);
        File.WriteAllText(goalPath, jsonString);
    }

    private static void loadGoals(){
        if (File.Exists(goalPath) == true){
            string jsonString = File.ReadAllText(goalPath);

            if (jsonString != ""){
                GoalJson reflectionJson =
                JsonSerializer.Deserialize<GoalJson>(jsonString)!;

                List<string> rawList = reflectionJson.goalList;

                UserScore = int.Parse(rawList[0]);
                rawList.RemoveAt(0);

                List<goalClass> lst = new List<goalClass>();

                foreach (string codedClass in rawList){

                    if (codedClass.StartsWith("Checklist\\\\")){
                        Checklist checklist = new Checklist();
                        checklist.decode(codedClass);
                        lst.Add(checklist);
                    }
                    else if (codedClass.StartsWith("Eternal\\\\")){
                        Eternal eternal = new Eternal();
                        eternal.decode(codedClass);
                        lst.Add(eternal);
                    }
                    else if (codedClass.StartsWith("Simple\\\\")){
                        Simple simple = new Simple();
                        simple.decode(codedClass);
                        lst.Add(simple);
                    }
                }

                foreach (goalClass gc in lst){
                    Goals[gc.Title] = gc;
                }
            }
            else{
                Goals = new Dictionary<string, goalClass>();
            }
            
        }
        else{
            File.Create(goalPath);
            Goals = new Dictionary<string, goalClass>();
        }
    }


    private static Dictionary<string, goalClass> Goals = new Dictionary<string, goalClass>();

    public static int UserScore = 0;

    public static string passToEditScore = null;

    public static void editScore(){
        

        bool validInput = false;

        while (validInput == false){
            string rawInput;
            if (passToEditScore == null){
                rawInput = Program.input("Input a value to add to user score: ");
            }
            else{
                rawInput = passToEditScore;
            }

            int scoreEditValue;

            if (rawInput == "back"){
                validInput = true;
            }
            else if (int.TryParse(rawInput, out scoreEditValue) == false){
                if (passToEditScore != null){
                    validInput = true;
                }
            }
            else{
                validInput = true;
                UserScore = UserScore + scoreEditValue;

                Program.print($"{UserScore} Points");
            }
        }

        passToEditScore = null;
        saveGoals();
    }

    public static string TitleToDelete = null;

    public static void DeleteGoal(string CommandPath){
        
        CommandPath = CommandPath + "/DeleteGoal";
        
        string inputTitle = "";

        List<string> keys = Goals.Keys.ToList();

        bool validInput = false;
        while (validInput == false){

            if (TitleToComplite != null){
                inputTitle = TitleToDelete;
            }
            else{
                inputTitle = Program.input("What goal would you like to complete: ");
            }
            

            if (inputTitle.ToLower() == "back"){
                validInput = true;
            }
            else if (inputTitle.ToLower() == "help"){
                displayGoals(CommandPath);
            }
            else if (keys.Contains(inputTitle) == false){
                Program.print($"There is no stored goal with the tile: {inputTitle}");
            }
            else{
                validInput = true;
                Goals.Remove(inputTitle);
                Program.print($"{inputTitle} Goal Removed.");
            }
        }

        saveGoals();
    }
}

class GoalJson{
    public List<string> goalList { get; set; }
}


public class Checklist:goalClass{
    
    public int IderationsToComplite{
        get;
        private set;
    }

    public int CurrentIderation{
        get;
        private set;
    }

    public int ScorePerIderation{
        get;
        private set;
    } 
    
    public Checklist(string title, string discription, int scoreOnComplete, int iderationsToComplite, int scorePerIderation, int currentIderation = 0){
        Title = title;
        Discription = discription;
        ScoreOnComplete = scoreOnComplete;
        IderationsToComplite = iderationsToComplite;
        currentIderation = CurrentIderation;
        ScorePerIderation = scorePerIderation;
        
    }

    public Checklist(){}

    public override void onCompletion(){
        
        if (CurrentIderation < IderationsToComplite){
            CurrentIderation ++;

            GoalRegistry.UserScore = GoalRegistry.UserScore + ScorePerIderation;

            if (CurrentIderation >= IderationsToComplite){
                GoalRegistry.UserScore = GoalRegistry.UserScore + ScoreOnComplete ?? 0 ;
                
                IsComplite = true;
            }
        }
        else{
            Program.print($"{Title} is already complete");
        }
        
    }

    public override string display(){
        if (IsComplite == false){
            return $"{Title} [{CurrentIderation}/{IderationsToComplite}]";
        }
        else{
            return $"{Title} [X]";
        }
    }

    public override string endcode(){
        return $"Checklist\\\\{Title}\\\\{Discription}\\\\{ScoreOnComplete}\\\\{IsComplite}\\\\{ScorePerIderation}\\\\{CurrentIderation}/{IderationsToComplite}";
    }

    public override void decode(string encodedClass){
        if (encodedClass.StartsWith("Checklist\\\\") == false){
            throw new ArgumentException("Invalid encoded string format");
        }

        string[] splitCode = encodedClass.Split("\\\\");

        Title = splitCode[1];

        Discription = splitCode[2];

        int copyInt;

        if (int.TryParse(splitCode[3], out copyInt) == true){
            ScoreOnComplete = copyInt;
        }
        else{
            throw new ArgumentException("Invalid encoded string format, ScoreOnComplite not int");
        }

        bool copyBool;

        if (bool.TryParse(splitCode[4], out copyBool)){
            IsComplite = copyBool;
        }
        else{
            throw new ArgumentException("Invalid encoded string format, IsComplite not bool");
        }

        if (int.TryParse(splitCode[5], out copyInt) == true){
            ScorePerIderation = copyInt;
        }
        else{
            throw new ArgumentException("Invalid encoded string format, ScorePerIderation not int");
        }

        string[] copyStrAr = splitCode[6].Split('/');

        if (int.TryParse(copyStrAr[0], out copyInt) == true){
            CurrentIderation = copyInt;
        }
        else{
            throw new ArgumentException("Invalid encoded string format, CurrentIderation not int");
        }

        if (int.TryParse(copyStrAr[1], out copyInt) == true){
            IderationsToComplite = copyInt;
        }
        else{
            throw new ArgumentException("Invalid encoded string format, IderationsToComplite not int");
        }
    }

}

public class Eternal:goalClass{
    public Eternal(string title, string discription, int scoreOnComplete){
        Title = title;
        Discription = discription;
        ScoreOnComplete = scoreOnComplete;
        
    }

    public Eternal(){}

    public List<DateTime> TimesCompleted {
        get;
        protected set;
    } = new List<DateTime>();

    public override void onCompletion(){
        GoalRegistry.UserScore = GoalRegistry.UserScore + ScoreOnComplete ?? 0;

        TimesCompleted.Add(DateTime.Now);

        IsComplite = true;
    }

    public override string display(){
        if (IsComplite == true){
            return $"{Title} [{TimesCompleted.Count}]";
        }
        else{
            return $"{Title} [ ]";
        }
    }

    public override string endcode(){

        // the below section was generated with the help of ChatGPT

        // Create a StringBuilder to build the encoded string
        StringBuilder stirngBuilder = new StringBuilder();

        // Loop through each DateTime in the list and append its string representation
        foreach (DateTime dt in TimesCompleted){
            
            // Append the DateTime in the format "yyyy-MM-dd HH:mm:ss.fffffff"
            stirngBuilder.Append(dt.ToString("o", CultureInfo.InvariantCulture));

            // Separate each DateTime with a comma
            stirngBuilder.Append(",");
        }

        // Remove the trailing comma from the encoded string
        if (stirngBuilder.Length > 0){

            stirngBuilder.Remove(stirngBuilder.Length - 1, 1);
        }

        // Return the encoded string
        return $"Eternal\\\\{Title}\\\\{Discription}\\\\{ScoreOnComplete}\\\\{IsComplite}\\\\{stirngBuilder.ToString()}";
    }

    public override void decode(string encodedClass){
        if (encodedClass.StartsWith("Eternal\\\\") == false){
            throw new ArgumentException("Invalid encoded string format");
        }

        string[] splitCode = encodedClass.Split("\\\\");

        Title = splitCode[1];

        Discription = splitCode[2];

        int copyInt;

        if (int.TryParse(splitCode[3], out copyInt) == true){
            ScoreOnComplete = copyInt;
        }
        else{
            throw new ArgumentException("Invalid encoded string format, ScoreOnComplite not int");
        }

        bool copyBool;

        if (bool.TryParse(splitCode[4], out copyBool)){
            IsComplite = copyBool;
        }
        else{
            throw new ArgumentException("Invalid encoded string format, IsComplite not bool");
        }

        // Split the encoded string by commas to separate each DateTime
        string[] dateTimeStrings = splitCode[5].Split(',');

        // Loop through each DateTime string and parse it into a DateTime object
        foreach (string dateTimeString in dateTimeStrings){

            // Parse the DateTime string in the format "yyyy-MM-dd HH:mm:ss.fffffff"
            DateTime dt = DateTime.ParseExact(dateTimeString, "o", CultureInfo.InvariantCulture);

            // Add the parsed DateTime to the List
            TimesCompleted.Add(dt);
        }

    }
}

public class Simple:goalClass{
    public Simple(string title, string discription, int scoreOnComplete){
        Title = title;
        Discription = discription;
        ScoreOnComplete = scoreOnComplete;
        
    }

    public Simple(){}

    public override void onCompletion(){
        GoalRegistry.UserScore = GoalRegistry.UserScore + ScoreOnComplete ?? 0;

        IsComplite = true;
    }

    public override string display(){
        if (IsComplite == true){
            return $"{Title} [X]";
        }
        else{
            return $"{Title} [ ]";
        }
    }

    public override string endcode(){
        return $"Simple\\\\{Title}\\\\{Discription}\\\\{ScoreOnComplete}\\\\{IsComplite}";
    }

    public override void decode(string encodedClass){
        if (encodedClass.StartsWith("Simple\\\\") == false){
            throw new ArgumentException("Invalid encoded string format");
        }

        string[] splitCode = encodedClass.Split("\\\\");

        Title = splitCode[1];

        Discription = splitCode[2];

        int copyInt;

        if (int.TryParse(splitCode[3], out copyInt) == true){
            ScoreOnComplete = copyInt;
        }
        else{
            throw new ArgumentException("Invalid encoded string format, ScoreOnComplite not int");
        }

        bool copyBool;

        if (bool.TryParse(splitCode[4], out copyBool)){
            IsComplite = copyBool;
        }
        else{
            throw new ArgumentException("Invalid encoded string format, IsComplite not bool");
        }

    }

}