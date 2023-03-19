class editPoints:functionClass{
    public editPoints(){
        Title = "Edit Points";

        Discription = "Edit the number of points the user has";

        CommandInputs = new List<string>{
            "ep", "editpoints", "editPoints", "eP"
        };

        FlagObjects = new List<flagClass>{
        };

        FlagRegistry = new Dictionary<string, flagClass>();

        constructFlagRegistry();
    }

    protected override void runNoFlag(){
        GoalRegistry.editScore();
    }

}