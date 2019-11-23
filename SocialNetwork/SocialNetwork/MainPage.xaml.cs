using SocialNetwork.Data;
using SocialNetwork.Data.Database;
using SocialNetwork.Services;
using SocialNetwork.UI;
using SocialNetwork.UI.DataRequests;
using SocialNetwork.UI.Editors;
using SocialNetwork.UI.Views;
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

        public MainPage(Themes themes, LocalData localData)
		{
            Debug.WriteLine("MainPage running");

            InitializeComponent();

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
				RequestForUser(UserRequestDialog.RequestPurpose.currentName);
			else
				SetUserView();

            Debug.WriteLine("MainPage end");
        }

        #region Views

        private void SetDialogView(User user, Conversation conversation) =>
            mainPageGrid.SetSingleChild(new DialogView(conversation, user, _themes.CurrentTheme, _localData));

        private void SetGroupView(User user, Group group) =>
            mainPageGrid.SetSingleChild(new GroupView(user, group, _localData));

        private void SetGroupsView()
        {
            GroupsView view = new GroupsView(_user);
            view.SetTheme(_themes.CurrentTheme);
            view.OpenGroupViewRequest += SetGroupView;
            view.ShowDialogRequest += RequestForGroup;
            mainPageGrid.SetSingleChild(view);
        }        

        private void SetFriendsView(FriendsView.Mode mode)
        {
            FriendsView view = new FriendsView(_user, mode, _localData);
            view.SetTheme(_themes.CurrentTheme);

            if (mode == FriendsView.Mode.ChooseNew)
                view.SetNewConversationRequest += SetDialogView;
            else if (mode == FriendsView.Mode.Default)
            {
                view.OpenUserViewRequest += SetUserView;
                view.ShowDialogRequest += RequestForUser;
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
            view.OpenDialodRequest += SetDialogView;
            view.OpenFriendsViewRequest += SetFriendsView;
        }

        private void SetSettingsView()
        {
            SettingsView view = new SettingsView(_user, themes);
            view.SetTheme(_themes.CurrentTheme);
            view.ChangeThemeRequest += ChangeTheme;
            view.ReloginRequest += RequestForUser;
            mainPageGrid.SetSingleChild(view);
        }

        private void SetUserView()
        {
            UserView view = new UserView(_user, _user, _localData);
            view.SetTheme(_themes.CurrentTheme);
            view.EditUserRequest += SetUserEditor;
            mainPageGrid.SetSingleChild(view);
        }

        private void SetUserView(User user)
        {
            UserView view = new UserView(user, _user, _localData);
            view.SetTheme(_themes.CurrentTheme);
            mainPageGrid.SetSingleChild(view);
        }

        #endregion

        #region Request Dialogs

        public void RequestForUser(UserRequestDialog.RequestPurpose purpose)
        {
            UserRequestDialog dialog = new UserRequestDialog(purpose, _localData.GetUsers());
            dialog.SetTheme(_themes.CurrentTheme);
            dialog.RequestCompleted += UserRequestCompleted;
            dialog.ShowUserEditorRequest += SetUserEditor;
            mainPageGrid.SetSingleChild(dialog);
        }

        private void RequestForGroup(GroupRequestDialog.RequestPurpose obj)
        {
            GroupRequestDialog dialog = new GroupRequestDialog(GroupRequestDialog.RequestPurpose.newGroupName, _localData.GetGroups());
            dialog.SetTheme(_themes.CurrentTheme);
            dialog.RequestCompleted += GroupRequestCompleted;
            mainPageGrid.SetSingleChild(dialog);
        }

        private void UserRequestCompleted(User user, UserRequestDialog.RequestPurpose purpose)
        {
            if (purpose == UserRequestDialog.RequestPurpose.currentName)
            {
                _user = user;
                _localData.ChangeUser(user);
                user.Friends = _localData.FindFriendsOfUser(user);
                user.Groups = _localData.FindGroupsOfUser(user);
                SetUserView();
            }
            else if (purpose == UserRequestDialog.RequestPurpose.newFriendName)
            {
                _localData.AddNewFriend(_user, user);
                _user.Friends.Add(user);
                SetFriendsView(FriendsView.Mode.Default);
            }
        }

        private void GroupRequestCompleted(Group group, GroupRequestDialog.RequestPurpose purpose)
        {
            if (purpose == GroupRequestDialog.RequestPurpose.newGroupName)
            {
                _localData.AddUserToGroup(_user, group);
                _user.Groups.Add(group);
                SetGroupsView();
            }
        }

        #endregion

        #region Editors

        private void SetUserEditor(UserEditor.EditPurpose purpose)
        {
            var editor = new UserEditor(purpose, _localData);
            editor.EditorResult += SetUserView;
            mainPageGrid.SetSingleChild(editor);
        }

        private void SetUserEditor(UserEditor.EditPurpose purpose, User user)
        {
            var editor = new UserEditor(purpose, _localData, user);
            editor.EditorResult += SetUserView;
            mainPageGrid.SetSingleChild(editor);
        }

        private void SetGroupEditor() => throw new NotImplementedException();

        #endregion

        #region Themes

        private void ThemeLoaded(Theme theme) => themes.Add(theme);

        private void ChangeTheme(Theme theme)
        {
            _themes.CurrentTheme = theme;
            menu.SetTheme(theme);
        }

        #endregion
    }
}
