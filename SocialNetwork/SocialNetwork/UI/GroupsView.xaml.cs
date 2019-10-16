using SocialNetwork.Data;
using SocialNetwork.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

//TODO: Open selected group page

namespace SocialNetwork.UI
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GroupsView : ContentView, IColorable
    {
        private User User;
        public List<Group> Groups;
        public List<string> GroupTitles;
        public event Action<User, Group> OpenGroupViewRequest;

        public GroupsView(User user)
        {
            InitializeComponent();

            User = user;

            if (user.Groups.Count == 0)
            {
                Label label = new Label
                {
                    Text = "No Groups",
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    FontSize = 90
                };
                Content = label;
            }
            else
            {
                Groups = user.Groups;
                GroupTitles = Groups.Select(x => x.Title).ToList();
                listView.ItemsSource = GroupTitles;
                listView.ItemSelected += ItemSelected;

                BindingContext = this;
            }

            SetTheme(user.Theme);
        }

        public void SetTheme(Theme theme) => (this as View).SetTheme(theme);

        private void ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            string groupName = e.SelectedItem as string;
            Group group = Groups.Find(X=>X.Title == groupName);
            OpenGroupViewRequest(User, group);
        }
    }
}