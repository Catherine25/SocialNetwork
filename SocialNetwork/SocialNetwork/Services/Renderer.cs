﻿using SocialNetwork.Data;
using SocialNetwork.Data.Database;
using SocialNetwork.UI.DataRequests;
using SocialNetwork.UI.Editors;
using SocialNetwork.UI.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialNetwork.Services
{
    class Renderer
    {
        private UserRequestDialog userRequestDialog;
        private UserEditor userEditor;

        private DialogView dialogView;
        private FriendsView friendsView;
        //private MenuView menuView;
        private MessagesView messagesView;
        private SettingsView settingsView;
        private UserView userView;

        public ref UserRequestDialog GetUserRequestDialog(UserRequestDialog.RequestPurpose purpose, List<User> users)
        {
            if (userRequestDialog != null)
            {
                userRequestDialog.Update(purpose, users);
                return ref userRequestDialog;
            }
            else
            {
                userRequestDialog = new UserRequestDialog(purpose, users);
                return ref userRequestDialog;
            }
        }
        
        public ref UserEditor GetUserEditor(UserEditor.EditPurpose purpose, LocalData localData, User user = null)
        {
            if (userEditor != null)
            {
                userEditor.Update(purpose, localData, user);
                return ref userEditor;
            }
            else
            {
                userEditor = new UserEditor(purpose, localData, user);
                return ref userEditor;
            }
        }

        public ref DialogView GetDialogView(Conversation conversaton, User user, LocalData localData)
        {
            if (dialogView != null)
            {
                dialogView.Update(conversaton, user, localData);
                return ref dialogView;
            }
            else
            {
                dialogView = new DialogView(conversaton, user, localData);
                return ref dialogView;
            }
        }

        public ref FriendsView GetFriendsView(User user, LocalData localData)
        {
            if (friendsView != null)
            {
                friendsView.Update(user, localData);
                return ref friendsView;
            }
            else
            {
                friendsView = new FriendsView(user, localData);
                return ref friendsView;
            }
        }

        public ref MessagesView GetMessagesView(User user, LocalData localData)
        {
            if (messagesView != null)
            {
                messagesView.Update(user, localData);
                return ref messagesView;
            }
            else
            {
                messagesView = new MessagesView(user, localData);
                return ref messagesView;
            }
        }

        public ref SettingsView GetSettingsView(User user) 
        {
            if (settingsView != null)
            {
                settingsView.Update(user);
                return ref settingsView;
            }
            else
            {
                settingsView = new SettingsView(user);
                return ref settingsView;
            }
        }

        public ref UserView GetUserView(User user, User visitor, LocalData localData)
        {
            if (userView != null)
            {
                userView.Update(user, visitor, localData);
                return ref userView;
            }
            else
            {
                userView = new UserView(user, visitor, localData);
                return ref userView;
            }
        }
    }
}
