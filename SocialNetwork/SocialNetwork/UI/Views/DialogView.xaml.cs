using SocialNetwork.Data;
using SocialNetwork.Data.Database;
using SocialNetwork.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SocialNetwork.UI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DialogView : ContentView, IColorable {

        private User User;
        private Conversation _conversation;
        private Dictionary<Guid, Message> messagesId;
        private Theme theme;
        private LocalData _localData;

        public DialogView(Conversation conversation, User user, Theme newTheme, LocalData localData)
        {
            InitializeComponent();

            newMessageEntry.Completed += NewMessageEntry_Completed;

            Update(conversation, user, newTheme, localData);
        }

        public void Update(Conversation conversation, User user, Theme newTheme, LocalData localData)
        {
            User = user;
            int id1 = conversation.member1.Id;
            int id2 = conversation.member2.Id;

            _conversation = localData.GetConversations().First(c => (c.member1.Id == id1 && c.member2.Id == id2) || (c.member1.Id == id2 && c.member2.Id == id1));
            messagesId = new Dictionary<Guid, Message>();
            theme = newTheme;
            _localData = localData;

            if (_conversation.messages == null)
                _conversation.messages = new List<Message>();
            
            List<Message> orderedEnumerable = _conversation.messages.OrderBy(x => x.DateTime).ToList();

            int length = orderedEnumerable.Count();

            stack.Children.Clear();

            for (int i = 0; i < length; i++)
            {
                Message message = orderedEnumerable.ElementAt(i);
                Button button = CreateButton(message, _conversation.member1 == user);
                button.SetTheme(theme);
                stack.Children.Add(button);
            }

        }

        private void NewMessageEntry_Completed(object sender, EventArgs e)
        {
            string text = (sender as Entry).Text;
            (sender as Entry).Text = "";
            
            Message message = new Message(0, text, DateTime.Now, _conversation.member1.Id == User.Id);
            _conversation.messages.Add(message);

            stack.Children.Add(CreateButton(message, message.IsFromMember1));

            _localData.AddNewMessage(message, _conversation);
        }

        public Button CreateButton(Message message, bool currentUserIsMember1)
        {
            Button button = new Button
            {
                TextColor = theme.TextColor,
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

        public void SetTheme(Theme theme) => (this as View).SetTheme(theme);
    }
}