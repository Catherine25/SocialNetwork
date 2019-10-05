using SocialNetwork.Data;
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
    public partial class FriendsView : ContentView
    {
        public List<User> Friends;
        public List<string> FriendNames;

        public FriendsView(User user)
        {
            InitializeComponent();

            if (user.Friends.Count == 0)
            {
                Label label = new Label
                {
                    Text = "No Friends"
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
    }
}