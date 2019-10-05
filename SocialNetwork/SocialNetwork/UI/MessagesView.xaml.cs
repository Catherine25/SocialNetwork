using SocialNetwork.Data;
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
            List<Message> messages = Messages.GetMessagesByUser(user).ToList();
            
            int length = messages.Count;
            if (length > 10)
                length = 10;

            for (int i = 0; i < length; i++)
                SetTextOnButton(i, messages[i]);

            for (int i = 0; i < 10; i++)
            {
                View a = messagesGrid.Children.First(x => x.Id == guids[i]);
                Button b = a as Button;
                if (b.Text == "" || b.Text == null)
                    b.IsVisible = false;
                else b.IsVisible = true;
            }
        }

        private void SetTextOnButton(int index, Message message)
        {
            string text = message.Sender.Name + ":" + "\n" + message.Text;

            Guid id = guids[index];

            View view = messagesGrid.Children.First(x => x.Id == id);

            Button button = view as Button;

            if (text == "" || text == null)
                button.IsVisible = false;
            else
                button.Text = text;
        }
    }
}