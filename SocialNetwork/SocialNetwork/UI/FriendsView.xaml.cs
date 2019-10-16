using SocialNetwork.Data;
using SocialNetwork.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SocialNetwork.UI
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FriendsView : ContentView, IColorable
    {
        public List<User> Friends;
        public List<string> FriendNames;

        public event Action<User> OpenUserViewRequest;

        public FriendsView(User user)
        {
            InitializeComponent();
            listView.ItemSelected += ItemSelected;

            if (user.Friends.Count == 0)
            {
                Label label = new Label
                {
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
            SetTheme(user.Theme);
        }

        private void ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            string friend = e.SelectedItem as string;
            User user = Friends.Find(X=>X .Name == friend);
            OpenUserViewRequest(user);
        }

        public void SetTheme(Theme theme) => (this as View).SetTheme(theme);
    }
}