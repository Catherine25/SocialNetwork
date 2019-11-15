using SocialNetwork.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SocialNetwork.UI
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RequestDialog : ContentView
	{
        private string text;
        private RequestPurpose _purpose;
        private List<User> _users;

        public enum RequestPurpose { currentName, newFriendName }
        public event Action<User, RequestPurpose> RequestCompleted;

		public RequestDialog(RequestPurpose purpose, List<User> users)
		{
			InitializeComponent();

            _purpose = purpose;
            _users = users;

            ConfirmBt.Clicked += ConfirmBt_Clicked;
            CancelBt.Clicked += CancelBt_Clicked;
            textEntry.Unfocused += TextEntry_Unfocused;
		}

        private void CancelBt_Clicked(object sender, EventArgs e) =>
            RequestCompleted(null, _purpose);

        private void ConfirmBt_Clicked(object sender, EventArgs e)
        {
            User user = _users.Find(u => u.Name == text);
            if (user != null)
                RequestCompleted(user, _purpose);
        }

        private void TextEntry_Unfocused(object sender, FocusEventArgs e) =>
            text = textEntry.Text;
    }
}