using System;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using CsvHelper;
using utils;

namespace WarThunder {
    public class GroundVehicle {
        // Tree info
        protected string name;
        string url;
        protected string nation;
        protected bool foldered;
        bool additionalInfo;
        // Additional data
        protected int rank;
        protected string role;
        protected float[] br = new float[3]; // AB/RB/SB
        float[] forwardSpeed = new float[2]; // AB/RB+SB
        float[] reverseSpeed = new float[2]; // AB/RB+SB
        float weight;
        float[] enginePowerStock = new float[2]; // AB/RB+SB
        float[] enginePowerUpgraded = new float[2]; // AB/RB+SB
        float[] pwrWtStock = new float[2]; // AB/RB+SB
        float[] pwrWtUpgraded = new float[2]; // AB/RB+SB
        protected float[] repairCost = new float[3];  // AB/RB/SB
        protected string[] features;
        protected string mainArmament;
        protected string purchaseType;
        

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
            if (CompReg.premiumPtrn.Match(page.Text).Success) {
                if(CompReg.marketPtrn.Match(page.Text).Success) {
                    this.purchaseType = "Market Premium";
                } else if (CompReg.packPtrn.Match(page.Text).Success) {
                    this.purchaseType = "Pack Premium";
                } else {
                    this.purchaseType = "Gift Premium";
                }
            } else if(CompReg.squadronPtrn.Match(page.Text).Success) {
                this.purchaseType = "Squadron";
            } else if(CompReg.giftPtrn.Match(page.Text).Success) {
                this.purchaseType = "Gift";
            } else {
                this.purchaseType = "Tech Tree";
            }


            //role
            this.role = CompReg.rolePtrn.Match(page.Text).Groups[3].Value;

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
            catch(FormatException) {this.enginePowerStock[0] = -1;}
            this.enginePowerUpgraded[0] = float.Parse(mobility.Groups[8].Value);
            //RB/SB
            try {this.enginePowerStock[1] = float.Parse(mobility.Groups[15].Value);}
            catch(FormatException) {this.enginePowerStock[1] = -1;}
            this.enginePowerUpgraded[1] = float.Parse(mobility.Groups[16].Value);
            //Pwr/Wt
            //AB
            try{this.pwrWtStock[0] = float.Parse(mobility.Groups[9].Value);}
            catch(FormatException) {this.pwrWtStock[0] = -1;}
            this.pwrWtUpgraded[0] = float.Parse(mobility.Groups[10].Value);
            //RB/SB
            try{this.pwrWtStock[1] = float.Parse(mobility.Groups[17].Value);}
            catch(FormatException) {this.pwrWtStock[1] = -1;}
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
            
            //TODO Purchase Price
            //TODO Crew Train
            //TODO Research Points
            //TODO Modifiers (SL/RP)

            //Features
            MatchCollection features = CompReg.featuresPtrn.Matches(page.Text);
            this.features = features.Cast<Match>().Select(m => m.Groups[1].Value).ToArray();

            //TODO Modifications
            //Main Armament
            Match mainArm = CompReg.mainArmamentPtrn.Match(page.Text);
            this.mainArmament = String.Empty.Equals(mainArm.Groups[1].Value) ? 
                                mainArm.Groups[2].Value :
                                mainArm.Groups[1] + " " + mainArm.Groups[2];

            this.mainArmament = this.mainArmament.Replace("_"," ").Replace("&quot;", "\"");
            this.mainArmament = RegFunc.Replace(this.mainArmament, CompReg.diameterPtrn, CompReg.diameterSub);
            this.mainArmament = RegFunc.Replace(this.mainArmament, CompReg.multipleGunsPtrn, CompReg.multiGunSub);

            //TODO Machine Gun(s)
            //TODO Additional Armament
            //TODO Largest Calibre Gun
            //TODO Ammunition Types
            //TODO Ammunition amounts
            //TODOHighest Penetration Round
            //TODO "Best" Ammunition Type
            //TODO Reload speed
            //TODO Guidance
            //TODO Hull Armor
            //TODO Turret Armor
            //...
            
            additionalInfo = true;
        }

        public override string ToString() {
            if(additionalInfo) {
                return $"Name: {name}\n"+
                       $"URL: {url}\nNation: {nation}\n"+
                       $"Foldered: {foldered}\n"+
                       $"Rank: {rank}\n"+
                       $"Premium: {purchaseType}\n"+
                       $"Role: {role}\n"+
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



    sealed public class GroundVehicleMap : CsvHelper.Configuration.ClassMap<GroundVehicle> {
        public GroundVehicleMap() {
            Map(m => m.name).Index(0).Name("name");
            Map(m => m.nation).Index(1).Name("nation");
            Map(m => m.foldered).Index(2).Name("foldered");
            Map(m => m.rank).Index(3).Name("rank");
            Map(m => m.role).Index(4).Name("role");
            Map(m => m.br).Index(5).Name("br_ab");
            Map().Index(6).Name("br_rb");
            Map().Index(7).Name("br_sb");
            Map(m => m.mainArmament).Index(8).Name("mainArm");
            Map(m => m.repairCost).Index(9).Name("repair_ab");
            Map().Index(10).Name("repair_rb");
            Map().Index(11).Name("repair_sb");
            Map(m => m.purchaseType).Index(12).Name("purchase_type");
        }
    }
    }
}
