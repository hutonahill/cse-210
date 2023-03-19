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

class GoalToCompliteFlag:flagClass{

    public GoalToCompliteFlag(){
        Title = "Goal To Complite";
        Discription = "Allows the user to pass in the goal to complite in the command.";
        Flags = new List<string>{
            "-goaltocomplite", "-goalToComplite", "-gtc", "-gTC", "-title"
        };
        Paramiters = "string title - the title of the goal to delete";
    }
    

    public override void run(string peramiter){
        if (peramiter == ""){
            Program.print($"{Title} requires a peramiter");
        }
        else{
            GoalRegistry.TitleToDelete = peramiter;
        }
    }
}