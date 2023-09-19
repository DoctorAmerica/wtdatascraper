using System;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using CsvHelper;
using utils;

namespace WarThunder {
    public class GroundVehicle {
        // Tree info
        string name;
        string url;
        string nation;
        bool foldered;
        bool additionalInfo;
        // Additional data
        int rank;
        bool premium;
        string role;
        bool squadron;
        float[] br = new float[3]; // AB/RB/SB
        float[] forwardSpeed = new float[2]; // AB/RB+SB
        float[] reverseSpeed = new float[2]; // AB/RB+SB
        float weight;
        float[] enginePowerStock = new float[2]; // AB/RB+SB
        float[] enginePowerUpgraded = new float[2]; // AB/RB+SB
        float[] pwrWtStock = new float[2]; // AB/RB+SB
        float[] pwrWtUpgraded = new float[2]; // AB/RB+SB
        float[] repairCost = new float[3];  // AB/RB/SB
        string[] features;
        string mainArmament;
        

        public GroundVehicle(string name, string url, string nation, bool foldered) {
            this.name = name;
            this.url = url;
            this.nation = nation;
            this.foldered = foldered;
            this.additionalInfo = false;
        }

        public string GetName() {
            return name;
        }

        public string GetURL() {
            return url;
        }

        public string GetNation() {
            return nation;
        }

        public bool IsFoldered() {
            return foldered;
        }

        public bool HasAdditionalInfo() {
            return additionalInfo;
        }

        public void GetInfoFromPage() {
            HtmlDocument page = Scraper.GetDocument(url);

            //rank
            this.rank = Conversions.RomanToInteger(
                CompReg.rankPtrn.Match(page.Text).Groups[2].Value);

            //premium
            this.premium = CompReg.premiumPtrn.Match(page.Text).Success;

            //role
            this.role = CompReg.rolePtrn.Match(page.Text).Groups[3].Value;

            //squadron
            this.squadron = CompReg.squadronPtrn.Match(page.Text).Success;

            //BR
            Match BRs = CompReg.brPtrn.Match(page.Text);
            this.br[0] = float.Parse(BRs.Groups[1].Value);
            this.br[1] = float.Parse(BRs.Groups[2].Value);
            this.br[2] = float.Parse(BRs.Groups[3].Value);
            //Specs

            //Mobility
            Match mobility = CompReg.mobilityPtrn.Match(page.Text);
            //Speed
            //AB
            this.forwardSpeed[0] = float.Parse(mobility.Groups[2].Value);
            this.reverseSpeed[0] = float.Parse(mobility.Groups[3].Value);
            //RB/SB
            this.forwardSpeed[1] = float.Parse(mobility.Groups[13].Value);
            this.reverseSpeed[1] = float.Parse(mobility.Groups[14].Value);
            //Weight
            this.weight = float.Parse(mobility.Groups[4].Value);
            //Power
            //AB
            try {this.enginePowerStock[0] = float.Parse(mobility.Groups[7].Value);}
            catch(FormatException e) {this.enginePowerStock[0] = -1;}
            this.enginePowerUpgraded[0] = float.Parse(mobility.Groups[8].Value);
            //RB/SB
            try {this.enginePowerStock[1] = float.Parse(mobility.Groups[15].Value);}
            catch(FormatException e) {this.enginePowerStock[1] = -1;}
            this.enginePowerUpgraded[1] = float.Parse(mobility.Groups[16].Value);
            //Pwr/Wt
            //AB
            try{this.pwrWtStock[0] = float.Parse(mobility.Groups[9].Value);}
            catch(FormatException e) {this.pwrWtStock[0] = -1;}
            this.pwrWtUpgraded[0] = float.Parse(mobility.Groups[10].Value);
            //RB/SB
            try{this.pwrWtStock[1] = float.Parse(mobility.Groups[17].Value);}
            catch(FormatException e) {this.pwrWtStock[1] = -1;}
            this.pwrWtUpgraded[1] = float.Parse(mobility.Groups[18].Value);

            //Repair Cost
            MatchCollection repairs = CompReg.repairPtrn.Matches(page.Text);
            if(repairs.Count < 3) {
                this.repairCost[0] = this.repairCost[1] = this.repairCost[2] = 0;
            } else {
                this.repairCost[0] = float.Parse(repairs[0].Groups[2].Value.Replace(" ",""));
                this.repairCost[1] = float.Parse(repairs[1].Groups[2].Value.Replace(" ",""));
                this.repairCost[2] = float.Parse(repairs[2].Groups[2].Value.Replace(" ",""));
            }
            
            //Purchase Price
            //Crew Train
            //Research Points

            //Features
            MatchCollection features = CompReg.featuresPtrn.Matches(page.Text);
            this.features = features.Cast<Match>().Select(m => m.Groups[1].Value).ToArray();

            //Modifications
            //Main Armament
            Match mainArm = CompReg.mainArmamentPtrn.Match(page.Text);
            this.mainArmament = (String.Empty.Equals(mainArm.Groups[1].Value)) ? 
                                mainArm.Groups[2].Value :
                                mainArm.Groups[1] + " " + mainArm.Groups[2];

            this.mainArmament = this.mainArmament.Replace("_"," ");
            this.mainArmament = RegFunc.Replace(this.mainArmament, CompReg.diameterPtrn, CompReg.diameterSub);
            this.mainArmament = RegFunc.Replace(this.mainArmament, CompReg.multipleGunsPtrn, CompReg.multiGunSub);

            //Machine Gun(s)
            //Additional Armament
            //Largest Calibre Gun
            //Ammunition Types
            //Ammunition amounts
            //Highest Penetration Round
            //"Best" Ammunition Type
            //Reload speed
            //Guidance
            //Hull Armor
            //Turret Armor
            //...
            
            additionalInfo = true;
        }

        public override string ToString() {
            if(additionalInfo) {
                return $"Name: {name}\n"+
                       $"URL: {url}\nNation: {nation}\n"+
                       $"Foldered: {foldered}\n"+
                       $"Rank: {rank}\n"+
                       $"Premium: {premium}\n"+
                       $"Role: {role}\n"+
                       $"Squadron: {squadron}\n"+
                       $"Battle Rating(s):\n"+
                       $"\tAB: {br[0]}\n"+
                       $"\tRB: {br[1]}\n"+
                       $"\tSB: {br[2]}\n"+
                       $"Speed (Forward/Reverse):\n"+
                       $"\t   AB: {forwardSpeed[0]}/{reverseSpeed[0]} km/h\n"+
                       $"\tRB+SB: {forwardSpeed[1]}/{reverseSpeed[1]} km/h\n"+
                       $"Weight: {weight} tons\n"+
                       $"Engine power (Stock/Upgraded):\n"+
                       $"\tAB   : {enginePowerStock[0]}/{enginePowerUpgraded[0]} hp\n"+
                       $"\tRB+SB: {enginePowerStock[1]}/{enginePowerUpgraded[1]} hp\n"+
                       $"Power/Weight ratio (Stock/Upgraded):\n"+
                       $"\tAB   : {pwrWtStock[0]}/{pwrWtUpgraded[0]} hp/ton\n"+
                       $"\tRB+SB: {pwrWtStock[1]}/{pwrWtUpgraded[1]} hp/ton\n"+
                       $"Repair cost:\n"+
                       $"\tAB: {repairCost[0]} SL\n"+
                       $"\tRB: {repairCost[1]} SL\n"+
                       $"\tSB: {repairCost[2]} SL\n"+
                       $"Features: {string.Join(", ", features)}\n"+
                       $"Main Armament: {mainArmament}\n";
            } else {
                return $"Name: {name}\nURL: {url}\nNation: {nation}\nFoldered: {foldered}\n";
            }
        }
    }

    // public class GroundVehicleMap : CsvHelper.Configuration.ClassMap<GroundVehicle> {
    //     public GroundVehicleMap() {
    //         Map(m => m.Name).Index(0).Name("name");
    //         Map(m => m.Nation).Index(1).Name("nation");
    //         Map(m => m.foldered).Index(2).Name("foldered");
    //         Map(m => m.rank).Index(3).Name("rank");
    //         Map(m => m.role).Index(4).Name("role");
    //         Map(m => m.br[1]).Index(5).Name("br");
    //         Map(m => m.mainArmament).Index(6).Name("mainArm");
    //     }
    // }
}
