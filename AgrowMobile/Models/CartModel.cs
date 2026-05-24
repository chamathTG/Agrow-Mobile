namespace AgrowMobile.Models;

public class CartModel
{
    public int Id { get; set; }

    public string CustomerName { get; set; } = "";

    public string FarmerName { get; set; } = "";

    public int ProductId { get; set; }

    public string Title { get; set; } = "";

    public int Qty { get; set; }

    public double Price { get; set; }

    public string Image { get; set; } = "";
}