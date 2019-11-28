using SocialNetwork.Data;
using SocialNetwork.Data.Database;
using SocialNetwork.Services;
using SocialNetwork.UI.DataRequests;
using SocialNetwork.UI.Editors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using Xamarin.Forms;

namespace SocialNetwork
{
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public enum ViewSet { DialogView, FriendsView, GroupsView, GroupView, MenuView, MessagesView, SettingsView, UserView  }
        public enum DialogSet { UserRequestDialog, GroupRequestDialog }
        public enum EditorSet { UserEditor, SetGroupEditor }

        private User _user;
        private Themes _themes;
        private LocalData _localData;
        private Renderer _renderer;

        public List<Theme> themes = new List<Theme>();
        public HashSet<ViewSet> _definedViews = new HashSet<ViewSet>();
        public HashSet<DialogSet> _definedDialogs = new HashSet<DialogSet>();
        public HashSet<EditorSet> _definedEditors = new HashSet<EditorSet>();

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
            _renderer = new Renderer();
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
            mainPageGrid.SetSingleChild(_renderer.GetDialogView(conversation, user, _themes.CurrentTheme, _localData));

        private void SetGroupView(User user, Group group)
        {
            var view = _renderer.GetGroupView(user, group, _localData);
            view.SetTheme(_themes.CurrentTheme);
            mainPageGrid.SetSingleChild(view);

            if (!_definedEditors.Contains(EditorSet.SetGroupEditor))
            {
                view.EditGroupRequest += SetGroupEditor;
                _definedEditors.Add(EditorSet.SetGroupEditor);
            }
        }

        private void SetGroupsView()
        {
            var view = _renderer.GetGroupsView(_user);
            view.SetTheme(_themes.CurrentTheme);
            mainPageGrid.SetSingleChild(view);

            if (!_definedViews.Contains(ViewSet.GroupsView))
            {
                view.OpenGroupViewRequest += SetGroupView;
                view.ShowDialogRequest += RequestForGroup;
                _definedViews.Add(ViewSet.GroupsView);
            }
        }        

        private void SetFriendsView(UI.Views.FriendsView.Mode mode)
        {
            var view = _renderer.GetFriendsView(_user, mode, _localData);
            view.SetTheme(_themes.CurrentTheme);

            if (!_definedViews.Contains(ViewSet.FriendsView))
            {
                if (mode == UI.Views.FriendsView.Mode.ChooseNew)
                    view.SetNewConversationRequest += SetDialogView;
                else if (mode == UI.Views.FriendsView.Mode.Default)
                {
                    view.OpenUserViewRequest += SetUserView;
                    view.ShowDialogRequest += RequestForUser;
                }
                else
                    throw new NotImplementedException();
                _definedViews.Add(ViewSet.FriendsView);
            }

            mainPageGrid.SetSingleChild(view);
        }

        private void SetMessagesView()
        {
            var view = _renderer.GetMessagesView(_user, _localData);
            mainPageGrid.SetSingleChild(view);
            view.SetTheme(_themes.CurrentTheme);

            if (!_definedViews.Contains(ViewSet.MessagesView))
            {
                view.OpenDialodRequest += SetDialogView;
                view.OpenFriendsViewRequest += SetFriendsView;
                _definedViews.Add(ViewSet.MessagesView);
            }
        }

        private void SetSettingsView()
        {
            var view = _renderer.GetSettingsView(_user, themes);
            view.SetTheme(_themes.CurrentTheme);
            mainPageGrid.SetSingleChild(view);
            
            if (!_definedViews.Contains(ViewSet.SettingsView))
            {
                view.ChangeThemeRequest += ChangeTheme;
                view.ReloginRequest += RequestForUser;
                _definedViews.Add(ViewSet.SettingsView);
            }
        }

        private void SetUserView()
        {
            var view = _renderer.GetUserView(_user, _user, _localData);
            view.SetTheme(_themes.CurrentTheme);
            mainPageGrid.SetSingleChild(view);

            if (!_definedViews.Contains(ViewSet.UserView))
            {
                view.EditUserRequest += SetUserEditor;
                _definedViews.Add(ViewSet.UserView);
            }
        }

        private void SetUserView(User user)
        {
            var view = _renderer.GetUserView(user, _user, _localData);
            view.SetTheme(_themes.CurrentTheme);
            mainPageGrid.SetSingleChild(view);
        }

        #endregion

        #region Request Dialogs

        public void RequestForUser(UserRequestDialog.RequestPurpose purpose)
        {
            UserRequestDialog dialog = _renderer.GetUserRequestDialog(purpose, _localData.GetUsers());
            dialog.SetTheme(_themes.CurrentTheme);
            mainPageGrid.SetSingleChild(dialog);

            if (!_definedDialogs.Contains(DialogSet.UserRequestDialog))
            {
                dialog.RequestCompleted += UserRequestCompleted;
                dialog.ShowUserEditorRequest += SetUserEditor;
                dialog.ShowFriendsViewRequest += SetFriendsView;

                _definedDialogs.Add(DialogSet.UserRequestDialog);
            }
        }

        private void RequestForGroup(GroupRequestDialog.RequestPurpose purpose)
        {
            var dialog = _renderer.GetGroupRequestDialog(purpose, _localData.GetGroups());
            dialog.SetTheme(_themes.CurrentTheme);
            mainPageGrid.SetSingleChild(dialog);

            if (!_definedDialogs.Contains(DialogSet.GroupRequestDialog))
            {
                dialog.RequestCompleted += GroupRequestCompleted;
                dialog.ShowGroupsViewRequest += SetGroupsView;
                dialog.ShowGroupEditorRequest += SetGroupEditor;

                _definedDialogs.Add(DialogSet.GroupRequestDialog);
            }
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
                SetFriendsView(UI.Views.FriendsView.Mode.Default);
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
            var editor = _renderer.GetUserEditor(purpose, _localData);
            mainPageGrid.SetSingleChild(editor);

            if (!_definedEditors.Contains(EditorSet.UserEditor))
            {
                editor.EditorResult += SetUserView;
                _definedEditors.Add(EditorSet.UserEditor);
            }
        }

        private void SetUserEditor(UserEditor.EditPurpose purpose, User user)
        {
            var editor = _renderer.GetUserEditor(purpose, _localData, user);
            mainPageGrid.SetSingleChild(editor);

            if (!_definedEditors.Contains(EditorSet.UserEditor))
            {
                editor.EditorResult += SetUserView;
                _definedEditors.Add(EditorSet.UserEditor);
            }
        }

        private void SetGroupEditor(GroupEditor.EditPurpose purpose)
        {
            var editor = new GroupEditor(purpose, _localData);
            mainPageGrid.SetSingleChild(editor);

            if (!_definedEditors.Contains(EditorSet.SetGroupEditor))
            {
                editor.EditorResult += SetGroupsView;
                _definedEditors.Add(EditorSet.SetGroupEditor);
            }
        }

        private void SetGroupEditor(GroupEditor.EditPurpose purpose, Group group)
        {
            var editor = new GroupEditor(purpose, _localData, group);
            mainPageGrid.SetSingleChild(editor);

            if (!_definedEditors.Contains(EditorSet.SetGroupEditor))
            {
                editor.EditorResult += SetGroupsView;
                _definedEditors.Add(EditorSet.SetGroupEditor);
            }
        }

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
