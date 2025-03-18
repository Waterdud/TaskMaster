using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;
using Microsoft.Maui.ApplicationModel;
using System;
using System.IO;
using System.Threading.Tasks;

namespace TaskMaster
{
    public partial class Kontaktandmed : ContentPage
    {
        public ObservableCollection<Contact> Contacts { get; set; }
        Image photo;
        Button takePhotoButton, pickPhotoButton, callButton, smsButton, emailButton, contactListButton;

        public Kontaktandmed(int k)
        {
            InitializeComponent();

            Contacts = new ObservableCollection<Contact>
            {
                new Contact { Name = "Dimasik", Photo = "dimas.jpg", Email = "dimitri.larionov1@gmail.com", Phone = "56997464", Description="What supp" }
            };

            ContactPicker.ItemsSource = Contacts;

            takePhotoButton = new Button { Text = "Tee foto" };
            pickPhotoButton = new Button { Text = "Vali galeriist" };
            callButton = new Button { Text = "Helista" };
            smsButton = new Button { Text = "Saada SMS" };
            emailButton = new Button { Text = "Saada Email" };
            photo = new Image { HeightRequest = 300 };
            contactListButton = new Button { Text = "Kontakt" };

            takePhotoButton.Clicked += async (s, e) => await TakePhoto();
            pickPhotoButton.Clicked += async (s, e) => await PickPhoto();
            callButton.Clicked += CallButton;
            smsButton.Clicked += SmsSendButton;
            emailButton.Clicked += EmailSendButton;
            contactListButton.Clicked += ViewContactListRedictPage;

            this.Content = new StackLayout
            {
                Children =
                {
                    ContactPicker,
                    new StackLayout
                    {
                        Padding = 20,
                        VerticalOptions = LayoutOptions.Center,
                        Children = { takePhotoButton, pickPhotoButton, photo, callButton, smsButton, emailButton, contactListButton }
                    }
                }
            };
        }

        private async Task TakePhoto()
        {
            try
            {
                var result = await MediaPicker.CapturePhotoAsync();
                if (result != null)
                {
                    var stream = await result.OpenReadAsync();
                    photo.Source = ImageSource.FromStream(() => stream);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Viga", ex.Message, "OK");
            }
        }

        private async Task PickPhoto()
        {
            try
            {
                var result = await MediaPicker.PickPhotoAsync();
                if (result != null)
                {
                    var stream = await result.OpenReadAsync();
                    photo.Source = ImageSource.FromStream(() => stream);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Viga", ex.Message, "OK");
            }
        }

        private async void CallButton(object sender, EventArgs e)
        {
            var selectedContact = ContactPicker.SelectedItem as Contact;
            if (selectedContact != null && !string.IsNullOrEmpty(selectedContact.Phone))
            {
                await Launcher.Default.OpenAsync($"tel:{selectedContact.Phone}");
            }
        }

        private async void SmsSendButton(object sender, EventArgs e)
        {
            var selectedContact = ContactPicker.SelectedItem as Contact;
            if (selectedContact != null && !string.IsNullOrEmpty(selectedContact.Phone))
            {
                Uri smsUri = new Uri($"sms:{selectedContact.Phone}?body=Tere Dmitri, mul on sulle ettepanek jalutama minna!");
                await Launcher.OpenAsync(smsUri);
            }
            else
            {
                await DisplayAlert("Viga", "Vali kontakt!", "OK");
            }
        }

        private async void EmailSendButton(object sender, EventArgs e)
        {
            var selectedContact = ContactPicker.SelectedItem as Contact;
            if (selectedContact != null && !string.IsNullOrEmpty(selectedContact.Email))
            {
                Uri emailUri = new Uri($"mailto:{selectedContact.Email}?subject=Tere&body=Maailm!");
                await Launcher.OpenAsync(emailUri);
            }
            else
            {
                await DisplayAlert("Viga", "Vali kontakt!", "OK");
            }
        }

        private void ViewContactListRedictPage(object sender, EventArgs e)
        {
            var image = new Image
            {
                Source = "Resources/Images/dimas.jpg", // Используйте нужное изображение
                HeightRequest = 100,
                IsVisible = false // Скрыто изначально
            };

            var toggleSwitch = new Switch();
            toggleSwitch.Toggled += (s, ev) => image.IsVisible = ev.Value; // Показываем/скрываем картинку

            var tableView = new TableView
            {
                Intent = TableIntent.Form,
                Root = new TableRoot("Kontaktandmed")
                {
                    new TableSection("Põhiandmed:")
                    {
                        new ViewCell
                        {
                            View = new StackLayout
                            {
                                Orientation = StackOrientation.Horizontal,
                                Children =
                                {
                                    new Label { Text = "Nimi:", FontAttributes = FontAttributes.Bold, TextColor = Colors.Red },
                                    new Entry { Placeholder = "Sisesta oma sõbra nimi" }
                                }
                            }
                        }
                    },
                    new TableSection("Kontaktandmed:")
                    {
                        new ViewCell
                        {
                            View = new StackLayout
                            {
                                Orientation = StackOrientation.Horizontal,
                                Children =
                                {
                                    new Label { Text = "Telefon", FontAttributes = FontAttributes.Bold, TextColor = Colors.Red },
                                    new Entry { Placeholder = "Sisesta tel. number" }
                                }
                            }
                        },
                        new ViewCell
                        {
                            View = new StackLayout
                            {
                                Orientation = StackOrientation.Horizontal,
                                Children =
                                {
                                    new Label { Text = "Email", FontAttributes = FontAttributes.Bold, TextColor = Colors.Red },
                                    new Entry { Placeholder = "Sisesta email" }
                                }
                            }
                        }
                    },
                    new TableSection
                    {
                        new ViewCell
                        {
                            View = new StackLayout
                            {
                                Orientation = StackOrientation.Horizontal,
                                Children =
                                {
                                    new Label { Text = "Näita veel" },
                                    toggleSwitch
                                }
                            }
                        }
                    },
                    new TableSection("Foto:")
                    {
                        new ViewCell
                        {
                            View = new StackLayout
                            {
                                Children = { image }
                            }
                        }
                    }
                }
            };

            this.Content = tableView;
        }
    }
}
