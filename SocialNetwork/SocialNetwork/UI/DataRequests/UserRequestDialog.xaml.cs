﻿using SocialNetwork.Data;
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
        private string text;
        private RequestPurpose _purpose;
        private List<User> _users;

        public enum RequestPurpose { currentName, newFriendName }
        public event Action<User, RequestPurpose> RequestCompleted;

		public UserRequestDialog(RequestPurpose purpose, List<User> users)
		{
			InitializeComponent();

            _purpose = purpose;
            _users = users;

            ConfirmBt.Clicked += ConfirmBt_Clicked;
            CancelBt.Clicked += CancelBt_Clicked;
            textEntry.Unfocused += TextEntry_Unfocused;
            textEntry.Completed += TextEntry_Completed;

            if (purpose == RequestPurpose.currentName)
                infoLabel.Text = "Enter your name";
            else if (purpose == RequestPurpose.newFriendName)
                infoLabel.Text = "Enter new friend name";
		}

        private void TextEntry_Completed(object sender, EventArgs e)
        {
            text = textEntry.Text;
            User user = _users.Find(u => u.Name == text);
            if (user != null)
                RequestCompleted(user, _purpose);
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