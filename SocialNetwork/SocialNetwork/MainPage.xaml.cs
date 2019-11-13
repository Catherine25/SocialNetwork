using SocialNetwork.Data;
using SocialNetwork.Services;
using SocialNetwork.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using Xamarin.Forms;

namespace SocialNetwork
{
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        private User _user;
        private Themes _themes;
        public List<Theme> themes = new List<Theme>();
        private Data.Database.LocalData _localData;

        public MainPage()
        {
            Debug.WriteLine("MainPage running");

            InitializeComponent();
        }

        public MainPage(User newUser, Themes themes, Data.Database.LocalData localData) {

            Debug.WriteLine("MainPage running");

            InitializeComponent();

            _user = newUser;
            _themes = themes;
            _localData = localData;
            _themes.ThemeLoaded += _themes_ThemeLoaded;

            menu.ButtonClicked += OpenViewRequest;
            menu.SetTheme(themes.CurrentTheme);
            OpenViewRequest(MenuView.ButtonName.userView);

            Debug.WriteLine("MainPage end");
        }

        private void _themes_ThemeLoaded(Theme theme) => themes.Add(theme);

        private void OpenViewRequest(MenuView.ButtonName bn)
        {
            switch (bn)
            {
                case MenuView.ButtonName.userView:
                {
                    UserView view = new UserView(_user, _user);
                    mainPageGrid.SetSingleChild(view);
                    view.SetTheme(_themes.CurrentTheme);
                }
                break;
                case MenuView.ButtonName.messagesView:
                {
                    MessagesView view = new MessagesView(_user, _localData);
                    mainPageGrid.SetSingleChild(view);
                    view.SetTheme(_themes.CurrentTheme);
                    view.OpenDialodRequest += OpenDialodRequest;
                    view.OpenFriendsViewRequest += View_OpenFriendsViewRequest;
                }
                break;
                case MenuView.ButtonName.friendsView:
                {
                    FriendsView view = new FriendsView(_user, FriendsView.Mode.Default);
                    mainPageGrid.SetSingleChild(view);
                    view.SetTheme(_themes.CurrentTheme);
                    view.OpenUserViewRequest += OpenUserViewRequest;
						view.ShowDialogRequest += View_ShowDialogRequest;
                }
                break;
                case MenuView.ButtonName.groupsView:
                {
                    GroupsView view = new GroupsView(_user);
                    mainPageGrid.SetSingleChild(view);
                    view.SetTheme(_themes.CurrentTheme);
                    view.OpenGroupViewRequest += View_OpenGroupViewRequest;
                }
                break;
                case MenuView.ButtonName.settingsView:
                {
                    SettingsView view = new SettingsView(_user, themes);
                    mainPageGrid.SetSingleChild(view);
                    view.SetTheme(_themes.CurrentTheme);
					view.ChangeThemeRequest += View_ChangeThemeRequest;
                }
                break;
                default: throw new Exception();
            }
        }

		private void View_ShowDialogRequest()
		{
			RequestDialog dialog = new RequestDialog();
			mainPageGrid.SetSingleChild(dialog);
		}

		private void View_ChangeThemeRequest(Theme theme)
        {
            _themes.CurrentTheme = theme;
            menu.SetTheme(theme);
        }

        private void View_OpenGroupViewRequest(User user, Group group) =>
            mainPageGrid.SetSingleChild(new GroupView(user, group));

        private void OpenDialodRequest(User user, Conversation conversation) =>
            mainPageGrid.SetSingleChild(new Dialog(conversation, user, _themes.CurrentTheme));

        private void OpenUserViewRequest(User obj) =>
            mainPageGrid.SetSingleChild(new UserView(obj, _user));

        private void View_OpenFriendsViewRequest()
        {
            FriendsView view = new FriendsView(_user, FriendsView.Mode.ChooseNew);
            view.CreateNewConversationRequest += View_CreateNewConversationRequest;
            mainPageGrid.SetSingleChild(view);
        }

        private void View_CreateNewConversationRequest(User user)
        {
            _localData.AddEmptyConversation(_user, user);
            OpenDialodRequest(user, _localData.Conversations.Find(c => c.member1 == _user && c.member2 == user));
        }
    }
}
