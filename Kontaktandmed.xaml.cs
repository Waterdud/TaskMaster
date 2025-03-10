using System.Collections.ObjectModel;

namespace TaskMaster
{
    public partial class Kontaktandmed : ContentPage
    {
        // Why using ObservableCollection?
        // Because it dynamically saves and easily syncs with all Contact list variable.
        public ObservableCollection<Contact> Contacts { get; set; }

        public Kontaktandmed(int v)
        {
            InitializeComponent();

            // Creating contact demo.
            Contacts = new ObservableCollection<Contact>
            {
                new Contact { Name = "Example", Photo = "default.png", Email = "example@example.com",Phone = "123456789", Description="Test test hello world" }
            };

            ContactPicker.ItemsSource = Contacts;
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
                Uri smsUri = new Uri($"sms:{selectedContact.Phone}?body=Hello world!");
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