using Microsoft.IdentityModel.Tokens;

public class DtodUser
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Phone { get; set; }
    public string Addres { get; set; }
    public string NatinalCode { get; set; }
    public string PerconalCode { get; set; }
    public string Profile { get; set; }

    public bool IsNullOrEmpty(){
        if(
            string.IsNullOrEmpty(Username) ||
            string.IsNullOrEmpty(Password) ||
            string.IsNullOrEmpty(FirstName) ||
            string.IsNullOrEmpty(LastName) ||
            string.IsNullOrEmpty(Phone) ||
            string.IsNullOrEmpty(Addres) ||
            string.IsNullOrEmpty(NatinalCode) ||
            string.IsNullOrEmpty(PerconalCode) ||
            string.IsNullOrEmpty(Profile)
        )
        return true;
        return false;
    }
}