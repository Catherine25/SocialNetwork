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
    public partial class UserView : ContentView
    {
        public UserView(User user)
        {
            InitializeComponent();

            image = user.Avatar;
            name.Text = user.Name;
            bio.Text = user.Bio;
        }
    }
}