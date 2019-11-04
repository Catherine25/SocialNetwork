using System;
using System.Collections.Generic;
using System.Text;
using SocialNetwork.Services;
using Xamarin.Forms;

namespace SocialNetwork.Data
{
    public class User
    {
        public User(int id, string image, string name, string bio)
        {
            AvatarLink = image;
            Name = name;
            Bio = bio;
            Id = id;

            if (AvatarLink == null)
                AvatarLink = "";
        }

        public User(string id, string name, string bio, string image)
        {
            AvatarLink = image;
            Name = name;
            Bio = bio;
            Id = int.Parse(id);

            if (AvatarLink == null)
                AvatarLink = "";
        }

        public string AvatarLink;
        public string Name;
        public string Bio;
        public int Id;

        public List<User> Friends;
        public List<Group> Groups;

        public override int GetHashCode() {
            var hashCode = 1434431970;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(AvatarLink);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Bio);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<User>>.Default.GetHashCode(Friends);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<Group>>.Default.GetHashCode(Groups);
            return hashCode;
        }
    }
}
