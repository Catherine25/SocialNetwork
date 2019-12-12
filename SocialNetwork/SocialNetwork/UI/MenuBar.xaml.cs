using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SocialNetwork.UI
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuBar : ContentView
    {
        public event Action<string> SearchRequest;

        public MenuBar() => InitializeComponent();

        public void Update(string name)
        {
            _name.Text = name;
            
            _menuButton.Clicked += (object o, EventArgs e) =>
            {
                throw new NotImplementedException();
            };

            _searchButton.Clicked += (object o, EventArgs e) =>
            {
                _menuButton.IsVisible = false;
                _searchButton.IsVisible = false;
                _companionNameEntry.IsVisible = true;
            };

            _companionNameEntry.Completed += (object o, EventArgs e) =>
            {
                _menuButton.IsVisible = true;
                _searchButton.IsVisible = true;
                _companionNameEntry.IsVisible = false;
                SearchRequest(_companionNameEntry.Text);
            };
        }
    }
}