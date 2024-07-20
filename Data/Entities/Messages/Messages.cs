using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

public class Messages : Parent
{
    public string? SerialNumber { get; set; }
    public int? SenderUserId { get; set; }
    public Users? SenderUser { get; set; }
    public string? Subject { get; set; }
    public string? BodyText { get; set; }

    // Trash and Delete
    public List<int>? Trashed { get; set; }
    public List<int>? Deleted { get; set; }

    public List<Recivers>? Recivers { get; set; }
    public List<Atteched>? Atteched { get; set; }

    public Messages()
    {
        this.SerialNumber = null;
        this.SenderUser = null;
        this.SenderUserId = null;
        this.Subject = null;
        this.BodyText = null;
        this.Trashed = new List<int>();
        this.Deleted = new List<int>();
        this.Atteched = null;
    }
}