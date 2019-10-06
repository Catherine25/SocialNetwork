using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SocialNetwork.Services
{
    public static class GridExtensions
    {
        public static void SetSingleChild(this Grid grid, View view)
        {
            int count = grid.Children.Count;

            for (int i = 1; i < count; i++)
                grid.Children.RemoveAt(count - i);
            
            grid.Children.Add(view);
        }
    }
}
