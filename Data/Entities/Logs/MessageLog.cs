public class MessageLog : Parent
{
    public int MessageId { get; set; }
    public Messages Message { get; set; }
    public int UserId { get; set; }
    public Users User { get; set; }
    public int LogAction { get; set; } // 1) read / 2) delete / 3) send / 4) Recive as to / 5) Recive As CC / 6)upload / 7) trash / 8) Untrash
}