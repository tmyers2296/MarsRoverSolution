public class Grid
{    
    // GridTracker variable, aa list of lists recording rows & columns of the grid and
    // which coordinates contain a rover:
    private List<List<bool>> GridTracker = new List<List<bool>>();

    // constructor, takes the top right coordinates to create an empty grid of a given size:
    public Grid(int topX, int topY)
    {
        for (int y = 0; y <= topY; y++)
        {
            GridTracker.Add(new List<bool>());
            
            for (int x = 0; x <= topX; x++)
            {
                GridTracker[y].Add(false);
            }

        }
    }

    // Prints the grid info to the console, diplaying which coordinates contain a rover:
    public void ShowGrid()
    {
        // start with blank line..
        Console.WriteLine("");

        // iterate through rows:
        for (int y = GridTracker.Count - 1; y >= 0; y--)
        {
            List<bool> GridRow = GridTracker[y];

            // iterate through columns in row:
            foreach (bool Status in GridRow)
            {
                // print x if there is a rover present:
                Console.Write(Status? "[x]": "[ ]");
            }

            // new line for next row:
            Console.WriteLine("");
        }
    }

    // sets a cell in the grid to true or false:
    public void SetCellState(int xCor, int yCor, bool status)
    {
        GridTracker[yCor][xCor] = status;
    }

    // checks for a rover at a given coordinate
    public bool GetCellState(int xCor, int yCor)
    {
        return GridTracker[yCor][xCor];
    }

    // get length of grid:
    public int GetLength()
    {
        return GridTracker.Count;
    }

    // get width of grid:
    public int GetWidth()
    {
        return GridTracker[0].Count;
    }
}