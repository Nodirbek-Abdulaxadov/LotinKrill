namespace LotinKrill
{
    internal static class LatinToCyrillic
    {
        internal static string Convert(string text)
        {
            var sb = new StringBuilder(text.Length);
            var i = 0;
            while (i < text.Length)
            {
                if (!Chars.IsLatinLetter(text[i]))
                {
                    sb.Append(text[i]);
                    i++;
                    continue;
                }
                var end = ScanWord(text, i);
                // Raqamga yopishgan lotin harflari — kod (01A123BC, v1, 12A): o'zgarishsiz
                if ((i > 0 && char.IsDigit(text[i - 1])) || (end < text.Length && char.IsDigit(text[end])))
                    sb.Append(text, i, end - i);
                else
                    AppendWord(sb, text.Substring(i, end - i));
                i = end;
            }
            return sb.ToString();
        }

        /// <summary>
        /// So'z: lotin harflari + so'z ichidagi apostrof va defis.
        /// So'z oxiridagi apostrof faqat g' (tog', bog') uchun so'zga qo'shiladi —
        /// boshqa holatda u qo'shtirnoq hisoblanadi (‘Mirzo’).
        /// </summary>
        private static int ScanWord(string text, int start)
        {
            var j = start + 1;
            while (j < text.Length)
            {
                var c = text[j];
                if (Chars.IsLatinLetter(c))
                {
                    j++;
                    continue;
                }
                var previousIsLetter = Chars.IsLatinLetter(text[j - 1]);
                var nextIsLetter = Chars.IsLatinLetterAt(text, j + 1);
                if (Chars.IsApostrophe(c) && previousIsLetter &&
                    (nextIsLetter || Chars.ToLowerAscii(text[j - 1]) == 'g'))
                {
                    j++;
                    continue;
                }
                if (c == '-' && previousIsLetter && nextIsLetter)
                {
                    j++;
                    continue;
                }
                break;
            }
            return j;
        }

        private static void AppendWord(StringBuilder sb, string word)
        {
            var allCaps = IsAllCaps(word);
            // Defisli istisnolar butun so'z bo'yicha: vitse-prezident, gastrol-konsert
            if (word.IndexOf('-') >= 0 && TryAppendException(sb, word, allCaps)) return;

            var segmentStart = 0;
            for (var i = 0; i <= word.Length; i++)
            {
                if (i < word.Length && word[i] != '-') continue;
                var segment = word.Substring(segmentStart, i - segmentStart);
                if (!TryAppendException(sb, segment, allCaps))
                    AppendByRules(sb, segment, 0, allCaps);
                if (i < word.Length) sb.Append('-');
                segmentStart = i + 1;
            }
        }

        private static bool IsAllCaps(string word)
        {
            var letters = 0;
            foreach (var c in word)
            {
                if (!Chars.IsLatinLetter(c)) continue;
                if (c is >= 'a' and <= 'z') return false;
                letters++;
            }
            return letters > 1;
        }

        /// <summary>Eng uzun istisno o'zagi + o'zbekcha qo'shimchalar zanjiri.</summary>
        private static bool TryAppendException(StringBuilder sb, string word, bool allCaps)
        {
            var key = NormalizeKey(word);
            for (var length = Math.Min(key.Length, ExceptionWords.MaxKeyLength); length >= 2; length--)
            {
                if (!ExceptionWords.LatinToCyrillic.TryGetValue(key.Substring(0, length), out var value)) continue;
                if (!Suffixes.IsChain(key, length)) continue;

                // ь qo'shimcha oldidan tushadi: rol → роль, roli → роли, oktabrdan → октябрдан
                if (length < key.Length && value[value.Length - 1] == 'ь')
                    value = value.Substring(0, value.Length - 1);
                sb.Append(allCaps ? value.ToUpperInvariant()
                    : char.IsUpper(word[0]) ? Chars.Capitalize(value)
                    : value);
                AppendByRules(sb, word, length, allCaps);
                return true;
            }
            return false;
        }

        private static string NormalizeKey(string word)
        {
            var chars = new char[word.Length];
            for (var i = 0; i < word.Length; i++)
                chars[i] = Chars.IsApostrophe(word[i]) ? '\'' : Chars.ToLowerAscii(word[i]);
            return new string(chars);
        }

        private static void AppendByRules(StringBuilder sb, string s, int start, bool allCaps)
        {
            var i = start;
            while (i < s.Length)
            {
                var c = s[i];
                if (Chars.IsApostrophe(c))
                {
                    AppendApostrophe(sb, s, i, allCaps);
                    i++;
                    continue;
                }
                if (!Chars.IsLatinLetter(c))
                {
                    sb.Append(c);
                    i++;
                    continue;
                }

                var lower = Chars.ToLowerAscii(c);
                var next = i + 1 < s.Length ? Chars.ToLowerAscii(s[i + 1]) : '\0';
                var nextIsApostrophe = i + 1 < s.Length && Chars.IsApostrophe(s[i + 1]);
                char cyrillic;
                var consumed = 1;
                switch (lower)
                {
                    case 'o' when nextIsApostrophe && Chars.IsLatinLetterAt(s, i + 2):
                        cyrillic = 'ў'; consumed = 2; break;
                    case 'g' when nextIsApostrophe:
                        cyrillic = 'ғ'; consumed = 2; break;
                    case 's' when next == 'h':
                        cyrillic = 'ш'; consumed = 2; break;
                    case 'c' when next == 'h':
                        cyrillic = 'ч'; consumed = 2; break;
                    // yo' → йў (Yo'ldosh), yo → ё
                    case 'y' when next == 'o' && !IsOApostrophe(s, i + 1):
                        cyrillic = 'ё'; consumed = 2; break;
                    case 'y' when next == 'u':
                        cyrillic = 'ю'; consumed = 2; break;
                    case 'y' when next == 'a':
                        cyrillic = 'я'; consumed = 2; break;
                    case 'y' when next == 'e':
                        cyrillic = 'е'; consumed = 2; break;
                    // e → э so'z boshida va unlidan keyin
                    case 'e':
                        cyrillic = i == 0 || FollowsVowel(s, i) ? 'э' : 'е'; break;
                    default:
                        cyrillic = MapLetter(lower);
                        if (cyrillic == '\0')
                        {
                            sb.Append(c); // c, w — o'zbek alifbosida yo'q
                            i++;
                            continue;
                        }
                        break;
                }

                sb.Append(allCaps || char.IsUpper(c) ? char.ToUpperInvariant(cyrillic) : cyrillic);
                i += consumed;
            }
        }

        /// <summary>o'/g' ga tegishli bo'lmagan apostrof.</summary>
        private static void AppendApostrophe(StringBuilder sb, string s, int i, bool allCaps)
        {
            var previousIsLetter = i > 0 && Chars.IsLatinLetter(s[i - 1]);
            if (!previousIsLetter || !Chars.IsLatinLetterAt(s, i + 1))
            {
                sb.Append(s[i]);
                return;
            }
            // s'h — s va h ni ajratuvchi belgi: Is'hoq → Исҳоқ
            if (Chars.ToLowerAscii(s[i - 1]) == 's' && Chars.ToLowerAscii(s[i + 1]) == 'h') return;
            // tutuq belgisi: ma'lumot → маълумот
            sb.Append(allCaps ? 'Ъ' : 'ъ');
        }

        private static bool IsOApostrophe(string s, int i) =>
            Chars.ToLowerAscii(s[i]) == 'o' && i + 1 < s.Length && Chars.IsApostrophe(s[i + 1]) &&
            Chars.IsLatinLetterAt(s, i + 2);

        private static bool FollowsVowel(string s, int i)
        {
            var previous = s[i - 1];
            if (Chars.IsLatinVowel(previous)) return true;
            // o' (ў) ham unli
            return Chars.IsApostrophe(previous) && i >= 2 && Chars.ToLowerAscii(s[i - 2]) == 'o';
        }

        private static char MapLetter(char lower) => lower switch
        {
            'a' => 'а', 'b' => 'б', 'd' => 'д', 'f' => 'ф', 'g' => 'г', 'h' => 'ҳ',
            'i' => 'и', 'j' => 'ж', 'k' => 'к', 'l' => 'л', 'm' => 'м', 'n' => 'н',
            'o' => 'о', 'p' => 'п', 'q' => 'қ', 'r' => 'р', 's' => 'с', 't' => 'т',
            'u' => 'у', 'v' => 'в', 'x' => 'х', 'y' => 'й', 'z' => 'з',
            _ => '\0',
        };
    }
}
