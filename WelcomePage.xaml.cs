using AndroidX.ConstraintLayout.Helper.Widget;

namespace Wordle;

public partial class WelcomePage : ContentPage
{
    public WelcomePage()
    {
        InitializeComponent();
    }
    private async void OnGoToMainPageClicked(object sender, EventArgs e)
    {
        string thisname = "";
        thisname = player1.Text;

        if (thisname == null || thisname.Length == 0)
        {
            await DisplayAlert("Enter Player Name", "You need to enter your name", "OK");
            return;
        }

        // Navigate to the Main Page
        await Navigation.PushAsync(new MainPage());
    }
    private async void btnHelp_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Help());
    }

    private async void btnStats_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new StatsView());
    }
}