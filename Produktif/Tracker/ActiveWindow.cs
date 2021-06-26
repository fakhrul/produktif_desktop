using System;

namespace Produktif
{
    public class ActiveWindow
    {
        public ActiveWindow(string process, string title, string className, string url,string data)
        {
            Process = process;
            Title = title;
            ClassName = className;
            CheckDateTime = DateTime.Now;
            Url = url;
            Text = data;
        }
        public string Process { get; set; }
        public string Title { get; set; }
        public string ClassName { get; set; }
        public string Text { get; set; }
        public string Url { get; set; }

        public DateTime CheckDateTime { get; set; }

        public override string ToString()
        {
            return string.Format("{0}$$$!!!{1}", Process, Title);
        }
    }
}
