﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
//using MySql.Data.MySqlClient;
using SocialNetwork.Data;
using SocialNetwork.Data.Database;
using SocialNetwork.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SocialNetwork
{
    public partial class App : Application
    {
        #region Variables

        NewMessagesImitator _bot;
        Themes _themes;
        User _currentUser;
        LocalData _localData;
        
        #endregion

        ///<summary> Constructor </summary>
        public App()
        {
            InitializeComponent();

            _localData = new LocalData();
            _themes = new Themes();

            //try
            //{
            //    Debug.WriteLine("Trying to connect to database");
            //}
            //catch
            //{
            //    Debug.WriteLine("Failed. Connecting to generator");
            //    //_loader = new GeneratorLoader();
            //}
			
            // _bot = new NewMessagesImitator(_currentUser, DateTime.Now, TimeSpan.FromSeconds(5));
            // _bot.MessageGenerated += BotGeneratedMessage;

            MainPage = new MainPage(_themes, _localData);
        }

		/// <summary>Special method with validation</summary>
		//private void TryAddConversation(Conversation conversation)
  //      {
  //          Debug.WriteLine("TryAddConversation() running");

  //          User user1 = conversation.member1;
  //          User user2 = conversation.member2;

  //          Debug.WriteLine("user1 = " + user1 + " user2 = " + user2);

  //          Conversation existing;
  //          existing = _localData.Conversations.Find(X => X.member1.Name == user1.Name && X.member2.Name == user2.Name);

  //          if (existing == null)
  //          {
  //              Debug.WriteLine("existing conversation not found");
  //              existing = _localData.Conversations.Find(X => X.member1.Name == user2.Name && X.member2.Name == user1.Name);
  //          }

  //          if (existing == null)
  //          {
  //              Debug.WriteLine("existing conversation not found again, adding");
  //              _localData.Conversations.Add(conversation);
  //          }
  //          else
  //          {
  //              Debug.WriteLine("existing conversation_1 found");
  //              existing.messages.AddRange(conversation.messages);
  //          }
  //      }

        ///<summary> Rises after NewMessagesImitator.MessageGenerated </summary>
        //private void BotGeneratedMessage(Message message, User author)
        //{
        //    //message.IsFromMember1 = true;
        //    //TryAddConversation(new Conversation(author, _currentUser), new List<Message> { message }));
        //}

        #region Overridings
        protected override void OnStart()
        {
            //new Thread(_bot.TryWork).Start();

            new Thread(_themes.LoadRomanukeThemes).Start();
        }

        protected override void OnSleep() => _bot.SuspendRequest();

        protected override void OnResume() { /* Handle when your app resumes */ }
        #endregion
    }
}
