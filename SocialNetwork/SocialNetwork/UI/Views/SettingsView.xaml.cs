using SocialNetwork.Data;
using SocialNetwork.UI.DataRequests;
using SocialNetwork.UI.Editors;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SocialNetwork.UI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsView : ContentView
    {
        private User _user;

        public event Action<UserRequestDialog.RequestPurpose> ReloginRequest;
        public event Action CreateDialogRequest;
        public event Action<UserEditor.EditPurpose, User> EditUserRequest;
        public event Action OpenMessagesView;

        public SettingsView(User user)
        {
            InitializeComponent();

            _user = user;

            _menu.OpenSettingsViewRequest += () => OpenMessagesView();

            _reloginBt.Clicked += (object sender, EventArgs e) => ReloginRequest(UserRequestDialog.RequestPurpose.currentName);
            _createDialog.Clicked += (object sender, EventArgs e) => CreateDialogRequest();
            _editProfile.Clicked += (object sender, EventArgs e) => EditUserRequest(UserEditor.EditPurpose.update, _user);

            Update(user);
        }

        public void Update(User user)
        {
            _user = user;

            _menu.Update(_user.Name);
        }
    }
}