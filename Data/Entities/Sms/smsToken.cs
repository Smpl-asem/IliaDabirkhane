using System.ComponentModel.DataAnnotations;

public class smsToken
{
    [Key]
    public int Id { get; set; }
    public string Token { get; set; }
}