using System.Text.RegularExpressions;
using HtmlAgilityPack;
using utils;
using System.Text;

namespace WarThunder
{
    public class GroundVehicle : IVehicle {
        // protected static List<string> allFeatures = new List<string>();
        public static List<string> FeatureList = new List<string>();
        
        // Tree info
        public string name { get; set; }
        public string url { get; set; }

        public string nation { get; set; }
        public bool foldered { get; set; }
        public bool additionalInfo { get; set; }
        // Additional data
        public int rank { get; set; }
        protected string role;
        public float[] br { get; set; } = new float[3]; // AB/RB/SB
        protected float[] forwardSpeed = new float[2]; // AB/RB+SB
        protected float[] reverseSpeed = new float[2]; // AB/RB+SB
        float weight;
        float[] enginePowerStock = new float[2]; // AB/RB+SB
        float[] enginePowerUpgraded = new float[2]; // AB/RB+SB
        float[] pwrWtStock = new float[2]; // AB/RB+SB
        float[] pwrWtUpgraded = new float[2]; // AB/RB+SB
        public float[] repairCost { get; set; } = new float[3];  // AB/RB/SB
        protected Dictionary<string, bool> features;
        protected string mainArmament;
        public string purchaseType { get; set; }
        protected float[] SLModifier = new float[3];
        protected float[] RPModifier = new float[3];
        protected float[] mainArmReload = new float[2];
        protected float mainArmDiameter;
        public bool isPremium { get; set; }
        public bool isReserve { get; set; }
        

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

        public void GetInfoFromPage()
        {
            HtmlDocument page = Scraper.GetDocument(url);
            GetVehicleGameMetadata(page);
            GetVehicleSpecs(page);
            additionalInfo = true;
        }

        private void GetVehicleSpecs(HtmlDocument page)
        {
            // Specs
            GetMobility(page);
            GetRepairCost(page);
            GetFeatures(page);
            GetMainArmament(page);
            //TODO Modifications
            GetMainArmamentDiameter();
            //TODO Machine Gun(s)
            //TODO Additional Armament
            //TODO Largest Calibre Gun
            //TODO Ammunition Types
            //TODO Ammunition amounts
            //TODO Highest Penetration Round
            //TODO "Best" Ammunition Type
            //Reload speed
            GetReloadSpeed(page);
            //TODO Guidance
            //TODO Hull Armor
            //TODO Turret Armor
            //...
        }

        private void GetVehicleGameMetadata(HtmlDocument page)
        {
            // Game metadata
            GetRank(page);
            GetPurchaseType(page);
            GetRole(page);
            GetBRs(page);
            GetReserve(page);
            //TODO Purchase Price
            //TODO Crew Train
            //TODO Research Points
            GetModifiers(page);
        }

        private void GetReserve(HtmlDocument page)
        {
            this.isReserve = CompReg.reservePtrn.Match(page.Text).Success;
        }

        private void GetReloadSpeed(HtmlDocument page)
        {
            MatchCollection reloads = CompReg.reloadPtrn.Matches(page.Text);
            try
            {
                this.mainArmReload[0] = float.Parse(reloads[0].Groups[1].Value);
                this.mainArmReload[1] = float.Parse(reloads[0].Groups[2].Value);
            }
            catch (FormatException)
            {
                this.mainArmReload[0] = this.mainArmReload[1] = float.Parse(reloads[0].Groups[3].Value);
            }
        }

        private void GetMainArmamentDiameter()
        {
            try
            {
                this.mainArmDiameter = float.Parse(CompReg.diameterPtrn.Match(mainArmament).Groups[1].Value);
            }
            catch (Exception)
            {
                if (this.mainArmament.Contains("Fliegerfaust 2 Stinger"))
                {
                    this.mainArmDiameter = 70;
                }
                else if (this.mainArmament.Contains("Type 91"))
                {
                    this.mainArmDiameter = 80;
                }
                else if (this.mainArmament.Contains("Mistral"))
                {
                    this.mainArmDiameter = 90;
                }
                else if (this.mainArmament.Contains("Rbs 70"))
                {
                    this.mainArmDiameter = 105;
                }
                else if (this.mainArmament.Contains("Type 64")
                        || this.mainArmament.Contains("9M37M"))
                {
                    this.mainArmDiameter = 120;
                }
                else if (this.mainArmament.Contains("MIM-72")
                        || this.mainArmament.Contains("ZT3"))
                {
                    this.mainArmDiameter = 127;
                }
                else if (this.mainArmament.Contains("Starstreak")
                        || this.mainArmament.Contains("9M114"))
                {
                    this.mainArmDiameter = 130;
                }
                else if (this.mainArmament.Contains("HOT"))
                {
                    this.mainArmDiameter = 136;
                }
                else if (this.mainArmament.Contains("TOW")
                        || this.mainArmament.Contains("MIM146")
                        || this.mainArmament.Contains("Rbs 55")
                        || this.mainArmament.Contains("HJ-9"))
                {
                    this.mainArmDiameter = 152;
                }
                else if (this.mainArmament.Contains("9M123"))
                {
                    this.mainArmDiameter = 155;
                }
                else if (this.mainArmament.Contains("Type 81"))
                {
                    this.mainArmDiameter = 160;
                }
                else if (this.mainArmament.Contains("MGM-166"))
                {
                    this.mainArmDiameter = 162;
                }
                else if (this.mainArmament.Contains("Tager")
                        || this.mainArmament.Contains("LFK SS.11"))
                {
                    this.mainArmDiameter = 164;
                }
                else if (this.mainArmament.Contains("Roland")
                        || this.mainArmament.Contains("VT1"))
                {
                    this.mainArmDiameter = 165;
                }
                else if (this.mainArmament.Contains("Swingfire"))
                {
                    this.mainArmDiameter = 170;
                }
                else if (this.mainArmament.Contains("3M7"))
                {
                    this.mainArmDiameter = 180;
                }
                else if (this.mainArmament.Contains("9M331"))
                {
                    this.mainArmDiameter = 239;
                }
            }
        }

        private void GetModifiers(HtmlDocument page)
        {
            Match SLMod = CompReg.SLModifierPtrn.Match(page.Text);
            this.SLModifier[0] = float.Parse(SLMod.Groups[2].Value);
            this.SLModifier[1] = float.Parse(SLMod.Groups[3].Value);
            this.SLModifier[2] = float.Parse(SLMod.Groups[4].Value);


            Match RPMod = CompReg.RPModifierPtrn.Match(page.Text);
            this.RPModifier[0] = float.Parse(RPMod.Groups[2].Value);
            this.RPModifier[1] = float.Parse(RPMod.Groups[3].Value);
            this.RPModifier[2] = float.Parse(RPMod.Groups[4].Value);

            if (!RPMod.Groups[1].Value.Equals(String.Empty))
            {
                for (int i = 0; i < 3; i++)
                {
                    this.SLModifier[i] *= 2;
                    this.RPModifier[i] *= 2;
                }
            }
        }

        private void GetMainArmament(HtmlDocument page)
        {
            Match mainArm = CompReg.mainArmamentPtrn.Match(page.Text);
            this.mainArmament = String.Empty.Equals(mainArm.Groups[1].Value) ?
                                mainArm.Groups[2].Value :
                                mainArm.Groups[1] + " " + mainArm.Groups[2];

            this.mainArmament = this.mainArmament.Replace("_", " ").Replace("&quot;", "\"");
            this.mainArmament = RegFunc.Replace(this.mainArmament, CompReg.diameterPtrn, CompReg.diameterSub);
            this.mainArmament = RegFunc.Replace(this.mainArmament, CompReg.multipleGunsPtrn, CompReg.multiGunSub);
        }

        private void GetFeatures(HtmlDocument page)
        {
            var features = CompReg.featuresPtrn.Matches(page.Text).Cast<Match>().Select(m => m.Groups[1].Value);
            this.features = CSV.OneHotEncode(features);
            foreach (var feature in features)
            {
                if (FeatureList.Contains(feature))
                {
                    continue;
                }
                FeatureList.Add(feature);
            }
        }

        private void GetRepairCost(HtmlDocument page)
        {
            MatchCollection repairs = CompReg.repairPtrn.Matches(page.Text);
            if (repairs.Count < 3)
            {
                this.repairCost[0] = this.repairCost[1] = this.repairCost[2] = 0;
            }
            else
            {
                this.repairCost[0] = float.Parse(repairs[0].Groups[2].Value.Replace(" ", ""));
                this.repairCost[1] = float.Parse(repairs[1].Groups[2].Value.Replace(" ", ""));
                this.repairCost[2] = float.Parse(repairs[2].Groups[2].Value.Replace(" ", ""));
            }
        }

        private void GetMobility(HtmlDocument page)
        {
            try
            {
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
                try { this.enginePowerStock[0] = float.Parse(mobility.Groups[7].Value); }
                catch (FormatException) { this.enginePowerStock[0] = -1; }
                this.enginePowerUpgraded[0] = float.Parse(mobility.Groups[8].Value);
                //RB/SB
                try { this.enginePowerStock[1] = float.Parse(mobility.Groups[15].Value); }
                catch (FormatException) { this.enginePowerStock[1] = -1; }
                this.enginePowerUpgraded[1] = float.Parse(mobility.Groups[16].Value);
                //Pwr/Wt
                //AB
                try { this.pwrWtStock[0] = float.Parse(mobility.Groups[9].Value); }
                catch (FormatException) { this.pwrWtStock[0] = -1; }
                this.pwrWtUpgraded[0] = float.Parse(mobility.Groups[10].Value);
                //RB/SB
                try { this.pwrWtStock[1] = float.Parse(mobility.Groups[17].Value); }
                catch (FormatException) { this.pwrWtStock[1] = -1; }
                this.pwrWtUpgraded[1] = float.Parse(mobility.Groups[18].Value);
            }
            catch (FormatException)
            {
                Match mobility = CompReg.mobilityPtrn2.Match(page.Text);
                //Speed
                //AB
                this.forwardSpeed[0] = float.Parse(mobility.Groups[1].Value.Replace(" ", ""));
                this.reverseSpeed[0] = float.Parse(mobility.Groups[2].Value.Replace(" ", ""));
                //RB/SB
                this.forwardSpeed[1] = float.Parse(mobility.Groups[3].Value.Replace(" ", ""));
                this.reverseSpeed[1] = float.Parse(mobility.Groups[4].Value.Replace(" ", ""));
                //Weight
                this.weight = float.Parse(mobility.Groups[7].Value.Replace(" ", ""));
                //Power
                //AB
                this.enginePowerStock[0] = -1;
                this.enginePowerUpgraded[0] = float.Parse(mobility.Groups[8].Value.Replace(" ", ""));
                //RB/SB
                this.enginePowerStock[1] = -1;
                this.enginePowerUpgraded[1] = float.Parse(mobility.Groups[9].Value.Replace(" ", ""));
                //Pwr/Wt
                //AB
                this.pwrWtStock[0] = -1;
                this.pwrWtUpgraded[0] = float.Parse(mobility.Groups[10].Value.Replace(" ", ""));
                //RB/SB
                this.pwrWtStock[1] = -1;
                this.pwrWtUpgraded[1] = float.Parse(mobility.Groups[11].Value.Replace(" ", ""));
            }
        }

        private void GetBRs(HtmlDocument page)
        {
            Match BRs = CompReg.brPtrn.Match(page.Text);
            this.br[0] = float.Parse(BRs.Groups[1].Value);
            this.br[1] = float.Parse(BRs.Groups[2].Value);
            this.br[2] = float.Parse(BRs.Groups[3].Value);
        }

        private void GetRole(HtmlDocument page)
        {
            this.role = CompReg.rolePtrn.Match(page.Text).Groups[3].Value;
        }

        private void GetPurchaseType(HtmlDocument page)
        {
            if (CompReg.premiumPtrn.Match(page.Text).Success)
            {
                this.isPremium = true;
                if (CompReg.marketPtrn.Match(page.Text).Success)
                {
                    this.purchaseType = "Market";
                }
                else if (CompReg.packPtrn.Match(page.Text).Success)
                {
                    this.purchaseType = "Pack";
                }
                else if (CompReg.gePattern.Match(page.Text).Success)
                {
                    this.purchaseType = "GE";
                }
                else
                {
                    this.purchaseType = "Gift";
                }
            }
            else if (CompReg.squadronPtrn.Match(page.Text).Success)
            {
                this.purchaseType = "Squadron";
            }
            else if (CompReg.marketPtrn.Match(page.Text).Success)
            {
                this.purchaseType = "Market";
            }
            else if (CompReg.giftPtrn.Match(page.Text).Success)
            {
                this.purchaseType = "Gift";
            }
            else
            {
                this.purchaseType = "Tech Tree";
            }
        }

        private void GetRank(HtmlDocument page)
        {
            this.rank = Conversions.RomanToInteger(
                            CompReg.rankPtrn.Match(page.Text).Groups[2].Value);
        }

        public override string ToString() {
            if(additionalInfo) {
                return $"Name: {name}\n"+
                       $"URL: {url}\nNation: {nation}\n"+
                       $"Foldered: {foldered}\n"+
                       $"Rank: {rank}\n"+
                       $"Premium: {isPremium}\n"+
                       $"Reserve: {isReserve}\n"+
                       $"Purchase Type: {purchaseType}\n"+
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
                       $"Features: {string.Join(", ", features.Keys)}\n"+
                       $"Main Armament: {mainArmament}\n"+
                       $"Main Armament reload: {mainArmReload[0]} -> {mainArmReload[1]}\n"+
                       $"Main Armament Diameter: {mainArmDiameter}mm\n";
            } else {
                return $"Name: {name}\nURL: {url}\nNation: {nation}\nFoldered: {foldered}\n";
            }
        }

        string ICSVObj.CSVRow() {
            StringBuilder sb = new StringBuilder();
            List<string> values = [
                name,
                nation,
                ""+foldered,
                ""+rank,
                role,
                mainArmament,
                ""+mainArmDiameter,
                purchaseType,
                ""+isPremium,
                ""+isReserve,
                string.Join(",",br),
                string.Join(",",repairCost),
                string.Join(",",SLModifier),
                string.Join(",",RPModifier),
                string.Join(",",mainArmReload),
                string.Join(",",forwardSpeed),
                string.Join(",",reverseSpeed),
                string.Join(",",CSV.ListFromOneHot(features, FeatureList))
                ];
            sb.AppendJoin(",",values);

            return sb.ToString();
        }

        public static List<string> CSVColumns() {
            List<string> columns = [
            "name",
            "nation",
            "foldered",
            "rank",
            "role",
            "mainArm",
            "main_arm_diameter",
            "purchase_type",
            "is_premium",
            "is_reserve",
            "br_ab",
            "br_rb",
            "br_sb",
            "repair_ab",
            "repair_rb",
            "repair_sb",
            "sl_mod_ab",
            "sl_mod_rb",
            "sl_mod_sb",
            "rp_mod_ab",
            "rp_mod_rb",
            "rp_mod_sb",
            "main_reload_base",
            "main_reload_upgraded",
            "forward_ab",
            "forward_rbsb",
            "reverse_ab",
            "reverse_rbsb",
            .. FeatureList,
            ];
            return columns;
        }

        List<string> ICSVObj.CSVColumns() {
            return GroundVehicle.CSVColumns();
        }

        /**
         name,
         nation,
         foldered,
         rank,
         role,
         br_ab,
         br_rb,
         br_sb,
         mainArm,
         repair_ab,
         repair_rb,
         repair_sb,
         purchase_type,
         sl_mod_ab,
         sl_mod_rb,
         sl_mod_sb,
         rp_mod_ab,
         rp_mod_rb,
         rp_mod_sb,
         main_reload_base,
         main_reload_upgraded,
         main_arm_diameter,
         forward_ab,
         forward_rbsb,
         reverse_ab,
         reverse_rbsb,
         has_stabilizer,
         can_float,
         has_smoke_grenade,
         has_ess,
         has_revers_gear,
         has_night_vision,
         has_rangefinder,
         has_dozer,
         has_era_armor,
         has_autoloader,
         has_thermal_vision,
         has_laser_rangefinder,
         has_hydro_suspension,
         has_composite_armor,
         has_lwr,is_premium
*/
    }
}
