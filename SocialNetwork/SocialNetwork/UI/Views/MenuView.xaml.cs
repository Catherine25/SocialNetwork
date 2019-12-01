using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        private void SettingsViewBt_Clicked(object sender, EventArgs e)
        {
            Debug.WriteLine("[m] [MenuView] SettingsViewBt_Clicked running");

            SetSettingsViewRequest();
        }

        private void GroupsViewBt_Clicked(object sender, EventArgs e)
        {
            Debug.WriteLine("[m] [MenuView] GroupsViewBt_Clicked running");

            SetGroupsViewRequest();
        }

        private void FriendsViewBt_Clicked(object sender, EventArgs e)
        {
            Debug.WriteLine("[m] [MenuView] FriendsViewBt_Clicked running");

            SetFriendsViewRequest(FriendsView.Mode.Editable);
        }

        private void MessagesViewBt_Clicked(object sender, EventArgs e)
        {
            Debug.WriteLine("[m] [MenuView] MessagesViewBt_Clicked running");

            SetMessagesViewRequest();
        }

        private void UserViewBt_Clicked(object sender, EventArgs e)
        {
            Debug.WriteLine("[m] [MenuView] UserViewBt_Clicked running");

            SetCurrentUserViewRequest();
        }

        public void SetTheme(Theme theme)
        {
            Debug.WriteLine("[m] [MenuView] SetTheme running");

            (this as View).SetTheme(theme);
        }
    }
}