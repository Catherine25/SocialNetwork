using System.Collections.Generic;
using System.Linq;

namespace SocialNetwork.Data.Database
{   
    public class LocalData
    {
        string connectionString;
        public LocalData()
        {
            string server = "35.180.63.12";
            string database = "Kate";
            string uid = "Admin";
            string password = "3555serg";

            connectionString =
                "SERVER=" + server +
                "; PORT = 3306 ;" +
                "DATABASE=" + database + ";" +
                "UID=" + uid + ";" +
                "PASSWORD=" + password + ";";

            _loader = new SQLLoader(connectionString);
            _publisher = new Publisher(connectionString);
        }

        private SQLLoader _loader;
        private Publisher _publisher;
        private List<Conversation> _conversations;
        
        
        #region Collections

        public List<User> GetUsers() => _loader.LoadUsers();
        public List<Conversation> GetConversations()
        {
            ConvertIntoLocalClasses();
            return _conversations;
        }
        public List<ConversationData> GetConversationsData() => _loader.LoadConversationsData();
        public List<MessageData> GetMessagesData() => _loader.LoadMessagesData();

        #endregion

        #region Single Update Request

        public void AddEmptyConversation(Conversation newC)
        {
            var conversations = GetConversations();
            if (conversations.Any(c =>
                (c.member1.Id == newC.member1.Id && c.member2.Id == newC.member2.Id) ||
                (c.member1.Id == newC.member2.Id && c.member2.Id == newC.member1.Id)))
                return;

            _publisher.PublishConversation(newC);
        }

        public void AddNewMessage(Message message, Conversation conversation)
        {
            message.Text = message.Text.Replace("'", "");
            _publisher.PublishMessage(conversation, message);
        }

        public void AddNewUser(User user) =>
            _publisher.PublishUser(user);
        public void UpdateUser(User oldUser, User user) =>
            _publisher.UpdateUser(oldUser, user);

        #endregion

        public User Update(User user) =>
            GetUsers().First(u => u.Name == user.Name);


        public List<Conversation> FindConversationsOfUser(User user) =>
            GetConversations().Where(c => c.member1.Id == user.Id || c.member2.Id == user.Id).ToList();

        public void ConvertIntoLocalClasses()
        {
            var conversations = new List<Conversation>();
            var conversationsData = GetConversationsData();
            var messagesData = GetMessagesData();
            var users = GetUsers();

            foreach (ConversationData cd in conversationsData)
            {
                Conversation conversation = new Conversation (
                    cd.c_id,
                    users.Find(c=>c.Id == cd.u1_id),
                    users.Find(c=>c.Id == cd.u2_id));
                conversations.Add(conversation);
            }

            foreach (MessageData md in messagesData)
            {
                Message message = new Message(md.m_id, md.text, md.dt, md.isFromMember1);
                Conversation existingConversation = conversations.Find(c => c.Id == md.c_id);
                List<Message> existingMessages = existingConversation.messages;
                
                if(existingMessages != null)
                    existingMessages.Add(message);
                else
                    existingMessages = new List<Message> { message };

                existingConversation.messages = existingMessages;
            }

            _conversations = conversations;
        }

    }
}
