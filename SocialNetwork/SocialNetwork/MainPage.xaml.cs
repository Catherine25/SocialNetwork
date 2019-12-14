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
        private LocalData _localData;
        private Renderer _renderer;

        private HashSet<ViewSet> _definedViews = new HashSet<ViewSet>();
        private HashSet<DialogSet> _definedDialogs = new HashSet<DialogSet>();
        private HashSet<EditorSet> _definedEditors = new HashSet<EditorSet>();

        public event Action<User> UserChangeRequest;

        public MainPage()
        {
            Debug.WriteLine("MainPage empty constructor running");

            InitializeComponent();
        }

        public MainPage(LocalData localData)
		{
            Debug.WriteLine("MainPage running");

            InitializeComponent();

            _localData = localData;
            _renderer = new Renderer();

            if (_user == null)
                RequestForUser(UserRequestDialog.RequestPurpose.currentName);
            else
                SetUserView();
            
            Debug.WriteLine("MainPage end");
        }

        #region Views

        private void SetDialogView(User user, Conversation conversation)
        {
            var view = _renderer.GetDialogView(conversation, user, _localData);

            if (!_definedViews.Contains(ViewSet.DialogView))
                view.OpenMessagesViewRequest += SetMessagesView;

            mainPageGrid.SetSingleChild(view);
        }

        private void SetFriendsView()
        {
            var view = _renderer.GetFriendsView(_user, _localData);

            if (!_definedViews.Contains(ViewSet.FriendsView))
            {
                view.SetNewConversationRequest += SetDialogView;
                view.OpenUserViewRequest += SetUserView;
                view.ShowDialogRequest += RequestForUser;
                
                _definedViews.Add(ViewSet.FriendsView);
            }

            mainPageGrid.SetSingleChild(view);
        }

        private void SetFriendsView(User user)
        {
            var view = _renderer.GetFriendsView(user, _localData);

            if (!_definedViews.Contains(ViewSet.FriendsView))
            {
                view.SetNewConversationRequest += SetDialogView;
                view.OpenUserViewRequest += SetUserView;
                view.ShowDialogRequest += RequestForUser;

                _definedViews.Add(ViewSet.FriendsView);
            }

            mainPageGrid.SetSingleChild(view);
        }

        private void SetMessagesView()
        {
            var view = _renderer.GetMessagesView(_user, _localData);
            mainPageGrid.SetSingleChild(view);

            if (!_definedViews.Contains(ViewSet.MessagesView))
            {
                view.OpenDialogRequest += SetDialogView;
                //view.OpenFriendsViewRequest += SetFriendsView;
                view.OpenSettingsViewRequest += SetSettingsView;
                _definedViews.Add(ViewSet.MessagesView);
            }
        }

        private void SetSettingsView()
        {
            var view = _renderer.GetSettingsView(_user);
            mainPageGrid.SetSingleChild(view);
            
            if (!_definedViews.Contains(ViewSet.SettingsView))
            {
                view.ReloginRequest += RequestForUser;
                view.CreateDialogRequest += SetFriendsView;
                view.EditUserRequest += SetUserEditor;
                view.OpenMessagesView += SetMessagesView;
                _definedViews.Add(ViewSet.SettingsView);
            }
        }

        private void SetUserView()
        {
            var view = _renderer.GetUserView(_user, _user, _localData);
            mainPageGrid.SetSingleChild(view);

            if (!_definedViews.Contains(ViewSet.UserView))
            {
                view.EditUserRequest += SetUserEditor;
                view.ShowFriendsListRequest += SetFriendsView;
                _definedViews.Add(ViewSet.UserView);
            }
        }

        private void SetUserView(User user)
        {
            var view = _renderer.GetUserView(user, _user, _localData);
            mainPageGrid.SetSingleChild(view);
        }

        #endregion

        #region Request Dialogs

        public void RequestForUser(UserRequestDialog.RequestPurpose purpose)
        {
            UserRequestDialog dialog = _renderer.GetUserRequestDialog(purpose, _localData.GetUsers());
            mainPageGrid.SetSingleChild(dialog);

            if (!_definedDialogs.Contains(DialogSet.UserRequestDialog))
            {
                dialog.RequestCompleted += UserRequestCompleted;
                dialog.ShowUserEditorRequest += SetUserEditor;
                dialog.ShowFriendsViewRequest += SetFriendsView;

                _definedDialogs.Add(DialogSet.UserRequestDialog);
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
                SetMessagesView();
            }
            else if (purpose == UserRequestDialog.RequestPurpose.newFriendName)
            {
                _localData.AddNewFriend(_user, user);
                _user.Friends.Add(user);
                SetFriendsView();
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
                editor.EditorResult += SetMessagesView;
                _definedEditors.Add(EditorSet.UserEditor);
            }
        }

        private void SetUserEditor(UserEditor.EditPurpose purpose, User user)
        {
            var editor = _renderer.GetUserEditor(purpose, _localData, user);
            mainPageGrid.SetSingleChild(editor);

            if (!_definedEditors.Contains(EditorSet.UserEditor))
            {
                editor.EditorResult += SetMessagesView;
                _definedEditors.Add(EditorSet.UserEditor);
            }
        }

        #endregion
    }
}
