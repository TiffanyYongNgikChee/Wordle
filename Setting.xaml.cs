using Wordle.ViewModel;
namespace Wordle;

public partial class Setting : ContentPage
{
    private GameViewModel game = new GameViewModel();
    public Setting()
    {
        InitializeComponent();
        BindingContext = game;
    }
    private void darkMode_switch_Toggled(object sender, ToggledEventArgs e)
    {
        //changes dark mode value and toggles theme.
        try
        {
            Application.Current.Resources["IsDarkMode"] = e.Value;
            darkMode_switch.IsToggled = e.Value;
            ToggleTheme(e.Value);

        }
        catch (Exception ex)
        {
            Shell.Current.DisplayAlert("Error toggling dark mode", ex.Message, "OK");
        }
    }
    private void ToggleTheme(bool isDarkMode)
    {
        //changes resource backgrounds depending on theme.
        var app = (App)Application.Current;

        if (isDarkMode)
        {
            app.Resources["BackgroundColor"] = Color.FromHex("#121212");
            app.Resources["TextColor"] = Color.FromHex("#FFFFFF");

        }
        else
        {
            app.Resources["BackgroundColor"] = Color.FromHex("#FFFFFF");
            app.Resources["TextColor"] = Color.FromHex("#000000");
        }
    }
}
