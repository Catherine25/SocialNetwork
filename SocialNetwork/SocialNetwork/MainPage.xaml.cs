using SocialNetwork.Data;
using SocialNetwork.Services;
using SocialNetwork.UI;
using System.ComponentModel;
using Xamarin.Forms;

namespace SocialNetwork
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        User user;

        public MainPage()
        {
            InitializeComponent();

            user = Test.GenerateUser();
            Messages.AddMessages(Test.GenerateMessages(user));
            user.Friends = Test.GenerateUsers();
            user.Groups = Test.GenerateGroups();

            menu.ButtonClicked += Menu_ButtonClicked;
        }

        private void Menu_ButtonClicked(MenuView.ButtonName bn)
        {
            switch (bn)
            {
                case MenuView.ButtonName.userView:
                    mainPageGrid.SetSingleChild(new UserView(user));
                    break;
                case MenuView.ButtonName.messagesView:
                    mainPageGrid.SetSingleChild(new MessagesView(user));
                    break;
                case MenuView.ButtonName.friendsView:
                    mainPageGrid.SetSingleChild(new FriendsView(user));
                    break;
                case MenuView.ButtonName.groupsView:
                    mainPageGrid.SetSingleChild(new GroupsView(user));
                    break;
                case MenuView.ButtonName.settingsView:
                    mainPageGrid.SetSingleChild(new SettingsView(user));
                    break;
                default:
                    throw new System.Exception();
            }
            //throw new System.NotImplementedException();
        }
    }
}
