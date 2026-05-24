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

        string qtyText =
            await DisplayPromptAsync(
                "Buy Product",
                "Enter quantity",
                "Next",
                "Cancel",
                keyboard: Keyboard.Numeric);

        if (string.IsNullOrWhiteSpace(qtyText))
            return;

        int qty = int.Parse(qtyText);

        string deliveryName =
            await DisplayPromptAsync(
                "Delivery",
                "Enter receiver name");

        if (string.IsNullOrWhiteSpace(deliveryName))
            return;

        string deliveryMobile =
            await DisplayPromptAsync(
                "Delivery",
                "Enter mobile number");

        if (string.IsNullOrWhiteSpace(deliveryMobile))
            return;

        string address =
            await DisplayPromptAsync(
                "Delivery",
                "Enter delivery address");

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
                "Order Placed Successfully",
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
}