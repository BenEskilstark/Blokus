namespace Blokus.Singletons;

using Blokus.Models;
using Blokus.DataStructures;
using Blokus.ECS;

public class LobbySingleton()
{
    public List<Player> Players { get; private set; } = [];
    public Player? GetPlayer(int playerID)
    {
        return Players.FirstOrDefault(p => p.PlayerID == playerID);
    }
    public Player JoinServer()
    {
        int playerID = Players.Count > 0 ? Players.Max(p => p.PlayerID) + 1 : 1;
        Player player = new(playerID);
        Players.Add(player);
        Notify?.Invoke();
        return player;
    }
    public void LeaveServer(Player player)
    {
        Players = Players.Where(p => p.PlayerID != player.PlayerID).ToList();
        Notify?.Invoke();
    }



    public List<Game> Games { get; private set; } = [];
    public Game? GetGame(int gameID)
    {
        return Games.FirstOrDefault(g => g.GameID == gameID);
    }
    public Game CreateGame(Player host)
    {
        int gameID = Games.Count > 0 ? Games.Max(g => g.GameID) + 1 : 0;
        Game game = new(gameID, host);
        Games.Add(game);
        host.Color = "red";
        Notify?.Invoke();
        return game;
    }
    public Game? JoinGame(Player player, int gameID)
    {
        Game? game = GetGame(gameID);
        if (game == null) return null;

        if (game.Players.Count < game.MaxPlayers)
        {
            player.Color = new List<string>(["steelblue", "green", "purple", "orange"])[game.Players.Count];
            game.Players.Add(player);
            Notify?.Invoke();
            return game;
        }
        return null;
    }
    public void StartGame(Player player, int gameID)
    {
        Game? game = GetGame(gameID);
        if (game == null) return;
        game.Start();
        NotifyGameStart?.Invoke(gameID);
    }
    public void LeaveGame(Player player, int gameID)
    {
        Game? game = GetGame(gameID);
        if (game == null) return;

        game.Players = game.Players.Where(p => p.PlayerID != player.PlayerID).ToList();
        if (game.Players.Count == 0)
        {
            Games = Games.Where(g => g.GameID != gameID).ToList();
            NotifyGameOver?.Invoke(gameID);
        }
        Notify?.Invoke();
    }
    public void EndGame(Player player, int gameID)
    {
        Game? game = GetGame(gameID);
        if (game == null) return;
        if (!game.Players.Contains(player)) return;

        Games = Games.Where(g => g.GameID != gameID).ToList();

        NotifyGameOver?.Invoke(gameID);
        Notify?.Invoke();
    }


    public List<Chat> ChatThread { get; set; } = [];
    public void SendChat(Player player, string text)
    {
        ChatThread.Add(new(player.Name, text));
        Notify?.Invoke();
    }


    public event Action? Notify;
    public event Action<int>? NotifyGameOver;
    public event Action<int>? NotifyGameStart;
}