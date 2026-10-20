namespace LotinKrill.Tests;

public class ToCyrillicTests
{
    public static readonly TheoryData<string> Apostrophes =
        new() { "'", "`", "ʹ", "ʻ", "ʼ", "ʽ", "ˊ", "ˋ", "‘", "’" };

    [Theory]
    [MemberData(nameof(Apostrophes))]
    public void Apostrophe_AllVariants(string a)
    {
        Assert.Equal("Қўқон", Transliterator.ToCyrillic($"Qo{a}qon"));
        Assert.Equal("маълумот", Transliterator.ToCyrillic($"ma{a}lumot"));
        Assert.Equal("тоғ", Transliterator.ToCyrillic($"tog{a}"));
        Assert.Equal("ЎҒЛИ", Transliterator.ToCyrillic($"O{a}G{a}LI"));
        Assert.Equal("Йўлдошев", Transliterator.ToCyrillic($"Yo{a}ldoshev"));
        Assert.Equal("Мирзо Улуғбек", Transliterator.ToCyrillic($"Mirzo Ulug{a}bek"));
    }

    [Theory]
    [InlineData("Is'hoq", "Исҳоқ")]                // s'h — ajratuvchi
    [InlineData("‘Mirzo’", "‘Мирзо’")]             // qo'shtirnoq, o' emas
    [InlineData("bog'i", "боғи")]
    public void Apostrophe_Context(string latin, string expected) =>
        Assert.Equal(expected, Transliterator.ToCyrillic(latin));

    // Python versiyada yumshoq belgi lug'ati so'z boshiga mos kelib buzardi
    [Theory]
    [InlineData("tushuntirish berildi", "тушунтириш берилди")]
    [InlineData("soliq", "солиқ")]
    [InlineData("tola", "тола")]
    [InlineData("bronza", "бронза")]
    [InlineData("kon", "кон")]
    [InlineData("tol", "тол")]
    [InlineData("mergan", "мерган")]
    [InlineData("sirka", "сирка")]
    [InlineData("yod", "ёд")]
    [InlineData("yodda", "ёдда")]
    [InlineData("tushum", "тушум")]
    public void ExceptionWords_DoNotMatchNativeWords(string latin, string expected) =>
        Assert.Equal(expected, Transliterator.ToCyrillic(latin));

    [Theory]
    [InlineData("kontrol", "контроль")]
    [InlineData("kontrolga", "контролга")]           // ь qo'shimcha oldidan tushadi
    [InlineData("oktabr", "октябрь")]
    [InlineData("1 oktabrdan", "1 октябрдан")]
    [InlineData("FEVRAL", "ФЕВРАЛЬ")]
    [InlineData("Roli", "Роли")]                     // Python: рольи
    [InlineData("Avtomobil modeli", "Автомобиль модели")]
    [InlineData("avtomobillar", "автомобиллар")]
    [InlineData("rayon", "район")]
    [InlineData("Rayoni", "Райони")]
    [InlineData("mayor", "майор")]
    [InlineData("yogurt", "йогурт")]
    [InlineData("foye", "фойе")]
    [InlineData("mushaf", "мусҳаф")]
    [InlineData("karate", "каратэ")]
    [InlineData("sirkda", "циркда")]
    [InlineData("degustatsiya", "дегустация")]
    public void ExceptionWords_WithSuffixes(string latin, string expected) =>
        Assert.Equal(expected, Transliterator.ToCyrillic(latin));

    [Theory]
    [InlineData("administratsiya", "администрация")]
    [InlineData("Tuman administratsiyasi", "Туман администрацияси")]  // Python: администратсияцси
    [InlineData("Administratsiya", "Администрация")]
    [InlineData("ADMINISTRATSIYA", "АДМИНИСТРАЦИЯ")]
    [InlineData("assotsiatsiya", "ассоциация")]                       // Python: ассоциатсия
    [InlineData("militsiyaga", "милицияга")]
    [InlineData("stansiyasi", "станцияси")]
    [InlineData("Kompensatsiya pullari", "Компенсация пуллари")]
    [InlineData("konferensiya", "конференция")]
    [InlineData("dezinfeksiyalashtirilgan", "дезинфекциялаштирилган")]
    public void TsWords(string latin, string expected) =>
        Assert.Equal(expected, Transliterator.ToCyrillic(latin));

    [Theory]
    [InlineData("Ergash", "Эргаш")]
    [InlineData("poema", "поэма")]
    [InlineData("Yevropa", "Европа")]
    [InlineData("KERIMBAYEV YERALI", "КЕРИМБАЕВ ЕРАЛИ")]
    [InlineData("ALIYEV ERGASH", "АЛИЕВ ЭРГАШ")]
    [InlineData("SHOKIROVA GULNORA ABDURAXIMOVNA", "ШОКИРОВА ГУЛНОРА АБДУРАХИМОВНА")]
    [InlineData("Chilonzor", "Чилонзор")]
    [InlineData("Sirdaryo", "Сирдарё")]
    [InlineData("Yunusobod", "Юнусобод")]
    public void LetterRules(string latin, string expected) =>
        Assert.Equal(expected, Transliterator.ToCyrillic(latin));

    [Theory]
    [InlineData("Toshkent shahri", "Тошкент шаҳри")]
    [InlineData("Yangiobod tumani", "Янгиобод тумани")]
    [InlineData("Jizzax viloyati", "Жиззах вилояти")]
    [InlineData("Qoraqalpog‘iston Respublikasi", "Қорақалпоғистон Республикаси")]
    [InlineData("Фарғона вилояти, Dang‘ara tumani", "Фарғона вилояти, Данғара тумани")]
    [InlineData("Navro‘z ko‘chasi, 108-uy", "Наврўз кўчаси, 108-уй")]
    public void RealData(string latin, string expected) =>
        Assert.Equal(expected, Transliterator.ToCyrillic(latin));

    [Theory]
    [InlineData("vitse-prezident", "вице-президент")]
    [InlineData("bugun-erta", "бугун-эрта")]
    [InlineData("Sotsial-iqtisodiy", "Социал-иқтисодий")]
    public void HyphenatedWords(string latin, string expected) =>
        Assert.Equal(expected, Transliterator.ToCyrillic(latin));

    [Theory]
    [InlineData("Тошкент шаҳри", "Тошкент шаҳри")]
    [InlineData("12.10.2026, №5", "12.10.2026, №5")]
    [InlineData("ASCII", "АСCИИ")] // c, w o'zbek alifbosida yo'q — o'zgarishsiz
    [InlineData("01A123BC raqamli avtomobil", "01A123BC рақамли автомобиль")] // raqamga yopishgan kod
    [InlineData("Ma'lumot (v1)", "Маълумот (v1)")]
    [InlineData("12A uy", "12A уй")]
    public void Passthrough(string text, string expected) =>
        Assert.Equal(expected, Transliterator.ToCyrillic(text));

    [Fact]
    public void NullAndEmpty()
    {
        Assert.Null(Transliterator.ToCyrillic(null));
        Assert.Equal("", Transliterator.ToCyrillic(""));
    }
}
