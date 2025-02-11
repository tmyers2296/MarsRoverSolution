using System.Text.RegularExpressions;

public class Rover
{
    // static variables:
    static string RotationMap = "NESW";
    static string RouteStringCharsRegex = @"^[RLM]+$";

    // instance variables:
    // current x & y coordinates:
    private int CurrentX;
    private int CurrentY;
    private char CurrentDirection;

    // x & y coordinates to track a potential route:
    private int PotentialX;
    private int PotentialY;
    private char PotentialDirection;

    private Grid CurrentGrid;

    // contstructor:
    public Rover(int xCor, int yCor, char Direction, Grid InitialGrid)
    {
        // variable constraints:
        if (xCor < 0 || xCor >= InitialGrid.GetLength())
        {
            throw new ArgumentException($"x coordinate {xCor} out of range of grid");
        }

        if (yCor < 0 || yCor >= InitialGrid.GetWidth())
        {
            throw new ArgumentException($"y coordinate {yCor} out of range of grid");
        }

        if (InitialGrid.GetCellState(xCor, yCor))
        {
            throw new ArgumentException($"Rover already placed at this location! (x: {xCor}, y: {yCor})");
        }

        if (!RotationMap.Contains(Direction))
        {
            throw new ArgumentException($"{Direction} not a valid direction needs to be either N, E, S or W..");
        }

        // assinging variables:
        CurrentX = xCor;
        CurrentY = yCor;
        CurrentDirection = Direction;

        CurrentGrid = InitialGrid;
        CurrentGrid.SetCellState(xCor, yCor, true);
    }

    // move rover in the direction it is facing by one coordinate:
    private void Move()
    {
        if(PotentialDirection == 'N')
        {
            PotentialY++;
        }
        else if (PotentialDirection == 'E')
        {
            PotentialX++;
        }
        else if (PotentialDirection == 'S')
        {
            PotentialY--;
        }
        else if (PotentialDirection == 'W')
        {
            PotentialX--;
        }
    }

    // rotate rover left or right & update direction:
    private void Turn(char TurnDirection)
    {
        // index of current direction in roationMap string:
        int DirectionIndex = RotationMap.IndexOf(PotentialDirection);

        // decrement or increment using rotation map to determine new direction:
        if(TurnDirection == 'L')
        {
            DirectionIndex--;
        }
        else if (TurnDirection == 'R')
        {
            DirectionIndex++;
        }

        // if index is out of bounds correct it (can only be out be one because we are only incrementing by one..)
        DirectionIndex = (DirectionIndex >= 0)? DirectionIndex % RotationMap.Length : RotationMap.Length - 1;

        // set new direction:
        PotentialDirection = RotationMap[DirectionIndex];
    }

    // checks whether a given route from the current position is viable
    // for a route to be viable the rover will not hit another rover on the way
    // & the rover will not travel out of bounds of the plateau
    private bool ValidateRoute(string RouteString)
    {
        // starts at current position:
        PotentialX = CurrentX;
        PotentialY = CurrentY;
        PotentialDirection = CurrentDirection;

        foreach(char Instruction in RouteString)
        {
            // if it's a rotation instruction:
            if (Instruction == 'L' || Instruction == 'R')
            {
                Turn(Instruction);
            }
            // if it's a move instruction:
            else if (Instruction == 'M')
            {
                Move();
            }

            // potential coordinates != current coordiates (only check for other
            // rovers if we're not on the current position of this rover):
            if(!(PotentialX == CurrentX && PotentialY == CurrentY))
            {
                // check whether coordinates are out of bounds (rover will fall of plateau):
                if ((PotentialX < 0 || PotentialX >= CurrentGrid.GetLength()) || (PotentialY < 0 || PotentialY >= CurrentGrid.GetWidth()))
                {
                    return false;
                }
                // checks for another rover in path on grid & returns false if it finds one:
                else if (CurrentGrid.GetCellState(PotentialX, PotentialY))
                {
                    return false;
                }
            }
        }

        return true;
    }

    // save new rover position (update current position & direction + update grid)
    private void SaveNewPosition(int newX, int newY, char newDirection)
    {
        // previous position set to false on grid, new position set to true:
        CurrentGrid.SetCellState(CurrentX, CurrentY, false);
        CurrentGrid.SetCellState(newX, newY, true);
        
        // current position & direction updated:
        CurrentX = newX;
        CurrentY = newY;
        CurrentDirection = newDirection;
    }

    // checks whether a route is viable and follows it if it is:
    public void FollowRouteIfViable(string RouteString)
    {
        // variable constraint:
        if (!Regex.IsMatch(RouteString, RouteStringCharsRegex))
        {
            throw new ArgumentException($"'{RouteString}' is ot a valid RouteString needs to be made up of R, L and M chars..");
        }

        bool routeValid = ValidateRoute(RouteString);

        if (routeValid)
        {
            SaveNewPosition(PotentialX, PotentialY, PotentialDirection);
        }
        else
        {
            if ((PotentialX < 0 || PotentialX >= CurrentGrid.GetLength()) || (PotentialY < 0 || PotentialY >= CurrentGrid.GetWidth()))
            {
                 throw new ArgumentException($"Route not viable. Will fall off plateau at (x:{PotentialX}, y: {PotentialY})!");
            }
            else
            {
                 throw new ArgumentException($"Route not viable. Will hit another rover at (x:{PotentialX}, y: {PotentialY})!");
            }
        }
    }

    // show current rover position & direction:
    public void ShowPosition()
    {
        Console.WriteLine($"\n* Rover Position -> {CurrentX} {CurrentY} {CurrentDirection} *\n");
    }

}