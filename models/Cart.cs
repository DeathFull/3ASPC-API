namespace _3ASPC_API.models;

public class Cart
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public List<CartItem> CartItems { get; set; }
}