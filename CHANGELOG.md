# Changelog

Barcha muhim o'zgarishlar shu faylda [Keep a Changelog](https://keepachangelog.com/en/1.1.0/) formatida yoziladi.

## [1.0.1] - 2026-10-20

### Added
- Paketga repository ma'lumotlari (GitHub havolasi, commit hash) va SourceLink.
- Debug simvollari paketi (`.snupkg`).

## [1.0.0] - 2026-10-20

### Added
- `Transliterator.ToCyrillic` — lotin → kirill: 10 xil apostrof, KATTA harfli matn, `s'h` → `сҳ`, ~1080 so'zli istisno lug'ati o'zbekcha qo'shimchalar bilan.
- `Transliterator.ToLatin` — kirill → lotin: `ц`/`е` kontekst qoidalari, `ъ` + `е/ё/ю/я`, `Сентябрь`/`Октябрь`.
- Raqamga yopishgan lotin kodlari (`01A123BC`, `v1`) o'zgarishsiz qoladi.
- Target framework'lar: `net6.0`, `net7.0`, `net8.0`, `net9.0`, `net10.0`, `netstandard2.0`.
- Paket icon'i (`assets/icon.png`).

### Fixed
- `transliterate.py` (Python) dagi 7 ta kamchilik — README'dagi "Manba" bo'limiga qarang.
- Qo'shimcha oldidan `ь` tushadi (`roli` → `роли`, Python: `рольи`).
- `degustatsiya` → `дегустация` (lug'atdagi `дегустатция` qiymati).

### Removed
- O'zbekcha so'z bilan bir xil yoziladigan lug'at yozuvlari: `kon`, `sol`, `tol`, `tush`, `polka`, `yod`, `yodlamoq`.
