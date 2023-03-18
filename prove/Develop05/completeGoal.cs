class compliteGoal:functionClass{
    public compliteGoal(){
        Title = "Complite Goal";

        Discription = "marks a goal as complite";

        CommandInputs = new List<string>{
            "cg", "complitegoal", "compliteGoal"
        };

        FlagObjects = new List<flagClass>{
        };

        FlagRegistry = new Dictionary<string, flagClass>();

        constructFlagRegistry();
    }

    protected override void runNoFlag(){
        GoalRegistry.completeGoal($"{Program.Title}/{Title}");
    }

}