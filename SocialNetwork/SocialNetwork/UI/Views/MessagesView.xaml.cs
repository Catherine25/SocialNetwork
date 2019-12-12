using SocialNetwork.Data;
using SocialNetwork.Data.Database;
using SocialNetwork.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SocialNetwork.UI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MessagesView : ContentView, IColorable
    {
        private User user;
        //private List<string> conversationsHeaders;
        private ObservableCollection<ImageCell> imageCells;
        private Dictionary<int, int> keyValues;
        public event Action<User, Conversation> OpenDialodRequest;
        public event Action<FriendsView.Mode> OpenFriendsViewRequest;
        private LocalData _localData;

        public MessagesView(User _user, LocalData localData)
        {
            Debug.WriteLine("[m] [MessagesView] Constructor running");

            InitializeComponent();

            menu.Update(_user.Name);

            menu.SearchRequest += (string s) =>
            {
                Reload();
                
                int length = imageCells.Count;
                for (int i = 0; i < length; i++)
                    if(!imageCells[i].Text.Contains(s))
                        imageCells.RemoveAt(i);
            };

            //NewConversationBt.Clicked += NewConversationBt_Clicked;
            listView.ItemTapped += ListView_ItemTapped;

            Update(_user, localData);
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Debug.WriteLine("[m] [MessagesView] ListView_ItemTapped running");

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
            Debug.WriteLine("[m] [MessagesView] Update running");

            user = _user;
            _localData = localData;
            keyValues = new Dictionary<int, int>();
            //conversationsHeaders = new List<string>();
            imageCells = new ObservableCollection<ImageCell>();

            imageCells.Clear();
            Reload();
        }

        private void NewConversationBt_Clicked(object sender, EventArgs e)
        {
            Debug.WriteLine("[m] [MessagesView] NewConversationBt_Clicked running");

            OpenFriendsViewRequest(FriendsView.Mode.ChooseNew);
        }

        private void Reload()
        {
            Debug.WriteLine("[m] [MessagesView] Reload running");

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
                //string s = GetHeader(c);
                var cell = CreateImageCell(c);
                //conversationsHeaders.Add(s);
                imageCells.Add(cell);
                keyValues.Add(i, c.Id);
            }

            //listView.ItemsSource = conversationsHeaders;
            listView.ItemsSource = imageCells;
            //BindingContext = this;
        }

        //private string GetHeader(Conversation conversation)
        //{
        //    Debug.WriteLine("[m] [MessagesView] GetHeader running");

        //    //get last message
        //    Message message = conversation.messages[conversation.messages.Count - 1];

        //    //get author name
        //    User author = message.IsFromMember1 ? conversation.member1 : conversation.member2;
        //    string authorName = author == user ? "You" : author.Name;

        //    string text = authorName + ": \t" + message.Text;
        //    return text;
        //}

        public void SetTheme(Theme theme)
        {
            Debug.WriteLine("[m] [MessagesView] SetTheme running");

            (this as View).SetTheme(theme);
        }

        public ImageCell CreateImageCell(Conversation conversation)
        {
            //get last message
            Message message = conversation.messages[conversation.messages.Count - 1];

            //get another user
            User another = conversation.member1.Id == user.Id ? conversation.member2 : conversation.member1;

            return new ImageCell
            {
                ImageSource = another.AvatarLink,
                Text = another.Name,
                Detail = message.Text
            };
        }
    }
}