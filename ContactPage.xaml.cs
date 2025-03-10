using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using TaskMaster;

namespace TaskMaster
{
    public partial class ContactsPage : ContentPage
    {
        public ObservableCollection<Contact> Contacts { get; set; }

        public ContactsPage(ObservableCollection<Contact> contacts)
        {
            InitializeComponent();
            Contacts = contacts;
            PopulateContacts();
        }

        private void PopulateContacts()
        {
            ContactGridLayout.Clear();

            foreach (var contact in Contacts)
            {
                var viewCell = new ViewCell();
                var grid = new Grid
                {
                    Padding = new Thickness(10),
                    RowSpacing = 0,
                    ColumnSpacing = 12,
                    RowDefinitions =
                    {
                        new RowDefinition { Height = GridLength.Auto },
                        new RowDefinition { Height = GridLength.Auto },
                        new RowDefinition { Height = GridLength.Auto },
                        new RowDefinition { Height = GridLength.Auto }
                    },
                    ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = GridLength.Auto },  // Column for images
                        new ColumnDefinition { Width = GridLength.Star }  // Column for lables
                    }
                };

                // Inserting data into labels
                var ImageControl = new Image
                {
                    Source = contact.Photo,
                    HeightRequest = 60,
                    WidthRequest = 60,
                    Aspect = Aspect.AspectFill,
                };

                var NameLabel = new Label
                {
                    Text = contact.Name,
                    FontAttributes = FontAttributes.Bold,
                    FontSize = 18,
                    LineBreakMode = LineBreakMode.WordWrap
                };

                var PhoneLabel = new Label
                {
                    Text = "Phone number: " + contact.Phone,
                    FontSize = 14,
                    LineBreakMode = LineBreakMode.WordWrap
                };

                var EmailLabel = new Label
                {
                    Text = "E-Mail address: " + contact.Email,
                    FontSize = 14,
                    LineBreakMode = LineBreakMode.WordWrap
                };

                var DescriptionLabel = new Label
                {
                    Text = "Description: " + contact.Description,
                    FontAttributes = FontAttributes.Italic,
                    FontSize = 12,
                    LineBreakMode = LineBreakMode.WordWrap
                };


                // Setting up layout for Contact Page
                grid.Children.Add(ImageControl);
                Grid.SetRowSpan(ImageControl, 4);
                Grid.SetColumn(ImageControl, 0);

                grid.Children.Add(NameLabel);
                Grid.SetRow(NameLabel, 0);
                Grid.SetColumn(NameLabel, 1);

                grid.Children.Add(PhoneLabel);
                Grid.SetRow(PhoneLabel, 1);
                Grid.SetColumn(PhoneLabel, 1);

                grid.Children.Add(EmailLabel);
                Grid.SetRow(EmailLabel, 2);
                Grid.SetColumn(EmailLabel, 1);

                grid.Children.Add(DescriptionLabel);
                Grid.SetRow(DescriptionLabel, 3);
                Grid.SetColumn(DescriptionLabel, 1);

                viewCell.View = grid;

                viewCell.Tapped += async (sender, e) =>
                {
                    await Navigation.PushAsync(new EditContactPage(contact, Contacts));
                };

                ContactGridLayout.Add((IView)viewCell);
            }
        }

        private async void OnAddContactClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EditContactPage(null, Contacts));
        }
    }
}
