using SocialNetwork.Data;
using SocialNetwork.Data.Database;
using SocialNetwork.Services;
using SocialNetwork.UI.Editors;
using System;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SocialNetwork.UI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserView : ContentView, IColorable
    {
        private const string AddString = "Add to friends list";
        private const string RemoveString = "Remove from friends list";
        private const string EditString = "Edit profile";
        private const string NoImageLink = "https://www.indiannaturaloils.com/categories-images/no-photo.jpg";

        private User Visitee;
        private User Visitor;
        private LocalData _localData;

        public event Action<UserEditor.EditPurpose, User> EditUserRequest;

        public UserView(User user, User visitor, LocalData localData)
        {
            Debug.WriteLine("[m] [UserView] Constructor running");

            InitializeComponent();

            bottomBt.Clicked += EditBt_Clicked;
            bottomBt.Clicked += RemoveBt_Clicked;

            Update(user, visitor, localData);
        }

        public void Update(User user, User visitor, LocalData localData)
        {
            Debug.WriteLine("[m] [UserView] Update running");

            Visitee = user;
            Visitor = visitor;
            _localData = localData;

            bottomBt.Text = visitor == user ? EditString : visitor.Friends.Contains(Visitee) ? RemoveString : AddString;

            try
            {
                image.Source = ImageSource.FromUri(new Uri(user.AvatarLink));
            }
            catch
            {
                image.Source = NoImageLink;
            }

            name.Text = user.Name;
            bio.Text = user.Bio;
        }

        public void SetTheme(Theme theme)
        {
            Debug.WriteLine("[m] [UserView] SetTheme running");

            (this as View).SetTheme(theme);
        }

        private void RemoveBt_Clicked(object sender, EventArgs e)
        {
            Debug.WriteLine("[m] [UserView] RemoveBt_Clicked running");

            if (sender is Button button)
            {
                if (Visitor.Friends.Contains(Visitee))
                {
                    Visitor.Friends.Remove(Visitee);
                    _localData.DeleteFriend(Visitor, Visitee);
                    button.Text = AddString;
                }
                else
                {
                    Visitor.Friends.Add(Visitee);
                    _localData.AddNewFriend(Visitor, Visitee);
                    button.Text = RemoveString;
                }
            }
        }

        private void EditBt_Clicked(object sender, EventArgs e)
        {
            Debug.WriteLine("[m] [UserView] EditBt_Clicked running");

            if(Visitee == null)
                EditUserRequest(UserEditor.EditPurpose.update, Visitor);
        }
    }
}