namespace _3ASPC_API.models;

public class ProductRequest
{
    public string Name { get; set; }
    public string Image { get; set; }
    public decimal Price { get; set; }
    public bool Available { get; set; }
    public DateTime AddedTime { get; set; }
    public int SellerId { get; set; }
}