using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SocialNetwork.Services
{
    class NewMessagesImitator
    {
        DateTime startTime;
        TimeSpan frequency;
        DateTime nextActionTime = DateTime.Now;
        Data.User user;
        bool suspend;

        public event Action<Data.Message, Data.User> MessageGenerated;

        ///<summary> Constructor </summary>
        public NewMessagesImitator(Data.User _user, DateTime _startTime, TimeSpan _frequency)
        {
            user = _user;
            startTime = _startTime;
            frequency = _frequency;
            nextActionTime = startTime + frequency;
        }

        ///<summary> Stops TryWork() </summary>
        public void SuspendRequest() => suspend = true;

        public void TryWork(object o)
        {
            while(!suspend)
                if(DateTime.Now >= nextActionTime)
                {
                    Work();
                    startTime = DateTime.Now;
                    nextActionTime = startTime + frequency;
                }
        }

        private void Work()
        {
            //generate message
            Data.Message message = Test.GenerateMessage();
            Debug.WriteLine("Bot generated new message");

            //setting new DateTime
            message.DateTime = DateTime.Now;

            //generate user to speak with
            Data.User newUser = Test.GenerateUser();

            //check if generated user == current
            if(newUser == user)
                Debug.WriteLine("Generated user already exists");
            else
            {
                Debug.WriteLine("Bot generated new message from " + newUser.Name);

                //generate event
                MessageGenerated(message, newUser);
            }
        }
    }
}