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
        
        public List<User> Friends;
        public List<string> FriendNames;

        public event Action<User> OpenUserViewRequest;
        public event Action<User> CreateNewConversationRequest;

        public FriendsView(User user, Mode mode)
        {
            InitializeComponent();
            _mode = mode;
            listView.ItemSelected += ItemSelected;

            if (user.Friends.Count == 0) {
                Label label = new Label {
                    Text = "No Friends",
                    FontSize = 90,
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center
                };
                Content = label;
            }
            else
            {
                Friends = user.Friends;
                FriendNames = Friends.Select(x => x.Name).ToList();
                listView.ItemsSource = FriendNames;

                BindingContext = this;
            }
        }

        private void ItemSelected(object sender, SelectedItemChangedEventArgs e) 
        {
            string friend = e.SelectedItem as string;
            User user = Friends.Find(X=>X .Name == friend);

            if (_mode == Mode.ChooseNew)
                CreateNewConversationRequest(user);
            else
                OpenUserViewRequest(user);
        }

        public void SetTheme(Theme theme) => (this as View).SetTheme(theme);
    }
}