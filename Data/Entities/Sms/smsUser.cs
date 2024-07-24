public class smsUser:Parent
{
    public string SmsCode { get; set; }
    public int TryCount { get; set; }
    public int UserId { get; set; }
    public bool IsValid { get; set; }
}