using SocialNetwork.Data;
using SocialNetwork.Data.Database;
using SocialNetwork.Services;
using SocialNetwork.UI.DataRequests;
using System;
using System.Collections.Generic;
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
        public enum Mode { Default, ChooseNew }
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
            InitializeComponent();

            Update(user, mode, localData);

			NewFriendBt.Clicked += NewFriendBt_Clicked;
            listView.ItemSelected += ItemSelected;
        }

        public void Update(User user, Mode mode, LocalData localData)
        {
            _mode = mode;
            _localData = localData;
            _user = user;

            if (_user.Friends.Count == 0)
            {
                NoFriendsLabel.IsVisible = true;
                listView.IsVisible = false;
            }
            else
            {
                NoFriendsLabel.IsVisible = false;
                listView.IsVisible = true;

                Friends = user.Friends;
                FriendNames = Friends.Select(x => x.Name).ToList();
                listView.ItemsSource = FriendNames;

                BindingContext = this;
            }
        }


        private void NewFriendBt_Clicked(object sender, EventArgs e) =>
            ShowDialogRequest(UserRequestDialog.RequestPurpose.newFriendName);

		private void ItemSelected(object sender, SelectedItemChangedEventArgs e) 
        {
            string friendName = e.SelectedItem as string;
            User friend = Friends.Find(X=>X .Name == friendName);

            if (_mode == Mode.ChooseNew)
            {
                Conversation c = new Conversation(0, _user, friend);
                _localData.AddEmptyConversation(c);
                SetNewConversationRequest(friend, c);
            }
            else
                OpenUserViewRequest(friend);
        }

        public void SetTheme(Theme theme) => (this as View).SetTheme(theme);
    }
}