using System.Text;

namespace utils
{

    class CSV {

        public static void writeToCsv(IEnumerable<ICSVObj> data, string filepath) {
            var stream = new StreamWriter(filepath, false, Encoding.ASCII);
            stream.WriteLine(string.Join(",", data.First().CSVColumns()));
            foreach(var row in data) {
                stream.WriteLine(row.CSVRow());
            }
            stream.Flush();
            stream.Close();
        }

        public static Dictionary<T, bool> OneHotEncode<T>(IEnumerable<T> items) {
            var result = new Dictionary<T,bool>();
            foreach (var item in items) {
                result.TryAdd(item,true);
            }
            return result;
        }

        public static List<bool> ListFromOneHot<T>(Dictionary<T, bool> dict, List<T> possibleValues) {
            List<bool> result = new List<bool>();
            foreach (var value in possibleValues) {
                result.Add(dict.GetValueOrDefault(value));
            }
            return result;
        }
    }

}