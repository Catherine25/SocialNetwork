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

//TODO: Open selected conversation dialog

namespace SocialNetwork.UI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MessagesView : ContentView, IColorable
    {
        private User user;
        private List<string> conversationsHeaders;
        private Dictionary<int, int> keyValues;
        public event Action<User, Conversation> OpenDialodRequest;
        public event Action<FriendsView.Mode> OpenFriendsViewRequest;
        private LocalData _localData;

        public MessagesView(User _user, LocalData localData)
        {
            InitializeComponent();

            NewConversationBt.Clicked += NewConversationBt_Clicked;
            //listView.ItemSelected += ItemSelected;
            listView.ItemTapped += ListView_ItemTapped;

            Update(_user, localData);
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (sender is ListView lv)
            {
                int index = e.ItemIndex;
                Conversation conversation = _localData.GetConversations().First(c => c.Id == keyValues[index]);
                OpenDialodRequest(user, conversation);
            }

            (sender as ListView).SelectedItem = null;
        }

        public void Update(User _user, LocalData localData)
        {
            user = _user;
            _localData = localData;
            keyValues = new Dictionary<int, int>();
            conversationsHeaders = new List<string>();

            Reload();
        }

        private void NewConversationBt_Clicked(object sender, EventArgs e) =>
            OpenFriendsViewRequest(FriendsView.Mode.ChooseNew);

        private void Reload()
        {
            user = _localData.Update(user);

            var filteredConversations = _localData.FindConversationsOfUser(user);
            filteredConversations = filteredConversations.Where(f => f.messages != null).ToList();
            filteredConversations = filteredConversations.Where(f => f.messages.Count != 0).ToList();
            int length = filteredConversations.Count;

            NoConversationsBt.IsVisible = length == 0;
            listView.IsVisible = length != 0;

            for (int i = 0; i < length; i++)
            {
                Conversation c = filteredConversations[i];
                string s = GetHeader(c);
                conversationsHeaders.Add(s);
                keyValues.Add(i, c.Id);
            }

            listView.ItemsSource = conversationsHeaders;
            BindingContext = this;
        }

        //private void ItemSelected(object sender, SelectedItemChangedEventArgs e)
        //{
        //    int index = e.SelectedItemIndex;
        //    Conversation conversation =_localData.GetConversations().First(c => c.Id == keyValues[index]);
        //    OpenDialodRequest(user, conversation);
        //}

        private string GetHeader(Conversation conversation)
        {
            //get last message
            Message message = conversation.messages[conversation.messages.Count - 1];

            //get author name
            User author = message.IsFromMember1 ? conversation.member1 : conversation.member2;
            string authorName = author == user ? "You" : author.Name;

            string text = authorName + ": \t" + message.Text;
            return text;
        }

        public void SetTheme(Theme theme) => (this as View).SetTheme(theme);
    }
}