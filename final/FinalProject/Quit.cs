/// <summary>
/// Function Class <c>quit</c> stops the program.
/// </summary>
class QuitFunction:functionClass{

    public QuitFunction(){
        Title = "Quit";

        Discription = "Quits the program";

        CommandInputs = new List<string>{
            "quit", "exit", "stop"
        };

        FlagObjects = new List<flagClass>{
            new changeExitMsgFlag()
        };

        FlagRegistry = new Dictionary<string, flagClass>();

        constructFlagRegistry();
    }

    protected override void runNoFlag(){
        Quit.run();
    }
}


static class Quit{
    public static string exitMsg = "";

    public static void run(){
        if (exitMsg == ""){
            Program.running = false;
        }
        else{
            Program.print(exitMsg);
            Program.running = false;
        }
    }
}

class changeExitMsgFlag:flagClass{

    public changeExitMsgFlag(){
        Title = "Exit Message";
        Discription = "allows the user to set message that prints as the program ends.";
        Flags = new List<string>{
            "-exitMsg", "-exitmsg", "-em"
        };
        Paramiters = "string peram - sting to output";
    }
    

    public override void run(string peramiter){
        if (peramiter == ""){
            Program.print($"{Title} requires a peramiter");
        }
        else{
            Quit.exitMsg = peramiter;
        }
    }
}