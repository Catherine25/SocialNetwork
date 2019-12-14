using SocialNetwork.Data;
using SocialNetwork.Data.Database;
using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SocialNetwork.UI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DialogView : ContentView
    {
        private User _user;
        private User _anotherUser;
        private Conversation _conversation;
        private Dictionary<Guid, Message> messagesId;
        private LocalData _localData;

        public event Action OpenMessagesViewRequest;

        public DialogView(Conversation conversation, User user, LocalData localData)
        {
            InitializeComponent();

            _bar.ReturnRequest += () => OpenMessagesViewRequest();

            newMessageEntry.Completed += NewMessageEntry_Completed;

            Update(conversation, user, localData);
        }

        public void Update(Conversation conversation, User user, LocalData localData)
        {
            _user = user;
            _anotherUser = (_user.Id == conversation.member1.Id) ? conversation.member2 : conversation.member1;

            _bar.Update(_anotherUser.AvatarLink, _anotherUser.Name);

            int id1 = conversation.member1.Id;
            int id2 = conversation.member2.Id;

            _conversation = localData.GetConversations().First(c => (c.member1.Id == id1 && c.member2.Id == id2) || (c.member1.Id == id2 && c.member2.Id == id1));
            messagesId = new Dictionary<Guid, Message>();

            _localData = localData;

            if (_conversation.messages == null)
                _conversation.messages = new List<Message>();
            
            List<Message> orderedEnumerable = _conversation.messages.OrderBy(x => x.DateTime).ToList();

            int length = orderedEnumerable.Count();

            stack.Children.Clear();

            for (int i = 0; i < length; i++)
            {
                Message message = orderedEnumerable.ElementAt(i);
                Button button = CreateButton(message, _conversation.member1 == _user);
                stack.Children.Add(button);
            }
        }

        private void NewMessageEntry_Completed(object sender, EventArgs e)
        {
            string text = (sender as Entry).Text;
            (sender as Entry).Text = "";
            
            Message message = new Message(0, text, DateTime.Now, _conversation.member1.Id == _user.Id);
            _conversation.messages.Add(message);

            stack.Children.Add(CreateButton(message, message.IsFromMember1));

            _localData.AddNewMessage(message, _conversation);
        }

        public Button CreateButton(Message message, bool currentUserIsMember1)
        {
            Button button = new Button
            { 
                Text = message.Text
            };
            messagesId.Add(button.Id, message);

            button.Margin = (currentUserIsMember1 && message.IsFromMember1) || (!currentUserIsMember1 && !message.IsFromMember1)
                ? new Thickness { Left = 100 }
                : new Thickness { Right = 100 };

            button.Clicked += messageClicked;
            return button;
        }

        private void messageClicked(object sender, EventArgs e)
        {
            Button button = sender as Button;
            Message message = messagesId[button.Id];

            button.Text = (button.Text == message.Text) ? message.DateTime.ToString() : message.Text;
        }
    }
}