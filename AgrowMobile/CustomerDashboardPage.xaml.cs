using AgrowMobile.Models;
using AgrowMobile.Services;

namespace AgrowMobile;

public partial class CustomerDashboardPage : ContentPage
{
    DbService db = new DbService();

    string customerName;

    public CustomerDashboardPage(
        string username)
    {
        InitializeComponent();

        customerName = username;

        CustomerNameLabel.Text =
            $"Welcome, {customerName}";

        LoadProducts();
    }

    private async void LoadProducts()
    {
        ProductsCollection.ItemsSource =
            await db.GetAllProducts();
    }

    private async void OnBuyClicked(
    object sender,
    EventArgs e)
    {
        Button btn = (Button)sender;

        int productId =
            (int)btn.CommandParameter;

        ProductModel? product =
            await db.GetProductById(productId);

        if (product == null)
            return;

        // ASK QTY
        string qtyText =
            await DisplayPromptAsync(
                "Buy Product",
                "Enter quantity",
                "Next",
                "Cancel",
                keyboard: Keyboard.Numeric);

        if (string.IsNullOrWhiteSpace(qtyText))
            return;

        int qty =
            int.TryParse(qtyText, out int q)
            ? q : 0;

        // STOCK CHECK
        if (qty <= 0 || qty > product.Qty)
        {
            await DisplayAlert(
                "Error",
                "Invalid quantity",
                "OK");

            return;
        }

        // DELIVERY NAME
        string deliveryName =
            await DisplayPromptAsync(
                "Delivery",
                "Receiver name");

        if (string.IsNullOrWhiteSpace(deliveryName))
            return;

        // MOBILE
        string deliveryMobile =
            await DisplayPromptAsync(
                "Delivery",
                "Mobile number");

        if (string.IsNullOrWhiteSpace(deliveryMobile))
            return;

        // ADDRESS
        string address =
            await DisplayPromptAsync(
                "Delivery",
                "Delivery address");

        if (string.IsNullOrWhiteSpace(address))
            return;

        double total =
            qty * product.Price;

        bool success =
            await db.AddOrder(
                customerName,
                product.FarmerName,
                product.Title,
                qty,
                total,
                deliveryName,
                deliveryMobile,
                address);

        if (success)
        {
            await DisplayAlert(
                "Success",
                $"Order Placed\nTotal: Rs. {total:F2}",
                "OK");
        }
        else
        {
            await DisplayAlert(
                "Failed",
                "Cannot Place Order",
                "OK");
        }
    }

    private async void OnAddToCartClicked(
    object sender,
    EventArgs e)
    {
        Button btn = (Button)sender;

        int productId =
            (int)btn.CommandParameter;

        ProductModel? product =
            await db.GetProductById(productId);

        if (product == null)
            return;

        bool success =
            await db.AddToCart(
                customerName,
                product);

        if (success)
        {
            await DisplayAlert(
                "Success",
                "Added To Cart",
                "OK");
        }
    }

    private async void OnCartClicked(
    object sender,
    EventArgs e)
    {
        await Navigation.PushAsync(
            new CartPage(customerName));
    }
}