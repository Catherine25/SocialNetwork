using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SocialNetwork.UI
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DialogBar : ContentView
    {
        public event Action ReturnRequest;

        public DialogBar() => InitializeComponent();

        public void Update(string link, string name)
        {
            _avatar.Source = ImageSource.FromUri(new Uri(link));
            _name.Text = name;

            _backButton.Clicked += (object o, EventArgs e) => ReturnRequest();
        }
    }
}