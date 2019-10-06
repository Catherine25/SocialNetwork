using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SocialNetwork.Data
{
    public class User
    {
        public User(Image image, string name, string bio)
        {
            Avatar = image;
            Name = name;
            Bio = bio;

            if (Avatar == null)
                Avatar = new Image();

            Friends = new List<User>();

            Theme = new Theme(
                "default", new Color[] {
                Color.FromRgb(26,24,24),
                Color.FromRgb(85,89,95),
                Color.Black,
                Color.Black });
        }

        public Image Avatar;
        public string Name;
        public string Bio;
        public Theme Theme;

        public List<User> Friends;
        public List<Group> Groups;
    }
}
