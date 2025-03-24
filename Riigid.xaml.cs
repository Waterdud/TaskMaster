using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;

namespace TaskMaster;

public class Riik
{
    public string Nimi { get; set; }
    public string Pealinn { get; set; }
    public string Rahvaarv { get; set; }
    public string Lipp { get; set; }
    public int Continent { get; set; }
}

public partial class Riigid : ContentPage
{
    public ObservableCollection<Riik> EuroopaRiigid { get; set; } = new();
    public ObservableCollection<Riik> AmeerikaRiigid { get; set; } = new();
    private ObservableCollection<Riik> riigid { get; set; } = new();

    public Riigid(int v)
    {
        riigid.Add(new Riik { Nimi = "Estonia", Pealinn = "Tallinn", Rahvaarv = "345987", Lipp = "eesti.png", Continent = 1 });
        riigid.Add(new Riik { Nimi = "Canada", Pealinn = "Ottawa", Rahvaarv = "678098", Lipp = "canada.png", Continent = 0 });

        foreach (var riik in riigid)
        {
            if (riik.Continent == 1)
                EuroopaRiigid.Add(riik);
            else
                AmeerikaRiigid.Add(riik);
        }

        Content = new StackLayout
        {
            Children =
            {
                new Label { Text = "Riigid", FontSize = 24, HorizontalOptions = LayoutOptions.Center },
                CreateListView("Euroopa riigid:", EuroopaRiigid),
                CreateListView("Ameerika riigid:", AmeerikaRiigid),
                new Button { Text = "Lisa", Command = new Command(async () => await LisaRiik()) },
                new Button { Text = "Kustuta", Command = new Command(KustutaRiik) }
            }
        };
    }

    private ListView CreateListView(string header, ObservableCollection<Riik> items)
    {
        return new ListView
        {
            Header = header,
            ItemsSource = items,
            ItemTemplate = new DataTemplate(() =>
            {
                var grid = new Grid();
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });

                var stack = new StackLayout { Orientation = StackOrientation.Horizontal, Spacing = 10 };

                var flagImage = new Image { WidthRequest = 50, HeightRequest = 30 };
                flagImage.SetBinding(Image.SourceProperty, "Lipp");

                var nameLabel = new Label { TextColor = Colors.Red, FontSize = 18 };
                nameLabel.SetBinding(Label.TextProperty, "Nimi");

                var capitalLabel = new Label { TextColor = Colors.Green, FontSize = 14 };
                capitalLabel.SetBinding(Label.TextProperty, new Binding("Pealinn", stringFormat: "Selle riigi pealinn on {0}"));

                stack.Children.Add(flagImage);
                stack.Children.Add(new StackLayout { Children = { nameLabel, capitalLabel } });

                var editButton = new Button { Text = "Muuda", BackgroundColor = Colors.LightGray };
                editButton.SetBinding(Button.CommandParameterProperty, new Binding("."));
                editButton.Clicked += async (sender, e) =>
                {
                    var button = (Button)sender;
                    var riik = (Riik)button.CommandParameter;
                    await MuudaRiik(riik);
                };

                grid.Children.Add(stack);
                Grid.SetColumn(stack, 0);
                grid.Children.Add(editButton);
                Grid.SetColumn(editButton, 1);

                return new ViewCell { View = grid };
            })
        };
    }

    private async Task LisaRiik()
    {
        string nimi = await DisplayPromptAsync("Sisesta nimi", "Sisesta nimi", keyboard: Keyboard.Default);
        if (riigid.Any(r => r.Nimi == nimi))
        {
            await DisplayAlert("Viga", "See riik on juba olemas!", "OK");
            return;
        }

        string pealinn = await DisplayPromptAsync("Sisesta pealinn", "Sisesta pealinn", keyboard: Keyboard.Default);
        string rahvaarv = await DisplayPromptAsync("Sisesta rahvaarv", "Sisesta rahvaarv", keyboard: Keyboard.Numeric);
        string kontinent = await DisplayPromptAsync("Vali kontinent", "Euroopa (1) või Ameerika (0)", keyboard: Keyboard.Numeric);

        var photo = await MediaPicker.PickPhotoAsync();
        var img = photo?.FileName ?? "defaultimage.jpg";

        var newRiik = new Riik { Nimi = nimi, Pealinn = pealinn, Rahvaarv = rahvaarv, Lipp = img, Continent = int.Parse(kontinent) };
        riigid.Add(newRiik);
        if (newRiik.Continent == 1)
            EuroopaRiigid.Add(newRiik);
        else
            AmeerikaRiigid.Add(newRiik);
    }

    private void KustutaRiik()
    {
        if (EuroopaRiigid.Count > 0)
            EuroopaRiigid.RemoveAt(EuroopaRiigid.Count - 1);
        else if (AmeerikaRiigid.Count > 0)
            AmeerikaRiigid.RemoveAt(AmeerikaRiigid.Count - 1);
    }

    private async Task MuudaRiik(Riik riik)
    {
        string uusNimi = await DisplayPromptAsync("Muuda nime", "", initialValue: riik.Nimi);
        string uusPealinn = await DisplayPromptAsync("Muuda pealinna", "", initialValue: riik.Pealinn);
        string uusRahvaarv = await DisplayPromptAsync("Muuda rahvaarvu", "", initialValue: riik.Rahvaarv, keyboard: Keyboard.Numeric);
        string uusKontinent = await DisplayPromptAsync("Muuda kontinent", "", initialValue: riik.Continent.ToString(), keyboard: Keyboard.Numeric);

        var photo = await MediaPicker.PickPhotoAsync();
        var uusLipp = photo?.FileName ?? riik.Lipp;

        riik.Nimi = uusNimi;
        riik.Pealinn = uusPealinn;
        riik.Rahvaarv = uusRahvaarv;
        riik.Lipp = uusLipp;
        riik.Continent = int.Parse(uusKontinent);

        EuroopaRiigid.Clear();
        AmeerikaRiigid.Clear();
        foreach (var r in riigid)
        {
            if (r.Continent == 1)
                EuroopaRiigid.Add(r);
            else
                AmeerikaRiigid.Add(r);
        }
    }
}
