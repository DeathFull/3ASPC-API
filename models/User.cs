using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _3ASPC_API.models;

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; }
    public string Email { get; set; }
    public string Pseudo { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }

}