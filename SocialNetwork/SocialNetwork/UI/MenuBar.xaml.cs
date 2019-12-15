using System;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SocialNetwork.UI
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuBar : ContentView
    {
        public event Action<string> SearchRequest;
        public event Action OpenSettingsViewRequest;

        public MenuBar()
        {
            InitializeComponent();
            IncludeDebug();

            _menuButton.Clicked += (object o, EventArgs e) => OpenSettingsViewRequest();

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
                _companionNameEntry.Text = "";
            };
        }

        public void Update(string name) => _name.Text = name;

        private void IncludeDebug()
        {
            _menuButton.Clicked += (object o, EventArgs e) => Debug.WriteLine("[m] [MenuBar] _menuButton Clicked");
            _searchButton.Clicked += (object o, EventArgs e) => Debug.WriteLine("[m] [MenuBar] _searchButton Clicked");
            _companionNameEntry.TextChanged += (object o, TextChangedEventArgs e) => Debug.WriteLine("[m] [MenuBar] _companionNameEntry Clicked");
        }
    }
}