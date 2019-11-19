using SocialNetwork.Data;
using SocialNetwork.Services;
using SocialNetwork.UI.DataRequests;
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
        List<Theme> themes;

        public event Action<Theme> ChangeThemeRequest;
        public event Action<UserRequestDialog.RequestPurpose> ReloginRequest;
        
        public SettingsView(User user, List<Theme> newThemes)
        {
            InitializeComponent();
			themes = new List<Theme>();
            ImportThemes(newThemes);

            themePicker.SelectedIndexChanged += ThemePicker_SelectedIndexChanged;
            ReloginBt.Clicked += ReloginBt_Clicked;
        }

        private void ReloginBt_Clicked(object sender, EventArgs e) =>
            ReloginRequest(UserRequestDialog.RequestPurpose.currentName);

        public void SetTheme(Theme theme) => (this as View).SetTheme(theme);

        private void ThemePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            string item = (string)themePicker.SelectedItem;
            Theme theme = themes.Find(X=>X.Name == item);
            color1.BackgroundColor = theme.TextColor;
            color2.BackgroundColor = theme.BackgroundColor;
            color3.BackgroundColor = theme.SeparatorColor;

            ChangeThemeRequest(theme);
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