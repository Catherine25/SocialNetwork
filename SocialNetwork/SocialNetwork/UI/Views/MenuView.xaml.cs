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

            #region Debug
            
            userViewBt.Clicked += (object sender, EventArgs e) =>
                Debug.WriteLine("[m] [MenuView] UserViewBt_Clicked running");
            settingsViewBt.Clicked += (object sender, EventArgs e) =>
                Debug.WriteLine("[m] [MenuView] SettingsViewBt_Clicked running");
            groupsViewBt.Clicked += (object sender, EventArgs e) =>
                Debug.WriteLine("[m] [MenuView] GroupsViewBt_Clicked running");
            friendsViewBt.Clicked += (object sender, EventArgs e) =>
                Debug.WriteLine("[m] [MenuView] FriendsViewBt_Clicked running");
            messagesViewBt.Clicked += (object sender, EventArgs e) =>
                Debug.WriteLine("[m] [MenuView] MessagesViewBt_Clicked running");

            #endregion

            userViewBt.Clicked += (object sender, EventArgs e) => SetCurrentUserViewRequest();
            messagesViewBt.Clicked += (object sender, EventArgs e) => SetMessagesViewRequest();
            friendsViewBt.Clicked += (object sender, EventArgs e) =>
                SetFriendsViewRequest(FriendsView.Mode.Editable);
            groupsViewBt.Clicked += (object sender, EventArgs e) => SetGroupsViewRequest();
            settingsViewBt.Clicked += (object sender, EventArgs e) => SetSettingsViewRequest();
        }

        public void SetTheme(Theme theme)
        {
            Debug.WriteLine("[m] [MenuView] SetTheme running");

            (this as View).SetTheme(theme);
        }

        public void SetButtonsEnable(bool value)
        {
            Debug.WriteLine("[m] [MenuView] SetButtonsVisibility running with value = {0}", value);

            userViewBt.IsEnabled = value;
            messagesViewBt.IsEnabled = value;
            friendsViewBt.IsEnabled = value;
            groupsViewBt.IsEnabled = value;
            settingsViewBt.IsEnabled = value;
        }
    }
}