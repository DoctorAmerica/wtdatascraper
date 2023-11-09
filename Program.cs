using System.Text.RegularExpressions;
using HtmlAgilityPack;
using utils;

namespace Program {

    class WTWikiDataScraper {

        static string link = "https://wiki.warthunder.com/Ground_vehicles";

        static void Main(string[] args) {
            HtmlDocument ground_vehicles = Scraper.GetDocument(link);
            
            List<WarThunder.Nation> nations = new List<WarThunder.Nation>();
            Regex groundNationPattern = new Regex("<a href=\"\\/Category:(.*)_ground_vehicles\" title=\"Category:\\1 ground vehicles\">[a-zA-Z]* ?\\1<\\/a>", RegexOptions.Compiled);
            foreach(Match match in groundNationPattern.Matches(ground_vehicles.Text)) {
                WarThunder.Nation nation = new WarThunder.Nation(match.Groups[1].Value);
                nations.Add(nation);
            }
            // nations.Add(new WarThunder.Nation("Italy"));
            foreach(WarThunder.Nation nation in nations) {
                List<WarThunder.GroundVehicle> removed = new List<WarThunder.GroundVehicle>();
                foreach(WarThunder.GroundVehicle vehicle in nation.GetGroundVehicles()) {
                    try {
                        vehicle.GetInfoFromPage();
                        Console.WriteLine(vehicle);
                    } catch (Exception e) {
                        Console.WriteLine($"{vehicle.GetName()} failed to be acquired and will be removed from the dataset. Investigate at the following link: ({vehicle.GetURL()})\n{e}\n");
                        removed.Add(vehicle);
                    }
                }
                nation.RemoveGroundVehicle(removed);
            }
            List<WarThunder.GroundVehicle> vehicles = nations.SelectMany(x => x.GroundVehicles).ToList();
            CSV.writeToCsv(vehicles, "out.csv");
        }
    }
}