namespace Blokus.Models;

public class Player(int playerID)
{
    public int PlayerID { get; } = playerID;
    public string Name { get; set; } = "Player " + playerID;

    public string Color { get; set; } = "red";
}