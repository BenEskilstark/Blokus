namespace Blokus.ECS;

using Blokus.Models;
using Blokus.ECS.Entities;
using Blokus.ECS.Systems;

public class Game(int gameID, Player host)
{
    public int GameID { get; } = gameID;
    public bool IsStarted { get; set; } = false;
    public event Action? Notify;

    public List<Player> Players { get; set; } = [host];
    public int MaxPlayers { get; } = 4;

    public bool IsHost(int playerID)
    {
        return Players.FirstOrDefault(p => true)?.PlayerID == playerID;
    }

    public int Width { get; private set; } = 55;
    public int Height { get; private set; } = 40;

    public EntityState Entities { get; set; } = new();
    private List<System> Systems { get; set; } = [
        new FuelSystem(),
        new PieceSystem(),
    ];
    public int Tick { get; private set; } = 0;
    private Timer? UpdateInterval { get; set; }

    public void Start()
    {
        IsStarted = true;
        Entities = new();

        Players.ForEach(player =>
        {
            Entities.Add(Piece.MakeOne(player.PlayerID, player.Color));
        });

        Tick = 0;
        UpdateInterval = new((object? s) => Update(), null, 0, 500);
        Notify?.Invoke();
    }

    public void Update()
    {
        Tick++;
        // Console.WriteLine("Tick");
        // Entities.ForEach(Console.WriteLine);
        Systems.ForEach(s => s.Update(Entities));
        Entities.OnAfterUpdate();
        Notify?.Invoke();
    }

}