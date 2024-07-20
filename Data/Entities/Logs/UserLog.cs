public class UserLog : Parent
{
    public int UserId { get; set; }
    public Users User { get; set; }
    public int LogAction { get; set; } // 1) Login / 2) Logout / 3) Register / 4) ResetPassword / 5) Verify password / 6) Update User
    public bool isSucces { get; set; }
}