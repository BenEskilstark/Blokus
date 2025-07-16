namespace Blokus.ECS;

using Blokus.Models;
using Blokus.ECS.Entities;
using Blokus.ECS.Systems;

using Coord = (int X, int Y);

public class Game(int gameID, Player host)
{
    public int GameID { get; } = gameID;
    public bool IsStarted { get; set; } = false;
    public event Action? Notify;

    public List<Player> Players { get; set; } = [host];
    public int MaxPlayers { get; } = 4;
    public Player? GameWinner { get; set; }

    public bool IsHost(int playerID)
    {
        return Players.FirstOrDefault(p => true)?.PlayerID == playerID;
    }

    public int Width { get; private set; } = 20;
    public int Height { get; private set; } = 20;

    public EntityContext EntContext { get; set; } = new();
    private List<System> Systems { get; set; } = [
        new FuelSystem(),
        new PieceSystem(),
    ];

    public Dictionary<int, List<Func<Piece>>> PieceBanks { get; set; } = [];
    public Dictionary<int, int> PieceIndices { get; set; } = [];

    public int Tick { get; private set; } = 0;
    public int TurnIndex { get; set; } = 0;
    private Timer? UpdateInterval { get; set; }

    public void Start()
    {
        IsStarted = true;
        EntContext = new();

        Players.ForEach(player =>
        {
            int playerID = player.PlayerID;
            string color = player.Color;

            PieceBanks.Add(player.PlayerID, [
                () => Piece.MakeOne(playerID, color),

                () => Piece.MakeT(playerID, color),
                () => Piece.Makel(playerID, color),
                () => Piece.MakeL(playerID, color),
                () => Piece.MakeStairs(playerID, color),
                () => Piece.MakeBoomerang(playerID, color),
                () => Piece.MakePlus(playerID, color),
                () => Piece.MakeBigL(playerID, color),
                () => Piece.MakeNubBox(playerID, color),
                () => Piece.MakeS(playerID, color),
                () => Piece.MakeU(playerID, color),
                () => Piece.MakeStaff(playerID, color),
                () => Piece.MakeLongZ(playerID, color),

                () => Piece.Maket(playerID, color),
                () => Piece.MakeFourL(playerID, color),
                () => Piece.MakeFourl(playerID, color),
                () => Piece.MakeBox(playerID, color),
                () => Piece.MakeZ(playerID, color),

                () => Piece.MakeLittleL(playerID, color),
                () => Piece.MakeLittlel(playerID, color),

                () => Piece.MakeTwo(playerID, color),
            ]);
            PieceIndices.Add(playerID, 0);
            EntContext.Add(PieceBanks[playerID][PieceIndices[playerID]]());
        });
        EntContext.OnAfterUpdate();

        if (Players.Count == 2)
        {
            Width = 14;
            Height = 14;
        }

        Tick = 0;
        TurnIndex = 0;
        // UpdateInterval = new((object? s) => Update(), null, 0, 500);
        Notify?.Invoke();
    }

    /** 
     * Tries to place this player's current piece, returns true if it does
     **/
    public bool TryPlacePiece(Player player)
    {
        // check that it is your turn
        if (!IsMyTurn(player)) return false;

        Piece? playerPiece = EntContext.GetPlayerPiece(player.PlayerID);
        if (playerPiece == null) return false;
        Coord pos = playerPiece.Position;
        bool canPlace = true;

        // check that you're not directly overlapping
        playerPiece.Cells.ForEach(c =>
        {
            Entity? overlap = EntContext.Grid.At((pos.X + c.X, pos.Y + c.Y));
            if (overlap != null && overlap.Name != "Piece") canPlace = false;
        });
        if (!canPlace) return false;

        // check that you are on a diagonal with your own block or at a corner
        canPlace = false;
        playerPiece.Cells.ForEach(c =>
        {
            EntContext.Grid
                .GetDiagonalNeighbors((pos.X + c.X, pos.Y + c.Y))
                .ForEach(n =>
                {
                    Entity? overlap = EntContext.Grid.At(n);
                    if (
                        overlap != null && overlap.Name == "Block" &&
                        overlap.PlayerID == player.PlayerID
                    ) canPlace = true;
                    if (
                        n == (-1, -1) ||
                        n == (-1, Height) ||
                        n == (Width, -1) ||
                        n == (Width, Height)
                    ) canPlace = true;
                });
        });
        if (!canPlace) return false;

        // check that you are fully inside the grid
        canPlace = true;
        playerPiece.Cells.ForEach(c =>
        {
            if (pos.X + c.X < 0) canPlace = false;
            if (pos.X + c.X >= Width) canPlace = false;
            if (pos.Y + c.Y < 0) canPlace = false;
            if (pos.Y + c.Y >= Height) canPlace = false;
        });
        if (!canPlace) return false;

        // check that you don't border your own blocks
        playerPiece.Cells.ForEach(c =>
        {
            EntContext.Grid
                .GetNeighbors((pos.X + c.X, pos.Y + c.Y))
                .ForEach(n =>
                {
                    Entity? overlap = EntContext.Grid.At(n);
                    if (
                        overlap != null && overlap.Name == "Block" &&
                        overlap.PlayerID == player.PlayerID
                    ) canPlace = false;
                });
        });
        if (!canPlace) return false;

        // place the blocks:
        playerPiece.Cells.ForEach(c =>
        {
            EntContext.Add(new Block()
            {
                PlayerID = player.PlayerID,
                Position = (pos.X + c.X, pos.Y + c.Y),
                Color = player.Color
            });
        });

        EntContext.OnAfterUpdate();

        // remove placed piece from their bank
        List<Func<Piece>> bank = PieceBanks[player.PlayerID];
        List<Func<Piece>> newBank = [];
        for (int i = 0; i < bank.Count; i++)
        {
            if (i != PieceIndices[player.PlayerID])
            {
                newBank.Add(bank[i]);
            }
        }
        PieceBanks[player.PlayerID] = newBank;
        PieceIndices[player.PlayerID] = PieceIndices[player.PlayerID] % newBank.Count;

        // check if game over:
        if (newBank.Count == 0)
        {
            GameWinner = player;
        }

        // set next player piece:
        EntContext
            .SetPlayerPiece(player.PlayerID, newBank[PieceIndices[player.PlayerID]]());

        return true;
    }


    public void CyclePieceUp(Player player)
    {
        List<Func<Piece>> bank = PieceBanks[player.PlayerID];
        int curIndex = PieceIndices[player.PlayerID];
        int nextIndex = (curIndex + 1) % bank.Count;

        PieceIndices[player.PlayerID] = nextIndex;
        EntContext.SetPlayerPiece(player.PlayerID, bank[nextIndex]());
    }
    public void CyclePieceDown(Player player)
    {
        List<Func<Piece>> bank = PieceBanks[player.PlayerID];
        int curIndex = PieceIndices[player.PlayerID];
        int nextIndex = (curIndex - 1) % bank.Count;

        PieceIndices[player.PlayerID] = nextIndex;
        EntContext.SetPlayerPiece(player.PlayerID, bank[nextIndex]());
    }


    public bool IsMyTurn(Player player)
    {
        return Players[TurnIndex].PlayerID == player.PlayerID;
    }

    public Player GetCurPlayerTurn()
    {
        return Players[TurnIndex];
    }

    public void EndTurn()
    {
        TurnIndex = (TurnIndex + 1) % Players.Count;
        Notify?.Invoke();
    }

    public void Update()
    {
        Tick++;
        // Console.WriteLine("Tick");
        // EntContext.ForEach(Console.WriteLine);
        // Systems.ForEach(s => s.Update(EntContext));
        EntContext.OnAfterUpdate();
        Notify?.Invoke();
    }

}