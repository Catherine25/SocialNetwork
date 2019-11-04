using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SocialNetwork.Data
{
    public class Group
    {
        public Group(int id, string newTitle, string newDescription, string imageLink)
        {
            Id = id;
            AvatarLink = imageLink;
            Title = newTitle;
            Description = newDescription;

            if (AvatarLink == null)
                AvatarLink = "";
        }

        public Group(string id, string newTitle, string newDescription, string imageLink)
        {
            Id = int.Parse(id);
            AvatarLink = imageLink;
            Title = newTitle;
            Description = newDescription;

            if (AvatarLink == null)
                AvatarLink = "";
        }

        public int Id;
        public string AvatarLink;
        public string Title;
        public string Description;
        public List<User> members;
    }
}
