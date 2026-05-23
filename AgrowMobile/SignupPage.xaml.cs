using AgrowMobile.Services;

namespace AgrowMobile;

public partial class SignupPage : ContentPage
{
    DbService db = new DbService();

    public SignupPage()
    {
        InitializeComponent();
    }

    private async void OnLoginTapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new MainPage());
    }

    private async void OnSignupClicked(object sender, EventArgs e)
    {
        string? role = SignupAsPicker.SelectedItem?.ToString();

        string mobile = MobileEntry.Text ?? "";
        string username = UsernameEntry.Text ?? "";
        string email = EmailEntry.Text ?? "";
        string password = PasswordEntry.Text ?? "";

        if (string.IsNullOrWhiteSpace(role) ||
            string.IsNullOrWhiteSpace(mobile) ||
            string.IsNullOrWhiteSpace(username) ||
            string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(password))
        {
            await DisplayAlert("Error", "Fill all fields", "OK");
            return;
        }

        bool success = await db.RegisterUser(
            role,
            mobile,
            username,
            email,
            password);

        if (success)
        {
            await DisplayAlert(
                "Success",
                "Account Created Successfully",
                "OK");

            await Navigation.PushAsync(new MainPage());
        }
        else
        {
            await DisplayAlert(
                "Failed",
                "Registration Failed",
                "OK");
        }
    }
}