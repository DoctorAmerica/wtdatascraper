using System;
using CsvHelper;
using System.Globalization;
using WarThunder;
using System.Text;

namespace utils {

    class CSV {

        public static void writeToCsv(List<GroundVehicle> data, string filepath) {
            var stream = new StreamWriter(filepath, false, Encoding.UTF8);
            var csv = new CsvWriter( stream, CultureInfo.InvariantCulture );
            csv.Context.RegisterClassMap<GroundVehicleMap>();
            csv.WriteRecords(data);
            stream.Flush();
            stream.Close();
        }
    }
}