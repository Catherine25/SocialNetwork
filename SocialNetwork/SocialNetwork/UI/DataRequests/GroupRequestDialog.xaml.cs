using SocialNetwork.Data;
using SocialNetwork.UI.Editors;
using SocialNetwork.UI.Views;
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
        private string enterTitle = "Enter new group title";
        
        private RequestPurpose _purpose;
        private List<Group> _groups;

        public enum RequestPurpose { newGroupName }
        public event Action<Group, RequestPurpose> RequestCompleted;
        public event Action<GroupEditor.EditPurpose> ShowGroupEditorRequest;
        public event Action ShowGroupsViewRequest;

        public GroupRequestDialog(RequestPurpose purpose, List<Group> groups)
        {
            InitializeComponent();

            _purpose = purpose;
            _groups = groups;

            ConfirmBt.Clicked += ConfirmBt_Clicked;
            CancelBt.Clicked += CancelBt_Clicked;
            RegistrateBt.Clicked += RegistrateBt_Clicked;
            textEntry.Completed += TextEntry_Completed;

            if (purpose == RequestPurpose.newGroupName)
                infoLabel.Text = enterTitle;
        }

        private void RegistrateBt_Clicked(object sender, EventArgs e) =>
            ShowGroupEditorRequest(GroupEditor.EditPurpose.createNew);


        private void TextEntry_Completed(object sender, EventArgs e) =>
            Analyze();

        private void CancelBt_Clicked(object sender, EventArgs e)
        {
            if (_purpose == RequestPurpose.newGroupName)
                ShowGroupsViewRequest();
            else throw new NotImplementedException();
        }

        private void ConfirmBt_Clicked(object sender, EventArgs e) =>
            Analyze();

        private void Analyze()
        {
            Group group = _groups.Find(u => u.Title == textEntry.Text);

            if (group != null)
                RequestCompleted(group, _purpose);
            else
                RegistrateBt.IsVisible = true;
        }
    }
}