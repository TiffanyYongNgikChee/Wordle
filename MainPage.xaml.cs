using Wordle.ViewModel;

namespace Wordle;

public partial class MainPage : ContentPage
{
    private string SaveFilePath => System.IO.Path.Combine(FileSystem.Current.AppDataDirectory, "savefile.txt");

    public MainPage()
    {
        InitializeComponent();
        BindingContext = new GameViewModel();

        var frame = new Frame();
    }

    private async void btnHelp_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Help());
    }

    private async void btnStats_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new StatsView());
    }

    private async void btnSetting_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Setting());
    }

    private void btnNewGame_Clicked(object sender, EventArgs e)
    {
        (BindingContext as GameViewModel)?.ResetGame();
    }
}


