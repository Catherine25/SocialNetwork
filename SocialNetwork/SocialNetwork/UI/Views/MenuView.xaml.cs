using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialNetwork.Data;
using SocialNetwork.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SocialNetwork.UI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuView : ContentView, IColorable
    {
        public event Action<FriendsView.Mode> SetFriendsViewRequest;
        public event Action SetGroupsViewRequest;
        public event Action SetMessagesViewRequest;
        public event Action SetSettingsViewRequest;
        public event Action SetCurrentUserViewRequest;
        
        public MenuView()
        {
            InitializeComponent();

            userViewBt.Clicked += UserViewBt_Clicked;
            messagesViewBt.Clicked += MessagesViewBt_Clicked;
            friendsViewBt.Clicked += FriendsViewBt_Clicked;
            groupsViewBt.Clicked += GroupsViewBt_Clicked;
            settingsViewBt.Clicked += SettingsViewBt_Clicked;
        }

        private void SettingsViewBt_Clicked(object sender, EventArgs e) =>
            SetSettingsViewRequest();

        private void GroupsViewBt_Clicked(object sender, EventArgs e) =>
            SetGroupsViewRequest();

        private void FriendsViewBt_Clicked(object sender, EventArgs e) =>
            SetFriendsViewRequest(FriendsView.Mode.Default);

        private void MessagesViewBt_Clicked(object sender, EventArgs e) =>
            SetMessagesViewRequest();

        private void UserViewBt_Clicked(object sender, EventArgs e) =>
            SetCurrentUserViewRequest();

        public void SetTheme(Theme theme) => (this as View).SetTheme(theme);
    }
}