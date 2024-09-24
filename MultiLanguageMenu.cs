using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace n_MultiLanguageMenu
{
    internal static partial class RegexCollection
    {
        [GeneratedRegex("(\\B[A-Z])")]
        public static partial Regex CapitalLettersRegex();
        [GeneratedRegex(@"\s+")]
        public static partial Regex WhiteSpacesRegex();
    }
    internal static partial class LanguageCode
    {
        public static readonly string Afrikaans = "af";
        public static readonly string Albanian = "sq";
        public static readonly string Amharic = "am";
        public static readonly string Arabic = "ar";
        public static readonly string Armenian = "hy";
        public static readonly string Assamese = "as";
        public static readonly string Aymara = "ay";
        public static readonly string Azerbaijani = "az";
        public static readonly string Bambara = "bm";
        public static readonly string Basque = "eu";
        public static readonly string Belarusian = "be";
        public static readonly string Bengali = "bn";
        public static readonly string Bhojpuri = "bho";
        public static readonly string Bosnian = "bs";
        public static readonly string Bulgarian = "bg";
        public static readonly string Catalan = "ca";
        public static readonly string Cebuano = "ceb";
        public static readonly string ChineseSimplified = "zhCN";
        public static readonly string ChineseTraditional = "zhTW";
        public static readonly string Corsican = "co";
        public static readonly string Croatian = "hr";
        public static readonly string Czech = "cs";
        public static readonly string Danish = "da";
        public static readonly string Dhivehi = "dv";
        public static readonly string Dogri = "doi";
        public static readonly string Dutch = "nl";
        public static readonly string English = "en";
        public static readonly string Esperanto = "eo";
        public static readonly string Estonian = "et";
        public static readonly string Ewe = "ee";
        public static readonly string FilipinoTagalog = "fil";
        public static readonly string Finnish = "fi";
        public static readonly string French = "fr";
        public static readonly string Frisian = "fy";
        public static readonly string Galician = "gl";
        public static readonly string Georgian = "ka";
        public static readonly string German = "de";
        public static readonly string Greek = "el";
        public static readonly string Guarani = "gn";
        public static readonly string Gujarati = "gu";
        public static readonly string HaitianCreole = "ht";
        public static readonly string Hausa = "ha";
        public static readonly string Hawaiian = "haw";
        public static readonly string Hebrew = "he";
        public static readonly string Hindi = "hi";
        public static readonly string Hmong = "hmn";
        public static readonly string Hungarian = "hu";
        public static readonly string Icelandic = "is";
        public static readonly string Igbo = "ig";
        public static readonly string Ilocano = "ilo";
        public static readonly string Indonesian = "id";
        public static readonly string Irish = "ga";
        public static readonly string Italian = "it";
        public static readonly string Japanese = "ja";
        public static readonly string Javanese = "jv";
        public static readonly string Kannada = "kn";
        public static readonly string Kazakh = "kk";
        public static readonly string Khmer = "km";
        public static readonly string Kinyarwanda = "rw";
        public static readonly string Konkani = "gom";
        public static readonly string Korean = "ko";
        public static readonly string Krio = "kri";
        public static readonly string Kurdish = "ku";
        public static readonly string KurdishSorani = "ckb";
        public static readonly string Kyrgyz = "ky";
        public static readonly string Lao = "lo";
        public static readonly string Latin = "la";
        public static readonly string Latvian = "lv";
        public static readonly string Lingala = "ln";
        public static readonly string Lithuanian = "lt";
        public static readonly string Luganda = "lg";
        public static readonly string Luxembourgish = "lb";
        public static readonly string Macedonian = "mk";
        public static readonly string Maithili = "mai";
        public static readonly string Malagasy = "mg";
        public static readonly string Malay = "ms";
        public static readonly string Malayalam = "ml";
        public static readonly string Maltese = "mt";
        public static readonly string Maori = "mi";
        public static readonly string Marathi = "mr";
        public static readonly string MeiteilonManipuri = "mniMtei";
        public static readonly string Mizo = "lus";
        public static readonly string Mongolian = "mn";
        public static readonly string MyanmarBurmese = "my";
        public static readonly string Nepali = "ne";
        public static readonly string Norwegian = "no";
        public static readonly string NyanjaChichewa = "ny";
        public static readonly string OdiaOriya = "or";
        public static readonly string Oromo = "om";
        public static readonly string Pashto = "ps";
        public static readonly string Persian = "fa";
        public static readonly string Polish = "pl";
        public static readonly string Portuguese = "pt";
        public static readonly string Punjabi = "pa";
        public static readonly string Quechua = "qu";
        public static readonly string Romanian = "ro";
        public static readonly string Russian = "ru";
        public static readonly string Samoan = "sm";
        public static readonly string Sanskrit = "sa";
        public static readonly string ScotsGaelic = "gd";
        public static readonly string Sepedi = "nso";
        public static readonly string Serbian = "sr";
        public static readonly string Sesotho = "st";
        public static readonly string Shona = "sn";
        public static readonly string Sindhi = "sd";
        public static readonly string Sinhalese = "si";
        public static readonly string Slovak = "sk";
        public static readonly string Slovenian = "sl";
        public static readonly string Somali = "so";
        public static readonly string Spanish = "es";
        public static readonly string Sundanese = "su";
        public static readonly string Swahili = "sw";
        public static readonly string Swedish = "sv";
        public static readonly string TagalogFilipino = "tl";
        public static readonly string Tajik = "tg";
        public static readonly string Tamil = "ta";
        public static readonly string Tatar = "tt";
        public static readonly string Telugu = "te";
        public static readonly string Thai = "th";
        public static readonly string Tigrinya = "ti";
        public static readonly string Tsonga = "ts";
        public static readonly string Turkish = "tr";
        public static readonly string Turkmen = "tk";
        public static readonly string TwiAkan = "ak";
        public static readonly string Ukrainian = "ua";
        public static readonly string Urdu = "ur";
        public static readonly string Uyghur = "ug";
        public static readonly string Uzbek = "uz";
        public static readonly string Vietnamese = "vi";
        public static readonly string Welsh = "cy";
        public static readonly string Xhosa = "xh";
        public static readonly string Yiddish = "yi";
        public static readonly string Yoruba = "yo";
        public static readonly string Zulu = "zu";
        public static SortedDictionary<string, string> GetCodesDict()
        {
            SortedDictionary<string, string> codes = [];
            foreach (FieldInfo field in typeof(LanguageCode).GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                codes.Add(field.Name, field.GetValue(null).ToString());
            }
            return codes;
        }
        public static List<string> GetLanguages()
        {
            List<string> languages = [];
            foreach (string language in GetCodesDict().Keys.ToList())
            {
                languages.Add(RegexCollection.CapitalLettersRegex().Replace(language, " $1"));
            }
            return languages;
        }
        public static List<string> GetCodes()
        {
            return [.. GetCodesDict().Values];
        }
        public static string GetLanguage(string code)
        {
            return GetCodesDict().FirstOrDefault(pair => EqualityComparer<string>.Default.Equals(pair.Value, code)).Key;
        }
        public static string GetCode(string language)
        {
            return GetCodesDict()[RegexCollection.WhiteSpacesRegex().Replace(language, "")];
        }
    }
    internal static partial class LanguageNativeName
    {
        public static readonly string Afrikaans = "Afrikaanse";
        public static readonly string Albanian = "shqipe";
        public static readonly string Amharic = "አማርኛ";
        public static readonly string Arabic = "اللغة";
        public static readonly string Armenian = "Հայոց";
        public static readonly string Assamese = "অসমীয়া";
        public static readonly string Aymara = "Aymara";
        public static readonly string Azerbaijani = "Azərbaycan";
        public static readonly string Bambara = "Bamanankan";
        public static readonly string Basque = "euskara";
        public static readonly string Belarusian = "Беларуская";
        public static readonly string Bengali = "বাংলা";
        public static readonly string Bhojpuri = "भोजपुरी";
        public static readonly string Bosnian = "bosanski";
        public static readonly string Bulgarian = "български";
        public static readonly string Catalan = "catalana";
        public static readonly string Cebuano = "Cebuano";
        public static readonly string ChineseSimplified = "简体中文";
        public static readonly string ChineseTraditional = "中國傳統的";
        public static readonly string Corsican = "corsa";
        public static readonly string Croatian = "hrvatski";
        public static readonly string Czech = "český";
        public static readonly string Danish = "dansk";
        public static readonly string Dhivehi = "ދިވެހި";
        public static readonly string Dogri = "डोगरी";
        public static readonly string Dutch = "hollandsk";
        public static readonly string English = "English";
        public static readonly string Esperanto = "Esperanto";
        public static readonly string Estonian = "eesti";
        public static readonly string Ewe = "Ewegbe";
        public static readonly string FilipinoTagalog = "Filipino Tagalog";
        public static readonly string Finnish = "Suomen";
        public static readonly string French = "Français";
        public static readonly string Frisian = "Fryske";
        public static readonly string Galician = "galisyen";
        public static readonly string Georgian = "Georgian";
        public static readonly string German = "deutsche";
        public static readonly string Greek = "ελληνική";
        public static readonly string Guarani = "Guarani";
        public static readonly string Gujarati = "ગુજરાતી";
        public static readonly string HaitianCreole = "kreyòl ayisyen";
        public static readonly string Hausa = "Hausa";
        public static readonly string Hawaiian = "Hawaiʻi";
        public static readonly string Hebrew = "עִברִית";
        public static readonly string Hindi = "हिन्दी";
        public static readonly string Hmong = "Hmoob";
        public static readonly string Hungarian = "magyar";
        public static readonly string Icelandic = "Íslensk";
        public static readonly string Igbo = "Igbo";
        public static readonly string Ilocano = "Ilocano";
        public static readonly string Indonesian = "Indo";
        public static readonly string Irish = "Gaeilge";
        public static readonly string Italian = "italiana";
        public static readonly string Japanese = "日本語";
        public static readonly string Javanese = "basa jawa";
        public static readonly string Kannada = "ಕನ್ನಡ";
        public static readonly string Kazakh = "Қазақ";
        public static readonly string Khmer = "ខ្មែរ";
        public static readonly string Kinyarwanda = "Kinyarwanda";
        public static readonly string Konkani = "कोंकणी";
        public static readonly string Korean = "한국어";
        public static readonly string Krio = "Krio";
        public static readonly string Kurdish = "kurdî";
        public static readonly string KurdishSorani = "Kurdî Soranî";
        public static readonly string Kyrgyz = "Кыргыз";
        public static readonly string Lao = "ພາສາລາວ";
        public static readonly string Latin = "Latinae";
        public static readonly string Latvian = "latviešu";
        public static readonly string Lingala = "Lingala";
        public static readonly string Lithuanian = "lietuvių";
        public static readonly string Luganda = "Oluganda";
        public static readonly string Luxembourgish = "Lëtzebuergesch";
        public static readonly string Macedonian = "Македонски";
        public static readonly string Maithili = "मैथिली";
        public static readonly string Malagasy = "malagasy";
        public static readonly string Malay = "melayu";
        public static readonly string Malayalam = "മലയാളം";
        public static readonly string Maltese = "Malti";
        public static readonly string Maori = "Maori";
        public static readonly string Marathi = "मराठी";
        public static readonly string MeiteilonManipuri = "ꯃꯦꯏꯇꯦꯏꯂꯣꯟꯃꯦꯏꯇꯦꯏꯂꯣꯟ ꯃꯅꯤꯄꯨꯔꯤ";
        public static readonly string Mizo = "Mizo";
        public static readonly string Mongolian = "Монгол";
        public static readonly string MyanmarBurmese = "မြန်မာစာ";
        public static readonly string Nepali = "नेपाली";
        public static readonly string Norwegian = "norsk";
        public static readonly string NyanjaChichewa = "Nyanja Chichewa";
        public static readonly string OdiaOriya = "ଓଡିଆ ଓଡିଆ |";
        public static readonly string Oromo = "Oromoo";
        public static readonly string Pashto = "پښتو";
        public static readonly string Persian = "فارسي";
        public static readonly string Polish = "polski";
        public static readonly string Portuguese = "portugues";
        public static readonly string Punjabi = "ਪੰਜਾਬੀ";
        public static readonly string Quechua = "ਕੇਚੂਆ";
        public static readonly string Romanian = "română";
        public static readonly string Russian = "русский";
        public static readonly string Samoan = "Samoa";
        public static readonly string Sanskrit = "संस्कृत";
        public static readonly string ScotsGaelic = "Scots gaelic";
        public static readonly string Sepedi = "Sepedi";
        public static readonly string Serbian = "српски";
        public static readonly string Sesotho = "Sesotho";
        public static readonly string Shona = "Shona";
        public static readonly string Sindhi = "سنڌي";
        public static readonly string Sinhalese = "සිංහල";
        public static readonly string Slovak = "slovenský";
        public static readonly string Slovenian = "slovenski";
        public static readonly string Somali = "soomaaliga";
        public static readonly string Spanish = "español";
        public static readonly string Sundanese = "Sunda";
        public static readonly string Swahili = "kiswahili";
        public static readonly string Swedish = "svenska";
        public static readonly string TagalogFilipino = "Tagalog Filipino";
        public static readonly string Tajik = "точикй";
        public static readonly string Tamil = "தமிழ்";
        public static readonly string Tatar = "Татар";
        public static readonly string Telugu = "తెలుగు";
        public static readonly string Thai = "ไทย";
        public static readonly string Tigrinya = "ትግርኛ";
        public static readonly string Tsonga = "Xitsonga";
        public static readonly string Turkish = "Türk";
        public static readonly string Turkmen = "Türkmen";
        public static readonly string TwiAkan = "Twi Akan";
        public static readonly string Ukrainian = "Українська";
        public static readonly string Urdu = "اردو";
        public static readonly string Uyghur = "ئۇيغۇر";
        public static readonly string Uzbek = "o'zbek";
        public static readonly string Vietnamese = "tiếng Việt";
        public static readonly string Welsh = "Gymraeg";
        public static readonly string Xhosa = "Xhosa";
        public static readonly string Yiddish = "יידישע";
        public static readonly string Yoruba = "Yorùbá";
        public static readonly string Zulu = "Zulu";
        public static SortedDictionary<string, string> GetNamesDict()
        {
            SortedDictionary<string, string> names = [];
            foreach (FieldInfo field in typeof(LanguageNativeName).GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                names.Add(field.Name, field.GetValue(null).ToString());
            }
            return names;
        }
        public static List<string> GetLanguages()
        {
            List<string> languages = [];
            foreach (string language in GetNamesDict().Keys.ToList())
            {
                languages.Add(RegexCollection.CapitalLettersRegex().Replace(language, " $1"));
            }
            return languages;
        }
        public static List<string> GetNames()
        {
            return [.. GetNamesDict().Values];
        }
        public static string GetLanguage(string name)
        {
            return GetNamesDict().FirstOrDefault(pair => EqualityComparer<string>.Default.Equals(pair.Value, name)).Key;
        }
        public static string GetName(string language)
        {
            return GetNamesDict()[RegexCollection.WhiteSpacesRegex().Replace(language, "")];
        }
    }
}