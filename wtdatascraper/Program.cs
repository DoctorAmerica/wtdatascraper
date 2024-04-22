using System.Text.RegularExpressions;
using HtmlAgilityPack;
using utils;
using WarThunder;

namespace Program {

    class WTWikiDataScraper {

        static string link = "https://wiki.warthunder.com/Ground_vehicles";
        static string replay = "test_files/";

        static void Main(string[] args) {
            // HtmlDocument ground_vehicles = Scraper.GetDocument(link);
            
            // List<WarThunder.Nation> nations = new List<WarThunder.Nation>();
            // List<WarThunder.GroundVehicle> allRemoved = new List<WarThunder.GroundVehicle>();
            // Regex groundNationPattern = new Regex("<a href=\"\\/Category:(.*)_ground_vehicles\" title=\"Category:\\1 ground vehicles\">[a-zA-Z]* ?\\1<\\/a>", RegexOptions.Compiled);
            // foreach(Match match in groundNationPattern.Matches(ground_vehicles.Text)) {
            //     WarThunder.Nation nation = new WarThunder.Nation(match.Groups[1].Value);
            //     nations.Add(nation);
            // }
            // // nations.Add(new WarThunder.Nation("Italy"));
            // foreach(WarThunder.Nation nation in nations) {
            //     List<WarThunder.GroundVehicle> removed = new List<WarThunder.GroundVehicle>();
            //     foreach(WarThunder.GroundVehicle vehicle in nation.GetGroundVehicles()) {
            //         try {
            //             vehicle.GetInfoFromPage();
            //             Console.WriteLine(vehicle);
            //         } catch (Exception e) {
            //             Console.WriteLine($"{vehicle.GetName()} failed to be acquired and will be removed from the dataset. Investigate at the following link: ({vehicle.GetURL()})\n{e}\n");
            //             removed.Add(vehicle);
            //             allRemoved.Add(vehicle);
            //         }
            //     }
            //     nation.RemoveGroundVehicle(removed);
            // }

            // if(allRemoved.Count() > 0) {
            //     Console.WriteLine("Removed the following vehicles:");
            //     foreach(GroundVehicle removedV in allRemoved) {
            //         Console.WriteLine(removedV.GetURL());
            //     }
            // }
            // List<WarThunder.GroundVehicle> vehicles = nations.SelectMany(x => x.GroundVehicles).ToList();
            // CSV.writeToCsv(vehicles, "out.csv");

            List<string> strings = new List<string>();
            byte[] replayBytes = File.ReadAllBytes($"{replay}/0000.wrpl");
            for (int i = 0; i < replayBytes.Length; i++) {
                char b = (char) replayBytes[i];
                if (isLetter(b)) {
                    string s = "";
                    while(isLetter(b)) {
                        b = (char) replayBytes[i];
                        s += b;
                        i++;
                        if (i >= replayBytes.Length)
                            break;
                    }
                    strings.Add(s);
                }
            }
            Console.WriteLine(string.Join("\n", strings));
        }

        static bool isLetter(char c) {
            var rgx = new Regex(@"[a-zA-Z/_\-]", RegexOptions.Compiled);
            return rgx.IsMatch($@"{c}");
        }
    }
}