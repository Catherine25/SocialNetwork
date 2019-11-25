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

        public GroupRequestDialog GetGroupRequestDialog(GroupRequestDialog.RequestPurpose purpose, List<Group> groups)
        {
            if (groupRequestDialog != null)
            {
                groupRequestDialog.Update(purpose, groups);
                return groupRequestDialog;
            }
            else
            {
                groupRequestDialog = new GroupRequestDialog(purpose, groups);
                return groupRequestDialog;
            }
        }

        public UserRequestDialog GetUserRequestDialog(UserRequestDialog.RequestPurpose purpose, List<User> users)
        {
            if (userRequestDialog != null)
            {
                userRequestDialog.Update(purpose, users);
                return userRequestDialog;
            }
            else
            {
                userRequestDialog = new UserRequestDialog(purpose, users);
                return userRequestDialog;
            }
        }

        public GroupEditor GetGroupEditor(GroupEditor.EditPurpose purpose, LocalData localData)
        {
            if (groupEditor != null)
            {
                groupEditor.Update(purpose, localData);
                return groupEditor;
            }
            else
            {
                groupEditor = new GroupEditor(purpose, localData);
                return groupEditor;
            }
        }
        
        public UserEditor GetUserEditor(UserEditor.EditPurpose purpose, LocalData localData, User user = null)
        {
            if (userEditor != null)
            {
                userEditor.Update(purpose, localData, user);
                return userEditor;
            }
            else
            {
                userEditor = new UserEditor(purpose, localData, user);
                return userEditor;
            }
        }

        public DialogView GetDialogView(Conversation conversaton, User user, Theme newTheme, LocalData localData)
        {
            if (dialogView != null)
            {
                dialogView.Update(conversaton, user, newTheme, localData);
                return dialogView;
            }
            else
            {
                dialogView = new DialogView(conversaton, user, newTheme, localData);
                return dialogView;
            }
        }
        
        public FriendsView GetFriendsView(User user, FriendsView.Mode mode, LocalData localData)
        {
            if (friendsView != null)
            {
                friendsView.Update(user, mode, localData);
                return friendsView;
            }
            else
            {
                friendsView = new FriendsView(user, mode, localData);
                return friendsView;
            }
        }

        public GroupsView GetGroupsView(User user)
        {
            if (groupsView != null)
            {
                groupsView.Update(user);
                return groupsView;
            }
            else
            {
                groupsView = new GroupsView(user);
                return groupsView;
            }
        }

        public GroupView GetGroupView(User user, Group group, LocalData localData)
        {
            if (groupView != null)
            {
                groupView.Update(user, group, localData);
                return groupView;
            }
            else
            {
                groupView = new GroupView(user, group, localData);
                return groupView;
            }
        }
        
        public MenuView GetMenuView()
        {
            if (menuView != null)
                return menuView;
            else
            {
                menuView = new MenuView();
                return menuView;
            }
        }
        
        public MessagesView GetMessagesView(User user, LocalData localData)
        {
            if (messagesView != null)
            {
                messagesView.Update(user, localData);
                return messagesView;
            }
            else
            {
                messagesView = new MessagesView(user, localData);
                return messagesView;
            }
        }
        
        public SettingsView GetSettingsView(User user, List<Theme> newThemes) 
        {
            if (settingsView != null)
            {
                settingsView.Update(user, newThemes);
                return settingsView;
            }
            else
            {
                settingsView = new SettingsView(user, newThemes);
                return settingsView;
            }
        }

        public UserView GetUserView(User user, User visitor, LocalData localData)
        {
            if (userView != null)
            {
                userView.Update(user, visitor, localData);
                return userView;
            }
            else
            {
                userView = new UserView(user, visitor, localData);
                return userView;
            }
        }
    }
}
