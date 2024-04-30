using utils;

namespace WarThunder {

    public interface IVehicle : ICSVObj {
        public string url { get; set; }
        public string name { get; set; }
        public string nation { get; set; }
        public bool foldered { get; set; }
        public int rank { get; set; }
        public float[] br { get; set; }
        public float[] repairCost { get; set; }
        public string purchaseType { get; set; }
        public bool isPremium { get; set; }
        public bool isReserve { get; set; }
        public void GetInfoFromPage();
    }
}
