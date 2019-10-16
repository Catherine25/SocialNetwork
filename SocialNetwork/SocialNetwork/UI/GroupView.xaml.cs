﻿using SocialNetwork.Data;
using SocialNetwork.Services;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SocialNetwork.UI
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GroupView : ContentView, IColorable
    {
        User CurrentUser;
        Group Group;

        public GroupView(User user, Group group)
        {
            InitializeComponent();

            CurrentUser = user;
            Group = group;

            removeBt.Clicked += RemoveBt_Clicked;
            removeBt.Text = CurrentUser.Groups.Contains(Group) ? "Remove from groups list" : "Add to groups list";

            if (group.AvatarLink != "")
                image.Source = ImageSource.FromUri(new Uri(group.AvatarLink));
            else
                image.Source = "https://www.indiannaturaloils.com/categories-images/no-photo.jpg";

            title.Text = group.Title;
            description.Text = group.Description;

            image.Clicked += Image_Clicked;

            SetTheme(user.Theme);
        }

        public void SetTheme(Theme theme) => (this as View).SetTheme(theme);

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

        private void RemoveBt_Clicked(object sender, EventArgs e)
        {
            Button button = sender as Button;

            if (CurrentUser.Groups.Contains(Group))
            {
                button.Text = "Add to groups list";
                CurrentUser.Groups.Remove(Group);
            }
            else
            {
                CurrentUser.Groups.Add(Group);
                button.Text = "Remove from groups list";
            }
        }
    }
}