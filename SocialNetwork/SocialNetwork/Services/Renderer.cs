using SocialNetwork.Data;
using SocialNetwork.Data.Database;
using SocialNetwork.UI.DataRequests;
using SocialNetwork.UI.Editors;
using SocialNetwork.UI.Views;
using System.Collections.Generic;

namespace SocialNetwork.Services
{
    class Renderer
    {
        private UserRequestDialog userRequestDialog;

        private UserEditor userEditor;

        private DialogView dialogView;
        private FriendsView friendsView;
        private MessagesView messagesView;
        private SettingsView settingsView;

        public ref UserRequestDialog GetUserRequestDialog(List<User> users)
        {
            if (userRequestDialog != null)
            {
                userRequestDialog.Update(users);
                return ref userRequestDialog;
            }
            else
            {
                userRequestDialog = new UserRequestDialog(users);
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
    }
}
