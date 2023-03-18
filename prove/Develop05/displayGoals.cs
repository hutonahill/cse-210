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