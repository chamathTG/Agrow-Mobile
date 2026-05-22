namespace AgrowMobile;

public partial class SignupPage : ContentPage
{
	public SignupPage()
	{
		InitializeComponent();
	}

    private async void OnLoginTapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}