using SocialNetwork.Data;
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
            Debug.WriteLine("[m] [GroupsView] Constructor running");

            InitializeComponent();

            NewGroupBt.Clicked += NewGroupBt_Clicked;

            listView.ItemSelected += ItemSelected;
            
            Update(user);
        }

        public void Update(User user)
        {
            Debug.WriteLine("[m] [GroupsView] Update running");

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

        private void NewGroupBt_Clicked(object sender, EventArgs e)
        {
            Debug.WriteLine("[m] [GroupsView] NewGroupBt_Clicked running");

            ShowDialogRequest(GroupRequestDialog.RequestPurpose.newGroupName);
        }

        public void SetTheme(Theme theme)
        {
            Debug.WriteLine("[m] [GroupsView] SetTheme running");

            (this as View).SetTheme(theme);
        }

        private void ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if ((sender as ListView).SelectedItem == null)
                return;

            Debug.WriteLine("[m] [GroupsView] ItemSelected running");

            string groupName = e.SelectedItem as string;
            Group group = Groups.Find(X=>X.Title == groupName);
            OpenGroupViewRequest(User, group);
            (sender as ListView).SelectedItem = null;
        }
    }
}