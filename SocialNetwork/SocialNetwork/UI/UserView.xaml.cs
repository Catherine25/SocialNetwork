using SocialNetwork.Data;
using SocialNetwork.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

//TODO: Add more fields to the view
//TODO: Add images

namespace SocialNetwork.UI
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserView : ContentView, IColorable
    {
        User CurrentUser;
        User Visitor;

        public UserView(User user, User visitor)
        {
            InitializeComponent();

            CurrentUser = user;
            Visitor = visitor;

            SetTheme(user.Theme);

            if (visitor == user)
                removeBt.IsVisible = false;
            else
            {
                removeBt.Clicked += RemoveBt_Clicked;
                removeBt.Text = visitor.Friends.Contains(CurrentUser) ? "Remove from friends list" : "Add to friends list";
            }

            if (user.AvatarLink != "")
                image.Source = ImageSource.FromUri(new Uri(user.AvatarLink));
            else
                image.Source = "https://www.indiannaturaloils.com/categories-images/no-photo.jpg";

            name.Text = user.Name;
            bio.Text = user.Bio;

            image.Clicked += Image_Clicked;
        }

        private async void Image_Clicked(object sender, EventArgs e)
        {
            string uriString = await Clipboard.GetTextAsync();

            try
            {
                image.Source = ImageSource.FromUri(new Uri(uriString));
            }
            catch
            {
                image.Source = "https://www.indiannaturaloils.com/categories-images/no-photo.jpg";
            }
        }

        public void SetTheme(Theme theme) => (this as View).SetTheme(theme);

        private void RemoveBt_Clicked(object sender, EventArgs e)
        {
            Button button = sender as Button;

            if(Visitor.Friends.Contains(CurrentUser))
            {
                button.Text = "Add to friends list";
                Visitor.Friends.Remove(CurrentUser);
            }
            else
            {
                Visitor.Friends.Add(CurrentUser);
                button.Text = "Remove from friends list";
            }
        }
    }
}