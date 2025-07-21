namespace Blokus.ECS.Entities;

using Blokus.DataStructures;

using Coord = (int X, int Y);

public class Piece() : Entity()
{
    public override string Name { get; set; } = "Piece";

    public override List<Coord> Cells { get; set; } = [];


    public Piece RotateClockwise()
    {
        Cells = [.. Cells.Transpose().Select(c => (-1 * c.X, c.Y))];
        return this;
    }
    public Piece RotateCounterClockwise()
    {
        Cells = [.. Cells.Transpose().Select(c => (c.X, -1 * c.Y))];
        return this;
    }

    // ------------------------------------------------------------------------
    // 5-Cell Pieces
    public static Piece MakeT(int? playerID = null, string color = "grey")
    {
        return new()
        {
            PlayerID = playerID,
            Cells = [(0, 0), (1, 0), (2, 0), (1, 1), (1, 2)],
            Color = color,
        };
    }

    public static Piece Makel(int? playerID = null, string color = "grey")
    {
        return new()
        {
            PlayerID = playerID,
            Cells = [(0, 0), (0, 1), (0, 2), (0, 3), (0, 4)],
            Color = color,
        };
    }

    public static Piece MakeL(int? playerID = null, string color = "grey")
    {
        return new()
        {
            PlayerID = playerID,
            Cells = [(0, 0), (0, 1), (0, 2), (0, 3), (1, 3)],
            Color = color,
        };
    }

    public static Piece MakeStairs(int? playerID = null, string color = "grey")
    {
        return new()
        {
            PlayerID = playerID,
            Cells = [(0, 0), (0, 1), (1, 1), (1, 2), (2, 2)],
            Color = color,
        };
    }

    public static Piece MakeBoomerang(int? playerID = null, string color = "grey")
    {
        return new()
        {
            PlayerID = playerID,
            Cells = [(0, 0), (1, 0), (1, 1), (1, 2), (2, 1)],
            Color = color,
        };
    }
    public static Piece MakePlus(int? playerID = null, string color = "grey")
    {
        return new()
        {
            PlayerID = playerID,
            Cells = [(0, 0), (-1, 1), (1, 1), (0, 1), (0, 2)],
            Color = color,
        };
    }
    public static Piece MakeBigL(int? playerID = null, string color = "grey")
    {
        return new()
        {
            PlayerID = playerID,
            Cells = [(0, 0), (1, 0), (2, 0), (0, 1), (0, 2)],
            Color = color,
        };
    }
    public static Piece MakeNubBox(int? playerID = null, string color = "grey")
    {
        return new()
        {
            PlayerID = playerID,
            Cells = [(0, 0), (1, 0), (1, 1), (0, 1), (2, 1)],
            Color = color,
        };
    }
    public static Piece MakeS(int? playerID = null, string color = "grey")
    {
        return new()
        {
            PlayerID = playerID,
            Cells = [(0, 0), (0, 1), (1, 1), (2, 1), (2, 2)],
            Color = color,
        };
    }
    public static Piece MakeU(int? playerID = null, string color = "grey")
    {
        return new()
        {
            PlayerID = playerID,
            Cells = [(0, 0), (1, 0), (1, 1), (1, 2), (0, 2)],
            Color = color,
        };
    }
    public static Piece MakeStaff(int? playerID = null, string color = "grey")
    {
        return new()
        {
            PlayerID = playerID,
            Cells = [(0, 0), (0, 1), (0, 2), (0, 3), (1, 1)],
            Color = color,
        };
    }
    public static Piece MakeLongZ(int? playerID = null, string color = "grey")
    {
        return new()
        {
            PlayerID = playerID,
            Cells = [(0, 0), (1, 0), (1, 1), (2, 1), (3, 1)],
            Color = color,
        };
    }

    // ------------------------------------------------------------------------
    // 4-Cell Pieces
    public static Piece Maket(int? playerID = null, string color = "grey")
    {
        return new()
        {
            PlayerID = playerID,
            Cells = [(0, 0), (0, 1), (0, 2), (1, 1)],
            Color = color,
        };
    }
    public static Piece MakeBox(int? playerID = null, string color = "grey")
    {
        return new()
        {
            PlayerID = playerID,
            Cells = [(0, 0), (0, 1), (1, 1), (1, 0)],
            Color = color,
        };
    }
    public static Piece MakeFourL(int? playerID = null, string color = "grey")
    {
        return new()
        {
            PlayerID = playerID,
            Cells = [(0, 0), (0, 1), (0, 2), (1, 2)],
            Color = color,
        };
    }
    public static Piece MakeFourl(int? playerID = null, string color = "grey")
    {
        return new()
        {
            PlayerID = playerID,
            Cells = [(0, 0), (0, 1), (0, 2), (0, 3)],
            Color = color,
        };
    }
    public static Piece MakeZ(int? playerID = null, string color = "grey")
    {
        return new()
        {
            PlayerID = playerID,
            Cells = [(0, 0), (0, 1), (1, 1), (1, 2)],
            Color = color,
        };
    }



    // ------------------------------------------------------------------------
    // 3-Cell Pieces
    public static Piece MakeLittleL(int? playerID = null, string color = "grey")
    {
        return new()
        {
            PlayerID = playerID,
            Cells = [(0, 0), (0, 1), (1, 1)],
            Color = color,
        };
    }

    public static Piece MakeLittlel(int? playerID = null, string color = "grey")
    {
        return new()
        {
            PlayerID = playerID,
            Cells = [(0, 0), (0, 1), (0, 2)],
            Color = color,
        };
    }

    // ------------------------------------------------------------------------
    // 2-Cell Piece
    public static Piece MakeTwo(int? playerID = null, string color = "grey")
    {
        return new()
        {
            PlayerID = playerID,
            Cells = [(0, 0), (0, 1)],
            Color = color,
        };
    }

    // ------------------------------------------------------------------------
    // 1-Cell Piece
    public static Piece MakeOne(int? playerID = null, string color = "grey")
    {
        return new()
        {
            PlayerID = playerID,
            Cells = [(0, 0)],
            Color = color,
        };
    }
}