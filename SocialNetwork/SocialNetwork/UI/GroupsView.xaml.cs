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
    public partial class GroupsView : ContentView
    {
        public List<Group> Groups;
        public List<string> GroupTitles;

        public GroupsView(User user)
        {
            InitializeComponent();

            if (user.Groups.Count == 0)
            {
                Label label = new Label
                {
                    Text = "No Groups"
                };
                Content = label;
            }
            else
            {
                Groups = user.Groups;
                GroupTitles = Groups.Select(x => x.Title).ToList();
                listView.ItemsSource = GroupTitles;

                BindingContext = this;
            }
        }
    }
}