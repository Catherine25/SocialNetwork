using SocialNetwork.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SocialNetwork.Services
{
    class Test
    {
        private const int standartCount = 10; 
        private static string GenerateAvatarLink() => Avatars[Random.Next(standartCount)];
        private static string GenerateName() => Names[Random.Next(standartCount)];
        private static string GenerateSentence() => Sentences[Random.Next(standartCount)];

        public static User GenerateUser() =>
            new User(Random.Next(), GenerateAvatarLink(), GenerateName(), GenerateSentence());

        public static Group GenerateGroup() =>
            new Group(Random.Next(), GenerateSentence(), GenerateSentence(), GenerateAvatarLink());

        public static Message GenerateMessage() =>
            new Message(Random.Next(), GenerateSentence(), GenerateDateTime(), Random.Next(2) == 0 );

        public static Conversation GenerateConversation(User user) 
        {
            Debug.WriteLine("GenerateConversation() running");

            List<Message> generatedMessages = new List<Message>(GenerateMessages(user));
            List<Message> messages = generatedMessages.OrderBy(X => X.DateTime).ToList();

            User newUser = GenerateUser();
            if (newUser == user)
            {
                Debug.WriteLine("Failed to generate conversation, created user already exists");
                return null;
            }
            else
            {
                Conversation c = new Conversation(0, user, newUser);
                c.messages = messages;
                Debug.WriteLine("New conversation created. " + "member1 is " + c.member1.Name + "member2 is " + c.member2.Name);
                return c;
            }
        }

        private static DateTime GenerateDateTime()
        {
            //Debug.WriteLine("GenerateDateTime() running");

            DateTime dateTime = new DateTime(1999, 1, 25);
            TimeSpan range = (DateTime.Now - dateTime);
            //Debug.WriteLine("range is " + range.ToString());
            
            int daysRange = range.Days;
            int hoursRange = range.Hours;
            int minutesRange = range.Minutes;
            int secondsRange = range.Seconds;

            dateTime = dateTime.AddDays(Random.Next(daysRange));
            dateTime = dateTime.AddHours(Random.Next(hoursRange));
            dateTime = dateTime.AddMinutes(Random.Next(minutesRange));
            dateTime = dateTime.AddSeconds(Random.Next(secondsRange));

            //Debug.WriteLine("Final dateTime is: " + dateTime);
            return dateTime;
        }

        public static IEnumerable<Message> GenerateMessages(User user)
        {

            Debug.WriteLine("GenerateMessages() running");

            IList<Message> messages = new List<Message>();

            int count = Random.Next(1, standartCount);

            for (int i = 0; i < count; i++)
            {
                Message message = GenerateMessage();

                if (messages.Any(X => X.DateTime.ToString() == message.DateTime.ToString()))
                    continue;
                else
                    messages.Add(message);
            }

            Debug.WriteLine("Generated " + messages.Count + " messages");

            return messages;
        }

        public static IEnumerable<Conversation> GenerateConversations(User user) {

            Debug.WriteLine("GenerateConversations() running");

            int length = Random.Next(standartCount);
            IList<Conversation> conversations = new List<Conversation>();

            for (int i = 0; i < length; i++)
            {
                Conversation conversation = GenerateConversation(user);
                if (conversations.Contains(conversation))
                {
                    Debug.WriteLine("User of generated conversation exists");
                    continue;
                }
                else if(conversation != null)
                    conversations.Add(conversation);
            }

            return conversations;
        }

        public static List<User> GenerateUsers()
        {
            List<User> friends = new List<User>();

            int length = Random.Next(standartCount);

            for (int i = 0; i < length; i++)
                friends.Add(GenerateUser());

            return friends;
        }

        public static List<Group> GenerateGroups()
        {
            List<Group> groups = new List<Group>();

            int length = Random.Next(standartCount);

            for (int i = 0; i < length; i++)
                groups.Add(GenerateGroup());

            return groups;
        }

        static Random Random = new Random();

        static string[] Names = new string[standartCount]
        {
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

        static string[] Sentences = new string[standartCount]
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

        static string[] Avatars = new string[standartCount]
        {
            "https://i.redd.it/ajixtzm7f6221.jpg",
            "https://media-01.creema.net/user/2061109/exhibits/4349352/0_5869938eb999b1235fe3a7fe7ac8d72b_583x585.jpg",
            "https://cdn4.vectorstock.com/i/1000x1000/58/03/cute-kitten-domestic-pet-pixel-art-isolated-vector-18645803.jpg",
            "https://ih0.redbubble.net/image.432932179.2151/flat,550x550,075,f.u1.jpg",
            "https://img21.shop-pro.jp/PA01377/893/product/120448091.jpg?cmsp_timestamp=20170719142403",
            "https://dplhqivlpbfks.cloudfront.net/box_resize/1220x1240/6f2d5a8c-392305.jpg",
            "https://dejpknyizje2n.cloudfront.net/marketplace/products/anime-pixel-art-girl-with-purple-and-pink-hair-sticker-1539282612.7939198.png",
            "https://dejpknyizje2n.cloudfront.net/marketplace/products/8-bit-pixel-art-pizza-sticker-1542411683.8391273.png",
            "https://www.sccpre.cat/mypng/detail/319-3194557_pixel-fox-easy-pixel-art-cat.png",
            "https://images-na.ssl-images-amazon.com/images/I/11Xv6Hb3OyL._SY355_.png"
        };
    }
}
