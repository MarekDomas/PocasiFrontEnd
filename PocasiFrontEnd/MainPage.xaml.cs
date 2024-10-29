using System.Diagnostics;
using PocasiFrontEnd.Classes;

namespace PocasiFrontEnd;

public partial class MainPage : ContentPage
{
    public List<Day> Days = [];
    public List<Place> Places = [new Place("Hradec Králové", "50.43340228879575", "15.352385419059253"), new Place("Praha", "50.0833", "14.4167"), new Place("Brno", "49.1952", "16.6084"), new Place("Ostrava", "49.8347", "18.2820"), new Place("Plzeň", "49.7475", "13.3777"), new Place("Liberec", "50.7671", "15.0562"), new Place("Olomouc", "49.5938", "17.2509")];

    public Place selectedPlace;
    //Day day1 = new(DateTime.Now, 1, new Image(), 20);
    public MainPage()
    {
        selectedPlace = Places[0];
        Day.Place = selectedPlace;
        populateList();
        InitializeComponent();
        refreshCollectionView();
        PlacesListView.SelectedItem = selectedPlace;
    }

    private void populateList()
    {
        Days = [];
        var root = Day.GetApiData();
     
        for (var i = 0; i < 7; i++)
        {
            Days.Add(new Day(root,i,selectedPlace));
        }
    }

    private void DayView_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        var day = e.CurrentSelection.FirstOrDefault() as Day;
        if (day is null) return;
        //Debug.Print($"{ day.Date}");
        var date = day.Date;
        var today = DateOnly.FromDateTime(DateTime.Today.Date );
        if (date == today)
        {
            TimeLabel.Text = $"Čas: {TimeOnly.Parse(day.CurrentData.time)}";
            TemperatureLabel.Text = $"Teplota: {day.CurrentData.temperature_2m} °C";
            CurrentImage.Source = day.CurrentData.IconPath;
        }
        else
        {
            TimeLabel.Text = $"Datum: {day.Date}";
            TemperatureLabel.Text = $"Maximální teplota: {day.MaxTemperature}\n Minimální teplota: {day.MinTemperature}";
            CurrentImage.Source = day.IconPath;
        }

        SunriseLabel.Text = $"Východ slunce: {day.Sunrise}";
        SunsetLabel.Text = $"Západ slunce: {day.Sunset}";
    }

    private void SubmitBtn_OnClicked(object? sender, EventArgs e)
    {
        var latitude = LatitudeEntry.Text.Replace('.', ',');
        var longitude = LongitudeEntry.Text.Replace('.', ',');
        var name = NameEntry.Text;
        if (!double.TryParse(latitude, out _) || !double.TryParse(longitude, out _))
        {
            DisplayAlert("Error", "Zadejte platnou šířku a délku", "Ok");
            return;
        }
        if(string.IsNullOrEmpty(name))
        {
            DisplayAlert("Error", "Zadejte název místa", "Ok");
            return;
        }

        latitude = latitude.Replace(',', '.');
        longitude = longitude.Replace(',', '.');

        Place place = new (name, latitude, longitude);

        if (Places.Contains(place))
        {
            DisplayAlert("Error", "Místo už existuje", "Ok");
            return;
        }

        Places.Add(place);
        selectedPlace = place;
        populateList();
        refreshCollectionView();
    }

    private void refreshCollectionView()
    {
        PlacesListView.ItemsSource = null;
        PlacesListView.ItemsSource = Places;

        DayView.ItemsSource = null;
        DayView.ItemsSource = Days;
        DayView.SelectedItem = Days[0];

        LongitudeEntry.Text = Day.Place.Longitude;
        LatitudeEntry.Text = Day.Place.Latitude;
    }

    private void PlacesListView_OnItemSelected(object? sender, SelectedItemChangedEventArgs e)
    {
        var place = e.SelectedItem as Place;
        if (place is null)
        {

            return;
        }
        DeleteBtn.IsEnabled = true;
        DeleteBtn.TextColor = Colors.White;
        selectedPlace = place;

        LongitudeEntry.Text = place.Longitude;
        LatitudeEntry.Text = place.Latitude;
        NameEntry.Text = place.Name;

        populateList();
        refreshCollectionView();
    }

    private void DeleteBtn_OnClicked(object? sender, EventArgs e)
    {
        Place place = new(NameEntry.Text, LatitudeEntry.Text,LongitudeEntry.Text);
        if (!Places.Contains(place)) return;
        Places.Remove(place);

        if (Places.Count != 0)
        {
            selectedPlace = Places[0];
        }
        populateList();
        refreshCollectionView();
        DeleteBtn.IsEnabled = false;
        DeleteBtn.TextColor = Colors.Gray;
    }
}