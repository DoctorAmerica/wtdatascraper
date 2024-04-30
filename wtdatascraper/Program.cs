using System.Text.RegularExpressions;
using HtmlAgilityPack;
using utils;
using WarThunder;

namespace Program
{

    class WTWikiDataScraper {

        static string link = "https://wiki.warthunder.com/Ground_vehicles";

        static void Main(string[] args) {
            HtmlDocument ground_vehicles = Scraper.GetDocument(link);
            
            List<WarThunder.Nation> nations = new List<WarThunder.Nation>();
            List<WarThunder.GroundVehicle> allRemoved = new List<WarThunder.GroundVehicle>();
            Regex groundNationPattern = new Regex("<a href=\"\\/Category:(.*)_ground_vehicles\" title=\"Category:\\1 ground vehicles\">[a-zA-Z]* ?\\1<\\/a>", RegexOptions.Compiled);
            foreach(Match match in groundNationPattern.Matches(ground_vehicles.Text)) {
                WarThunder.Nation nation = new WarThunder.Nation(match.Groups[1].Value);
                nations.Add(nation);
            }
            // nations.Add(new WarThunder.Nation("Italy"));
            foreach(WarThunder.Nation nation in nations) {
                allRemoved.AddRange(nation.GetVehicleInfo());
            }

            if(allRemoved.Count() > 0) {
                Console.WriteLine("Removed the following vehicles:");
                foreach(GroundVehicle removedV in allRemoved) {
                    Console.WriteLine(removedV.GetURL());
                }
            }
            List<WarThunder.GroundVehicle> vehicles = nations.SelectMany(x => x.GroundVehicles).ToList();
            CSV.writeToCsv(vehicles, "out.csv");
        }
    }
}