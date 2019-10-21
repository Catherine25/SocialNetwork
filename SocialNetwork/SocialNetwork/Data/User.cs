using System;
using System.Collections.Generic;
using System.Text;
using SocialNetwork.Services;
using Xamarin.Forms;

namespace SocialNetwork.Data
{
    public class User
    {
        public User(string image, string name, string bio)
        {
            AvatarLink = image;
            Name = name;
            Bio = bio;

            if (AvatarLink == null)
                AvatarLink = "";

            Friends = new List<User>();
        }

        public string AvatarLink;
        public string Name;
        public string Bio;

        public List<User> Friends;
        public List<Group> Groups;

        public static bool operator ==(User user1, User user2) =>
            user1.Name == user2.Name;
        
        public static bool operator !=(User user1, User user2) =>
            user1.Name != user2.Name;

        public override bool Equals(object obj) =>
            obj is User user && Name == user.Name;

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
