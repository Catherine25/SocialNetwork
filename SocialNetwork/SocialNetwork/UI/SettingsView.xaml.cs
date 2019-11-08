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
        List<Theme> themes;

        public event Action<string> ChangeThemeRequest;
        
        public SettingsView(User user, List<Theme> newThemes)
        {
            InitializeComponent();
            ImportThemes(newThemes);

            CurrentUser = user;

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

            ChangeThemeRequest(theme.Name);
        }

        private void ImportThemes(List<Theme> newThemes)
        {
            foreach(var theme in newThemes)
            {
                themes.Add(theme);
                themePicker.Items.Add(theme.Name);
            }
        }
    }
}