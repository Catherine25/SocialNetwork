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
        private List<string> conversationsHeaders;
        private SortedSet<Conversation> conversations;
        public event Action<User, Conversation> OpenDialodRequest;

        public MessagesView(User _user, List<Conversation> _conversations)
        {
            InitializeComponent();

            user = _user;
            conversations = new SortedSet<Conversation>(_conversations);

            conversationsHeaders = new List<string>();

            Reload();
        }

        private void Reload()
        {
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
                {
                    string header = GetHeader(conversations.ElementAt(i));
                    if (conversationsHeaders.Any(X => X == header))
                        ;//throw new Exception();
                    else
                        conversationsHeaders.Add(header);
                }

                listView.ItemsSource = conversationsHeaders;
            }

            listView.ItemSelected += ItemSelected;

            BindingContext = this;
        }

        private void ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            int i = e.SelectedItemIndex;
            OpenDialodRequest(user, conversations.ElementAt(i));
        }

        private string GetHeader(Conversation conversation)
        {
            //get last message
            Message message = conversation.messages[conversation.messages.Count - 1];

            //get author name
            User author = message.IsFromMember1 ? conversation.member1 : conversation.member2;
            string authorName = author == user ? "You" : author.Name;

            string text = authorName + ": " + message.Text;
            return text;
        }

        public void SetTheme(Theme theme)
        {
            (this as View).SetTheme(theme);
        }
    }
}