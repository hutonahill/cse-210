class newGoal:functionClass{
    public newGoal(){
        Title = "New Goal";

        Discription = "Creates a new Goal";

        CommandInputs = new List<string>{
            "ng", "newgoal", "newGoal"
        };

        FlagObjects = new List<flagClass>{
        };

        FlagRegistry = new Dictionary<string, flagClass>();

        constructFlagRegistry();
    }

    protected override void runNoFlag(){
        GoalRegistry.newGoal($"{Program.Title}/{Title}");
    }

}

