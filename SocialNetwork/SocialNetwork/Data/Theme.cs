using System;
using System.Collections.Generic;

using System.Text;
using Xamarin.Forms;

namespace SocialNetwork.Data
{
    public class Theme
    {
        public Theme(string name, Color[] colors)
        {
            Name = name;
            this.colors = colors;
        }

        public string Name;
        public Color[] colors;
    }
}
