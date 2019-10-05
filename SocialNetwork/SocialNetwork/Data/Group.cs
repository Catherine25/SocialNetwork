using System;
using System.Collections.Generic;
using System.Text;

namespace SocialNetwork.Data
{
    public class Group
    {
        public Group(string newTitle, string newDescription)
        {
            Title = newTitle;
            Description = newDescription;
        }

        public string Title;
        public string Description;
        public List<User> members;
    }
}
