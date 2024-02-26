
namespace utils {

    class Conversions {
        private static Dictionary<char, int> RomanMap = new Dictionary<char, int>() {
            {'I', 1},
            {'V', 5},
            {'X', 10},
            {'L', 50},
            {'C', 100},
            {'D', 500},
            {'M', 1000}
        };

        public static int RomanToInteger(string roman) {
        int number = 0;
        for (int i = 0; i < roman.Length; i++) {
            if (i + 1 < roman.Length && RomanMap[roman[i]] < RomanMap[roman[i + 1]])
            {
                number -= RomanMap[roman[i]];
            }
            else
            {
                number += RomanMap[roman[i]];
            }
        }

        return number;
        }
    }
}