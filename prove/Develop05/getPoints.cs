class getPoints:functionClass{
    public getPoints(){
        Title = "Get Points";

        Discription = "Show how many points the user has";

        CommandInputs = new List<string>{
            "gp", "getpoints", "getPoints", "gP"
        };

        FlagObjects = new List<flagClass>{
        };

        FlagRegistry = new Dictionary<string, flagClass>();

        constructFlagRegistry();
    }

    protected override void runNoFlag(){
        Program.print($"{GoalRegistry.UserScore} points");
    }

}