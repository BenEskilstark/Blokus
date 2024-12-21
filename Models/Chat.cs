namespace Blokus.Models;

public class Chat(string name, string text)
{
    public DateTime TimeStamp { get; set; } = DateTime.Now;
    public string Name { get; set; } = name;
    public string Text { get; set; } = text;
}