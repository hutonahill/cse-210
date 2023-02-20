using System;
					
public class Program{
	public static void Main(){	
		string userInput = input("test Command Parceing: ");


        string output = ExtractParameter(userInput);
        
        if (output != null){
            print(output);
        }
        

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
    /// Extracts parameter from a string.
    /// </summary>
    /// <param name="input">A string</param>
    /// <returns>A parameter (words surrounded with parentheses)</returns>
    /// <author>This code snippet was written with the help of ChatGPT.</author>
    public static string ExtractParameter(string input){
        int openParentheses = 0;
        int closeParentheses = 0;
        string result = "";

        // loop through every character in the input
        for (int i = 0; i < input.Length; i++){

            // if the current char is '('
            if (input[i] == '('){

                // If a nested parenthesis is detected,
                // inform the user and return null
                if (openParentheses > 0){
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
                if (closeParentheses > 0){
                    print(
                        "The command you input includes nested closing parentheses.  " +
                        "Please try again."
                    );
                    return null;
                }
                // if not, add the detected ')' to the count
                else{
                    closeParentheses++;

                    // make sure tehre arnt multiple perams
                    if (closeParentheses == 2){
                        print(
                            "the command you input includes two peramiters attached to one flag.  " + 
                            "Please try again."
                        );
                        return null;
                    }
                }

                // if the number of '(' and ')' are the same, add what
                // between them to the output string.
                if (closeParentheses == openParentheses){
                    
                    // add one to the starting index to not include the '('
                    int startIndex = input.LastIndexOf('(', i - 1) + 1;

                    // subtract 1 form the length so the ')' is not included
                    int length = i - startIndex;

                    // extract the enclosed string
                    result = input.Substring(startIndex, length);
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

        // If there are any open or close parentheses remaining, inform the user and return null
        if (openParentheses > 0 || closeParentheses > 0){
            print(
                "The command you input has an unclosed parentheses. " +
                "Please try again."
            );
            return null;
        }

        // Return the extracted parameter
        return result;
    }


    /// <summary>
    /// extract the command from a string.
    /// </summary>
    /// <param name="input">a string</param>
    /// <returns>a list where the first index is the command and the second is the rest of the string</returns>
    /// <author>This code snippet was written with the help of ChatGPT.</author>
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
    /// Extracts a flag and parameter as a single string from a given input string.
    /// A flag is defined as a word that starts with the '-' character. 
    /// The parameter is at the end of the flag and is surrounded by parentheses. 
    /// </summary>
    /// <param name="input">The input string to extract flags and parameters from</param>
    /// <returns>A List of strings containing the flags and their corresponding parameters (if any)</returns>
    /// <author>This code snippet was written with the help of ChatGPT.</author>
    public static List<string> ExtractFlagsAndParameters(string input){
        List<string> flags = new List<string>();
        int index = 0;

        bool running = true;

        // Continue looping until all flags are found
        while ((index < input.Length) && (running == true)){
            
            // Find the next flag
            int flagStart = input.IndexOf('-', index);
            if (flagStart == -1){
                // No more flags found, exit the loop

                running = false;
                return flags;
            }

            else{
                // Check if the flag is inside parentheses
                bool insideParentheses = false;
                for (int i = flagStart; i >= 0; i--){
                    if (input[i] == '('){
                        insideParentheses = true;
                        i = -1;
                    }
                    else if (input[i] == ')'){
                        insideParentheses = false;
                        i = -1;
                    }
                }

                if (insideParentheses == false){

                    // Find the end of the flag

                    // the end of the flag can ether be a ' ' or 
                    // a ')'
                    int nextOpen = input.IndexOf('(', flagStart);
                    int nextClose = input.IndexOf(')', flagStart);
                    int nextSpace = input.IndexOf(' ', flagStart);

                    int flagEnd;
                    
                    // if the next '(' is closer than the next ' ',
                    // then the flag is ended with a ')'
                    if (nextOpen < nextSpace){
                        flagEnd = nextClose;
                    }
                    
                    // if not its ended with a space.
                    else{
                        flagEnd = nextSpace;
                    }
                    

                    if ((flagEnd == -1)){
                        
                        // No ')' found, exit the loop
                        print(
                            "The command you input contains an unclosed parentheses.  " + 
                            "Please try again."
                        );
                        running = true;
                        return null;
                    }

                    else{
                        // Check for nested parentheses
                        int nestedStart = input.IndexOf('(', flagStart + 1);

                        while (nestedStart != -1 && nestedStart < flagEnd){
                            int nestedEnd = input.IndexOf(')', nestedStart);
                            if (nestedEnd == -1){
                                // No closing parenthesis found, exit the loop
                                return null;
                            }
                            nestedStart = input.IndexOf('(', nestedStart + 1);
                        }

                        // Add the flag with parameter to the list
                        flags.Add(input.Substring(flagStart, flagEnd - flagStart + 1));
                        index = flagEnd + 1;
                    }
                }
                else{
                    // Flag is inside parentheses, skip it
                    index = flagStart + 1;
                }
            }

        } // while index < input.Length) && (running == true)

        // Return the list of flags
        return flags;
    }

}
