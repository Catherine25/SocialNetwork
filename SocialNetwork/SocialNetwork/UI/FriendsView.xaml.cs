using SocialNetwork.Data;
using SocialNetwork.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SocialNetwork.UI {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FriendsView : ContentView, IColorable
    {
        public enum Mode { Default, ChooseNew }
        private Mode _mode;
        private SQLLoader _loader;
        private User _user;
        
        public List<User> Friends;
        public List<string> FriendNames;

        public event Action<User> OpenUserViewRequest;
        public event Action<User, Conversation> SetNewConversationRequest;
		public event Action<RequestDialog.RequestPurpose> ShowDialogRequest;

        public FriendsView(User user, Mode mode, SQLLoader loader)
        {
            InitializeComponent();
            _mode = mode;
            _loader = loader;
            _user = user;

            listView.ItemSelected += ItemSelected;
			NewFriendBt.Clicked += NewFriendBt_Clicked;

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
            ShowDialogRequest(RequestDialog.RequestPurpose.newFriendName);

		private void ItemSelected(object sender, SelectedItemChangedEventArgs e) 
        {
            string friendName = e.SelectedItem as string;
            User friend = Friends.Find(X=>X .Name == friendName);

            if (_mode == Mode.ChooseNew)
            {
                Conversation c = new Conversation(0, _user, friend);
                _loader.AddEmptyConversation(c);
                SetNewConversationRequest(friend, c);
            }
            else
                OpenUserViewRequest(friend);
        }

        public void SetTheme(Theme theme) => (this as View).SetTheme(theme);
    }
}