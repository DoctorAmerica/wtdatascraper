// using System;
// using CsvHelper;
// using System.Globalization;

// namespace utils {

//     class CSV {

//         public static void WriteToCSV(List<WarThunder.GroundVehicle> vehicles, string filepath) {

//             using( var writer = new StreamWriter(filepath))
//             using( var csv = new CsvWriter(writer, CultureInfo.InvariantCulture)) {
//                 csv.Context.RegisterClassMap<WarThunder.GroundVehicleMap>();
//                 csv.WriterRecords(vehicles);
//             }
//         }
//     }
// }