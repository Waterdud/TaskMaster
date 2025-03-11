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
                new Contact { Name = "Dimasik", Photo = "dimas.jpg", Email = "dimitri.larionov1@gmail.com",Phone = "56997464", Description="What supp" }
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