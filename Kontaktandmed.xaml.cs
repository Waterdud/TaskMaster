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

        public Kontaktandmed(int v)
        {
            InitializeComponent();

            Contacts = new ObservableCollection<Contact>
            {
                new Contact { Name = "Dimasik", Photo = "dimas.jpg", Email = "dimitri.larionov1@gmail.com", Phone = "56997464", Description="What supp" }
            };

            ContactPicker.ItemsSource = Contacts;

            takePhotoButton = new Button { Text = "Сделать фото" };
            pickPhotoButton = new Button { Text = "Выбрать из галереи" };
            callButton = new Button { Text = "Позвонить" };
            smsButton = new Button { Text = "Отправить SMS" };
            emailButton = new Button { Text = "Отправить Email" };
            photo = new Image { HeightRequest = 300 };

            takePhotoButton.Clicked += async (s, e) => await TakePhoto();
            pickPhotoButton.Clicked += async (s, e) => await PickPhoto();
            callButton.Clicked += CallButton;
            smsButton.Clicked += SmsSendButton;
            emailButton.Clicked += EmailSendButton;
            contactListButton.Clicked += ViewContactListRedictPage;

            var buttonStack = new StackLayout
            {
                Padding = 20,
                VerticalOptions = LayoutOptions.Center,
                Children = { takePhotoButton, pickPhotoButton, photo, callButton, smsButton, emailButton, contactListButton }
            };

            this.Content = new StackLayout
            {
                Children = { ContactPicker, buttonStack }
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
                await DisplayAlert("Ошибка", ex.Message, "OK");
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
                await DisplayAlert("Ошибка", ex.Message, "OK");
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
                Uri smsUri = new Uri($"sms:{selectedContact.Phone}?body=Здраствуйте Димитрий, у меня к вам предложение погулять!");
                await Launcher.OpenAsync(smsUri);
            }
            else
            {
                await DisplayAlert("Error", "Select contact!", "OK");
            }
        }

        private async void EmailSendButton(object sender, EventArgs e)
        {
            var selectedContact = ContactPicker.SelectedItem as Contact;
            if (selectedContact != null && !string.IsNullOrEmpty(selectedContact.Email))
            {
                Uri emailUri = new Uri($"mailto:{selectedContact.Email}?subject=Hello&body=World!");
                await Launcher.OpenAsync(emailUri);
            }
            else
            {
                await DisplayAlert("Error", "Select contact!", "OK");
            }
        }

        private async void ViewContactListRedictPage(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ContactsPage(Contacts));
        }
    }
}
