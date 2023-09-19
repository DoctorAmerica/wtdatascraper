using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace WarThunder {
    class Nation {
        public List<GroundVehicle> GroundVehicles;
        private string name;
        private HtmlDocument ground_tree;
        private static Regex groundVehiclePattern = new Regex("(  ){0,1}<div class=\"tree-item\">.*href=\"\\/([^\"]*)\" title=\"([^\"]*)\"[^\\r\\n]*", RegexOptions.Compiled);

        public Nation (string name) {
            this.name = name;
            getVehicles();
        }

        public List<GroundVehicle> GetGroundVehicles() {
            return GroundVehicles;
        }

        public void AddGroundVehicle(GroundVehicle gv) {
            GroundVehicles.Add(gv);
        }

        public string GetName(){ 
            return name;
        }

        private void getVehicles() {
            GroundVehicles = new List<GroundVehicle>();
            ground_tree = utils.Scraper.GetDocument($"https://wiki.warthunder.com/Category:{name}_ground_vehicles");
            foreach(Match match in groundVehiclePattern.Matches(ground_tree.Text)) {
                AddGroundVehicle(
                    new GroundVehicle(
                        match.Groups[3].Value, // name
                        "https://wiki.warthunder.com/"+match.Groups[2].Value, // link/url
                        this.name, // nation
                        (match.Groups[1] is not null && match.Groups[1].Value is not null && match.Groups[1].Value == "  ") // foldered
                    )
                );
            }
        }

        public override string ToString() {
            return name;
        }
    }
}