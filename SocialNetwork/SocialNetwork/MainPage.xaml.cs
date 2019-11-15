using SocialNetwork.Data;
using SocialNetwork.Data.Database;
using SocialNetwork.Services;
using SocialNetwork.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SocialNetwork
{
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        private User _user;
        private Themes _themes;
        private LocalData _localData;

        public List<Theme> themes = new List<Theme>();

        public event Action<User> UserChangeRequest;

        public MainPage()
        {
            Debug.WriteLine("MainPage empty constructor running");

            InitializeComponent();
        }

        public MainPage(User newUser, Themes themes, LocalData localData)
		{
            Debug.WriteLine("MainPage running");

            InitializeComponent();

            _user = newUser;
            _themes = themes;
            _localData = localData;
            _themes.ThemeLoaded += ThemeLoaded;

            menu.SetTheme(themes.CurrentTheme);
            menu.SetFriendsViewRequest += SetFriendsView;
            menu.SetGroupsViewRequest += SetGroupsView;
            menu.SetMessagesViewRequest += SetMessagesView;
            menu.SetSettingsViewRequest += SetSettingsView;
            menu.SetCurrentUserViewRequest += SetUserView;

			if (_user == null)
				RequestForText(RequestDialog.RequestPurpose.currentName);
			else
				SetUserView();

            Debug.WriteLine("MainPage end");
        }

        #region Requests handling

        #region Set view

        private void SetDialog(User user, Conversation conversation) =>
            mainPageGrid.SetSingleChild(new Dialog(conversation, user, _themes.CurrentTheme));

        private void SetGroupView(User user, Group group) =>
            mainPageGrid.SetSingleChild(new GroupView(user, group));

        private void SetGroupsView()
        {
            GroupsView view = new GroupsView(_user);
            view.SetTheme(_themes.CurrentTheme);
            view.OpenGroupViewRequest += SetGroupView;
            mainPageGrid.SetSingleChild(view);
        }

        private void SetFriendsView(FriendsView.Mode mode)
        {
            FriendsView view = new FriendsView(_user, mode);
            view.SetTheme(_themes.CurrentTheme);

            if (mode == FriendsView.Mode.ChooseNew)
                view.CreateNewConversationRequest += CreateNewConversation;
            else if (mode == FriendsView.Mode.Default)
            {
                view.OpenUserViewRequest += SetUserView;
                //view.ShowDialogRequest += RequestForText;
            }
            else
                throw new NotImplementedException();

            mainPageGrid.SetSingleChild(view);
        }

        private void SetMessagesView()
        {
            MessagesView view = new MessagesView(_user, _localData);
            mainPageGrid.SetSingleChild(view);
            view.SetTheme(_themes.CurrentTheme);
            view.OpenDialodRequest += SetDialog;
            view.OpenFriendsViewRequest += SetFriendsView;
        }

        public void RequestForText(RequestDialog.RequestPurpose purpose)
        {
            RequestDialog dialog = new RequestDialog(purpose, _localData.Users);
            dialog.SetTheme(_themes.CurrentTheme);
            dialog.RequestCompleted += Dialog_RequestCompleted;
            mainPageGrid.SetSingleChild(dialog);
        }

        private void SetSettingsView()
        {
            SettingsView view = new SettingsView(_user, themes);
            view.SetTheme(_themes.CurrentTheme);
            view.ChangeThemeRequest += ChangeTheme;
            mainPageGrid.SetSingleChild(view);
        }

        private void SetUserView()
        {
            UserView view = new UserView(_user, _user);
            view.SetTheme(_themes.CurrentTheme);
            mainPageGrid.SetSingleChild(view);
        }
        
        private void SetUserView(User user)
        {
            UserView view = new UserView(user, _user);
            view.SetTheme(_themes.CurrentTheme);
            mainPageGrid.SetSingleChild(view);
        }

        #endregion

        private void ThemeLoaded(Theme theme) => themes.Add(theme);

        private void ChangeTheme(Theme theme)
        {
            _themes.CurrentTheme = theme;
            menu.SetTheme(theme);
        }

        private void Dialog_RequestCompleted(User user, RequestDialog.RequestPurpose purpose)
        {
            if (purpose == RequestDialog.RequestPurpose.currentName)
                UserChangeRequest(user);
            else if (purpose == RequestDialog.RequestPurpose.newFriendName)
            {
                AddNewFriend();
                //TODO: _localData.SyncWithServer();
                SetFriendsView(FriendsView.Mode.Default);
            }
        }

        #region Change database

        private void CreateNewConversation(User user)
        {
            _localData.AddEmptyConversation(_user, user);
            SetDialog(user, _localData.Conversations.Find(c => c.member1 == _user && c.member2 == user));
        }

        private void AddNewFriend() => throw new NotImplementedException();

        #endregion

        #endregion
    }
}
