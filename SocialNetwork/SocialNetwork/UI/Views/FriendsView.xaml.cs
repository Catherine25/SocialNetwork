﻿using SocialNetwork.Data;
using SocialNetwork.Data.Database;
using SocialNetwork.Services;
using SocialNetwork.UI.DataRequests;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SocialNetwork.UI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FriendsView : ContentView, IColorable
    {
        public enum Mode { Editable, ChooseNew, ReadOnly }
        private Mode _mode;
        private LocalData _localData;
        private User _user;
        
        public List<User> Friends;
        public List<string> FriendNames;

        public event Action<User> OpenUserViewRequest;
        public event Action<User, Conversation> SetNewConversationRequest;
		public event Action<UserRequestDialog.RequestPurpose> ShowDialogRequest;

        public FriendsView(User user, Mode mode, LocalData localData)
        {
            Debug.WriteLine("[m] [FriendsView] Constructor running");

            InitializeComponent();

            Update(user, mode, localData);

			_newFriendBt.Clicked += NewFriendBt_Clicked;
            _listView.ItemSelected += ItemSelected;
        }

        public void Update(User user, Mode mode, LocalData localData)
        {
            Debug.WriteLine("[m] [FriendsView] Update running");

            _mode = mode;
            _localData = localData;
            _user = user;

            _newFriendBt.IsVisible = mode != Mode.ReadOnly;

            if (mode == Mode.ReadOnly)
                _user.Friends = _localData.FindFriendsOfUser(_user);

            if (_user.Friends.Count == 0)
            {
                _noFriendsLabel.IsVisible = true;
                _listView.IsVisible = false;
            }
            else
            {
                _noFriendsLabel.IsVisible = false;
                _listView.IsVisible = true;

                Friends = user.Friends;
                FriendNames = Friends.Select(x => x.Name).ToList();
                _listView.ItemsSource = FriendNames;

                BindingContext = this;
            }
        }


        private void NewFriendBt_Clicked(object sender, EventArgs e)
        {
            Debug.WriteLine("[m] [FriendsView] NewFriendBt_Clicked running");

            ShowDialogRequest(UserRequestDialog.RequestPurpose.newFriendName);
        }

        private void ItemSelected(object sender, SelectedItemChangedEventArgs e) 
        {
            Debug.WriteLine("[m] [FriendsView] ItemSelected running");

            if (e.SelectedItem == null)
                return;

            string friendName = e.SelectedItem as string;
            User friend = Friends.Find(X => X.Name == friendName);

            if (_mode == Mode.ChooseNew)
            {
                Conversation c = new Conversation(0, _user, friend);
                _localData.AddEmptyConversation(c);
                SetNewConversationRequest(friend, c);
            }
            else
                OpenUserViewRequest(friend);

            (sender as ListView).SelectedItem = null;
        }

        public void SetTheme(Theme theme)
        {
            Debug.WriteLine("[m] [FriendsView] SetTheme running");

            (this as View).SetTheme(theme);
        }
    }
}