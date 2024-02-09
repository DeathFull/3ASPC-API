namespace _3ASPC_API.models;

public class ProductItem
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public Byte[]? Image { get; set; }
    public decimal Price { get; set; }
    public bool IsAvailable { get; set; }
    public TimeSpan AddedTime { get; set; }

}