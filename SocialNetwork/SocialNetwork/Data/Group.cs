using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SocialNetwork.Data
{
    public class Group
    {
        public Group(string newTitle, string newDescription, string imageLink)
        {
            AvatarLink = imageLink;
            Title = newTitle;
            Description = newDescription;

            if (AvatarLink == null)
                AvatarLink = "";
        }

        public string AvatarLink;
        public string Title;
        public string Description;
        public List<User> members;
    }
}
