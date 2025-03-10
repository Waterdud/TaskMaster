using Microsoft.Maui.ApplicationModel.Communication;
using System.Collections.ObjectModel;

namespace TaskMaster
{
    public partial class EditContactPage : ContentPage
    {
        private Contact _Contact;
        private ObservableCollection<Contact> _Contacts;
        private bool _IsNewContact;
        private int v;

        public EditContactPage(Contact SelectedContact, ObservableCollection<Contact> CurrentContactList)
        {
            InitializeComponent();

            _Contacts = CurrentContactList;

            if (SelectedContact == null)
            {
                _Contact = new Contact();
                _IsNewContact = true;
            }
            else
            {
                _Contact = SelectedContact;
                _IsNewContact = false;
            }

            BindingContext = _Contact;
        }

        public EditContactPage(int v)
        {
            this.v = v;
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            if (_IsNewContact)
            {

                _Contacts.Add(_Contact);
            }
            else
            {

                var index = _Contacts.IndexOf(_Contact);
                if (index >= 0)
                {
                    _Contacts[index] = _Contact;
                }
            }

            await Navigation.PopAsync();
        }

        private async void OnAddPhotoClicked(object sender, EventArgs e)
        {
            try
            {

                var result = await MediaPicker.PickPhotoAsync();

                if (result != null)
                {
                    _Contact.Photo = result.FullPath;
                    ContactPhoto.Source = _Contact.Photo;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "No photo: " + ex.Message, "OK");
            }
        }
    }
}