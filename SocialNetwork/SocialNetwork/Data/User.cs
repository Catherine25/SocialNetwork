using System.Collections.Generic;

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
    }
}
