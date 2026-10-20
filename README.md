# LotinKrill

O'zbek tili uchun lotin ↔ kirill transliteratori. Tashqi bog'liqliklarsiz.

Qo'llab-quvvatlanadi: .NET 6, 7, 8, 9, 10 va .NET Standard 2.0 (.NET Framework 4.6.1+).

```bash
dotnet add package LotinKrill
```

```csharp
using LotinKrill;

Transliterator.ToCyrillic("Qo'qon shahri, Navro‘z ko‘chasi 108-uy");
// Қўқон шаҳри, Наврўз кўчаси 108-уй

Transliterator.ToCyrillic("Tuman administratsiyasi");   // Туман администрацияси
Transliterator.ToCyrillic("SHOKIROVA GULNORA");         // ШОКИРОВА ГУЛНОРА

Transliterator.ToLatin("Ўзбекистон Республикаси");      // Oʻzbekiston Respublikasi
```

`null` berilsa `null` qaytadi. Kirill harflari, raqamlar va tinish belgilari o'zgarishsiz qoladi, shuning uchun aralash matnni ham to'g'ridan-to'g'ri uzatish mumkin.

## Lotin → kirill qoidalari

- **Apostroflar** — `'` `` ` `` `ʹ` `ʻ` `ʼ` `ʽ` `ˊ` `ˋ` `‘` `’` bir xil ishlanadi:
  - `o'` → `ў`, `g'` → `ғ` (so'z oxirida ham: `tog'` → `тоғ`)
  - so'z ichida → `ъ` (`ma'lumot` → `маълумот`)
  - `s'h` → `сҳ` (`Is'hoq` → `Исҳоқ`)
  - so'z chegarasida — qo'shtirnoq, o'zgarishsiz (`‘Mirzo’` → `‘Мирзо’`)
- `e` → `э` so'z boshida va unlidan keyin, aks holda `е`; `ye`/`yo`/`yu`/`ya` → `е`/`ё`/`ю`/`я`.
- **Istisno lug'ati** (~1080 o'zlashma so'z): `ц` (`administratsiya` → `администрация`), `ь` (`oktabr` → `октябрь`), `э` (`karate` → `каратэ`), `йо`/`йу`/`йа` (`rayon` → `район`), `сҳ` (`mushaf` → `мусҳаф`).
  - O'zbekcha qo'shimchalar bilan ishlaydi: `administratsiyasining`, `militsiyaga`, `dezinfeksiyalashtirilgan`.
  - Qo'shimcha oldidan `ь` tushadi: `1 oktabrdan` → `1 октябрдан`, `roli` → `роли`.
  - O'zak faqat to'liq so'zga yoki qo'shimchalar zanjiriga mos keladi, shuning uchun o'zbekcha so'zlar buzilmaydi: `sirka` → `сирка` (`цирк` emas), `mergan` → `мерган` (`мэр` emas).
- **KATTA harfli matn** to'liq katta harfda chiqadi: `YO‘LDOSHEV` → `ЙЎЛДОШЕВ`.
- **Raqamga yopishgan lotin harflari** kod hisoblanadi va o'zgarmaydi: `01A123BC`, `v1`, `12A`.

## Kirill → lotin qoidalari

- `ц` → `s` so'z boshida va undoshdan keyin, `ts` unlidan keyin (`цирк` → `sirk`, `федерация` → `federatsiya`).
- `е` → `ye` so'z boshida, unlidan va `ъ`/`ь` dan keyin (`Европа` → `Yevropa`, `съезд` → `syezd`).
- `ў`/`ғ` → `oʻ`/`gʻ` (U+02BB), `ъ` → `ʼ` (U+02BC), `ь` tushadi.
- `Сентябрь`/`Октябрь` → `Sentabr`/`Oktabr`.

## Cheklovlar

- Lotin yozuvi kirillga nisbatan kam ma'lumot saqlaydi. Lug'atda yo'q o'zlashma so'zlar harfma-harf o'giriladi: `Tsoy` → `Тсой`, `obyekt` → `обект`.
- `c` va `w` harflari o'zbek alifbosida yo'q, ular o'zgarishsiz qoladi.
- Manbadagi imlo xatolari tuzatilmaydi (`xokimiyati` → `хокимияти`).
- `"` apostrof sifatida qabul qilinmaydi (`Ulug"bek`).
- Qisqartmalar oddiy so'z kabi o'giriladi (`PDF` → `ПДФ`).

## Manba

Istisno lug'atlari va asosiy qoidalar Anvar'ning [`transliterate.py`](https://github.com/anvarnarz/telegram-analysis/blob/6cc6b7ed02f0774b362bca2377e49b7531c3338f/transliterate.py) faylidan olingan (asl repoda litsenziya ko'rsatilmagan). C# kodi yangidan yozilgan va Python versiyadagi quyidagi kamchiliklar tuzatilgan:

- faqat `ʻ`/`‘` apostroflari tanilardi;
- lug'at o'zaklari so'z boshiga mos kelib, o'zbekcha so'zlarni buzardi (`tushuntirish` → `тушьунтириш`);
- istisnolar so'z matn boshida bo'lmasa buzilardi (`Tuman administratsiyasi` → `Туман администратсияцси`);
- istisnolar faqat kichik harfli so'zlarda ishlardi;
- bir so'zdagi ikkinchi `ts` almashtirilmasdi (`assotsiatsiya` → `ассоциатсия`);
- `йо`/`йу`/`йа`/`йе`/`сҳ` lug'atlari ishlatilmasdi (`rayon` → `раён`);
- KATTA harfli matn lotinga `ShOKIROVA` ko'rinishida chiqardi;
- har bir chaqiruvda ~1100 ta regex o'tishi — C# versiya bir o'tishda ishlaydi (~2000 marta tez).

Lug'atdan o'zbekcha so'z bilan bir xil yoziladigan `kon`, `sol`, `tol`, `tush`, `polka`, `yod`, `yodlamoq` chiqarib tashlangan.

## Litsenziya

[MIT](LICENSE)
