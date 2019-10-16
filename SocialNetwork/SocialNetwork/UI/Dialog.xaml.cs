using SocialNetwork.Data;
using SocialNetwork.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SocialNetwork.UI
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Dialog : ContentView, IColorable
    {
        private User User;
        private Conversation Conversation;

        public Dialog(Conversation conversaton, User user)
        {
            InitializeComponent();
            
            User = user;
            Conversation = conversaton;

            IOrderedEnumerable<Message> orderedEnumerable = conversaton.messages.OrderBy(x => x.DateTime);

            int length = orderedEnumerable.Count();

            for (int i = 0; i < length; i++)
            {
                Button button = CreateButton(orderedEnumerable.ElementAt(i), conversaton.member1 == user);
                button.SetTheme(user.Theme);
                stack.Children.Add(button);
            }

            newMessageEntry.Completed += NewMessageEntry_Completed;
        }

        private void NewMessageEntry_Completed(object sender, EventArgs e)
        {
            string text = (sender as Entry).Text;
            (sender as Entry).Text = "";
            
            Message message = new Message(text, DateTime.Now, Conversation.member1 == User ? true : false);
            Conversation.messages.Add(message);

            stack.Children.Add(CreateButton(message, message.IsFromMember1));
        }

        public Button CreateButton(Message message, bool currentUserIsMember1)
        {
            Button button = new Button
            {
                TextColor = User.Theme.TextColor,
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
            Message message = Conversation.messages.Find(X=>X.Text == button.Text);

            if(message == null)
            {
                message = Conversation.messages.Find(X => X.DateTime.ToString() == button.Text);
                button.Text = message.Text;
            }
            else
                button.Text = message.DateTime.ToString();
        }

        private void messageFocused(object sender, FocusEventArgs e)
        {
            Button button = sender as Button;
            Message message = Conversation.messages.Find(X=>X.DateTime.ToString() == button.Text);
            button.Text = message.Text;
        }

        public void SetTheme(Theme theme) => (this as View).SetTheme(theme);
    }
}