using SocialNetwork.Data;
using SocialNetwork.Services;
using SocialNetwork.UI;
using System;
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
            Conversations.AddConversations(Test.GenerateConversations(user));
            user.Friends = Test.GenerateUsers();
            user.Groups = Test.GenerateGroups();

            menu.ButtonClicked += OpenViewRequest;
        }

        private void OpenViewRequest(MenuView.ButtonName bn)
        {
            switch (bn)
            {
                case MenuView.ButtonName.userView:
                    mainPageGrid.SetSingleChild(new UserView(user, user));
                    break;
                case MenuView.ButtonName.messagesView:
                    mainPageGrid.SetSingleChild(new MessagesView(user));
                    break;
                case MenuView.ButtonName.friendsView:
                {
                    FriendsView view = new FriendsView(user);
                    mainPageGrid.SetSingleChild(view);
                    view.OpenUserViewRequest += OpenUserViewRequest;
                }
                    break;
                case MenuView.ButtonName.groupsView:
                    mainPageGrid.SetSingleChild(new GroupsView(user));
                    break;
                case MenuView.ButtonName.settingsView:
                    mainPageGrid.SetSingleChild(new SettingsView(user));
                    break;
                default:
                    throw new Exception();
            }
        }

        private void OpenUserViewRequest(User obj) =>
            mainPageGrid.SetSingleChild(new UserView(obj, user));
    }
}
