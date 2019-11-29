using SocialNetwork.Data;
using SocialNetwork.Data.Database;
using SocialNetwork.Services;
using SocialNetwork.UI.Editors;
using System;
using System.Diagnostics;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SocialNetwork.UI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GroupView : ContentView, IColorable
    {
        User CurrentUser;
        Group Group;
        LocalData _localData;

        public event Action<GroupEditor.EditPurpose, Group> EditGroupRequest;

        public GroupView(User user, Group group, LocalData localData)
        {
            Debug.WriteLine("[m] [GroupView] Constructor running");

            InitializeComponent();

            removeBt.Clicked += RemoveBt_Clicked;
            image.Clicked += Image_Clicked;
            editBt.Clicked += EditBt_Clicked;

            Update(user, group, localData);
        }

        private void EditBt_Clicked(object sender, EventArgs e)
        {
            Debug.WriteLine("[m] [GroupView] EditBt_Clicked running");

            EditGroupRequest(GroupEditor.EditPurpose.edit, Group);
        }

        public void Update(User user, Group group, LocalData localData)
        {
            Debug.WriteLine("[m] [GroupView] Update running");

            CurrentUser = user;
            Group = group;
            _localData = localData;

            editBt.IsEnabled = CurrentUser.Id == _localData.FindUserByName("Admin").Id;
            removeBt.Text = CurrentUser.Groups.Contains(Group) ? "Remove from groups list" : "Add to groups list";

            try
            {
                image.Source = ImageSource.FromUri(new Uri(group.AvatarLink));
            }
            catch
            {
                image.Source = "https://www.indiannaturaloils.com/categories-images/no-photo.jpg";
            }

            title.Text = group.Title;
            description.Text = group.Description;
        }

        public void SetTheme(Theme theme)
        {
            Debug.WriteLine("[m] [GroupView] SetTheme running");

            (this as View).SetTheme(theme);
        }

        private async void Image_Clicked(object sender, EventArgs e)
        {
            Debug.WriteLine("[m] [GroupView] Image_Clicked running");

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

        private void RemoveBt_Clicked(object sender, EventArgs e)
        {
            Debug.WriteLine("[m] [GroupView] RemoveBt_Clicked running");

            Button button = sender as Button;

            if (CurrentUser.Groups.Contains(Group))
            {
                button.Text = "Add to groups list";
                CurrentUser.Groups.Remove(Group);
                _localData.DeleteUserFromGroup(CurrentUser, Group);
            }
            else
            {
                button.Text = "Remove from groups list";
                CurrentUser.Groups.Add(Group);
                _localData.AddUserToGroup(CurrentUser, Group);
            }
        }
    }
}