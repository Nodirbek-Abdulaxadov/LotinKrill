namespace LotinKrill
{
    internal static class CyrillicToLatin
    {
        internal static string Convert(string text)
        {
            var sb = new StringBuilder(text.Length + text.Length / 4);
            var i = 0;
            while (i < text.Length)
            {
                if (!Chars.IsCyrillicLetter(text[i]))
                {
                    sb.Append(text[i]);
                    i++;
                    continue;
                }
                var end = i + 1;
                while (end < text.Length && Chars.IsCyrillicLetter(text[end])) end++;
                AppendWord(sb, text, i, end);
                i = end;
            }
            return sb.ToString();
        }

        private static void AppendWord(StringBuilder sb, string text, int start, int end)
        {
            var allCaps = IsAllCaps(text, start, end);
            for (var i = start; i < end; i++)
            {
                var c = text[i];
                var latin = MapLetter(text, start, end, i);
                if (latin is null)
                {
                    sb.Append(c); // щ, ы va boshqa o'zbek alifbosida yo'q harflar
                    continue;
                }
                sb.Append(allCaps ? latin.ToUpperInvariant()
                    : char.IsUpper(c) ? Chars.Capitalize(latin)
                    : latin);
            }
        }

        private static bool IsAllCaps(string text, int start, int end)
        {
            var letters = 0;
            for (var i = start; i < end; i++)
            {
                if (char.IsLower(text[i])) return false;
                letters++;
            }
            return letters > 1;
        }

        private static string? MapLetter(string text, int start, int end, int i)
        {
            var wordStart = i == start;
            var previous = wordStart ? '\0' : char.ToLowerInvariant(text[i - 1]);
            return char.ToLowerInvariant(text[i]) switch
            {
                'а' => "a", 'б' => "b", 'в' => "v", 'г' => "g", 'д' => "d",
                // е → ye so'z boshida, unlidan va ъ/ь dan keyin
                'е' => wordStart || Chars.IsCyrillicVowel(previous) || previous is 'ъ' or 'ь' ? "ye" : "e",
                'ё' => "yo", 'ж' => "j", 'з' => "z", 'и' => "i", 'й' => "y", 'к' => "k",
                'л' => "l", 'м' => "m", 'н' => "n", 'о' => "o", 'п' => "p", 'р' => "r",
                'с' => "s", 'т' => "t", 'у' => "u", 'ф' => "f", 'х' => "x",
                // ц → s so'z boshida va undoshdan keyin, ts unlidan keyin
                'ц' => !wordStart && Chars.IsCyrillicVowel(previous) ? "ts" : "s",
                // ъ + е/ё/ю/я → y-tovush o'zi yetarli: объект → obyekt, съезд → syezd
                'ъ' => i + 1 < end && char.ToLowerInvariant(text[i + 1]) is 'е' or 'ё' or 'ю' or 'я' ? "" : "ʼ",
                'ч' => "ch", 'ш' => "sh", 'ь' => "", 'э' => "e", 'ю' => "yu",
                'я' => IsSentabrOktabr(text, start, end, i) ? "a" : "ya",
                'ў' => "oʻ", 'қ' => "q", 'ғ' => "gʻ", 'ҳ' => "h",
                _ => null,
            };
        }

        // Сентябрь → Sentabr, Октябрь → Oktabr
        private static bool IsSentabrOktabr(string text, int start, int end, int i)
        {
            if (!EqualsIgnoreCase(text, i + 1, "бр", end)) return false;
            return (i - 4 >= start && EqualsIgnoreCase(text, i - 4, "сент", end)) ||
                   (i - 3 >= start && EqualsIgnoreCase(text, i - 3, "окт", end));
        }

        private static bool EqualsIgnoreCase(string text, int index, string value, int end)
        {
            if (index + value.Length > end) return false;
            for (var k = 0; k < value.Length; k++)
                if (char.ToLowerInvariant(text[index + k]) != value[k]) return false;
            return true;
        }
    }
}
