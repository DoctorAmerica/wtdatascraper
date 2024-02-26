using System;
using CsvHelper;
using System.Globalization;
using WarThunder;
using System.Text;

namespace utils {

    class CSV {

        public static void writeToCsv(List<GroundVehicle> data, string filepath) {
            var stream = new StreamWriter(filepath, false, Encoding.ASCII);
            var csv = new CsvWriter( stream, CultureInfo.InvariantCulture );
            csv.Context.RegisterClassMap<GroundVehicle.GroundVehicleMap>();
            csv.WriteRecords(data);
            stream.Flush();
            stream.Close();
        }
    }
}