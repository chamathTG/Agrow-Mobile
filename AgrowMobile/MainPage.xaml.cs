namespace AgrowMobile;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private async void OnCreateAccountTapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(SignupPage));
    }

    private void OnForgotPasswordTapped(object sender, EventArgs e)
    {
        ForgotPopup.IsVisible = true;
    }

    private void OnCancelForgotPopup(object sender, EventArgs e)
    {
        ForgotPopup.IsVisible = false;
    }

    private async void OnVerifyForgotPassword(object sender, EventArgs e)
    {
        string username = ForgotUsernameEntry.Text;
        string mobile = ForgotMobileEntry.Text;

        if (string.IsNullOrWhiteSpace(username) ||
            string.IsNullOrWhiteSpace(mobile))
        {
            await DisplayAlert("Error", "Fill all fields", "OK");
            return;
        }

        // Example verification
        if (username == "admin" && mobile == "0771234567")
        {
            await DisplayAlert(
                "Verified",
                "You can now reset password",
                "OK");

            ForgotPopup.IsVisible = false;
        }
        else
        {
            await DisplayAlert(
                "Failed",
                "Invalid username or mobile number",
                "OK");
        }
    }
}