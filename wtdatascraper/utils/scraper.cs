using HtmlAgilityPack;

namespace utils
{

    public class Scraper {
        static HtmlWeb webBrowser = new HtmlWeb();
        
        public static HtmlDocument GetDocument(string url) {
            Console.WriteLine($"Downloading page: {url}\n");
            HtmlDocument doc = webBrowser.Load(url);
            return doc;
        }
    }
}