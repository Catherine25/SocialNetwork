using SocialNetwork.Data;
using SocialNetwork.Services;
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
    public partial class SettingsView : ContentView, IColorable
    {
        User CurrentUser;
        List<Theme> themes = new List<Theme>();
        
        public SettingsView(User user)
        {
            InitializeComponent();
            ImportThemes();

            this.CurrentUser = user;

            themePicker.SelectedIndexChanged += ThemePicker_SelectedIndexChanged;
        }

        public void SetTheme(Theme theme) => (this as View).SetTheme(theme);

        private void ThemePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            string item = (string)themePicker.SelectedItem;
            Theme theme = themes.Find(X=>X.Name == item);
            color1.BackgroundColor = theme.TextColor;
            color2.BackgroundColor = theme.BackgroundColor;
            color3.BackgroundColor = theme.SeparatorColor;

            CurrentUser.Theme = theme;
        }

        private void ImportThemes() {
            foreach(var theme in Themes.ThemesList)
            {
                themes.Add(theme);
                themePicker.Items.Add(theme.Name);
            }
        }
    }
}