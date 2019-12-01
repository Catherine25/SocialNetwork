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
        private string _noImageLink = "https://www.indiannaturaloils.com/categories-images/no-photo.jpg";

        public event Action<GroupEditor.EditPurpose, Group> EditGroupRequest;
        public event Action<FriendsView.Mode> ShowMembersRequest;

        public GroupView(User user, Group group, LocalData localData)
        {
            Debug.WriteLine("[m] [GroupView] Constructor running");

            InitializeComponent();
            
            _image.Clicked += Image_Clicked;

            _removeBt.Clicked += (object sender, EventArgs e) =>
            {
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
            };
            
            _editBt.Clicked += (object sender, EventArgs e) => EditGroupRequest(GroupEditor.EditPurpose.edit, Group);

            _showMembersBt.Clicked += (object o, EventArgs e) => ShowMembersRequest(FriendsView.Mode.ReadOnly);

            //debug

            _editBt.Clicked += (object o, EventArgs e) => Debug.WriteLine("[m] [GroupView] _editBt Clicked");
            _removeBt.Clicked += (object sender, EventArgs e) => Debug.WriteLine("[m] [GroupView] _removeBt Clicked");
            _image.Clicked += (object sender, EventArgs e) => Debug.WriteLine("[m] [GroupView] _image Clicked");
            _showMembersBt.Clicked += (object o, EventArgs e) => Debug.WriteLine("[m] [GroupView] _showMembersBt Clicked running");

            //debug

            Update(user, group, localData);
        }

        public void Update(User user, Group group, LocalData localData)
        {
            CurrentUser = user;
            Group = group;
            _localData = localData;

            _editBt.IsEnabled = CurrentUser.Id == _localData.FindUserByName("Admin").Id;
            _removeBt.Text = CurrentUser.Groups.Contains(Group) ? "Remove from groups list" : "Add to groups list";

            SetImage(Group.AvatarLink);

            _title.Text = group.Title;
            _description.Text = group.Description;
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

            SetImage(uriString);
        }

        private void SetImage(string uriString)
        {
            try
            {
                _image.Source = ImageSource.FromUri(new Uri(uriString));
            }
            catch
            {
                _image.Source = _noImageLink;
            }
        }
    }
}