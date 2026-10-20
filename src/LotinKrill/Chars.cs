namespace LotinKrill
{
    internal static class Chars
    {
        /// <summary>
        /// O'zbek lotin matnida ishlatiladigan apostrof turlari:
        /// ' ` ʹ ʻ ʼ ʽ ˊ ˋ ‘ ’
        /// </summary>
        internal static bool IsApostrophe(char c) => c is
            '\u0027' or '\u0060' or '\u02B9' or '\u02BB' or '\u02BC' or
            '\u02BD' or '\u02CA' or '\u02CB' or '\u2018' or '\u2019';

        internal static bool IsLatinLetter(char c) => c is >= 'a' and <= 'z' or >= 'A' and <= 'Z';

        internal static bool IsLatinLetterAt(string s, int index) => index < s.Length && IsLatinLetter(s[index]);

        internal static char ToLowerAscii(char c) => c is >= 'A' and <= 'Z' ? (char)(c + 32) : c;

        internal static bool IsLatinVowel(char c) => ToLowerAscii(c) is 'a' or 'e' or 'i' or 'o' or 'u';

        /// <summary>Kirill harfi (rus + o'zbek: ў, қ, ғ, ҳ).</summary>
        internal static bool IsCyrillicLetter(char c) => c is >= '\u0400' and <= '\u04FF';

        internal static bool IsCyrillicVowel(char c) => char.ToLowerInvariant(c) is
            'а' or 'е' or 'ё' or 'и' or 'о' or 'у' or 'э' or 'ю' or 'я' or 'ў';

        internal static string Capitalize(string value) =>
            value.Length == 0 ? value : char.ToUpperInvariant(value[0]) + value.Substring(1);
    }
}
