using SocialNetwork.Data;
using SocialNetwork.UI.Editors;
using SocialNetwork.UI.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SocialNetwork.UI.DataRequests
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class UserRequestDialog : ContentView
	{
        private RequestPurpose _purpose;
        private List<User> _users;

        private string EnterName = "Enter your name";
        private string EnterFriend = "Enter new friend name";

        public enum RequestPurpose { currentName, newFriendName }
        public event Action<User, RequestPurpose> RequestCompleted;
        public event Action<UserEditor.EditPurpose> ShowUserEditorRequest;
        public event Action<FriendsView.Mode> ShowFriendsViewRequest;

        public UserRequestDialog(RequestPurpose purpose, List<User> users)
		{
			InitializeComponent();

            _purpose = purpose;
            _users = users;

            ConfirmBt.Clicked += ConfirmBt_Clicked;
            CancelBt.Clicked += CancelBt_Clicked;
            RegistrateBt.Clicked += RegistrateBt_Clicked;
            textEntry.Completed += TextEntry_Completed;

            if (purpose == RequestPurpose.currentName)
                infoLabel.Text = EnterName;
            else if (purpose == RequestPurpose.newFriendName)
                infoLabel.Text = EnterFriend;

            RegistrateBt.IsVisible = false;
		}

        private void RegistrateBt_Clicked(object sender, EventArgs e) =>
            ShowUserEditorRequest(UserEditor.EditPurpose.createNew);

        private void TextEntry_Completed(object sender, EventArgs e) =>
            Analyze();

        private void CancelBt_Clicked(object sender, EventArgs e)
        {
            if (_purpose == RequestPurpose.currentName)
                ;
            else if (_purpose == RequestPurpose.newFriendName)
                ShowFriendsViewRequest(FriendsView.Mode.Default);
            //RequestCompleted(null, _purpose);
            else throw new NotImplementedException();
        }

        private void ConfirmBt_Clicked(object sender, EventArgs e) => Analyze();

        private void Analyze()
        {
            User user = _users.Find(u => u.Name == textEntry.Text);

            if (user != null)
                RequestCompleted(user, _purpose);
            else
                RegistrateBt.IsVisible = true;
        }
    }
}