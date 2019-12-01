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

            _editBt.Clicked += EditBt_Clicked;
            _removeBt.Clicked += RemoveBt_Clicked;

            Update(user, visitor, localData);
        }

        public void Update(User user, User visitor, LocalData localData)
        {
            Debug.WriteLine("[m] [UserView] Update running");

            Visitee = user;
            Visitor = visitor;
            _localData = localData;

            Visitee = _localData.Update(Visitee);

            _removeBt.Text = Visitor == Visitee ? EditString : visitor.Friends.Contains(Visitee) ? RemoveString : AddString;
            _removeBt.IsVisible = Visitor != Visitee;
            _editBt.IsVisible = Visitor.Id == Visitee.Id;

            try
            {
                image.Source = ImageSource.FromUri(new Uri(Visitee.AvatarLink));
            }
            catch
            {
                image.Source = NoImageLink;
            }

            name.Text = Visitee.Name;
            bio.Text = Visitee.Bio;
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

            EditUserRequest(UserEditor.EditPurpose.update, Visitor);
        }
    }
}