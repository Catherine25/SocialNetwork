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

namespace SocialNetwork.UI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MessagesView : ContentView, IColorable
    {
        private User user;
        private List<string> conversationsHeaders;
        public event Action<User, Conversation> OpenDialodRequest;
        public event Action<FriendsView.Mode> OpenFriendsViewRequest;
        private Data.Database.LocalData _localData;

        public MessagesView(User _user, Data.Database.LocalData localData)
        {
            InitializeComponent();

            user = _user;
            _localData = localData;

            conversationsHeaders = new List<string>();

            NewConversationBt.Clicked += NewConversationBt_Clicked;

            Reload();
        }

        private void NewConversationBt_Clicked(object sender, EventArgs e) =>
            OpenFriendsViewRequest(FriendsView.Mode.ChooseNew);

        private void Reload()
        {
            var conversations = _localData.GetConversations().Where(c => c.member1 == user || c.member2 == user);
            int length = conversations.Count();
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
                    string header = GetHeader(_localData.GetConversations().ElementAt(i));
                    //if (conversationsHeaders.Any(X => X == header))
                    //    ;//throw new Exception();
                    //else
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
            OpenDialodRequest(user, _localData.GetConversations().ElementAt(i));
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

        public void SetTheme(Theme theme) => (this as View).SetTheme(theme);
    }
}