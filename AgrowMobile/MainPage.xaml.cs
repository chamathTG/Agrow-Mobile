using AgrowMobile.Services;

namespace AgrowMobile;

public partial class MainPage : ContentPage
{
    DbService db = new DbService();

    public MainPage()
    {
        InitializeComponent();
    }

    private async void OnCreateAccountTapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(SignupPage));
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        string? role = LoginAsPicker.SelectedItem?.ToString();

        string username = UsernameEntry.Text ?? "";
        string password = PasswordEntry.Text ?? "";

        if (string.IsNullOrWhiteSpace(role) ||
            string.IsNullOrWhiteSpace(username) ||
            string.IsNullOrWhiteSpace(password))
        {
            await DisplayAlert("Error", "Fill all fields", "OK");
            return;
        }

        string? result = await db.LoginUser(
            role,
            username,
            password);

        if (result == "Customer")
        {
            await DisplayAlert(
                "Success",
                "Customer Login Successful",
                "OK");

            await Navigation.PushAsync(
                new CustomerDashboardPage());
        }
        else if (result == "Farmer")
        {
            await DisplayAlert(
                "Success",
                "Farmer Login Successful",
                "OK");

            await Navigation.PushAsync(
                new FarmerDashboardPage(username));
        }
        else
        {
            await DisplayAlert(
                "Failed",
                "Invalid Login Details",
                "OK");
        }
    }

    private async void OnForgotPasswordTapped(
    object sender,
    EventArgs e)
    {
        string username =
            await DisplayPromptAsync(
                "Forgot Password",
                "Enter your username",
                "Next",
                "Cancel");

        if (string.IsNullOrWhiteSpace(username))
            return;

        string mobile =
            await DisplayPromptAsync(
                "Verification",
                "Enter your mobile number",
                "Verify",
                "Cancel",
                keyboard: Keyboard.Telephone);

        if (string.IsNullOrWhiteSpace(mobile))
            return;

        string newPassword =
            await DisplayPromptAsync(
                "Reset Password",
                "Enter new password",
                "Reset",
                "Cancel");

        if (string.IsNullOrWhiteSpace(newPassword))
            return;

        bool success =
            await db.ResetPassword(
                username,
                mobile,
                newPassword);

        if (success)
        {
            await DisplayAlert(
                "Success",
                "Password Reset Successful",
                "OK");
        }
        else
        {
            await DisplayAlert(
                "Failed",
                "Username or mobile incorrect",
                "OK");
        }
    }
}