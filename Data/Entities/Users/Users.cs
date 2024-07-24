public class Users : Parent
{
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? Token { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Phone { get; set; }
    public string? Addres { get; set; }
    public string? NatinalCode { get; set; }
    public string? PerconalCode { get; set; }
    public string? Profile { get; set; }
    public List<UserRole> Roles { get; set; }
    
    public Users()
    {
        this.Username= null;
        this.Password= null;
        this.Token= null;
        this.FirstName= null;
        this.LastName= null;
        this.Phone= null;
        this.Addres= null;
        this.NatinalCode= null;
        this.PerconalCode= null;
        this.Profile = null;
    }
}