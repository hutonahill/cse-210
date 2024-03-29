class diplayGoals:functionClass{
    public diplayGoals(){
        Title = "Display Goals";

        Discription = "Displays all goals";

        CommandInputs = new List<string>{
            "dg", "displaygoals", "displayGoals", "displaygoal", "displayGoal",
            "dG"
        };

        FlagObjects = new List<flagClass>{
        };

        FlagRegistry = new Dictionary<string, flagClass>();

        constructFlagRegistry();
    }

    protected override void runNoFlag(){
        GoalRegistry.displayGoals($"{Program.Title}/{Title}");
    }

}

class GoalToDisplayFlag:flagClass{

    public GoalToDisplayFlag(){
        Title = "Goal To Delete";
        Discription = "Allows the user to pass in the goal to delete in the command.";
        Flags = new List<string>{
            "-goaltodisplay", "-goalToDisplay", "-gtd", "-gTD", "-title"
        };
        Paramiters = "string title - the title of the goal to delete";
    }
    

    public override void run(string peramiter){
        if (peramiter == ""){
            Program.print($"{Title} requires a peramiter");
        }
        else{
            GoalRegistry.TitleToDisplay = peramiter;
        }
    }
}