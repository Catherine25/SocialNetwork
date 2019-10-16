using System;
using System.Collections.Generic;

using System.Text;
using Xamarin.Forms;

namespace SocialNetwork.Data
{
    public class Theme
    {
        public Theme(string name,
                     Color textColor,
                     Color backgroundColor,
                     Color separatorColor,
                     Color titleColor, 
                     Color placeholderColor)
        {
            Name = name;

            TextColor = textColor;
            BackgroundColor = backgroundColor;
            SeparatorColor = separatorColor;
            TitleColor = titleColor;
            PlaceholderColor = placeholderColor;
        }

        public string Name;
        public Color TextColor;
        public Color BackgroundColor;
        public Color SeparatorColor;
        public Color TitleColor;
        public Color PlaceholderColor;

    }
}
