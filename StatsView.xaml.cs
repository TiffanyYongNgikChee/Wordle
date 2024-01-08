using Wordle.ViewModel;

namespace Wordle;

public partial class StatsView : ContentPage
{
    public StatsView()
    {
        InitializeComponent();
        BindingContext = new StatsViewModel();
    }
    public StatsView(StatsViewModel statsViewModel)
    {
        InitializeComponent();
        BindingContext = statsViewModel;
    }
}