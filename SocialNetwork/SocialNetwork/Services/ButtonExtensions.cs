using SocialNetwork.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SocialNetwork.Services
{
    public static class ButtonExtensions
    {
        public static void SetTheme(this Button button, Theme theme)
        {
            button.TextColor = theme.TextColor;
            button.BackgroundColor = theme.BackgroundColor;
        }
    }
}
