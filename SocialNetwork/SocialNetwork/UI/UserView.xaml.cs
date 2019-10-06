using SocialNetwork.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

//TODO: Add more fields to the view
//TODO: Add images

namespace SocialNetwork.UI
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserView : ContentView
    {
        User CurrentUser;
        User Visitor;

        public UserView(User user, User visitor)
        {
            InitializeComponent();

            CurrentUser = user;
            Visitor = visitor;

            if (visitor == user)
                removeBt.IsVisible = false;
            else
            {
                removeBt.Clicked += RemoveBt_Clicked;
                removeBt.Text = visitor.Friends.Contains(CurrentUser) ? "Remove from friends" : "Add as friend";
            }

            image = user.Avatar;
            name.Text = user.Name;
            bio.Text = user.Bio;

            image.BackgroundColor = user.Theme.colors[0];
            name.TextColor = user.Theme.colors[0];
            bio.TextColor = user.Theme.colors[0];
        }

        private void RemoveBt_Clicked(object sender, EventArgs e)
        {
            Button button = sender as Button;

            if(Visitor.Friends.Contains(CurrentUser))
            {
                button.Text = "Add as friend";
                Visitor.Friends.Remove(CurrentUser);
            }
            else
            {
                Visitor.Friends.Add(CurrentUser);
                button.Text = "Remove from friends";
            }
        }
    }
}