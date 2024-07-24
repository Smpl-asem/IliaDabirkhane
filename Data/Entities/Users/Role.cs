using System.ComponentModel.DataAnnotations;
public class Role
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }

    //relationship
    public ICollection<UserRole> UserRoles { get; set; }
    public ICollection<RolePermission> RolePermissions { get; set; }
}