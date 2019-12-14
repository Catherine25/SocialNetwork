using SocialNetwork.Data;
using SocialNetwork.Data.Database;
using Xamarin.Forms;

namespace SocialNetwork
{
    public partial class App : Application
    {
        private LocalData _localData;

        public App()
        {
            InitializeComponent();

            _localData = new LocalData();

            MainPage = new MainPage(_localData);
        }
    }
}
