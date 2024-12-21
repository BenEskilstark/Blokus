namespace Blokus.Services;

using System;
using Blokus.Models;
using Blokus.Singletons;
using Blokus.ECS;


public class LobbyService : IDisposable
{
    public LobbySingleton LobbyServer { get; }
    public Player Player { get; }

    public Game? JoinedGame { get; set; }

    public LobbyService(LobbySingleton lobbyServer)
    {
        LobbyServer = lobbyServer;
        Player = lobbyServer.JoinServer();
        LobbyServer.NotifyGameOver += (int gameID) =>
        {
            if (JoinedGame?.GameID == gameID)
            {
                JoinedGame = null;
            }
        };
    }

    public void Dispose()
    {
        if (JoinedGame != null)
        {
            LobbyServer.LeaveGame(Player, JoinedGame.GameID);
        }
        LobbyServer.LeaveServer(Player);
        JoinedGame = null;
    }

}