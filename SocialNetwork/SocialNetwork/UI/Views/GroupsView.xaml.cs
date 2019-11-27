using SocialNetwork.Data;
using SocialNetwork.Services;
using SocialNetwork.UI.DataRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

//TODO: Open selected group page

namespace SocialNetwork.UI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GroupsView : ContentView, IColorable
    {
        private User User;
        public List<Group> Groups;
        public List<string> GroupTitles;
        public event Action<User, Group> OpenGroupViewRequest;
        public event Action<GroupRequestDialog.RequestPurpose> ShowDialogRequest;

        public GroupsView(User user)
        {
            InitializeComponent();

            NewGroupBt.Clicked += NewGroupBt_Clicked;

            listView.ItemSelected += ItemSelected;
            
            Update(user);
        }

        public void Update(User user)
        {
            User = user;

            if (user.Groups.Count == 0)
            {
                NoGroupsLabel.IsVisible = true;
                listView.IsVisible = false;
            }
            else
            {
                NoGroupsLabel.IsVisible = false;
                listView.IsVisible = true;

                Groups = user.Groups;
                GroupTitles = Groups.Select(x => x.Title).ToList();
                listView.ItemsSource = GroupTitles;

                BindingContext = this;
            }
        }

        private void NewGroupBt_Clicked(object sender, EventArgs e) =>
            ShowDialogRequest(GroupRequestDialog.RequestPurpose.newGroupName);

        public void SetTheme(Theme theme) => (this as View).SetTheme(theme);

        private void ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            string groupName = e.SelectedItem as string;
            Group group = Groups.Find(X=>X.Title == groupName);
            OpenGroupViewRequest(User, group);
            (sender as ListView).SelectedItem = null;
        }
    }
}