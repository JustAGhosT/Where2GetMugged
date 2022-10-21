using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;

namespace Where2GetMuggedNet7;

public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage()
	{
		InitializeComponent();
	}
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        var hanaLoc = new Location(20.7557, -155.9880);

        MapSpan mapSpan = MapSpan.FromCenterAndRadius(hanaLoc, Distance.FromKilometers(3));
        map.MoveToRegion(mapSpan);
        map.Pins.Add(new Pin
        {
            Label = "Welcome to .NET MAUI!",
            Location = hanaLoc,
        });
    }
    //private void OnCounterClicked(object sender, EventArgs e)
    //{
    //	count++;

    //	if (count == 1)
    //		CounterBtn.Text = $"Clicked {count} time";
    //	else
    //		CounterBtn.Text = $"Clicked {count} times";

    //	SemanticScreenReader.Announce(CounterBtn.Text);
    //}
}

