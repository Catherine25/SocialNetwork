using SocialNetwork.Data;
using SocialNetwork.Data.Database;
using SocialNetwork.UI.DataRequests;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SocialNetwork.UI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FriendsView : ContentView
    {
        private LocalData _localData;
        private User _user;
        
        public List<User> Users;
        public List<string> UserNames;

        public event Action<User> OpenUserViewRequest;
        public event Action<User, Conversation> SetNewConversationRequest;
		public event Action<UserRequestDialog.RequestPurpose> ShowDialogRequest;

        public FriendsView(User user, LocalData localData)
        {
            Debug.WriteLine("[m] [FriendsView] Constructor running");

            InitializeComponent();

            Update(user, localData);

            _listView.ItemSelected += ItemSelected;
        }

        public void Update(User user, LocalData localData)
        {
            Debug.WriteLine("[m] [FriendsView] Update running");

            _localData = localData;
            _user = user;

            Users = _localData.GetUsers();

            UserNames = Users.Select(x => x.Name).ToList();
            _listView.ItemsSource = UserNames;

            BindingContext = this;
        }

        private void ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Debug.WriteLine("[m] [FriendsView] ItemSelected running");

            if (e.SelectedItem == null)
                return;

            string friendName = e.SelectedItem as string;
            User friend = Users.Find(X => X.Name == friendName);

            Conversation c = new Conversation(0, _user, friend);
            _localData.AddEmptyConversation(c);
            SetNewConversationRequest(friend, c);

            (sender as ListView).SelectedItem = null;
        }
    }
}