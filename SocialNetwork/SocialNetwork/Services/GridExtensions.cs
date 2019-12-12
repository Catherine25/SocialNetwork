using Xamarin.Forms;

namespace SocialNetwork.Services
{
    public static class GridExtensions
    {
        public static void SetSingleChild(this Grid grid, View view)
        {
            grid.Children.Clear();
            grid.Children.Add(view);
        }
    }
}
