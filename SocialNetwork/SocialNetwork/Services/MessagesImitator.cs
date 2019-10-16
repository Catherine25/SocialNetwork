using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SocialNetwork.Services
{
    class NewMessagesImitator : IActivityImitator
    {
        DateTime startTime;
        TimeSpan frequency;
        DateTime nextActionTime = DateTime.Now;
        Data.User user;
        bool suspend = false;

        public NewMessagesImitator(Data.User _user) => user = _user;

        public DateTime StartTime
        {
            get => startTime;
            set => startTime = value;
        }

        public TimeSpan Frequency
        {
            get => frequency;
            set => frequency = value;
        }

        public void SuspendRequest() => suspend = true;

        public void TryWork(object o)
        {
            Debug.WriteLine("TryWork() running");
            while(!suspend)
                if(DateTime.Now >= nextActionTime)
                {
                    Work();
                    startTime = DateTime.Now;
                }
            Debug.WriteLine("TryWork() stopped");
        }

        private void Work()
        {
            Debug.WriteLine("Work() running");

            //generate message
            Data.Message message = Services.Test.GenerateMessage();
            message.DateTime = DateTime.Now;

            //generate user to speak with
            Data.User newUser = Services.Test.GenerateUser();

            //get user's conversations and count
            List<Data.Conversation> conversations = Data.Messages.GetConversationsByUser(user).ToList();
            
            //get conversation with the generated user
            Data.Conversation conversation = conversations.Find(X => X.member1 == newUser || X.member2 == newUser);

            if(conversation != null)
                conversation.messages.Add(message);
            else
                conversations.Add(new Data.Conversation(user, newUser, new List<Data.Message>() { message }));
            
            Debug.WriteLine("Work() stopped");
        }
    }
}