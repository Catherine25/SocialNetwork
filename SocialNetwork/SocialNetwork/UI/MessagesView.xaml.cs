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
        private List<Conversation> conversations;
        public event Action<User, Conversation> OpenDialodRequest;

        public MessagesView(User _user, List<Conversation> _conversations)
        {
            InitializeComponent();

            //CopyCheck(_conversations);

            user = _user;
            conversations = new List<Conversation>(_conversations);

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

        //private void CopyCheck(List<Conversation> conversations)
        //{
        //    int length = conversations.Count;
        //    List<string> names1 = new List<string>();
        //    List<string> names2 = new List<string>();

        //    for (int i = 0; i < length; i++)
        //    {
        //        string name1 = conversations[i].member1.Name;
        //        string name2 = conversations[i].member2.Name;
                
        //        if (name1 == name2)
        //            throw new Exception("name duplicates");

        //        names1.Add(name1);
        //        names2.Add(name2);
        //    }

        //    for (int i = 0; i < length; i++)
        //    {
        //        for (int j = 0; j < length; j++)
        //        {
        //            if (names1[i] == names1[j] && names2[i] == names2[j])
        //                throw new Exception("two pairs!");
        //        }
        //    }
        //}
    }
}