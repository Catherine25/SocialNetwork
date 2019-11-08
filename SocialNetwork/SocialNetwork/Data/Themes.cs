using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using Xamarin.Forms;
using static Xamarin.Forms.Color;

namespace SocialNetwork.Data {
    public class Themes {

        private Stack<string> ThemeLinks = new Stack<string>();
        public List<Theme> ThemesList = new List<Theme>
        {
            new Theme(
                "Default",
                AliceBlue,
                CadetBlue,
                CornflowerBlue,
                DeepSkyBlue,
                DodgerBlue)
        };
        public Theme CurrentTheme;
        public bool IsPageLoadCompleted = false;
        public bool IsAllDownloaded = false;
        public event Action<Theme> ThemeLoaded;

        public Themes() => CurrentTheme = ThemesList.Find(t => t.Name == "Default");

        private void LoadTheme(object o)
        {
            Debug.WriteLine("LoadTheme() running");

            WebRequest webRequest = WebRequest.Create(ThemeLinks.Pop());
            WebResponse webResponse = webRequest.GetResponse();

            Stream data = webResponse.GetResponseStream();

            string html;
            using (StreamReader streamReader = new StreamReader(data))
            {
                html = streamReader.ReadToEnd();
            }

            ////<div data-clipboard-text="#ecfde6">
            Regex regex = new Regex(@"<div data-clipboard-text=.#.......>");
            MatchCollection matchCollection = regex.Matches(html);

            if(matchCollection.Count != 0)
            {
                Regex colorHex = new Regex(@"#......");

                Color color0 = FromHex(colorHex.Match(matchCollection[0].Value).Value);
                Color color1 = FromHex(colorHex.Match(matchCollection[1].Value).Value);
                Color color2 = FromHex(colorHex.Match(matchCollection[2].Value).Value);
                Color color3 = FromHex(colorHex.Match(matchCollection[3].Value).Value);
                Color color4 = FromHex(colorHex.Match(matchCollection[4].Value).Value);

                Theme theme = new Theme(ThemesList.Count.ToString(), color0, color1, color2, color3, color4);
                ThemesList.Add(theme);
                ThemeLoaded(theme);
            }
        }

        public void LoadRomanukeThemes(object o)
        {
            Debug.WriteLine("LoadRomanukeThemes() running");
            
            WebRequest webRequest = WebRequest.Create("https://colorpalettes.net");
            WebResponse webResponse = webRequest.GetResponse();
            
            Stream data = webResponse.GetResponseStream();
            
            string html;
            using(StreamReader streamReader = new StreamReader(data))
            {
               html = streamReader.ReadToEnd();
            }

            Regex regex = new Regex(@"https://colorpalettes.net/color-palette-\d{4}/");
            MatchCollection matchCollection = regex.Matches(html);
            
            foreach (var m in matchCollection)
            {
                string mstring = m.ToString();

                if(!ThemeLinks.Contains(mstring))
                    ThemeLinks.Push(mstring);
            }

            while(ThemeLinks.Count != 0)
            {
                new Thread(LoadTheme).Start();
            }
        }
    }
}