using SocialNetwork.Data;
using Xamarin.Forms;

namespace SocialNetwork.Services
{
    public static class EntryExtensions
    {
        public static void SetTheme(this Entry entry, Theme theme)
        {
            entry.BackgroundColor = theme.BackgroundColor;
            entry.PlaceholderColor = theme.SeparatorColor;
            entry.TextColor = theme.TextColor;
        }
    }
}