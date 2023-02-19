using System;
					
public class Program{
	public static void Main(){	
		string userInput = input("test Command Parceing: ");

	
        List<string> output = ExtractCommand(userInput);
        Console.WriteLine(output[0]); // Output: "Hello"
        Console.WriteLine(output[1]); // Output: "world, how are you?"

        

	}

    /// <summary>
    /// gets input from the user
    /// </summary>
    /// <param name="prompt">a string displayed to the user</param>
    /// <returns>the string the user input</returns>
	public static string input(string prompt){
        Console.Write(prompt);
        return Console.ReadLine();
    }
    
    /// <summary>
    /// Displays a string to the user
    /// </summary>
    /// <param name="msg">the string to be displayed to the user</param>

    public static void print(string msg){
        Console.WriteLine(msg);
    }
	
    /// <summary>
    /// extracts peramiters from a string.
    /// This code snippet was written with the help of ChatGPT.
    /// </summary>
    /// <param name="input">a string</param>
    /// <returns>A list of peramiters (words surounded with perenthisies, like this :) )</returns>
    public static List<string> ExtractParamiters(string input){
        
        int openParentheses = 0;
        int closeParentheses = 0;
        List<string> results = new List<string>();

        // loop though every charicter in the input
        for (int i = 0; i < input.Length; i++){
            
            //if the current char is '('
            if (input[i] == '('){

                // If a nested parenthesis is detected, 
                // inform the user and return null
                if (openParentheses > 0) {
                    
                    print(
                        "The command you input includes nested opening parentheses. " + 
                        "Please try again."
                    );
                    return null;
                }

                // add one to the count of '('.
                openParentheses++;
            }

            // if the current char is ')'
            else if (input[i] == ')'){

                

                // If a nested ')' is detected, 
                // inform the user and return null
                if (closeParentheses > 0) {
                    print(
                        "The command you input includes nested closing parentheses. " + 
                        "Please try again."
                    );
                    return null;
                }
                // if not, add the detected ')' to the count
                else{
                    closeParentheses++;
                }

                // if the number of '(' and ')' are the same, add what 
                // between them to the output list.
                if (closeParentheses == openParentheses){
                    
                    // add one to the starting index to not include the '('
                    int startIndex = input.LastIndexOf('(', i - 1) + 1;

                    // subtract 1 form the length so the ')' is not included
                    int length = i - startIndex;

                    // extract the enclose string
                    string result = input.Substring(startIndex, length);

                    //append the string to the output list
                    results.Add(result);

                    // remove one form the count of closed and open
                    closeParentheses --;
                    openParentheses --;
                }

                // if there are more ')' than '(' inform the user and return null
                else if (closeParentheses > openParentheses){
                    print(
                        "The command you input has a closing parentheses " + 
                        "that comes before an opening parentheses. " + 
                        "Please try again."
                    );
                    return null;
                }
            }
        }

        // make sure all parentheses are paired properly.
        if (openParentheses != closeParentheses){
            print(
                "The command you input includes unmatched parentheses." + 
                "Please try again."
            );
            return null;
        }

        return results;
    }


    /// <summary>
    /// extract the command from a string.
    /// This code snippet was written with the help of ChatGPT.
    /// </summary>
    /// <param name="input">a string</param>
    /// <returns>a list where the first index is the command and the second is the rest of the string</returns>
    public static List<string> ExtractCommand(string input){
        List<string> output = new List<string>();
        int firstSpace = input.IndexOf(' ');
        if (firstSpace == -1){
            // There is only one word in the input
            output.Add(input);
            output.Add("");
        }
        else{
            output.Add(input.Substring(0, firstSpace));
            output.Add(input.Substring(firstSpace + 1));
        }
        return output;
    }


    /// <summary>
    /// extract flags from a string
    /// This code snippet was written with the help of ChatGPT.
    /// </summary>
    /// <param name="input">a string</param>
    /// <returns>a list of flags (paramiters removed)</returns>
    public static List<string> ExtractFlags(string input){
        List<string> flags = new List<string>();

        // Split the input string into words
        string[] words = input.Split(' ');

        // Loop through each word and identify flags
        foreach (string word in words)
        {
            if (word.StartsWith("-"))
            {
                // If the flag contains a '(', remove the part after it
                int index = word.IndexOf('(');
                if (index != -1)
                {
                    flags.Add(word.Substring(0, index));
                }
                else
                {
                    flags.Add(word);
                }
            }
        }

        return flags;
    }

}
