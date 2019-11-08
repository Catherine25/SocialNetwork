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
        private User user;
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

            user = newUser;
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
                    UserView view = new UserView(user, user);
                    mainPageGrid.SetSingleChild(view);
                    view.SetTheme(_themes.CurrentTheme);
                }
                break;
                case MenuView.ButtonName.messagesView:
                {
                    MessagesView view = new MessagesView(user, _localData);
                    mainPageGrid.SetSingleChild(view);
                    view.SetTheme(_themes.CurrentTheme);
                    view.OpenDialodRequest += OpenDialodRequest;
                }
                break;
                case MenuView.ButtonName.friendsView:
                {
                    FriendsView view = new FriendsView(user);
                    mainPageGrid.SetSingleChild(view);
                    view.SetTheme(_themes.CurrentTheme);
                    view.OpenUserViewRequest += OpenUserViewRequest;
                }
                break;
                case MenuView.ButtonName.groupsView:
                {
                    GroupsView view = new GroupsView(user);
                    mainPageGrid.SetSingleChild(view);
                    view.SetTheme(_themes.CurrentTheme);
                    view.OpenGroupViewRequest += View_OpenGroupViewRequest;
                }
                break;
                case MenuView.ButtonName.settingsView:
                {
                    SettingsView view = new SettingsView(user, themes);
                    mainPageGrid.SetSingleChild(view);
                    view.SetTheme(_themes.CurrentTheme);
					view.ChangeThemeRequest += View_ChangeThemeRequest;
                }
                break;
                default: throw new Exception();
            }
        }

		private void View_ChangeThemeRequest(Theme theme) => _themes.CurrentTheme = theme;

		private void View_OpenGroupViewRequest(User user, Group group) =>
            mainPageGrid.SetSingleChild(new GroupView(user, group));

        private void OpenDialodRequest(User user, Conversation conversation) =>
            mainPageGrid.SetSingleChild(new Dialog(conversation, user, _themes.CurrentTheme));

        private void OpenUserViewRequest(User obj) =>
            mainPageGrid.SetSingleChild(new UserView(obj, user));
    }
}
