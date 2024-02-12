using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _3ASPC_API.models;

[Table("products")]
public class Product
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Image { get; set; }
    public decimal Price { get; set; }
    public bool Available { get; set; }
    public DateTime AddedTime { get; set; }
    public int SellerId { get; set; }
    [ForeignKey("SellerId")]
    public User SellerUser { get; set; }
}