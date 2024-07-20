using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

public class Atteched : Parent
{
    public string? FileName { get; set; }

    [ForeignKey("Messages")]
    public int? MessageId { get; set; }
    public Messages? Message { get; set; }
    public string? FilePath { get; set; }
    public string? FileType { get; set; }

    public Atteched()
    {
        this.FileName = null;
        this.MessageId = null;
        this.Message = null;
        this.FilePath = null;
        this.FileType = null;
    }
}