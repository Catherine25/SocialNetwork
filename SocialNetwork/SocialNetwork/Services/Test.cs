using SocialNetwork.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialNetwork.Services
{
    class Test
    {
        public static User GenerateUser() =>
            new User(null, Names[Random.Next(0, 10)], Sentences[Random.Next(0, 10)]);

        public static Group GenerateGroup() =>
            new Group(Sentences[Random.Next(0, 10)], Sentences[Random.Next(0, 10)]);

        public static IEnumerable<Message> GenerateMessages(User user) {

            IList<Message> messages = new List<Message>();

            int count = Random.Next(0, 10);
            for (int i = 0; i < count; i++)
            {
                messages.Add(new Message
                {
                    Reciever = user,
                    Sender = GenerateUser(),
                    Text = Sentences[Random.Next(0, 10)]
                });
            }

            return messages;
        }

        public static List<User> GenerateUsers()
        {
            List<User> friends = new List<User>();

            int length = Random.Next(0, 10);

            for (int i = 0; i < length; i++)
                friends.Add(GenerateUser());

            return friends;
        }

        public static List<Group> GenerateGroups()
        {
            List<Group> groups = new List<Group>();

            int length = Random.Next(0, 10);

            for (int i = 0; i < length; i++)
                groups.Add(GenerateGroup());

            return groups;
        }

        static Random Random = new Random();

        static string[] Names = new string[10] {
            "Vincent Little",
            "Matthew Garrett",
            "Sonja Hodges",
            "Lillie Brown",
            "Phil Brewer",
            "Rickey Barrett",
            "Lorraine Watkins",
            "Harvey Wade",
            "Pauline Willis",
            "Esther Palmer"
        };

        static string[] Sentences = new string[10]
        {
            "makudonarudo",
            "guguru",
            "toiretto",
            "kitto katto",
            "dizunirando",
            "takushi go hoteru",
            "sebun irebun",
            "miruku",
            "baasu biiru",
            "sutaabakkusu"
        };
    }
}
