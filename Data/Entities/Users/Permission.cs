using System.ComponentModel.DataAnnotations;

// Permission.cs
public class Permission
{
     [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<RolePermission> RolePermissions { get; set; }
}