using SocialNetwork.Data;
using SocialNetwork.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

//TODO: Open selected conversation dialog

namespace SocialNetwork.UI
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MessagesView : ContentView, IColorable
    {
        private User user;
        private List<string> conversationsHeaders = new List<string>();
        List<Conversation> conversations;
        public event Action<User, Conversation> OpenDialodRequest;

        public MessagesView(User _user)
        {
            InitializeComponent();

            user = _user;

            SetTheme(user.Theme);

            Reload();
        }

        private void Reload()
        {
            //USE ONLY MESSAGES WHERE CURRENT USER IS AUTHOR OR RECIEVER
            conversations = Conversations.GetConversationsByUser(user).ToList();

            int length = conversations.Count;
            if(length == 0)
            {
                Label label = new Label
                {
                    Text = "No Conversations",
                    FontSize = 90,
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center
                };
                messagesGrid.SetSingleChild(label);
            }
            else
            {
                for (int i = 0; i < length; i++)
                    conversationsHeaders.Add(GetHeader(conversations[i]));

                listView.ItemsSource = conversationsHeaders;
            }

            listView.ItemSelected += ItemSelected;

            BindingContext = this;
        }

        private void ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            int i = e.SelectedItemIndex;
            OpenDialodRequest(user, conversations[i]);
        }

        private string GetHeader(Conversation conversation)
        {
            Message message = conversation.messages[conversation.messages.Count - 1];
            string text = (message.IsFromMember1 ? conversation.member1.Name : conversation.member2.Name) + ": " + message.Text;
            return text;
        }

        public void SetTheme(Theme theme) => (this as View).SetTheme(theme);
    }
}