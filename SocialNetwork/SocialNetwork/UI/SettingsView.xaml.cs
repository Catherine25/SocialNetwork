using SocialNetwork.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

//TODO: Add themes

namespace SocialNetwork.UI
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsView : ContentView
    {
        User user;
        Dictionary<string, Theme> themes = new Dictionary<string, Theme>
        {
            {
                "Theme1", new Theme("default", new Color[] {
                Color.FromRgb(26,24,24),
                Color.Black,
                Color.Black,
                Color.Black })
            }
        };
        
        public SettingsView(User user)
        {
            InitializeComponent();

            this.user = user;

            themePicker.SelectedIndexChanged += ThemePicker_SelectedIndexChanged;
        }

        private void ThemePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            string item = (string)themePicker.SelectedItem;
            Theme theme = themes[item];
            color1.BackgroundColor = theme.colors[0];
            user.Theme = theme;
        }
    }
}