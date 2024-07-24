public class UserLog : Parent
{
    public int UserId { get; set; }
    public Users User { get; set; }
    public int LogAction { get; set; } 
    public bool isSucces { get; set; }
}
//1) Login /2) Logout /3) Register /4) ResetPassword /5) Verify password /6) Update User /7) Turn to Clinet /8) admin /9) owner