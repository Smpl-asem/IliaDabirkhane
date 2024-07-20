using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

public class Recivers : Parent
{
    public int? ReciverId { get; set; }
    public Users? Reciver { get; set; }

    [ForeignKey("Messages")]
    public int? MessageId { get; set; }
    public Messages? Message { get; set; }
    public string? Type { get; set; } // to ، cc

    public Recivers()
    {
        this.ReciverId = null;
        this.Reciver = null;
        this.Message = null;
        this.MessageId = null;
        this.Type = null;
    }
}