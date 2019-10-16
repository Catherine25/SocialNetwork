using System;
using System.Collections.Generic;
using System.Text;
using SocialNetwork.Services;
using Xamarin.Forms;

namespace SocialNetwork.Data
{
    public class User
    {
        public event Action<Theme> ThemeChanged;
        public User(string image, string name, string bio)
        {
            AvatarLink = image;
            Name = name;
            Bio = bio;

            if (AvatarLink == null)
                AvatarLink = "";

            Friends = new List<User>();

            Theme = Themes.ThemesList[0];
        }

        public string AvatarLink;
        public string Name;
        public string Bio;
        private Theme theme;

        public List<User> Friends;
        public List<Group> Groups;

        public Theme Theme
        {
            get => theme;
            set
            {
                // if(theme != value && theme != null) {
                //     theme = value;
                //     ThemeChanged(theme);                
                // }
                theme = value;
                if(ThemeChanged != null)
                    ThemeChanged(theme);
            }
        }
    }
}
