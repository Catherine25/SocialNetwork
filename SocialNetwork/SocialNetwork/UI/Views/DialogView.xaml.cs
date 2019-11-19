using SocialNetwork.Data;
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
        private Conversation Conversation;
        private Dictionary<Guid, Message> messagesId;
        private Theme theme;
        private SQLLoader _loader;

        public DialogView(Conversation conversaton, User user, Theme newTheme, SQLLoader loader)
        {
            InitializeComponent();
            
            User = user;
            Conversation = conversaton;
            messagesId = new Dictionary<Guid, Message>();
            theme = newTheme;
            _loader = loader;

            List<Message> orderedEnumerable = conversaton.messages.OrderBy(x => x.DateTime).ToList();

            int length = orderedEnumerable.Count();

            for (int i = 0; i < length; i++)
            {
                Message message = orderedEnumerable.ElementAt(i);
                Button button = CreateButton(message, conversaton.member1 == user);
                messagesId.Add(button.Id, message);
                button.SetTheme(theme);
                stack.Children.Add(button);
            }

            newMessageEntry.Completed += NewMessageEntry_Completed;
        }

        private void NewMessageEntry_Completed(object sender, EventArgs e)
        {
            string text = (sender as Entry).Text;
            (sender as Entry).Text = "";
            
            Message message = new Message(0, text, DateTime.Now, Conversation.member1 == User ? true : false);
            Conversation.messages.Add(message);

            stack.Children.Add(CreateButton(message, message.IsFromMember1));

            _loader.AddNewMessage(message, Conversation);
        }

        public Button CreateButton(Message message, bool currentUserIsMember1)
        {
            Button button = new Button
            {
                TextColor = theme.TextColor,
                Text = message.Text
            };
            if ((currentUserIsMember1 && message.IsFromMember1) || (!currentUserIsMember1 && !message.IsFromMember1))
                button.Margin = new Thickness { Left = 100 };
            else
                button.Margin = new Thickness { Right = 100 };
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