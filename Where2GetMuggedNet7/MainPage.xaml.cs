using Azure.Messaging.WebPubSub;
using Microsoft.Maui.Devices.Sensors;
using Newtonsoft.Json;
using System.Net;
using System.Text.Json.Nodes;
using System.Timers;
using static System.Net.WebRequestMethods;
using Websocket.Client;

namespace Where2GetMuggedNet7;

public partial class MainPage : ContentPage
{
    private Location _location;
    private string _connectionString = @"Endpoint=https://wheretobemugged.webpubsub.azure.com;AccessKey=7x8vT+pF5baTrrDW5Qu8w8A/5pkFRF/8VtM2RjpZmHg=;Version=1.0;";
    private string _hub = "pubsub";

    public MainPage()
    {
        InitializeComponent();

        Load();        
    }

    private void Load()
    {
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

        SubcribeToLocation();
        
    }

    private async Task SubcribeToLocation()
    {
        var serviceClient = new WebPubSubServiceClient(_connectionString, _hub);
        var url = serviceClient.GetClientAccessUri();

        using (var client = new WebsocketClient(url))
        {
            // Disable the auto disconnect and reconnect because the sample would like the client to stay online even no data comes in
            client.ReconnectTimeout = null;
            client.MessageReceived.Subscribe(msg => UpdateClientLocation(msg));
            await client.Start();
        }
        Load();
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

        var options = new MapLaunchOptions { Name = "Mugger", NavigationMode = NavigationMode.Driving};
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
            await PublishLocation(_location);
        }
        catch
        {

        }
    }

    private async Task PublishLocation(Location location)
    {
        location = new Location(47.645160, -122.1306032);
        var message = JsonConvert.SerializeObject(location);
        var serviceClient = new WebPubSubServiceClient(_connectionString, _hub);
        await serviceClient.SendToAllAsync(message);
    }

    private void UpdateClientLocation(ResponseMessage responseMessage)
    {
        var clientLocation = JsonConvert.DeserializeObject<Location>(responseMessage.Text);

        //var pin = BitmapDescriptorFactory.FromAsset("car.png");
        mappy.Pins.Add(new Microsoft.Maui.Controls.Maps.Pin
        {
            Label = "Client Location",
            Location = clientLocation
        });
    }
}

