class DeleteGoal:functionClass{
    public DeleteGoal(){
        Title = "Delete Goal";

        Discription = "Deleates a goal";

        CommandInputs = new List<string>{
            "delg", "deletegoal", "deleteGoal", "delG"
        };

        FlagObjects = new List<flagClass>{
            new GoalToDeleteFlag()
        };

        FlagRegistry = new Dictionary<string, flagClass>();

        constructFlagRegistry();
    }

    protected override void runNoFlag(){
        GoalRegistry.DeleteGoal($"{Program.Title}/{Title}");
    }
}

class GoalToDeleteFlag:flagClass{

    public GoalToDeleteFlag(){
        Title = "Goal To Delete";
        Discription = "Allows the user to pass in the goal to delete in the command.";
        Flags = new List<string>{
            "-goaltodelete", "-goalToDelete", "-gtdel", "-gTDel", "-title"
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

