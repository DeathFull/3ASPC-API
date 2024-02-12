using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _3ASPC_API.models;

[Table("CartItems")]
public class CartItem
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int CartId { get; set; }
    [ForeignKey("CartId")] public Cart Cart { get; set; }
    public int ProductId { get; set; }

    [ForeignKey("ProductId")] public Product Product { get; set; }
    public int Quantity { get; set; }
}