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

            Friends = new List<User>();
        }

        public Image Avatar;
        public string Name;
        public string Bio;

        public List<User> Friends;
        public List<Group> Groups;
    }
}
