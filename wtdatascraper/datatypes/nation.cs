using System.Text.RegularExpressions;
using HtmlAgilityPack;
using System.Web;

namespace WarThunder
{
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

        public void RemoveGroundVehicle(GroundVehicle gv) {
            GroundVehicles.Remove(gv);
        }

        public void RemoveGroundVehicle(List<GroundVehicle> gvs) {
            foreach (GroundVehicle gv in gvs) {
                RemoveGroundVehicle(gv);
            }
        }

        public string GetName(){ 
            return name;
        }

        private void getVehicles() {
            GroundVehicles = new List<GroundVehicle>();
            ground_tree = utils.Scraper.GetDocument($"https://wiki.warthunder.com/Category:{name}_ground_vehicles");
            foreach(Match match in groundVehiclePattern.Matches(ground_tree.Text)) {
                try {
                    AddGroundVehicle(
                        new GroundVehicle(
                            HttpUtility.HtmlDecode(match.Groups[3].Value), // name
                            "https://wiki.warthunder.com/"+match.Groups[2].Value, // link/url
                            this.name, // nation
                            match.Groups[1] is not null && match.Groups[1].Value is not null && match.Groups[1].Value == "  " // foldered
                        )
                    );
                } catch (Exception e) {
                    Console.Error.WriteLine($"Failed to parse page https://wiki.warthunder.com/{match.Groups[2].Value}", e);

                }
            }
        }

        public List<GroundVehicle> GetVehicleInfo() {
            List<WarThunder.GroundVehicle> removed = new List<WarThunder.GroundVehicle>();
            List<Task> tasks = new List<Task>();
            foreach(WarThunder.GroundVehicle vehicle in this.GetGroundVehicles()) {
                Task thread = new Task(vehicle.GetInfoFromPage);
                tasks.Add(thread);
                thread.Start();
            }
            for (int i = 0; i < tasks.Count; i++) {
                Task thread = tasks.ElementAt(i);
                GroundVehicle vehicle = this.GetGroundVehicles().ElementAt(i);
                try {
                    thread.Wait();
                    // Console.WriteLine(vehicle);
                } catch (Exception e) {
                    bool acquired = false;
                    for (int j = 0; j < 3; j++) {
                        try {
                            vehicle.GetInfoFromPage();
                            acquired = true;
                            break;
                        } catch (Exception) {}
                    }
                    if (!acquired) {
                        Console.WriteLine($"{vehicle.GetName()} failed to be acquired and will be removed from the dataset. Investigate at the following link: ({vehicle.GetURL()})\n{e}\n");
                        removed.Add(vehicle);
                    }
                }
            }
            this.RemoveGroundVehicle(removed);
            return removed;
        }

        public override string ToString() {
            return name;
        }
    }
}