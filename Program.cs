﻿using System.Text.RegularExpressions;
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
            // nations.Add(new WarThunder.Nation("USA"));
            foreach(WarThunder.Nation nation in nations) {
                foreach(WarThunder.GroundVehicle vehicle in nation.GetGroundVehicles()) {
                    vehicle.GetInfoFromPage();
                    Console.WriteLine(vehicle);
                }
            }
            List<WarThunder.GroundVehicle> vehicles = nations.SelectMany(x => x.GroundVehicles).ToList();
            CSV.writeToCsv(vehicles, "out.csv");
        }
    }
}