using System.Collections.Generic;
using System.Diagnostics;
using SocialNetwork.Data;
using Xamarin.Forms;

namespace SocialNetwork.Services
{
    public static class ViewExtensions
    {
        public static void SetTheme(this View view, Theme theme)
        {
            //if(theme == null)
                //throw new System.Exception();

            Debug.WriteLine("SetTheme running");

            Stack<View> children = new Stack<View>();
            children.Push(view);

            while(children.Count != 0)
            {
                Debug.WriteLine("children.Count is " + children.Count.ToString());
                View view1 = children.Pop();
                ApplyTheme(view1, theme);
                IList<View> list = GetChildrenFromView(view1);
                foreach(View v in list)
                    children.Push(v);
            }
        }

        private static IList<View> GetChildrenFromView(View view) {

            Debug.WriteLine("GetChildrenFromView running on " + view.Id.ToString());

            switch (view)
            {
                case Grid grid:
                    return grid.Children;
                case StackLayout stackLayout:
                    return stackLayout.Children;
                case ContentView contentView:
                    return new List<View>() { contentView.Content };
                default:
                    return new List<View>();
            }
        }

        private static void ApplyTheme(View view, Theme theme) {

            Debug.WriteLine("ApplyTheme running on " + view.Id.ToString());

            view.BackgroundColor = theme.BackgroundColor;

            if (view is Button button)
                button.TextColor = theme.TextColor;
            else if(view is Entry entry) {
                entry.TextColor = theme.TextColor;
                entry.PlaceholderColor = theme.PlaceholderColor;
            }
            else if(view is Picker picker)
            {
                picker.TextColor = theme.TextColor;
                picker.TitleColor = theme.TitleColor;
            }
            else if(view is Label label)
                label.TextColor = theme.TextColor;
        }
    }
}