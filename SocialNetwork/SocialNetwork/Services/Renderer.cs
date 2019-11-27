using SocialNetwork.Data;
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
        private GroupRequestDialog groupRequestDialog;
        private UserRequestDialog userRequestDialog;
        private GroupEditor groupEditor;
        private UserEditor userEditor;

        private DialogView dialogView;
        private FriendsView friendsView;
        private GroupsView groupsView;
        private GroupView groupView;
        private MenuView menuView;
        private MessagesView messagesView;
        private SettingsView settingsView;
        private UserView userView;

        public ref GroupRequestDialog GetGroupRequestDialog(GroupRequestDialog.RequestPurpose purpose, List<Group> groups)
        {
            if (groupRequestDialog != null)
            {
                groupRequestDialog.Update(purpose, groups);
                return ref groupRequestDialog;
            }
            else
            {
                groupRequestDialog = new GroupRequestDialog(purpose, groups);
                return ref groupRequestDialog;
            }
        }

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

        public ref GroupEditor GetGroupEditor(GroupEditor.EditPurpose purpose, LocalData localData)
        {
            if (groupEditor != null)
            {
                groupEditor.Update(purpose, localData);
                return ref groupEditor;
            }
            else
            {
                groupEditor = new GroupEditor(purpose, localData);
                return ref groupEditor;
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

        public ref DialogView GetDialogView(Conversation conversaton, User user, Theme newTheme, LocalData localData)
        {
            if (dialogView != null)
            {
                dialogView.Update(conversaton, user, newTheme, localData);
                return ref dialogView;
            }
            else
            {
                dialogView = new DialogView(conversaton, user, newTheme, localData);
                return ref dialogView;
            }
        }

        public ref FriendsView GetFriendsView(User user, FriendsView.Mode mode, LocalData localData)
        {
            if (friendsView != null)
            {
                friendsView.Update(user, mode, localData);
                return ref friendsView;
            }
            else
            {
                friendsView = new FriendsView(user, mode, localData);
                return ref friendsView;
            }
        }

        public ref GroupsView GetGroupsView(User user)
        {
            if (groupsView != null)
            {
                groupsView.Update(user);
                return ref groupsView;
            }
            else
            {
                groupsView = new GroupsView(user);
                return ref groupsView;
            }
        }

        public ref GroupView GetGroupView(User user, Group group, LocalData localData)
        {
            if (groupView != null)
            {
                groupView.Update(user, group, localData);
                return ref groupView;
            }
            else
            {
                groupView = new GroupView(user, group, localData);
                return ref groupView;
            }
        }

        public ref MenuView GetMenuView()
        {
            if (menuView != null)
                return ref menuView;
            else
            {
                menuView = new MenuView();
                return ref menuView;
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

        public ref SettingsView GetSettingsView(User user, List<Theme> newThemes) 
        {
            if (settingsView != null)
            {
                settingsView.Update(user, newThemes);
                return ref settingsView;
            }
            else
            {
                settingsView = new SettingsView(user, newThemes);
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
