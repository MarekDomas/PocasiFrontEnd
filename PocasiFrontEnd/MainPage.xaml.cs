using System.Diagnostics;
using PocasiFrontEnd.Classes;

namespace PocasiFrontEnd;

public partial class MainPage : ContentPage
{
    public List<Day> Days = [];
    //Day day1 = new(DateTime.Now, 1, new Image(), 20);
    public MainPage()
    {
        populateList();

        InitializeComponent();
        refreshCollectionView();
    }

    private void populateList()
    {
        Days = [];
        var root = Day.GetApiData();
        for (var i = 0; i < 7; i++)
        {
            Days.Add(new Day(root,i));
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

        sunriseLabel.Text = $"Východ slunce: {day.Sunrise}";
        sunsetLabel.Text = $"Západ slunce: {day.Sunset}";
    }

    private void SubmitBtn_OnClicked(object? sender, EventArgs e)
    {
        var latitude = LatitudeEntry.Text.Replace('.', ',');
        var longitude = LongitudeEntry.Text.Replace('.', ',');
        if (!double.TryParse(latitude, out _) || !double.TryParse(longitude, out _))
        {
            DisplayAlert("Error", "Zadejte platnou šířku a délku", "Ok");
            return;
        }

        Day.Latitude =  LatitudeEntry.Text.Replace(',','.');
        Day.Longitude = LongitudeEntry.Text.Replace(',', '.'); 

        populateList();
        refreshCollectionView();
    }

    private void refreshCollectionView()
    {
        DayView.ItemsSource = null;
        DayView.ItemsSource = Days;
        DayView.SelectedItem = Days[0];
    }
}