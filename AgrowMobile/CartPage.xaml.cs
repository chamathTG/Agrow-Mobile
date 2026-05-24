using AgrowMobile.Models;
using AgrowMobile.Services;

namespace AgrowMobile;

public partial class CartPage : ContentPage
{
    DbService db = new DbService();

    string customerName;

    List<CartModel> cartItems = new();

    public CartPage(string username)
    {
        InitializeComponent();

        customerName = username;

        LoadCart();
    }

    private async void LoadCart()
    {
        cartItems =
            await db.GetCartItems(customerName);

        CartCollection.ItemsSource =
            cartItems;
    }

    private async void OnCheckoutClicked(
     object sender,
     EventArgs e)
    {
        if (cartItems.Count == 0)
        {
            await DisplayAlert(
                "Empty",
                "Cart is empty",
                "OK");

            return;
        }

        // RECEIVER
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

        double grandTotal = 0;

        bool allSuccess = true;

        foreach (var item in cartItems)
        {
            double total =
                item.Price * item.Qty;

            grandTotal += total;

            bool success =
                await db.AddOrder(
                    customerName,
                    item.FarmerName,
                    item.Title,
                    item.Qty,
                    total,
                    deliveryName,
                    deliveryMobile,
                    address);

            if (!success)
            {
                allSuccess = false;
            }
        }

        if (allSuccess)
        {
            await db.ClearCart(customerName);

            await DisplayAlert(
                "Success",
                $"Checkout Complete\nTotal: Rs. {grandTotal:F2}",
                "OK");

            LoadCart();
        }
        else
        {
            await DisplayAlert(
                "Failed",
                "Checkout Failed",
                "OK");
        }
    }

    private async void OnIncreaseQty(
    object sender,
    EventArgs e)
    {
        Button btn = (Button)sender;

        int cartId =
            (int)btn.CommandParameter;

        CartModel item =
            cartItems.First(x => x.Id == cartId);

        item.Qty++;

        await db.UpdateCartQty(
            cartId,
            item.Qty);

        LoadCart();
    }

    private async void OnDecreaseQty(
    object sender,
    EventArgs e)
    {
        Button btn = (Button)sender;

        int cartId =
            (int)btn.CommandParameter;

        CartModel item =
            cartItems.First(x => x.Id == cartId);

        if (item.Qty > 1)
        {
            item.Qty--;

            await db.UpdateCartQty(
                cartId,
                item.Qty);

            LoadCart();
        }
    }

    private async void OnRemoveClicked(
    object sender,
    EventArgs e)
    {
        Button btn = (Button)sender;

        int cartId =
            (int)btn.CommandParameter;

        bool success =
            await db.DeleteCartItem(cartId);

        if (success)
        {
            await DisplayAlert(
                "Removed",
                "Item Removed From Cart",
                "OK");

            LoadCart();
        }
    }
}