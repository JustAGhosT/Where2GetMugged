using Microsoft.Maui.Devices.Sensors;
using System.Timers;

namespace Where2GetMuggedNet7;

public partial class MainPage : ContentPage
{
    int count = 0;
    private Location _location;

    public MainPage()
    {
        InitializeComponent();

        

        var location = new Location(47.645160, -122.1306032);
        ResetLocation(location);

        // Create a timer
        var myTimer = new System.Timers.Timer();
        // Tell the timer what to do when it elapses
        myTimer.Elapsed += ((sender, args) => GetCurrentLocation());
        // Set it to go off every five seconds
        myTimer.Interval = 10000;
        myTimer.AutoReset = true;
        // And start it        
        myTimer.Enabled = true;
    }

    private void ResetLocation(Location loc)
    {
        mappy.Pins.Add(new Microsoft.Maui.Controls.Maps.Pin
        {
            Label = "Subscribe to my channel?",
            Location = loc,
        });
        NavigateToBuilding25(loc);
    }

    public async Task NavigateToBuilding25(Location location)
    {
        
        var options = new MapLaunchOptions { Name = "Microsoft Building 25" };
        try 
        { 
            await Map.Default.OpenAsync(location, options); 
        }
        catch (Exception ex)
        {         // No map application available to open     } }
        }
    }

    private CancellationTokenSource _cancelTokenSource; private bool _isCheckingLocation;

    private async Task GetCurrentLocation()
    {
        try { 
            _isCheckingLocation = true; 
            GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10)); 
            _cancelTokenSource = new CancellationTokenSource(); 
            Location location = await Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token);
            _location = location;
            ResetLocation(_location);
        }
        catch
        {
            
        }
    }
}

