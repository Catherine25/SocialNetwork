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

        public string AvatarLink;
        public string Name;
        public string Bio;
        public int Id;
    }
}
