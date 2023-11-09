using System;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using CsvHelper;
using utils;
using System.Runtime.CompilerServices;
using System.Linq.Expressions;

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
        protected float[] SLModifier = new float[3];
        protected float[] RPModifier = new float[3];
        protected float[] mainArmReload = new float[2];

        protected float mainArmDiameter;
        

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

            //Features
            MatchCollection features = CompReg.featuresPtrn.Matches(page.Text);
            this.features = features.Cast<Match>().Select(m => m.Groups[1].Value).ToArray();

            //Main Armament
            Match mainArm = CompReg.mainArmamentPtrn.Match(page.Text);
            this.mainArmament = String.Empty.Equals(mainArm.Groups[1].Value) ? 
                                mainArm.Groups[2].Value :
                                mainArm.Groups[1] + " " + mainArm.Groups[2];

            this.mainArmament = this.mainArmament.Replace("_"," ").Replace("&quot;", "\"");
            this.mainArmament = RegFunc.Replace(this.mainArmament, CompReg.diameterPtrn, CompReg.diameterSub);
            this.mainArmament = RegFunc.Replace(this.mainArmament, CompReg.multipleGunsPtrn, CompReg.multiGunSub);

            //TODO Purchase Price
            //TODO Crew Train
            //TODO Research Points
            //Modifiers (SL/RP)
            Match SLMod = CompReg.SLModifierPtrn.Match(page.Text);
            this.SLModifier[0] = float.Parse(SLMod.Groups[2].Value);
            this.SLModifier[1] = float.Parse(SLMod.Groups[3].Value);
            this.SLModifier[2] = float.Parse(SLMod.Groups[4].Value);

            
            Match RPMod = CompReg.RPModifierPtrn.Match(page.Text);
            this.RPModifier[0] = float.Parse(RPMod.Groups[2].Value);
            this.RPModifier[1] = float.Parse(RPMod.Groups[3].Value);
            this.RPModifier[2] = float.Parse(RPMod.Groups[4].Value);
            
            if(!RPMod.Groups[1].Value.Equals(String.Empty)) {
                for(int i = 0; i < 3; i++) {
                    this.SLModifier[i] *= 2;
                    this.RPModifier[i] *= 2;
                }
            }
            //TODO Modifications

            //Main Armament Diameter
            try {
                this.mainArmDiameter = (float)Double.Parse(CompReg.diameterPtrn.Match(mainArmament).Groups[1].Value);
            } catch (Exception) {
                if(this.mainArmament.Contains("Fliegerfaust 2 Stinger")) {
                    this.mainArmDiameter = 70;
                } else if(this.mainArmament.Contains("Type 91")) {
                    this.mainArmDiameter = 80;
                } else if (this.mainArmament.Contains("Mistral")) {
                    this.mainArmDiameter = 90;
                } else if (this.mainArmament.Contains("Rbs 70")) {
                    this.mainArmDiameter = 105;
                } else if (this.mainArmament.Contains("Type 64")
                        || this.mainArmament.Contains("9M37M")) {
                    this.mainArmDiameter = 120;
                } else if (this.mainArmament.Contains("MIM-72")
                        || this.mainArmament.Contains("ZT3")) {
                    this.mainArmDiameter = 127;
                } else if (this.mainArmament.Contains("Starstreak")
                        || this.mainArmament.Contains("9M114")) {
                    this.mainArmDiameter = 130;
                } else if (this.mainArmament.Contains("HOT")) {
                    this.mainArmDiameter = 136;
                } else if (this.mainArmament.Contains("TOW")
                        || this.mainArmament.Contains("MIM146")
                        || this.mainArmament.Contains("Rbs 55")
                        || this.mainArmament.Contains("HJ-9")) {
                    this.mainArmDiameter = 152;
                } else if (this.mainArmament.Contains("9M123")) {
                    this.mainArmDiameter = 155;
                } else if (this.mainArmament.Contains("Type 81")) {
                    this.mainArmDiameter = 160;
                } else if (this.mainArmament.Contains("MGM-166")) {
                    this.mainArmDiameter = 162;
                } else if (this.mainArmament.Contains("Tager")
                        || this.mainArmament.Contains("LFK SS.11")) {
                    this.mainArmDiameter = 164;
                } else if (this.mainArmament.Contains("Roland")
                        || this.mainArmament.Contains("VT1")) {
                    this.mainArmDiameter = 165;
                } else if (this.mainArmament.Contains("Swingfire")) {
                    this.mainArmDiameter = 170;
                } else if (this.mainArmament.Contains("3M7")) {
                    this.mainArmDiameter = 180;
                } else if (this.mainArmament.Contains("9M331")) {
                    this.mainArmDiameter = 239;
                }
            }

            //TODO Machine Gun(s)
            //TODO Additional Armament
            //TODO Largest Calibre Gun
            //TODO Ammunition Types
            //TODO Ammunition amounts
            //TODO Highest Penetration Round
            //TODO "Best" Ammunition Type
            //Reload speed
            MatchCollection reloads = CompReg.reloadPtrn.Matches(page.Text);
            try {
                this.mainArmReload[0] = float.Parse(reloads[0].Groups[1].Value);
                this.mainArmReload[1] = float.Parse(reloads[0].Groups[2].Value);
            } catch (FormatException) {
                this.mainArmReload[0] = this.mainArmReload[1] = float.Parse(reloads[0].Groups[3].Value);
            }

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
                       $"Modifiers:\n"+
                       $"\tAB: {SLModifier[0]}% SL / {RPModifier[0]}% RP\n"+
                       $"\tRB: {SLModifier[1]}% SL / {RPModifier[0]}% RP\n"+
                       $"\tSB: {SLModifier[2]}% SL / {RPModifier[0]}% RP\n"+
                       $"Features: {string.Join(", ", features)}\n"+
                       $"Main Armament: {mainArmament}\n"+
                       $"Main Armament reload: {mainArmReload[0]} -> {mainArmReload[1]}\n"+
                       $"Main Armament Diameter: {mainArmDiameter}mm\n";
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
            Map(m => m.SLModifier).Index(13).Name("sl_mod_ab");
            Map().Index(14).Name("sl_mod_rb");
            Map().Index(15).Name("sl_mod_sb");
            Map(m => m.RPModifier).Index(16).Name("rp_mod_ab");
            Map().Index(17).Name("rp_mod_rb");
            Map().Index(18).Name("rp_mod_sb");
            Map(m => m.mainArmReload).Index(19).Name("main_reload_base");
            Map().Index(20).Name("main_reload_upgraded");
            Map(m => m.mainArmDiameter).Index(21).Name("main_arm_diameter");
        }
    }
    }
}
