using SocialNetwork.Data;
using SocialNetwork.Data.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SocialNetwork.UI.Editors
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserEditor : ContentView
    {
        public enum EditPurpose { createNew, update }

        private string NoUserAvatarLink = "https://www.indiannaturaloils.com/categories-images/no-photo.jpg";
        private LocalData _localData;
        private string link;
        private EditPurpose _purpose;
        private User oldUser;

        public event Action EditorResult;

        public UserEditor(EditPurpose purpose, LocalData localData, User user)
        {
            InitializeComponent();

            _purpose = purpose;
            _localData = localData;
            oldUser = user;

            TrySetImage(oldUser.AvatarLink);
            NameEntry.Text = oldUser.Name;
            BioEntry.Text = oldUser.Bio;

            ImagePreview.Clicked += Image_Clicked;
            CompleteBt.Clicked += UserCompleted;
        }

        public UserEditor(EditPurpose purpose, LocalData localData)
        {
            InitializeComponent();

            _purpose = purpose;
            _localData = localData;

            ImagePreview.Clicked += Image_Clicked;
            CompleteBt.Clicked += UserCompleted;
        }

        private void UserCompleted(object sender, EventArgs e)
        {
            string name = NameEntry.Text;
            string bio = BioEntry.Text;

            if (name == null || name == "")
            {
                NameEntry.TextColor = Color.Accent;
            }
            else if (_purpose == EditPurpose.createNew && _localData.GetUsers().Any(u => u.Name == name))
            {
                NameEntry.TextColor = Color.Accent;
            }
            else if(bio == null || bio == "")
            {
                NameEntry.TextColor = Color.DarkCyan;
                BioEntry.TextColor = Color.Accent;
            }
            else
            {
                NameEntry.TextColor = Color.DarkCyan;
                BioEntry.TextColor = Color.DarkCyan;

                User user = new User(0, link, name, bio);

                if (_purpose == EditPurpose.createNew)
                {
                    _localData.AddNewUser(user);
                    user.Id = _localData.GetUsers().Find(u => u.Name == user.Name).Id;
                }
                else if(_purpose == EditPurpose.update)
                {
                    _localData.UpdateUser(oldUser, user);
                    user = _localData.GetUsers().Find(u => u.Id == oldUser.Id);
                }
                else throw new NotImplementedException();
                
                EditorResult();
            }
        }

        private async void Image_Clicked(object sender, EventArgs e)
        {
            link = await Clipboard.GetTextAsync();

            TrySetImage(link);
        }

        private void TrySetImage(string link)
        {
            try
            {
                ImagePreview.Source = ImageSource.FromUri(new Uri(link));
            }
            catch
            {
                ImagePreview.Source = NoUserAvatarLink;
            }
        }
    }
}