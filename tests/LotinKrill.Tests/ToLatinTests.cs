namespace LotinKrill.Tests;

public class ToLatinTests
{
    [Theory]
    [InlineData("Тошкент шаҳри", "Toshkent shahri")]
    [InlineData("Чилонзор", "Chilonzor")]
    [InlineData("ЧИЛОНЗОР", "CHILONZOR")]                 // Python: ChILONZOR
    [InlineData("ШОКИРОВА ГУЛНОРА", "SHOKIROVA GULNORA")] // Python: ShOKIROVA
    [InlineData("Ёқуб", "Yoqub")]
    [InlineData("ЁҚУБ", "YOQUB")]
    [InlineData("Ўзбекистон", "Oʻzbekiston")]
    [InlineData("ЎЗБЕКИСТОН", "OʻZBEKISTON")]
    [InlineData("Қўқон", "Qoʻqon")]
    [InlineData("маъно", "maʼno")]
    public void LetterRules(string cyrillic, string expected) =>
        Assert.Equal(expected, Transliterator.ToLatin(cyrillic));

    [Theory]
    [InlineData("цирк", "sirk")]
    [InlineData("федерация", "federatsiya")]
    [InlineData("функция", "funksiya")]
    [InlineData("Ер", "Yer")]
    [InlineData("поэма", "poema")]
    [InlineData("Европа", "Yevropa")]
    [InlineData("ЕВРОПА", "YEVROPA")]
    [InlineData("съезд", "syezd")]
    [InlineData("объект", "obyekt")]
    [InlineData("пьеса", "pyesa")]
    [InlineData("Сентябрь", "Sentabr")]
    [InlineData("ОКТЯБРЬ", "OKTABR")]
    [InlineData("октябрда", "oktabrda")]
    public void ContextRules(string cyrillic, string expected) =>
        Assert.Equal(expected, Transliterator.ToLatin(cyrillic));

    [Theory]
    [InlineData("Oʻzbekiston Respublikasi")]
    [InlineData("Toshkent shahri, Chilonzor tumani")]
    [InlineData("SHOKIROVA GULNORA ABDURAXIMOVNA")]
    public void RoundTrip(string latin) =>
        Assert.Equal(latin, Transliterator.ToLatin(Transliterator.ToCyrillic(latin)));

    [Fact]
    public void NullEmptyAndLatinPassthrough()
    {
        Assert.Null(Transliterator.ToLatin(null));
        Assert.Equal("", Transliterator.ToLatin(""));
        Assert.Equal("Toshkent, 2026", Transliterator.ToLatin("Toshkent, 2026"));
    }
}
