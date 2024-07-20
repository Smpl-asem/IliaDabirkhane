public class DtoMessage
{
    public string SerialNumber { get; set; }
    public int SenderUserId { get; set; }
    public string Subject { get; set; }
    public string BodyText { get; set; }
    public List<DtoRecivers> Resivers { get; set; }
    public List<DtoAttached>? Atachments { get; set; }
    public DtoMessage()
    {
        this.Atachments = null;
    }
}


