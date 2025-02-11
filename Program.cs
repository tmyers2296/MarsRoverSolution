using System.Text.RegularExpressions;

internal class Program
{
    static void Main(string[] args)
    {
        // Grid creation:
        Grid Plateau = CreateGridFromUserInput();

        //Rover1 creation & movement:
        Rover Rover1 = CreateRoverFromUserInput(Plateau);
        MoveRoverFromUserInput(Rover1);
        Rover1.ShowPosition();

        //Rover2 creation & movement:
        Rover Rover2 = CreateRoverFromUserInput(Plateau);
        MoveRoverFromUserInput(Rover2);
        Rover2.ShowPosition();
    }

    // validates user string, keeps asking till user inputs a valid string:
    static Grid CreateGridFromUserInput()
    {
        // track whether user string is valid:
        bool UserStringValid = false;
        
        // variables to save parsed ints to:
        int MaxX = 0;
        int MaxY = 0;

        // Ask for user input until user provides valid input:
        while(!UserStringValid)
        {
            // prompt user for input:
            Console.WriteLine("\nEnter Plateau size (2 numbers separated by spaces):");
            string GridSizeString = Console.ReadLine() ?? "";

            // split string on spaces:
            string[] PotentialInts = GridSizeString.Split(' ');

            // if there are 2 potential integers:
            if (PotentialInts.Length == 2)
            {   
                if(int.TryParse(PotentialInts[0], out MaxX) && int.TryParse(PotentialInts[1], out MaxY))
                {
                    UserStringValid = true;
                }
            }

            if (!UserStringValid)
            {
                Console.WriteLine("Coundn't convert input to 2 integers, please try again..");
            }
        }

        return new Grid(MaxX, MaxY);
    }

    // validates user string, keeps asking till user inputs a valid string:
    static Rover CreateRoverFromUserInput(Grid RoverGrid)
    {
        // track whether user string is valid:
        bool UserStringValid = false;

        // variables to save parsed parameters to:
        int initialX = 0;
        int initialY = 0;
        char initialDirection = ' ';

        // regex to check valid chars:
        string RouteStringCharsRegex = @"^[NESWnesw]+$";

        // Ask for user input until user provides valid input:
        while(true)
        {
            // reset valid status:
            UserStringValid = false;

            // prompt user for input:
            Console.WriteLine("\nEnter Rover starting point (2 numbers & character from 'NESW' separated by spaces):");
            string RoverCreationString = Console.ReadLine() ?? "";

            // split string on spaces:
            string[] PotentialParams = RoverCreationString.Split(' ');

            // if there are 2 potential parameters:
            if (PotentialParams.Length == 3)
            {
                if(int.TryParse(PotentialParams[0], out initialX) && int.TryParse(PotentialParams[1], out initialY) &&
                    Regex.IsMatch(PotentialParams[2], RouteStringCharsRegex) && PotentialParams[2].Length == 1)
                {
                    initialDirection = char.Parse(PotentialParams[2]);
                    UserStringValid = true;

                    try
                    {
                        return new Rover(initialX, initialY, initialDirection, RoverGrid);
                    }
                    catch (ArgumentException Exception)
                    {
                        Console.WriteLine(Exception.Message);
                    }
                }
            }

            if (!UserStringValid)
            {
                Console.WriteLine("Coundn't convert input to 2 integers & a NESW char, please try again..");
            }

        }
    }

    // validates user string, & checks whether route is viable, if it's not catches the exception. Keeps asking till user inputs a valid string for a viable route:
    static void MoveRoverFromUserInput(Rover RoverToMove)
    {
        // track whether user string is valid:
        bool UserStringValidAndRouteViable = false;

        while (!UserStringValidAndRouteViable)
        {
            Console.WriteLine("\nEnter Rover Route (string made up of 'LRM' chars):");
            string RoverMovementString = Console.ReadLine() ?? "";

            try
            {
                RoverToMove.FollowRouteIfViable(RoverMovementString);
                UserStringValidAndRouteViable = true;
            }
            catch (ArgumentException Exception)
            {
                Console.WriteLine(Exception.Message);
            }
        }
    }
}