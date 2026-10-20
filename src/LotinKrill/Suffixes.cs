namespace LotinKrill
{
    /// <summary>
    /// Istisno so'z o'zagidan keyingi qism o'zbekcha qo'shimchalar zanjiri ekanini tekshiradi.
    /// Shu tufayli "kontrol" lug'atdagi o'zak "kontrolga"ga mos keladi, lekin
    /// "mer" (мэр) o'zagi "mergan"ga, "sirk" (цирк) o'zagi "sirka"ga mos kelmaydi.
    /// </summary>
    internal static class Suffixes
    {
        // Ot yasovchi, egalik, kelishik va boshqa ot turkumi qo'shimchalari
        private static readonly string[] Nominal =
        {
            "lar", "m", "im", "ng", "ing", "i", "si", "miz", "imiz", "ngiz", "ingiz",
            "ning", "ni", "ga", "ka", "qa", "da", "dan", "gacha", "dagi", "gi", "ki", "niki",
            "li", "siz", "chi", "lik", "cha", "chalik", "dek", "day", "gina",
            "dor", "kor", "xona", "zor", "shunos", "simon", "viy", "bop", "mi", "dir", "man", "san",
        };

        // Otdan fe'l yasovchi qo'shimchalar (ot → fe'l)
        private static readonly string[] Verbalizers = { "la", "lash", "lashtir", "lan", "lantir" };

        // Fe'l o'zagiga qo'shiladigan nisbat qo'shimchalari (fe'l → fe'l)
        private static readonly string[] VerbVoice = { "tir", "il", "in", "lan", "lash" };

        // Fe'l shakllari (fe'l → ot kabi qo'shimcha oladi)
        private static readonly string[] VerbEndings =
        {
            "moq", "ish", "sh", "gan", "di", "ib", "b", "ydi", "yapti", "digan", "ydigan", "gach",
        };

        /// <summary><paramref name="key"/> ning <paramref name="start"/> dan keyingi qismi qo'shimchalar zanjirimi.</summary>
        internal static bool IsChain(string key, int start) => IsNominalChain(key, start);

        private static bool IsNominalChain(string key, int start)
        {
            if (start == key.Length) return true;
            foreach (var suffix in Nominal)
                if (StartsWith(key, start, suffix) && AllowedAfter(suffix, key[start - 1]) &&
                    IsNominalChain(key, start + suffix.Length))
                    return true;
            foreach (var suffix in Verbalizers)
                if (StartsWith(key, start, suffix) && IsVerbChain(key, start + suffix.Length))
                    return true;
            return false;
        }

        private static bool IsVerbChain(string key, int start)
        {
            if (start == key.Length) return true; // buyruq shakli: filtrla
            foreach (var suffix in VerbVoice)
                if (StartsWith(key, start, suffix) && IsVerbChain(key, start + suffix.Length))
                    return true;
            foreach (var suffix in VerbEndings)
                if (StartsWith(key, start, suffix) && IsNominalChain(key, start + suffix.Length))
                    return true;
            return false;
        }

        // -ka faqat k/g dan, -qa faqat q/g' dan keyin keladi
        private static bool AllowedAfter(string suffix, char previous) => suffix switch
        {
            "ka" => previous is 'k' or 'g',
            "qa" => previous is 'q' or '\'',
            _ => true,
        };

        private static bool StartsWith(string key, int start, string suffix) =>
            start + suffix.Length <= key.Length &&
            string.CompareOrdinal(key, start, suffix, 0, suffix.Length) == 0;
    }
}
