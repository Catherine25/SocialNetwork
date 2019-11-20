using SocialNetwork.Data;
using SocialNetwork.Data.Database;
using SocialNetwork.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SocialNetwork.UI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserView : ContentView, IColorable
    {
        User Visitee;
        User Visitor;
        LocalData _localData;

        public UserView(User user, User visitor, LocalData localData)
        {
            InitializeComponent();

            Visitee = user;
            Visitor = visitor;
            _localData = localData;

            if (visitor == user)
                removeBt.IsVisible = false;
            else
            {
                removeBt.Clicked += RemoveBt_Clicked;
                removeBt.Text = visitor.Friends.Contains(Visitee) ? "Remove from friends list" : "Add to friends list";
            }

            try
            {
                image.Source = ImageSource.FromUri(new Uri(user.AvatarLink));
            }
            catch
            {
                image.Source = "https://www.indiannaturaloils.com/categories-images/no-photo.jpg";
            }

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

            if(Visitor.Friends.Contains(Visitee))
            {
                Visitor.Friends.Remove(Visitee);
                _localData.DeleteFriend(Visitor, Visitee);
                button.Text = "Add to friends list";
            }
            else
            {
                Visitor.Friends.Add(Visitee);
                _localData.AddNewFriend(Visitor, Visitee);
                button.Text = "Remove from friends list";
            }
        }
    }
}