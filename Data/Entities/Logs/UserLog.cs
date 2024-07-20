public class UserLog : Parent
{
    public int UserId { get; set; }
    public int LogAction { get; set; } // 1) Login / 2) Logout / 3) Register / 4) ResetPassword / 5) Verify password
    public bool isSucces { get; set; }
    public int Priority { get; set; } // 1) VeryHigh / 2) High / 3) Normal / 4) Low / 5) veryLow
    public string? Description { get; set; }

    public UserLog()
    {
        Description = null;
    }
}