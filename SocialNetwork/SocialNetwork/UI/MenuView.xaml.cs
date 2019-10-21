using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialNetwork.Data;
using SocialNetwork.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SocialNetwork.UI
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuView : ContentView, IColorable
    {
        public enum ButtonName { userView, messagesView, friendsView, groupsView, settingsView }
        public event Action<ButtonName> ButtonClicked;
        
        public MenuView()
        {
            InitializeComponent();

            userViewBt.Clicked += UserViewBt_Clicked;
            messagesViewBt.Clicked += MessagesViewBt_Clicked;
            friendsViewBt.Clicked += FriendsViewBt_Clicked;
            groupsViewBt.Clicked += GroupsViewBt_Clicked;
            settingsViewBt.Clicked += SettingsViewBt_Clicked;

            //UserViewBt_Clicked(null, null);
        }

        private void SettingsViewBt_Clicked(object sender, EventArgs e) =>
            ButtonClicked(ButtonName.settingsView);

        private void GroupsViewBt_Clicked(object sender, EventArgs e) =>
            ButtonClicked(ButtonName.groupsView);

        private void FriendsViewBt_Clicked(object sender, EventArgs e) =>
            ButtonClicked(ButtonName.friendsView);

        private void MessagesViewBt_Clicked(object sender, EventArgs e) =>
            ButtonClicked(ButtonName.messagesView);

        private void UserViewBt_Clicked(object sender, EventArgs e) =>
            ButtonClicked(ButtonName.userView);

        private void ThemeChanged(Theme theme) => SetTheme(theme);

        public void SetTheme(Theme theme) => (this as View).SetTheme(theme);
    }
}