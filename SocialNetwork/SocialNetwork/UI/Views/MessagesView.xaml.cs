using SocialNetwork.Data;
using SocialNetwork.Data.Database;
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
    public partial class MessagesView : ContentView
    {
        private User user;
        private ObservableCollection<ImageCell> imageCells;
        private Dictionary<int, int> keyValues;

        public event Action<User, Conversation> OpenDialogRequest;
        public event Action OpenSettingsViewRequest;
        
        private LocalData _localData;

        public MessagesView(User _user, LocalData localData)
        {
            Debug.WriteLine("[m] [MessagesView] Constructor running");

            InitializeComponent();

            IncludeDebug();

            menu.Update(_user.Name);
            menu.OpenSettingsViewRequest += () => OpenSettingsViewRequest();

            menu.SearchRequest += (string s) =>
            {
                Reload();

                List<ImageCell> toRemove = new List<ImageCell>();

                foreach (var item in imageCells)
                    if (!item.Text.Contains(s))
                        toRemove.Add(item);

                foreach (ImageCell item in toRemove)
                    imageCells.Remove(item);
            };

            listView.ItemTapped += (object sender, ItemTappedEventArgs e) =>
            {
                if (sender is ListView lv)
                {
                    int index = e.ItemIndex;
                    Conversation conversation = _localData.GetConversations().First(c => c.Id == keyValues[index]);
                    OpenDialogRequest(user, conversation);
                }

                (sender as ListView).SelectedItem = null;
            };

            Update(_user, localData);
        }

        private void IncludeDebug()
        {
            listView.ItemTapped += (object sender, ItemTappedEventArgs e) =>  Debug.WriteLine("[m] [MessagesView] ListView_ItemTapped running");
        }

        public void Update(User _user, LocalData localData)
        {
            Debug.WriteLine("[m] [MessagesView] Update running");

            user = _user;
            _localData = localData;
            keyValues = new Dictionary<int, int>();
            imageCells = new ObservableCollection<ImageCell>();

            menu.Update(_user.Name);

            Reload();
        }

        private void Reload()
        {
            Debug.WriteLine("[m] [MessagesView] Reload running");

            user = _localData.Update(user);

            //filter
            List<Conversation> filteredConversations = _localData.FindConversationsOfUser(user);
            filteredConversations = filteredConversations.Where(f => f.messages != null).ToList();
            filteredConversations = filteredConversations.Where(f => f.messages.Count != 0).ToList();
            int length = filteredConversations.Count;

            NoConversationsBt.IsVisible = length == 0;
            listView.IsVisible = length != 0;

            keyValues.Clear();
            imageCells.Clear();

            for (int i = 0; i < length; i++)
            {
                Conversation c = filteredConversations[i];
                var cell = CreateImageCell(c);
                imageCells.Add(cell);
                keyValues.Add(i, c.Id);
            }

            listView.ItemsSource = imageCells;
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