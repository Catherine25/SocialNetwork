using SocialNetwork.Data;
using SocialNetwork.Services;
using SocialNetwork.UI;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
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
            Debug.WriteLine("MainPage running");

            InitializeComponent();

            user = Test.GenerateUser();
            Conversations.AddConversations(Test.GenerateConversations(user));
            user.Friends = Test.GenerateUsers();
            user.Groups = Test.GenerateGroups();

            menu.ButtonClicked += OpenViewRequest;
            menu.SubscribeForTheme(user);
            (menu as View).SetTheme(user.Theme);

            OpenViewRequest(MenuView.ButtonName.userView);

            //SetupActivityImitators();
        }

        private void OpenViewRequest(MenuView.ButtonName bn)
        {
            switch (bn)
            {
                case MenuView.ButtonName.userView:
                    mainPageGrid.SetSingleChild(new UserView(user, user));
                    break;
                case MenuView.ButtonName.messagesView:
                {
                    MessagesView view = new MessagesView(user);
                    mainPageGrid.SetSingleChild(view);
                    view.OpenDialodRequest += OpenDialodRequest;
                    break;
                }
                case MenuView.ButtonName.friendsView:
                {
                    FriendsView view = new FriendsView(user);
                    mainPageGrid.SetSingleChild(view);
                    view.OpenUserViewRequest += OpenUserViewRequest;
                    break;
                }
                case MenuView.ButtonName.groupsView:
                {
                    GroupsView view = new GroupsView(user);
                    mainPageGrid.SetSingleChild(view);
                    view.OpenGroupViewRequest += View_OpenGroupViewRequest;
                    break;
                }
                case MenuView.ButtonName.settingsView:
                    mainPageGrid.SetSingleChild(new SettingsView(user));
                    break;
                default:
                    throw new Exception();
            }
        }

        private void View_OpenGroupViewRequest(User user, Group group) =>
            mainPageGrid.SetSingleChild(new GroupView(user, group));

        private void OpenDialodRequest(User user, Conversation conversation) =>
            mainPageGrid.SetSingleChild(new Dialog(conversation, user));

        private void OpenUserViewRequest(User obj) =>
            mainPageGrid.SetSingleChild(new UserView(obj, user));

        //private void SetupActivityImitators() {
        //    IActivityImitator activityImitator = new NewMessagesImitator(user)
        //    {
        //        StartTime = DateTime.Now,
        //        Frequency = TimeSpan.FromSeconds(1)
        //    };
        //    ThreadPool.QueueUserWorkItem(new WaitCallback(activityImitator.TryWork));
        //}
    }
}
