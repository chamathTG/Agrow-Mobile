using AgrowMobile.Models;
using AgrowMobile.Services;

namespace AgrowMobile;

public partial class FarmerDashboardPage : ContentPage
{
    DbService db = new DbService();

    string imagePath = "";

    string farmerName;

    public FarmerDashboardPage(string username)
    {
        InitializeComponent();

        farmerName = username;

        FarmerNameLabel.Text =
            $"Welcome, {farmerName}";

        LoadProducts();
    }

    private async void OnPickImage(
        object sender,
        EventArgs e)
    {
        var result = await FilePicker.Default.PickAsync(
            new PickOptions
            {
                PickerTitle = "Select Product Image"
            });

        if (result != null)
        {
            imagePath = result.FullPath;

            ProductImage.Source =
                ImageSource.FromFile(imagePath);
        }
    }

    private async void OnAddProduct(
    object sender,
    EventArgs e)
    {
        string title = TitleEntry.Text ?? "";

        string description =
            DescriptionEditor.Text ?? "";

        int qty =
            int.TryParse(QtyEntry.Text, out int q)
            ? q : 0;

        double price =
            double.TryParse(PriceEntry.Text, out double p)
            ? p : 0;

        bool success = await db.AddProduct(
            farmerName,
            title,
            description,
            qty,
            price,
            imagePath);

        if (success)
        {
            await DisplayAlert(
                "Success",
                "Product Added",
                "OK");

            // CLEAR FIELDS
            TitleEntry.Text = "";
            DescriptionEditor.Text = "";
            QtyEntry.Text = "";
            PriceEntry.Text = "";

            imagePath = "";

            ProductImage.Source = null;

            LoadProducts();
        }
    }

    private async void OnDeleteClicked(
        object sender,
        EventArgs e)
    {
        Button btn = (Button)sender;

        int productId = (int)btn.CommandParameter;

        bool success =
            await db.DeleteProduct(productId);

        if (success)
        {
            await DisplayAlert(
                "Deleted",
                "Product Deleted",
                "OK");

            LoadProducts();
        }
    }

    private async void LoadProducts()
    {
        List<ProductModel> products =
            await db.GetProducts(farmerName);

        ProductsCollection.ItemsSource = products;
    }
}