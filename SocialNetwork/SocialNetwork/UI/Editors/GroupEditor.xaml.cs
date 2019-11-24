using SocialNetwork.Data;
using SocialNetwork.Data.Database;
using System;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SocialNetwork.UI.Editors
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GroupEditor : ContentView
    {
        public enum EditPurpose { createNew }

        private string _noGroupAvatarLink = "https://www.indiannaturaloils.com/categories-images/no-photo.jpg";
        private string link;
        private EditPurpose _purpose;
        private LocalData _localData;

        public event Action EditorResult;

        public GroupEditor(EditPurpose purpose, LocalData localData)
        {
            InitializeComponent();

            _purpose = purpose;
            _localData = localData;

            ImagePreview.Clicked += ImagePreview_Clicked;
            CompleteBt.Clicked += CompleteBt_Clicked;
        }

        private void CompleteBt_Clicked(object sender, EventArgs e)
        {
            string title = TitleEntry.Text;
            string description = DescriptionEntry.Text;

            if (title == null || title == "")
                TitleEntry.TextColor = Color.Accent;
            else if (_purpose == EditPurpose.createNew && _localData.GetGroups().Any(u => u.Title == title))
                TitleEntry.TextColor = Color.Accent;
            else if (description == null || description == "")
            {
                TitleEntry.TextColor = Color.DarkCyan;
                DescriptionEntry.TextColor = Color.Accent;
            }
            else
            {
                TitleEntry.TextColor = Color.DarkCyan;
                DescriptionEntry.TextColor = Color.DarkCyan;

                Group group = new Group(0, title, description, link);

                if (_purpose == EditPurpose.createNew)
                {
                    _localData.AddNewGroup(group);
                    group.Id = _localData.GetGroups().Find(u => u.Title == group.Title).Id;
                }
                else throw new NotImplementedException();

                EditorResult();
            }
        }

        private void TrySetImage(string link)
        {
            try
            {
                ImagePreview.Source = ImageSource.FromUri(new Uri(link));
            }
            catch
            {
                ImagePreview.Source = _noGroupAvatarLink;
            }
        }

        private async void ImagePreview_Clicked(object sender, EventArgs e)
        {
            link = await Clipboard.GetTextAsync();

            TrySetImage(link);
        }
    }
}