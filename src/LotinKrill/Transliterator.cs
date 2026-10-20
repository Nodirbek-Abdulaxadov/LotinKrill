namespace LotinKrill
{
    /// <summary>O'zbek tili uchun lotin ↔ kirill transliteratori.</summary>
    public static class Transliterator
    {
        /// <summary>
        /// Lotin yozuvidagi o'zbekcha matnni kirillga o'giradi.
        /// Kirill harflari, raqamlar va boshqa belgilar o'zgarishsiz qoladi,
        /// shuning uchun aralash matnni ham xavfsiz uzatish mumkin.
        /// </summary>
        /// <param name="text">Lotin (yoki aralash) matn.</param>
        /// <returns>Kirill matn; <paramref name="text"/> <c>null</c> bo'lsa — <c>null</c>.</returns>
        [return: NotNullIfNotNull(nameof(text))]
        public static string? ToCyrillic(string? text)
        {
            if (text is null || text.Length == 0) return text;
            return LatinToCyrillic.Convert(text);
        }

        /// <summary>
        /// Kirill yozuvidagi o'zbekcha matnni lotinga o'giradi (o‘/g‘ uchun ʻ, tutuq uchun ʼ).
        /// Lotin harflari, raqamlar va boshqa belgilar o'zgarishsiz qoladi.
        /// </summary>
        /// <param name="text">Kirill (yoki aralash) matn.</param>
        /// <returns>Lotin matn; <paramref name="text"/> <c>null</c> bo'lsa — <c>null</c>.</returns>
        [return: NotNullIfNotNull(nameof(text))]
        public static string? ToLatin(string? text)
        {
            if (text is null || text.Length == 0) return text;
            return CyrillicToLatin.Convert(text);
        }
    }
}
