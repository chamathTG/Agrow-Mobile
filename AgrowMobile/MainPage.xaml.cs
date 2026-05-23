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

    private async void OnForgotPasswordTapped(object sender, EventArgs e)
    {
        // Ask username
        string username = await DisplayPromptAsync(
            "Forgot Password",
            "Enter your username",
            "Next",
            "Cancel");

        if (string.IsNullOrWhiteSpace(username))
            return;

        // Ask mobile number
        string mobile = await DisplayPromptAsync(
            "Verification",
            "Enter your mobile number",
            "Verify",
            "Cancel",
            keyboard: Keyboard.Telephone);

        if (string.IsNullOrWhiteSpace(mobile))
            return;

        // Example verification
        if (username == "admin" && mobile == "0771234567")
        {
            await DisplayAlert(
                "Verified",
                "You can now reset your password",
                "OK");
        }
        else
        {
            await DisplayAlert(
                "Failed",
                "Username or mobile number is incorrect",
                "OK");
        }
    }
}