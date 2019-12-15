using SocialNetwork.Data;
using SocialNetwork.UI.Editors;
using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SocialNetwork.UI.DataRequests
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class UserRequestDialog : ContentView
	{
        private List<User> _users;

        private string EnterName = "Enter your name";

        public event Action<User> RequestCompleted;
        public event Action<UserEditor.EditPurpose> ShowUserEditorRequest;
        public event Action ShowFriendsViewRequest;

        public UserRequestDialog(List<User> users)
		{
			InitializeComponent();

            ConfirmBt.Clicked += ConfirmBt_Clicked;
            CancelBt.Clicked += CancelBt_Clicked;
            RegistrateBt.Clicked += RegistrateBt_Clicked;
            textEntry.Completed += TextEntry_Completed;

            Update(users);
		}

        public void Update(List<User> users)
        {
            _users = users;

            infoLabel.Text = EnterName;

            RegistrateBt.IsVisible = false;
        }

        private void RegistrateBt_Clicked(object sender, EventArgs e) =>
            ShowUserEditorRequest(UserEditor.EditPurpose.createNew);

        private void TextEntry_Completed(object sender, EventArgs e) =>
            Analyze();

        private void CancelBt_Clicked(object sender, EventArgs e) => ShowFriendsViewRequest();

        private void ConfirmBt_Clicked(object sender, EventArgs e) => Analyze();

        private void Analyze()
        {
            User user = _users.Find(u => u.Name == textEntry.Text);

            if (user != null)
                RequestCompleted(user);
            else
                RegistrateBt.IsVisible = true;
        }
    }
}