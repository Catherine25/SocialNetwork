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
    public partial class MessagesView : ContentView
    {
        private User user;
        private Guid[] guids;

        public MessagesView(User _user)
        {
            InitializeComponent();

            user = _user;

            guids = new Guid[10];

            guids[0] = buttonRow0.Id;
            guids[1] = buttonRow1.Id;
            guids[2] = buttonRow2.Id;
            guids[3] = buttonRow3.Id;
            guids[4] = buttonRow4.Id;
            guids[5] = buttonRow5.Id;
            guids[6] = buttonRow6.Id;
            guids[7] = buttonRow7.Id;
            guids[8] = buttonRow8.Id;
            guids[9] = buttonRow9.Id;

            Reload();
        }

        private void Reload()
        {
            //USE ONLY MESSAGES WHERE CURRENT USER IS AUTHOR OR RECIEVER
            List<Conversation> conversations = Conversations.GetConversationsByUser(user).ToList();

            int length = conversations.Count;
            if(length == 0)
            {
                Label label = new Label
                {
                    Text = "No Conversations"
                };
                messagesGrid.SetSingleChild(label);
            }
            else
            {
                if (length > 10)
                    length = 10;

                for (int i = 0; i < length; i++)
                    SetTextOnButton(i, conversations[i]);

                for (int i = 0; i < 10; i++)
                {
                    View a = messagesGrid.Children.First(x => x.Id == guids[i]);
                    Button b = a as Button;
                    if (b.Text == "" || b.Text == null)
                        b.IsVisible = false;
                    else b.IsVisible = true;
                }
            }
        }

        private void SetTextOnButton(int index, Conversation conversation)
        {
            Message message = conversation.messages[conversation.messages.Count - 1];
            string text = (message.isFromMember1 ? conversation.member1.Name : conversation.member2.Name) + ": " + message.Text;
            Guid id = guids[index];

            View view = messagesGrid.Children.First(x => x.Id == id);

            Button button = view as Button;
            
            button.Text = text;
        }
    }
}