using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SocialNetwork.UI
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DialogBar : ContentView
    {
        private string NoUserAvatarLink = "https://www.indiannaturaloils.com/categories-images/no-photo.jpg";
        public event Action ReturnRequest;

        public DialogBar() => InitializeComponent();

        public void Update(string link, string name)
        {
            _name.Text = name;
            TrySetImage(link);

            _backButton.Clicked += (object o, EventArgs e) => ReturnRequest();
        }

        private void TrySetImage(string link)
        {
            try
            {
                _avatar.Source = ImageSource.FromUri(new Uri(link));
            }
            catch
            {
                _avatar.Source = NoUserAvatarLink;
            }
        }
    }
}