using SocialNetwork.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SocialNetwork.UI.DataRequests
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GroupRequestDialog : ContentView
    {
        private string text;
        private RequestPurpose _purpose;
        private List<Group> _groups;

        public enum RequestPurpose { newGroupName }
        public event Action<Group, RequestPurpose> RequestCompleted;

        public GroupRequestDialog(RequestPurpose purpose, List<Group> groups)
        {
            InitializeComponent();

            _purpose = purpose;
            _groups = groups;

            ConfirmBt.Clicked += ConfirmBt_Clicked;
            CancelBt.Clicked += CancelBt_Clicked;
            textEntry.Unfocused += TextEntry_Unfocused;
            textEntry.Completed += TextEntry_Completed;

            if (purpose == RequestPurpose.newGroupName)
                infoLabel.Text = "Enter new group name";
        }

        private void TextEntry_Completed(object sender, EventArgs e)
        {
            text = textEntry.Text;
            Group group = _groups.Find(g => g.Title == text);
            if (group != null)
                RequestCompleted(group, _purpose);
        }

        private void CancelBt_Clicked(object sender, EventArgs e) =>
            RequestCompleted(null, _purpose);

        private void ConfirmBt_Clicked(object sender, EventArgs e)
        {
            Group group = _groups.Find(u => u.Title == text);
            if (group != null)
                RequestCompleted(group, _purpose);
        }

        private void TextEntry_Unfocused(object sender, FocusEventArgs e) =>
            text = textEntry.Text;
    }
}