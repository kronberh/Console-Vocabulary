using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using n_Menu;
using n_ForeignWord;
using n_Vocabulary;
using n_FileManager;
using n_MultiLanguageMenu;
using n_ExceptionHandler;
using System.IO;
using System.Text.RegularExpressions;

namespace n_VocabularyMenu
{
    enum VocabularyEnumConstants
    {
        CELLWIDTH = 35
    }
    [Serializable]
    internal class VocabularyAppMenu(string uiLanguage) : Menu
    {
        public VocabularyAppMenu() : this("English") { }
        readonly string adminInfoFilename = "_admin.dat";
        [Required]
        string uiLanguage = (2 * Convert.ToInt32(LanguageCode.GetLanguages().Contains(uiLanguage)) + Convert.ToInt32(LanguageCode.GetCodes().Contains(uiLanguage))) switch
        {
            3 => uiLanguage,
            2 => uiLanguage,
            1 => LanguageCode.GetLanguage(uiLanguage),
            _ => "English"
        };
        public string UILanguage { get => this.uiLanguage; }
        static readonly char[] separators = Enumerable.Range(char.MinValue, char.MaxValue - char.MinValue + 1).Select(ch => Convert.ToChar(ch)).Where(c => char.IsWhiteSpace(c) || char.IsSeparator(c) || char.IsPunctuation(c)).ToArray();
        public override void MainMenu()
        {
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;
            Console.CursorVisible = false;
            while (true)
            {
                try
                {
                    PrintInMiddle(
                        "             ████████████████████████████████████████████████████                     ",
                        "           ████████████████████████████████████████████████████████                   ",
                        "         ████████████████████████████████████████████████████████████                 ",
                        "         █████████                                          █████████                 ",
                        "         █████████           ████████████████████           █████████                 ",
                        "         █████████          ██████████████████████          █████████                 ",
                        "         █████████          ███████  █████  ██████          █████████                 ",
                        "         █████████          ██████  █ █ █ █  █████                                    ",
                        "         █████████          █████  █ ██ ██ █  ██  ███████████████████   ██            ",
                        "         █████████          █████  █████████  █   ███████████████████   ████          ",
                        "         █████████          █████  █ ██ ██ █  █   ███████████████████   ██████        ",
                        "         █████████          ██████  █ █ █ █  ██   ███████████████████   ████████      ",
                        "         █████████          ███████  █████  ███   ████████    ███████   ██████████    ",
                        "         █████████          ███████████████████   ████████    ███████   ████████████  ",
                        "         █████████          ███              ██  █████████    ███████                 ",
                        "         █████████          ███████████████████   ████████    ████████████████████████",
                        "         █████████           ██████████████████   ████████    ██████████  ████████████",
                        "         █████████                             ███████████    █████████    ███████████",
                        "█████████████████████████████████████████████  ███████████    ████████      ██████████",
                        "█████████████████████████████████████████████  ███████████    ███████   ██   █████████",
                        " ████████████████████████████████████████████  ███████████    ██████          ████████",
                        "  ███████████████████████████████████████████  ███████████    █████            ███████",
                        "    █████████████████████████████████████████  ███████████    ██████   ████   ████████",
                        "                                               ███████████    ████████████████████████",
                        "                                               ███████████    ██████          ████████",
                        "                  press ENTER                  ███████            ██████     █████████",
                        "                   to start                    █████████        ███████    ███████████",
                        "            the CONSOLE VOCABULARY             ██████████      ██████     ████████████",
                        "                                               ████████████  ███████          ████████",
                        "                                                █████████████████████████████████████ ");
                    string logpass = ReadHiddenLine();

                    if (logpass == ConsoleKey.Escape.ToString())
                    {
                        Console.CursorVisible = true;
                        return;
                    }
                    else if (logpass.ToString() == FileManager.ReadStringFromBinaryFile(this.adminInfoFilename))
                    {
                        AdminMenu(logpass.ToString());
                    }
                    else
                    {
                        UserMenu();
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    // Ловимо виняток, коли екран занадто вузький
                    Console.Clear();
                    Console.WriteLine("Please widen your console");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();  // Чекаємо, поки користувач натисне клавішу
                    continue; // Повторно виводимо меню
                }
                catch (n_ExceptionHandler.AccessViolationException ave)
                {
                    Console.WriteLine($"Access denied: {ave.Message}");
                    Console.ReadKey();
                }
                catch (Exception e)
                {
                    Console.WriteLine(ExceptionHandler.GetUnxpectedExceptionMessage(e));
                    Console.ReadKey();
                }
            }
        }
        public void AdminMenu(string logpass)
        {
            try
            {
                if (logpass != FileManager.ReadStringFromBinaryFile(this.adminInfoFilename))
                {
                    throw new n_ExceptionHandler.AccessViolationException("Incorrect password.");
                }
                string nav = "Admin Home";
                List<string> menuItems =
                [
                    "\x1b[3;95mSelect (or create) vocabulary by language\x1b[0m",
                    "\x1b[3;92mList of available vocabularies\x1b[0m",
                    "\x1b[3;93mChange UI language\x1b[0m", // not implemented
                    "\x1b[3;95mChange admin password\x1b[0m",
                    "\x1b[3;37mExit\x1b[0m"
                ];
                while (true)
                {
                    try
                    {
                        int choice = PrintMenu(nav, "Choose an operation", menuItems);
                        if (choice == menuItems.Count - 1 || choice == -1)
                        {
                            return;
                        }
                        else if (choice == menuItems.Count - 2)
                        {
                            ChangePasswordAdminMenu();
                        }
                        else if (choice == menuItems.Count - 3)
                        {
                            ChangeLanguageMenu(nav);
                        }
                        else if (choice == menuItems.Count - 4)
                        {
                            VocabulariesAdminMenu(nav);
                        }
                        else if (choice == menuItems.Count - 5)
                        {
                            LanguageFromAdminMenu(nav);
                        }
                        else
                        {
                            throw new ArgumentOutOfRangeException(string.Empty, $"Chosen option has returned an unexpected value that does not define any operations ({choice})");
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(ExceptionHandler.GetUnxpectedExceptionMessage(e));
                        Console.ReadKey();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(ExceptionHandler.GetUnxpectedExceptionMessage(e));
                Console.ReadKey();
            }
        }
        public void UserMenu()
        {
            try
            {
                string nav = "Home";
                List<string> menuItems =
                [
                    "\x1b[3;92mSelect vocabulary by language\x1b[0m",
                    "\x1b[3;92mList of available vocabularies\x1b[0m",
                    "\x1b[3;93mChange UI language\x1b[0m", // not implemented
                    "\x1b[3;37mExit\x1b[0m"
                ];
                while (true)
                {
                    try
                    {
                        int choice = PrintMenu(nav, "Choose an operation", menuItems);
                        if (choice == menuItems.Count - 1 || choice == -1)
                        {
                            return;
                        }
                        else if (choice == menuItems.Count - 2)
                        {
                            ChangeLanguageMenu(nav);
                        }
                        else if (choice == menuItems.Count - 3)
                        {
                            VocabulariesMenu(nav);
                        }
                        else if (choice == menuItems.Count - 4)
                        {
                            LanguageFromMenu(nav);
                        }
                        else
                        {
                            throw new ArgumentOutOfRangeException(string.Empty, $"Chosen option has returned an unexpected value that does not define any operations ({choice})");
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(ExceptionHandler.GetUnxpectedExceptionMessage(e));
                        Console.ReadKey();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(ExceptionHandler.GetUnxpectedExceptionMessage(e));
                Console.ReadKey();
            }
        }
        void ChangePasswordAdminMenu()
        {
            Console.CursorVisible = true;
            try
            {
                Console.Write("Old password: ");
                string pass = ReadHiddenLine();
                if (pass != FileManager.ReadStringFromBinaryFile(this.adminInfoFilename))
                {
                    throw new n_ExceptionHandler.AccessViolationException("Incorrect password.");
                }
                Console.Write("New password: ");
                pass = ReadHiddenLine();
                Console.Write("Confirm password: ");
                string confirmPass = ReadHiddenLine();
                if (confirmPass != pass)
                {
                    throw new ConfirmationFailedException("Password and confirmation do not match.");
                }
                Console.CursorVisible = false;
                FileManager.WriteStringFromBinaryFile(this.adminInfoFilename, pass);
            }
            catch (ConfirmationFailedException ae)
            {
                Console.WriteLine($"Confirmation failed: {ae.Message}");
                Console.CursorVisible = false;
                Console.ReadKey();
            }
            catch (n_ExceptionHandler.AccessViolationException ave)
            {
                Console.WriteLine($"Operation refused: {ave.Message}");
                Console.CursorVisible = false;
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine(ExceptionHandler.GetUnxpectedExceptionMessage(e));
                Console.CursorVisible = false;
                Console.ReadKey();
            }
        }
        void ChangeLanguageMenu(string nav)
        {
            try
            {
                nav += (string.IsNullOrEmpty(nav) ? "" : " > ") + "change language";
                List<string> menuItems = LanguageCode.GetLanguages().Select(language => LanguageNativeName.GetName(language) + " (" + language + ")").ToList();
                int choice = PrintMenu(nav, "Select preferred UI language", true, menuItems);
                menuItems.Add("\x1b[3;37mCancel\x1b[0m");
                if (choice == menuItems.Count - 1 || choice == -1)
                {
                    return;
                }
                else
                {
                    this.uiLanguage = LanguageNativeName.GetLanguage(menuItems[choice].Split(" (")[0]);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(ExceptionHandler.GetUnxpectedExceptionMessage(e));
                Console.ReadKey();
            }
        }
        void VocabulariesAdminMenu(string nav)
        {
            try
            {
                nav += (string.IsNullOrEmpty(nav) ? "" : " > ") + "vocabularies";
                while (true)
                {
                    try
                    {
                        List<string> menuItems = FileManager.GetXmlFiles().Where(filename => Regex.IsMatch(filename, $"^{string.Join("|", LanguageCode.GetCodes())}-{string.Join("|", LanguageCode.GetCodes())}\\.[a-zA-Z]+$")).Select(filename => LanguageNativeName.GetName(LanguageCode.GetLanguage(filename.Split('-')[0])) + " to " + LanguageNativeName.GetName(LanguageCode.GetLanguage(Path.GetFileNameWithoutExtension(filename).Split('-')[1]))).ToList();
                        menuItems.Add("\x1b[3;37mCancel\x1b[0m");
                        int choice = PrintMenu(nav, "Select vocabulary", true, menuItems);
                        if (choice == menuItems.Count - 1 || choice == -1)
                        {
                            return;
                        }
                        else
                        {
                            VocabularyAdminMenu(nav, menuItems[choice].Split(" to ")[0], menuItems[choice].Split(" to ")[1]);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(ExceptionHandler.GetUnxpectedExceptionMessage(e));
                        Console.ReadKey();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(ExceptionHandler.GetUnxpectedExceptionMessage(e));
                Console.ReadKey();
            }
        }
        void VocabulariesMenu(string nav)
        {
            try
            {
                nav += (string.IsNullOrEmpty(nav) ? "" : " > ") + "vocabularies";
                while (true)
                {
                    try
                    {
                        List<string> menuItems = FileManager.GetXmlFiles().Where(filename => Regex.IsMatch(filename, $"^{string.Join("|", LanguageCode.GetCodes())}-{string.Join("|", LanguageCode.GetCodes())}\\.[a-zA-Z]+$")).Select(filename => LanguageNativeName.GetName(LanguageCode.GetLanguage(filename.Split('-')[0])) + " to " + LanguageNativeName.GetName(LanguageCode.GetLanguage(Path.GetFileNameWithoutExtension(filename).Split('-')[1]))).ToList();
                        menuItems.Add("\x1b[3;37mCancel\x1b[0m");
                        int choice = PrintMenu(nav, "Select vocabulary", true, menuItems);
                        if (choice == menuItems.Count - 1 || choice == -1)
                        {
                            return;
                        }
                        else
                        {
                            VocabularyMenu(nav, menuItems[choice].Split(" to ")[0], menuItems[choice].Split(" to ")[1]);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(ExceptionHandler.GetUnxpectedExceptionMessage(e));
                        Console.ReadKey();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(ExceptionHandler.GetUnxpectedExceptionMessage(e));
                Console.ReadKey();
            }
        }
        void LanguageFromAdminMenu(string nav)
        {
            try
            {
                nav += (string.IsNullOrEmpty(nav) ? "" : " > ") + "from";
                while (true)
                {
                    try
                    {
                        List<string> menuItems = LanguageCode.GetLanguages().Select(language => LanguageNativeName.GetName(language) + " (" + language + ")").ToList();
                        menuItems.Add("\x1b[3;37mCancel\x1b[0m");
                        int choice = PrintMenu(nav, "Select language to translate FROM", true, menuItems);
                        if (choice == menuItems.Count - 1 || choice == -1)
                        {
                            return;
                        }
                        else
                        {
                            LanguageToAdminMenu(nav, menuItems[choice]);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(ExceptionHandler.GetUnxpectedExceptionMessage(e));
                        Console.ReadKey();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(ExceptionHandler.GetUnxpectedExceptionMessage(e));
                Console.ReadKey();
            }
        }
        void LanguageFromMenu(string nav)
        {
            try
            {
                nav += (string.IsNullOrEmpty(nav) ? "" : " > ") + "from";
                while (true)
                {
                    try
                    {
                        List<string> menuItems = LanguageCode.GetLanguages().Select(language => LanguageNativeName.GetName(language) + " (" + language + ")").ToList();
                        menuItems.Add("\x1b[3;37mCancel\x1b[0m");
                        int choice = PrintMenu(nav, "Select language to translate FROM", true, menuItems);
                        if (choice == menuItems.Count - 1 || choice == -1)
                        {
                            return;
                        }
                        else
                        {
                            LanguageToMenu(nav, menuItems[choice]);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(ExceptionHandler.GetUnxpectedExceptionMessage(e));
                        Console.ReadKey();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(ExceptionHandler.GetUnxpectedExceptionMessage(e));
                Console.ReadKey();
            }
        }
        void LanguageToAdminMenu(string nav, string languageFrom)
        {
            try
            {
                nav += (string.IsNullOrEmpty(nav) ? "" : " > ") + "to";
                while (true)
                {
                    try
                    {
                        List<string> menuItems = LanguageCode.GetLanguages().Select(language => LanguageNativeName.GetName(language) + " (" + language + ")" + (FileManager.IsFileExisting($"{LanguageCode.GetCode(LanguageNativeName.GetLanguage(languageFrom.Split(" (")[0]))}-{LanguageCode.GetCode(language)}".ToXmlFile()) ? "" : " \x1b[90m(create)\x1b[0m")).ToList();
                        menuItems.Remove(languageFrom);
                        menuItems.Add("\x1b[3;37mCancel\x1b[0m");
                        int choice = PrintMenu(nav, $"Select language to translate from {languageFrom} TO", true, menuItems);
                        if (choice == menuItems.Count - 1 || choice == -1)
                        {
                            return;
                        }
                        else
                        {
                            VocabularyAdminMenu(nav, languageFrom.Split(" (")[0], menuItems[choice].Split(" (")[0]);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(ExceptionHandler.GetUnxpectedExceptionMessage(e));
                        Console.ReadKey();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(ExceptionHandler.GetUnxpectedExceptionMessage(e));
                Console.ReadKey();
            }
        }
        void LanguageToMenu(string nav, string languageFrom)
        {
            try
            {
                nav += (string.IsNullOrEmpty(nav) ? "" : " > ") + "to";
                while (true)
                {
                    try
                    {
                        List<(string, bool)> menuItems = LanguageCode.GetLanguages().Select(language => (LanguageNativeName.GetName(language) + " (" + language + ")", FileManager.IsFileExisting($"{LanguageCode.GetCode(LanguageNativeName.GetLanguage(languageFrom.Split(" (")[0]))}-{LanguageCode.GetCode(language)}".ToXmlFile()))).ToList();
                        menuItems.Remove((languageFrom, true));
                        menuItems.Remove((languageFrom, false));
                        menuItems.Add(("\x1b[3;37mCancel\x1b[0m", true));
                        int choice = PrintMenu(nav, $"Select language to translate from {languageFrom} TO", true, menuItems);
                        if (choice == menuItems.Count - 1 || choice == -1)
                        {
                            return;
                        }
                        else
                        {
                            VocabularyMenu(nav, languageFrom.Split(" (")[0], menuItems[choice].Item1.Split(" (")[0]);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(ExceptionHandler.GetUnxpectedExceptionMessage(e));
                        Console.ReadKey();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(ExceptionHandler.GetUnxpectedExceptionMessage(e));
                Console.ReadKey();
            }
        }
        void VocabularyAdminMenu(string nav, string languageFrom, string languageTo)
        {
            try
            {
                nav += (string.IsNullOrEmpty(nav) ? "" : " > ") + $"{new Vocabulary(LanguageCode.GetCode(LanguageNativeName.GetLanguage(languageFrom)), LanguageCode.GetCode(LanguageNativeName.GetLanguage(languageTo))).Name}";
                Vocabulary vocabulary = new(LanguageCode.GetCode(LanguageNativeName.GetLanguage(languageFrom)), LanguageCode.GetCode(LanguageNativeName.GetLanguage(languageTo)));
                while (true)
                {
                    try
                    {
                        vocabulary.Load();
                        List<string> menuItems = vocabulary.WordsDict.Select(item => item.Key.ToString() + " - " + string.Join(" / ", (item.Value.Count > 3) ? item.Value.Select(word => word.Spelling).Take(3).Append("...") : item.Value.Select(word => word.Spelling))).ToList();
                        menuItems.Add("\x1b[3;95mAdd new word\x1b[0m");
                        menuItems.Add("\x1b[3;93mSave vocabulary to text file\x1b[0m");
                        menuItems.Add("\x1b[3;91mDelete vocabulary\x1b[0m");
                        menuItems.Add("\x1b[3;37mBack\x1b[0m");
                        int choice = PrintMenu(nav, $"{languageFrom} to {languageTo} vocabulary: {vocabulary.Keys.Count} words, {vocabulary.Values.Select(set => set.Count).Sum()} translations", true, menuItems);
                        if (choice == menuItems.Count - 1 || choice == -1)
                        {
                            return;
                        }
                        else if (choice == menuItems.Count - 2)
                        {
                            if (DeleteVocabularyConfirmationMenu(nav, vocabulary))
                            {
                                return;
                            }
                        }
                        else if (choice == menuItems.Count - 3)
                        {
                            SaveVocabularyToTextFileMenu(nav, vocabulary);
                        }
                        else if (choice == menuItems.Count - 4)
                        {
                            AddWordMenu(nav, vocabulary);
                        }
                        else
                        {
                            WordAdminMenu(nav, vocabulary, choice);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(ExceptionHandler.GetUnxpectedExceptionMessage(e));
                        Console.ReadKey();
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(ExceptionHandler.GetUnxpectedExceptionMessage(e));
                Console.ReadKey();
            }
        }
        void VocabularyMenu(string nav, string languageFrom, string languageTo)
        {
            try
            {
                nav += (string.IsNullOrEmpty(nav) ? "" : " > ") + $"{new Vocabulary(LanguageCode.GetCode(LanguageNativeName.GetLanguage(languageFrom)), LanguageCode.GetCode(LanguageNativeName.GetLanguage(languageTo))).Name}";
                Vocabulary vocabulary = new(LanguageCode.GetCode(LanguageNativeName.GetLanguage(languageFrom)), LanguageCode.GetCode(LanguageNativeName.GetLanguage(languageTo)));
                List<string> menuItems =
                [
                    "\x1b[3;93mSave vocabulary to text file\x1b[0m",
                    "\x1b[3;37mBack\x1b[0m"
                ];
                while (true)
                {
                    try
                    {
                        vocabulary.Load();
                        int choice = PrintVocabularyAsMenu(nav, vocabulary, true, menuItems);
                        if (choice == vocabulary.Count + menuItems.Count - 1 || choice == -1)
                        {
                            return;
                        }
                        else if (choice == vocabulary.Count + menuItems.Count - 2)
                        {
                            SaveVocabularyToTextFileMenu(nav, vocabulary);
                        }
                        else
                        {
                            WordMenu(nav, vocabulary, choice);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(ExceptionHandler.GetUnxpectedExceptionMessage(e));
                        Console.ReadKey();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(ExceptionHandler.GetUnxpectedExceptionMessage(e));
                Console.ReadKey();
            }
        }
        bool DeleteVocabularyConfirmationMenu(string nav, Vocabulary vocabulary)
        {
            nav += (string.IsNullOrEmpty(nav) ? "" : " > ") + "delete";
            while (nav.Length > Console.WindowWidth)
            {
                string[] nav_arr = nav.Split(' ').Skip(1).ToArray();
                if (nav_arr.Length <= 1)
                {
                    break;
                }
                nav_arr[0] = "...";
                nav = string.Join(" ", nav_arr);
            }
            Console.Clear();
            Console.Write("\x1b[3J");
            string confirmationLine = $"DELETE {vocabulary.Name}";
            Console.WriteLine($"\x1b[34m{nav}\x1b[0m");
            Console.Write($"Type \"{confirmationLine}\" to proceed: ");
            if (Console.ReadLine() == confirmationLine)
            {
                File.Delete(vocabulary.Name.ToXmlFile());
                return true;
            }
            else
            {
                Console.WriteLine("Deletion was canselled.");
                Console.ReadKey();
                return false;
            }
        }
        void AddWordMenu(string nav, Vocabulary vocabulary)
        {
            Console.CursorVisible = true;
            try
            {
                string tempNav = nav + (string.IsNullOrEmpty(nav) ? "" : " > ") + "add word";
                string spelling = null;
                string transcription = null;
                string t_spelling = null;
                string t_transcription = null;
                while (tempNav.Length > Console.WindowWidth)
                {
                    string[] nav_arr = tempNav.Split(' ').Skip(1).ToArray();
                    if (nav_arr.Length <= 1)
                    {
                        break;
                    }
                    nav_arr[0] = "...";
                    tempNav = string.Join(" ", nav_arr);
                }
                Console.Clear();
                Console.Write("\x1b[3J");
                Console.WriteLine($"\x1b[34m{tempNav}\x1b[0m");
                Console.Write($"Enter {LanguageNativeName.GetName(LanguageCode.GetLanguage(vocabulary.LanguaveCodeFrom))} word's spelling: ");
                spelling = Console.ReadLine();
                if (string.IsNullOrEmpty (spelling))
                {
                    throw new EmptySpellingException("Word's spelling cannot be empty.");
                }
                Console.Write($"Enter (or skip) {LanguageNativeName.GetName(LanguageCode.GetLanguage(vocabulary.LanguaveCodeFrom))} word's transcription: ");
                transcription = Console.ReadLine();
                if (vocabulary.ContainsKey(new ForeignWord(spelling, transcription)))
                {
                    throw new DublicateWordException("This word already exists in this vocabulary.");
                }
                Console.Write($"Enter {LanguageNativeName.GetName(LanguageCode.GetLanguage(vocabulary.LanguaveCodeTo))} word's spelling: ");
                t_spelling = Console.ReadLine();
                if (string.IsNullOrEmpty(t_spelling))
                {
                    throw new EmptySpellingException("Word's spelling cannot be empty.");
                }
                Console.Write($"Enter (or skip) {LanguageNativeName.GetName(LanguageCode.GetLanguage(vocabulary.LanguaveCodeTo))} word's transcription: ");
                t_transcription = Console.ReadLine();
                Console.CursorVisible = false;
                vocabulary.Add(new ForeignWord(spelling, transcription), new ForeignWord(t_spelling, t_transcription));
                vocabulary.Save();
                WordAdminMenu(nav, vocabulary, vocabulary.Keys.ToList().IndexOf(new ForeignWord(spelling, transcription)));
            }
            catch (VocabularyException ve)
            {
                Console.WriteLine();
                Console.WriteLine(ve.Message);
                Console.CursorVisible = false;
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine(ExceptionHandler.GetUnxpectedExceptionMessage(e));
                Console.CursorVisible = false;
                Console.ReadKey();
            }
        }
        void WordAdminMenu(string nav, Vocabulary vocabulary, int foreignWordIndex)
        {
            try
            {
                while (true)
                {
                    try
                    {
                        KeyValuePair<ForeignWord, SortedSet<ForeignWord>> foreignWord = vocabulary.ElementAt(foreignWordIndex);
                        IEnumerable<KeyValuePair<ForeignWord, SortedSet<ForeignWord>>> foreignWordUsages = vocabulary.Where(word => IsContaining(word.Key.Spelling, vocabulary.ElementAt(foreignWordIndex).Key.Spelling) && !EqualityComparer<KeyValuePair<ForeignWord, SortedSet<ForeignWord>>>.Default.Equals(word, foreignWord));
                        IEnumerable<KeyValuePair<ForeignWord, SortedSet<ForeignWord>>> foreignWordSimilars = vocabulary.Where(word => IsContaining(word.Key.Spelling, vocabulary.ElementAt(foreignWordIndex).Key.Spelling.Split(separators, StringSplitOptions.RemoveEmptyEntries)) && !EqualityComparer<KeyValuePair<ForeignWord, SortedSet<ForeignWord>>>.Default.Equals(word, foreignWord));
                        List<(string, bool)> menuItems =
                        [
                            ("\x1b[3;93mAdd word to text file\x1b[0m", true),
                            ($"\x1b[3;92mView word's usages ({foreignWordUsages.Count()})\x1b[0m", foreignWordUsages.Any()),
                            ($"\x1b[3;92mView similar words ({foreignWordSimilars.Count()})\x1b[0m", foreignWordSimilars.Any()),
                            ("\x1b[3;95mEdit word spelling\x1b[0m", true),
                            ("\x1b[3;95mEdit word transcription\x1b[0m", true),
                            ("\x1b[3;95mManage word translations\x1b[0m", true),
                            ("\x1b[3;91mDelete word\x1b[0m", true),
                            ("\x1b[3;37mBack\x1b[0m", true)
                        ];
                        string tempNav = nav + (string.IsNullOrEmpty(nav) ? "" : " > ") + foreignWord.Key.Spelling;
                        int choice = PrintMenu(tempNav, $"{foreignWord.Key} - {string.Join(" / ", foreignWord.Value.Select(word => word.Spelling))}", menuItems);
                        if (choice == menuItems.Count - 1 || choice == -1)
                        {
                            return;
                        }
                        else if (choice == menuItems.Count - 2)
                        {
                            vocabulary.Remove(foreignWord);
                            vocabulary.Save();
                            return;
                        }
                        else if (choice == menuItems.Count - 3)
                        {
                            TranslationsAdminMenu(tempNav, vocabulary, foreignWordIndex);
                        }
                        else if (choice == menuItems.Count - 4)
                        {
                            EditWordTranscriptionMenu(tempNav, vocabulary, foreignWordIndex);
                        }
                        else if (choice == menuItems.Count - 5)
                        {
                            EditWordSpellingMenu(tempNav, vocabulary, foreignWordIndex);
                        }
                        else if (choice == menuItems.Count - 6)
                        {
                            SimilarWordsAdminMenu(tempNav, vocabulary, foreignWord);
                        }
                        else if (choice == menuItems.Count - 7)
                        {
                            WordUsagesAdminMenu(tempNav, vocabulary, foreignWord);
                        }
                        else if (choice == menuItems.Count - 8)
                        {
                            SaveWordToTextFileMenu(tempNav, foreignWord);
                        }
                        else
                        {
                            throw new ArgumentOutOfRangeException(string.Empty, $"Chosen option has returned an unexpected value that does not define any operations ({choice})");
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(ExceptionHandler.GetUnxpectedExceptionMessage(e));
                        Console.ReadKey();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(ExceptionHandler.GetUnxpectedExceptionMessage(e));
                Console.ReadKey();
            }
        }
        void WordMenu(string nav, Vocabulary vocabulary, int foreignWordIndex)
        {
            try
            {
                while (true)
                {
                    try
                    {
                        KeyValuePair<ForeignWord, SortedSet<ForeignWord>> foreignWord = vocabulary.ElementAt(foreignWordIndex);
                        IEnumerable<KeyValuePair<ForeignWord, SortedSet<ForeignWord>>> foreignWordUsages = vocabulary.Where(word => IsContaining(word.Key.Spelling, vocabulary.ElementAt(foreignWordIndex).Key.Spelling) && !EqualityComparer<KeyValuePair<ForeignWord, SortedSet<ForeignWord>>>.Default.Equals(word, foreignWord));
                        IEnumerable<KeyValuePair<ForeignWord, SortedSet<ForeignWord>>> foreignWordSimilars = vocabulary.Where(word => IsContaining(word.Key.Spelling, vocabulary.ElementAt(foreignWordIndex).Key.Spelling.Split(separators, StringSplitOptions.RemoveEmptyEntries)) && !EqualityComparer<KeyValuePair<ForeignWord, SortedSet<ForeignWord>>>.Default.Equals(word, foreignWord));
                        List<(string, bool)> menuItems =
                        [
                            ("\x1b[3;93mAdd word to text file\x1b[0m", true),
                            ($"\x1b[3;92mView word's usages ({foreignWordUsages.Count()})\x1b[0m", foreignWordUsages.Any()),
                            ($"\x1b[3;92mView similar words ({foreignWordSimilars.Count()})\x1b[0m", foreignWordSimilars.Any()),
                            ("\x1b[3;37mBack\x1b[0m", true)
                        ];
                        string tempNav = nav + (string.IsNullOrEmpty(nav) ? "" : " > ") + foreignWord.Key.Spelling;
                        int choice = PrintMenu(tempNav, $"{foreignWord.Key} - {string.Join(" / ", foreignWord.Value.Select(word => word.Spelling))}", menuItems);
                        if (choice == menuItems.Count - 1 || choice == -1)
                        {
                            return;
                        }
                        else if (choice == menuItems.Count - 2)
                        {
                            SimilarWordsMenu(tempNav, vocabulary, foreignWord);
                        }
                        else if (choice == menuItems.Count - 3)
                        {
                            WordUsagesMenu(tempNav, vocabulary, foreignWord);
                        }
                        else if (choice == menuItems.Count - 4)
                        {
                            SaveWordToTextFileMenu(nav, foreignWord);
                        }
                        else
                        {
                            throw new ArgumentOutOfRangeException(string.Empty, $"Chosen option has returned an unexpected value that does not define any operations ({choice})");
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(ExceptionHandler.GetUnxpectedExceptionMessage(e));
                        Console.ReadKey();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(ExceptionHandler.GetUnxpectedExceptionMessage(e));
                Console.ReadKey();
            }
        }
        void WordUsagesAdminMenu(string nav, Vocabulary vocabulary, KeyValuePair<ForeignWord, SortedSet<ForeignWord>> foreignWord)
        {
            try
            {
                nav += (string.IsNullOrEmpty(nav) ? "" : " > ") + "usages";
                while (true)
                {
                    try
                    {
                        List<string> menuItems = vocabulary.Where(word => IsContaining(word.Key.Spelling, foreignWord.Key.Spelling) && !EqualityComparer<KeyValuePair<ForeignWord, SortedSet<ForeignWord>>>.Default.Equals(word, foreignWord)).Select(item => item.Key.ToString() + " - " + string.Join(" / ", item.Value.Count > 3 ? item.Value.Select(word => word.Spelling).Append("...").Take(3) : item.Value.Select(word => word.Spelling))).ToList();
                        menuItems.Add("\x1b[3;37mBack\x1b[0m");
                        int choice = PrintMenu(nav, $"Usages of {foreignWord.Key}", true, menuItems);
                        if (choice == menuItems.Count - 1 || choice == -1)
                        {
                            return;
                        }
                        else
                        {
                            WordAdminMenu(nav, vocabulary, vocabulary.ToList().IndexOf(vocabulary.Where(word => IsContaining(word.Key.Spelling, foreignWord.Key.Spelling) && !EqualityComparer<KeyValuePair<ForeignWord, SortedSet<ForeignWord>>>.Default.Equals(word, foreignWord)).ElementAt(choice)));
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(ExceptionHandler.GetUnxpectedExceptionMessage(e));
                        Console.ReadKey();
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(ExceptionHandler.GetUnxpectedExceptionMessage(e));
                Console.ReadKey();
            }
        }
        void WordUsagesMenu(string nav, Vocabulary vocabulary, KeyValuePair<ForeignWord, SortedSet<ForeignWord>> foreignWord)
        {
            try
            {
                nav += (string.IsNullOrEmpty(nav) ? "" : " > ") + $"usages";
                List<string> menuItems =
                [
                    "\x1b[3;37mBack\x1b[0m"
                ];
                while (true)
                {
                    try
                    {
                        int choice = PrintVocabularyAsMenu(nav, $"Usages of {foreignWord.Key}", vocabulary, new Func<KeyValuePair<ForeignWord, SortedSet<ForeignWord>>, bool> (word => IsContaining(word.Key.Spelling, foreignWord.Key.Spelling) && !EqualityComparer<KeyValuePair<ForeignWord, SortedSet<ForeignWord>>>.Default.Equals(word, foreignWord)), true, menuItems);
                        if (choice == vocabulary.Count + menuItems.Count - 1 || choice == -1)
                        {
                            return;
                        }
                        else
                        {
                            WordMenu(nav, vocabulary, choice);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(ExceptionHandler.GetUnxpectedExceptionMessage(e));
                        Console.ReadKey();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(ExceptionHandler.GetUnxpectedExceptionMessage(e));
                Console.ReadKey();
            }
        }
        void SimilarWordsAdminMenu(string nav, Vocabulary vocabulary, KeyValuePair<ForeignWord, SortedSet<ForeignWord>> foreignWord)
        {
            try
            {
                nav += (string.IsNullOrEmpty(nav) ? "" : " > ") + "similars";
                while (true)
                {
                    try
                    {
                        List<string> menuItems = vocabulary.Where(word => IsContaining(word.Key.Spelling, foreignWord.Key.Spelling.Split(separators, StringSplitOptions.RemoveEmptyEntries)) && !EqualityComparer<KeyValuePair<ForeignWord, SortedSet<ForeignWord>>>.Default.Equals(word, foreignWord)).Select(item => item.Key.ToString() + " - " + string.Join(" / ", item.Value.Count > 3 ? item.Value.Select(word => word.Spelling).Append("...").Take(3) : item.Value.Select(word => word.Spelling))).ToList();
                        menuItems.Add("\x1b[3;37mBack\x1b[0m");
                        int choice = PrintMenu(nav, $"Similars to {foreignWord.Key}", true, menuItems);
                        if (choice == menuItems.Count - 1 || choice == -1)
                        {
                            return;
                        }
                        else
                        {
                            WordAdminMenu(nav, vocabulary, vocabulary.ToList().IndexOf(vocabulary.Where(word => IsContaining(word.Key.Spelling, foreignWord.Key.Spelling.Split(separators, StringSplitOptions.RemoveEmptyEntries)) && !EqualityComparer<KeyValuePair<ForeignWord, SortedSet<ForeignWord>>>.Default.Equals(word, foreignWord)).ElementAt(choice)));
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(ExceptionHandler.GetUnxpectedExceptionMessage(e));
                        Console.ReadKey();
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(ExceptionHandler.GetUnxpectedExceptionMessage(e));
                Console.ReadKey();
            }
        }
        void SimilarWordsMenu(string nav, Vocabulary vocabulary, KeyValuePair<ForeignWord, SortedSet<ForeignWord>> foreignWord)
        {
            try
            {
                nav += (string.IsNullOrEmpty(nav) ? "" : " > ") + $"similars";
                List<string> menuItems =
                [
                    "\x1b[3;37mBack\x1b[0m"
                ];
                while (true)
                {
                    try
                    {
                        int choice = PrintVocabularyAsMenu(nav, $"Similars to {foreignWord.Key}", vocabulary, new Func<KeyValuePair<ForeignWord, SortedSet<ForeignWord>>, bool>(word => IsContaining(word.Key.Spelling, foreignWord.Key.Spelling.Split(separators, StringSplitOptions.RemoveEmptyEntries)) && !EqualityComparer<KeyValuePair<ForeignWord, SortedSet<ForeignWord>>>.Default.Equals(word, foreignWord)), true, menuItems);
                        if (choice == vocabulary.Count + menuItems.Count - 1 || choice == -1)
                        {
                            return;
                        }
                        else
                        {
                            WordMenu(nav, vocabulary, choice);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(ExceptionHandler.GetUnxpectedExceptionMessage(e));
                        Console.ReadKey();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(ExceptionHandler.GetUnxpectedExceptionMessage(e));
                Console.ReadKey();
            }
        }
        void TranslationsAdminMenu(string nav, Vocabulary vocabulary, int foreignWordIndex)
        {
            try
            {
                nav += (string.IsNullOrEmpty(nav) ? "" : " > ") + "translations";
                while (true)
                {
                    try
                    {
                        KeyValuePair<ForeignWord, SortedSet<ForeignWord>> foreignWord = vocabulary.ElementAt(foreignWordIndex);
                        List<string> menuItems = [];
                        foreach (ForeignWord translation in foreignWord.Value)
                        {
                            menuItems.Add(translation.ToString());
                        }
                        menuItems.Add("\x1b[3;95mAdd new translation\x1b[0m");
                        menuItems.Add("\x1b[3;37mBack\x1b[0m");
                        int choice = PrintMenu(nav, $"{foreignWord.Key} - ...", menuItems);
                        if (choice == menuItems.Count - 1 || choice == -1)
                        {
                            return;
                        }
                        else if (choice == menuItems.Count - 2)
                        {
                            AddTranslationAdminMenu(nav, vocabulary, foreignWordIndex);
                        }
                        else
                        {
                            TranslationAdminMenu(nav, vocabulary, foreignWordIndex, choice);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(ExceptionHandler.GetUnxpectedExceptionMessage(e));
                        Console.ReadKey();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(ExceptionHandler.GetUnxpectedExceptionMessage(e));
                Console.ReadKey();
            }
        }
        void AddTranslationAdminMenu(string nav, Vocabulary vocabulary, int foreignWordIndex)
        {
            Console.CursorVisible = true;
            try
            {
                nav += (string.IsNullOrEmpty(nav) ? "" : " > ") + "add translation";
                string spelling = null;
                string transcription = null;
                while (nav.Length > Console.WindowWidth)
                {
                    string[] nav_arr = nav.Split(' ').Skip(1).ToArray();
                    if (nav_arr.Length <= 1)
                    {
                        break;
                    }
                    nav_arr[0] = "...";
                    nav = string.Join(" ", nav_arr);
                }
                Console.Clear();
                Console.Write("\x1b[3J");
                Console.WriteLine($"\x1b[34m{nav}\x1b[0m");
                Console.Write($"Enter {LanguageNativeName.GetName(LanguageCode.GetLanguage(vocabulary.LanguaveCodeTo))} word's spelling: ");
                spelling = Console.ReadLine();
                if (string.IsNullOrEmpty(spelling))
                {
                    throw new EmptySpellingException("Word's spelling cannot be empty.");
                }
                Console.Write($"Enter (or skip) {LanguageNativeName.GetName(LanguageCode.GetLanguage(vocabulary.LanguaveCodeTo))} word's transcription: ");
                transcription = Console.ReadLine();
                if (vocabulary.ElementAt(foreignWordIndex).Value.Contains(new ForeignWord(spelling, transcription)))
                {
                    throw new DublicateWordException("This translation already exists.");
                }
                Console.CursorVisible = false;
                KeyValuePair<ForeignWord, SortedSet<ForeignWord>> temp = vocabulary.ElementAt(foreignWordIndex);
                vocabulary.Remove(temp);
                temp.Value.Add(new ForeignWord(spelling, transcription));
                vocabulary.Add(temp);
                vocabulary.Save();
            }
            catch (VocabularyException ve)
            {
                Console.WriteLine();
                Console.WriteLine(ve.Message);
                Console.CursorVisible = false;
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine(ExceptionHandler.GetUnxpectedExceptionMessage(e));
                Console.CursorVisible = false;
                Console.ReadKey();
            }
        }
        void TranslationAdminMenu(string nav, Vocabulary vocabulary, int foreignWordIndex, int translationIndex)
        {
            try
            {
                List<(string, bool)> menuItems =
                [
                    ("\x1b[3;95mEdit translation spelling\x1b[0m", true),
                    ("\x1b[3;95mEdit translation transcription\x1b[0m", true),
                    ("\x1b[3;91mDelete translation\x1b[0m", vocabulary.ElementAt(foreignWordIndex).Value.Count > 1),
                    ("\x1b[3;37mBack\x1b[0m", true)
                ];
                while (true)
                {
                    try
                    {
                        KeyValuePair<ForeignWord, SortedSet<ForeignWord>> foreignWord = vocabulary.ElementAt(foreignWordIndex);
                        string tempNav = nav + (string.IsNullOrEmpty(nav) ? "" : " > ") + foreignWord.Value.ElementAt(translationIndex).Spelling;
                        int choice = PrintMenu(tempNav, $"{foreignWord.Key} - {foreignWord.Value.ElementAt(translationIndex)}", menuItems);
                        if (choice == menuItems.Count - 1 || choice == -1)
                        {
                            return;
                        }
                        else if (choice == menuItems.Count - 2)
                        {
                            SortedSet<ForeignWord> temp = foreignWord.Value;
                            temp.Remove(foreignWord.Value.ElementAt(translationIndex));
                            vocabulary.Remove(foreignWord);
                            vocabulary.Add(foreignWord.Key, temp);
                            vocabulary.Save();
                            return;
                        }
                        else if (choice == menuItems.Count - 3)
                        {
                            EditTranslationTranscriptionMenu(tempNav, vocabulary, foreignWordIndex, translationIndex);
                        }
                        else if (choice == menuItems.Count - 4)
                        {
                            EditTranslationSpellingMenu(tempNav, vocabulary, foreignWordIndex, translationIndex);
                        }
                        else
                        {
                            throw new ArgumentOutOfRangeException(string.Empty, $"Chosen option has returned an unexpected value that does not define any operations ({choice})");
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(ExceptionHandler.GetUnxpectedExceptionMessage(e));
                        Console.ReadKey();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(ExceptionHandler.GetUnxpectedExceptionMessage(e));
                Console.ReadKey();
            }
        }
        void EditWordSpellingMenu(string nav, Vocabulary vocabulary, int foreignWordIndex)
        {
            try
            {
                nav += (string.IsNullOrEmpty(nav) ? "" : " > ") + "edit spelling";
                Console.CursorVisible = true;
                KeyValuePair<ForeignWord, SortedSet<ForeignWord>> foreignWord = vocabulary.ElementAt(foreignWordIndex);
                string spelling = ReadLineWirhSuggestion(nav, $"Enter {LanguageNativeName.GetName(LanguageCode.GetLanguage(vocabulary.LanguaveCodeFrom))} word's spelling", foreignWord.Key.Spelling);
                if (spelling == ConsoleKey.Escape.ToString())
                {
                    Console.CursorVisible = false;
                    return;
                }
                else if (string.IsNullOrEmpty (spelling))
                {
                    throw new EmptySpellingException("Word's spelling cannot be empty.");
                }
                else if (vocabulary.ContainsKey(new ForeignWord(spelling, foreignWord.Key.Transcribtion)))
                {
                    throw new DublicateWordException("This word already exists in this vocabulary.");
                }
                Console.CursorVisible = false;
                vocabulary.Remove(foreignWord);
                vocabulary.Add(new ForeignWord(spelling, foreignWord.Key.Transcribtion), foreignWord.Value);
                vocabulary.Save();
            }
            catch (VocabularyException ve)
            {
                Console.WriteLine();
                Console.WriteLine(ve.Message);
                Console.CursorVisible = false;
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine(ExceptionHandler.GetUnxpectedExceptionMessage(e));
                Console.CursorVisible = false;
                Console.ReadKey();
            }
        }
        void EditWordTranscriptionMenu(string nav, Vocabulary vocabulary, int foreignWordIndex)
        {
            try
            {
                nav += (string.IsNullOrEmpty(nav) ? "" : " > ") + "edit translation";
                Console.CursorVisible = true;
                KeyValuePair<ForeignWord, SortedSet<ForeignWord>> foreignWord = vocabulary.ElementAt(foreignWordIndex);
                string transcription = ReadLineWirhSuggestion(nav, $"Enter {LanguageNativeName.GetName(LanguageCode.GetLanguage(vocabulary.LanguaveCodeFrom))} word's transcription", foreignWord.Key.Transcribtion);
                if (transcription == ConsoleKey.Escape.ToString())
                {
                    Console.CursorVisible = false;
                    return;
                }
                else if (vocabulary.ContainsKey(new ForeignWord(foreignWord.Key.Spelling, transcription)))
                {
                    throw new DublicateWordException("This word already exists in this vocabulary.");
                }
                Console.CursorVisible = false;
                vocabulary.Remove(foreignWord);
                vocabulary.Add(new ForeignWord(foreignWord.Key.Spelling, transcription), foreignWord.Value);
                vocabulary.Save();
            }
            catch (VocabularyException ve)
            {
                Console.WriteLine();
                Console.WriteLine(ve.Message);
                Console.CursorVisible = false;
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine(ExceptionHandler.GetUnxpectedExceptionMessage(e));
                Console.CursorVisible = false;
                Console.ReadKey();
            }
        }
        void EditTranslationSpellingMenu(string nav, Vocabulary vocabulary, int foreignWordIndex, int translationIndex)
        {
            try
            {
                nav += (string.IsNullOrEmpty(nav) ? "" : " > ") + "edit spelling";
                Console.CursorVisible = true;
                KeyValuePair<ForeignWord, SortedSet<ForeignWord>> foreignWord = vocabulary.ElementAt(foreignWordIndex);
                string spelling = ReadLineWirhSuggestion(nav, $"Enter {LanguageNativeName.GetName(LanguageCode.GetLanguage(vocabulary.LanguaveCodeTo))} word's spelling", foreignWord.Value.ElementAt(translationIndex).Spelling);
                if (spelling == ConsoleKey.Escape.ToString())
                {
                    Console.CursorVisible = false;
                    return;
                }
                else if (string.IsNullOrEmpty(spelling))
                {
                    throw new EmptySpellingException("Word's spelling cannot be empty.");
                }
                else if (foreignWord.Value.Contains(new ForeignWord(spelling, foreignWord.Value.ElementAt(translationIndex).Transcribtion)))
                {
                    throw new DublicateWordException("This word already exists in this vocabulary.");
                }
                Console.CursorVisible = false;
                SortedSet<ForeignWord> temp = foreignWord.Value;
                string tempTranscription = foreignWord.Value.ElementAt(translationIndex).Transcribtion;
                temp.Remove(foreignWord.Value.ElementAt(translationIndex));
                temp.Add(new ForeignWord(spelling, tempTranscription));
                vocabulary.Remove(foreignWord);
                vocabulary.Add(foreignWord.Key, temp);
                vocabulary.Save();
            }
            catch (VocabularyException ve)
            {
                Console.WriteLine();
                Console.WriteLine(ve.Message);
                Console.CursorVisible = false;
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine(ExceptionHandler.GetUnxpectedExceptionMessage(e));
                Console.CursorVisible = false;
                Console.ReadKey();
            }
        }
        void EditTranslationTranscriptionMenu(string nav, Vocabulary vocabulary, int foreignWordIndex, int translationIndex)
        {
            try
            {
                nav += (string.IsNullOrEmpty(nav) ? "" : " > ") + "edit transcription";
                Console.CursorVisible = true;
                KeyValuePair<ForeignWord, SortedSet<ForeignWord>> foreignWord = vocabulary.ElementAt(foreignWordIndex);
                string transcription = ReadLineWirhSuggestion(nav, $"Enter {LanguageNativeName.GetName(LanguageCode.GetLanguage(vocabulary.LanguaveCodeTo))} word's transcription", foreignWord.Value.ElementAt(translationIndex).Transcribtion);
                if (transcription == ConsoleKey.Escape.ToString())
                {
                    Console.CursorVisible = false;
                    return;
                }
                else if (foreignWord.Value.Contains(new ForeignWord(foreignWord.Value.ElementAt(translationIndex).Spelling, transcription)))
                {
                    throw new DublicateWordException("This translation already exists in this vocabulary.");
                }
                Console.CursorVisible = false;
                SortedSet<ForeignWord> temp = foreignWord.Value;
                temp.Remove(foreignWord.Value.ElementAt(translationIndex));
                string tempSpelling = foreignWord.Value.ElementAt(translationIndex).Spelling;
                temp.Add(new ForeignWord(tempSpelling, transcription));
                vocabulary.Remove(foreignWord);
                vocabulary.Add(foreignWord.Key, temp);
                vocabulary.Save();
            }
            catch (VocabularyException ve)
            {
                Console.WriteLine();
                Console.WriteLine(ve.Message);
                Console.CursorVisible = false;
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine(ExceptionHandler.GetUnxpectedExceptionMessage(e));
                Console.CursorVisible = false;
                Console.ReadKey();
            }
        }
        void SaveWordToTextFileMenu(string nav, KeyValuePair<ForeignWord, SortedSet<ForeignWord>> foreignWord)
        {
            try
            {
                nav += (string.IsNullOrEmpty(nav) ? "" : " > ") + "save";
                Console.CursorVisible = true;
                string path = ReadLineWirhSuggestion(nav, "Enter file path", Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + '\\');
                if (path == ConsoleKey.Escape.ToString())
                {
                    Console.CursorVisible = false;
                    return;
                }
                AppendWordToTextFile(foreignWord, path.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine(ExceptionHandler.GetUnxpectedExceptionMessage(e));
                Console.ReadKey();
            }
        }
        void SaveVocabularyToTextFileMenu(string nav, Vocabulary vocabulary)
        {
            try
            {
                nav += (string.IsNullOrEmpty(nav) ? "" : " > ") + "save";
                Console.CursorVisible = true;
                string path = ReadLineWirhSuggestion(nav, "Enter file path", Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + '\\');
                if (path == ConsoleKey.Escape.ToString())
                {
                    Console.CursorVisible = false;
                    return;
                }
                Console.CursorVisible = false;
                foreach (KeyValuePair<ForeignWord, SortedSet<ForeignWord>> foreignWord in vocabulary)
                {
                    AppendWordToTextFile(foreignWord, path.ToString());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(ExceptionHandler.GetUnxpectedExceptionMessage(e));
                Console.CursorVisible = false;
                Console.ReadKey();
            }
        }
        static void AppendWordToTextFile(KeyValuePair<ForeignWord, SortedSet<ForeignWord>> foreignWord, string path)
        {
            try
            {
                bool first = false;
                if (!File.Exists(path))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                    FileStream tempFs = File.Create(path);
                    using (StreamWriter tempWriter = new(tempFs))
                    {
                        string title;
                        tempWriter.Write("╔");
                        tempWriter.Write(new string('═', (int)VocabularyEnumConstants.CELLWIDTH));
                        tempWriter.Write("╤");
                        tempWriter.Write(new string('═', (int)VocabularyEnumConstants.CELLWIDTH));
                        tempWriter.Write("╤");
                        tempWriter.Write(new string('═', (int)VocabularyEnumConstants.CELLWIDTH));
                        tempWriter.Write("╤");
                        tempWriter.Write(new string('═', (int)VocabularyEnumConstants.CELLWIDTH));
                        tempWriter.WriteLine("╗");
                        tempWriter.Write('║');
                        title = "Word";
                        tempWriter.Write(title.PadLeft(((int)VocabularyEnumConstants.CELLWIDTH - title.Length) / 2 + title.Length).PadRight((int)VocabularyEnumConstants.CELLWIDTH));
                        tempWriter.Write('│');
                        title = "Transcription";
                        tempWriter.Write(title.PadLeft(((int)VocabularyEnumConstants.CELLWIDTH - title.Length) / 2 + title.Length).PadRight((int)VocabularyEnumConstants.CELLWIDTH));
                        tempWriter.Write('│');
                        title = "Translation(s)";
                        tempWriter.Write(title.PadLeft(((int)VocabularyEnumConstants.CELLWIDTH - title.Length) / 2 + title.Length).PadRight((int)VocabularyEnumConstants.CELLWIDTH));
                        tempWriter.Write('│');
                        title = "Tr. transcription(s)";
                        tempWriter.Write(title.PadLeft(((int)VocabularyEnumConstants.CELLWIDTH - title.Length) / 2 + title.Length).PadRight((int)VocabularyEnumConstants.CELLWIDTH));
                        tempWriter.WriteLine("║");
                        tempWriter.Write("╚");
                        tempWriter.Write(new string('═', (int)VocabularyEnumConstants.CELLWIDTH));
                        tempWriter.Write("╧");
                        tempWriter.Write(new string('═', (int)VocabularyEnumConstants.CELLWIDTH));
                        tempWriter.Write("╧");
                        tempWriter.Write(new string('═', (int)VocabularyEnumConstants.CELLWIDTH));
                        tempWriter.Write("╧");
                        tempWriter.Write(new string('═', (int)VocabularyEnumConstants.CELLWIDTH));
                        tempWriter.WriteLine("╝");
                        first = true;
                    }
                    tempFs.Close();
                }
                using FileStream fs = new(path, FileMode.Open, FileAccess.ReadWrite);
                int newlineCount = 0;
                if (fs.Length > 0)
                {
                    long position;
                    for (position = fs.Seek(-1, SeekOrigin.End); position >= 0; position--)
                    {
                        fs.Seek(position, SeekOrigin.Begin);
                        if (fs.ReadByte() == '\n' && ++newlineCount == 2)
                        {
                            break;
                        }
                    }
                    fs.Seek((newlineCount < 2) ? 0 : (position), SeekOrigin.Begin);
                }
                using StreamWriter writer = new(fs);
                if (first)
                {
                    writer.Write("╠");
                    writer.Write(new string('═', (int)VocabularyEnumConstants.CELLWIDTH));
                    writer.Write("╪");
                    writer.Write(new string('═', (int)VocabularyEnumConstants.CELLWIDTH));
                    writer.Write("╪");
                    writer.Write(new string('═', (int)VocabularyEnumConstants.CELLWIDTH));
                    writer.Write("╪");
                    writer.Write(new string('═', (int)VocabularyEnumConstants.CELLWIDTH));
                    writer.WriteLine("╣");
                }
                else
                {
                    writer.Write("╟");
                    writer.Write(new string('─', (int)VocabularyEnumConstants.CELLWIDTH));
                    writer.Write("┼");
                    writer.Write(new string('─', (int)VocabularyEnumConstants.CELLWIDTH));
                    writer.Write("┼");
                    writer.Write(new string('─', (int)VocabularyEnumConstants.CELLWIDTH));
                    writer.Write("┼");
                    writer.Write(new string('─', (int)VocabularyEnumConstants.CELLWIDTH));
                    writer.WriteLine("╢");
                }
                Queue<string> originalSpellings = new(foreignWord.Key.Spelling.Split(' '));
                Queue<string> originalTranscriptions = new(foreignWord.Key.Transcribtion.Split(' ').Append("]"));
                Queue<Queue<string>> translationsSpellings = new(foreignWord.Value.Select(value => new Queue<string>(value.Spelling.Split(' '))));
                Queue<Queue<string>> translationsTranscriptions = new(foreignWord.Value.Select(value => new Queue<string>(value.Transcribtion.Split(' ').Append("]"))));
                bool first11 = true;
                bool first12 = true;
                bool first21 = true;
                bool first22 = true;
                Queue<string> nextTranslationSpellings = new();
                Queue<string> nextTranslationTranscriptions = new();
                int remainingCornerWidth;
                string next = null;
                while (originalSpellings.Count > 0 || originalTranscriptions.Count > 0 || translationsSpellings.Count > 0 || translationsTranscriptions.Count > 0 || nextTranslationSpellings.Count > 0 || nextTranslationTranscriptions.Count > 0)
                {
                    writer.Write("║");
                    remainingCornerWidth = (int)VocabularyEnumConstants.CELLWIDTH;
                    if (originalSpellings.Count > 0)
                    {
                        do
                        {
                            if (first11)
                            {
                                writer.Write(" ");
                                remainingCornerWidth--;
                                first11 = false;
                            }
                            next = originalSpellings.Dequeue();
                            writer.Write(next);
                            remainingCornerWidth -= next.Length;
                            if (remainingCornerWidth > 0)
                            {
                                writer.Write(" ");
                                remainingCornerWidth--;
                            }
                        }
                        while (originalSpellings.Count > 0 && remainingCornerWidth >= originalSpellings.Peek().Length);
                    }
                    writer.Write(new string(' ', remainingCornerWidth));
                    writer.Write("│");
                    remainingCornerWidth = (int)VocabularyEnumConstants.CELLWIDTH;
                    if (originalTranscriptions.Count > 0)
                    {
                        do
                        {
                            if (first12)
                            {
                                writer.Write(" [");
                                remainingCornerWidth -= 2;
                                first12 = false;
                            }
                            next = originalTranscriptions.Dequeue();
                            writer.Write(next);
                            remainingCornerWidth -= next.Length;
                            if (remainingCornerWidth > 0)
                            {
                                writer.Write(" ");
                                remainingCornerWidth--;
                            }
                        }
                        while (originalTranscriptions.Count > 0 && remainingCornerWidth >= originalTranscriptions.Peek().Length);
                    }
                    writer.Write(new string(' ', remainingCornerWidth));
                    writer.Write("│");
                    remainingCornerWidth = (int)VocabularyEnumConstants.CELLWIDTH;
                    if (nextTranslationSpellings.Count == 0 && nextTranslationTranscriptions.Count == 0)
                    {
                        nextTranslationSpellings = (translationsSpellings.Count > 0) ? translationsSpellings.Dequeue() : new();
                        nextTranslationTranscriptions = (translationsTranscriptions.Count > 0) ? translationsTranscriptions.Dequeue() : new();
                        first21 = true;
                        first22 = true;
                    }
                    if (nextTranslationSpellings.Count > 0)
                    {
                        do
                        {
                            if (first21)
                            {
                                writer.Write(" ");
                                remainingCornerWidth--;
                                first21 = false;
                            }
                            next = nextTranslationSpellings.Dequeue();
                            writer.Write(next);
                            remainingCornerWidth -= next.Length;
                            if (remainingCornerWidth > 0)
                            {
                                writer.Write(" ");
                                remainingCornerWidth--;
                            }
                        }
                        while (nextTranslationSpellings.Count > 0 && remainingCornerWidth >= nextTranslationSpellings.Peek().Length);
                    }
                    writer.Write(new string(' ', remainingCornerWidth));
                    writer.Write("│");
                    remainingCornerWidth = (int)VocabularyEnumConstants.CELLWIDTH;
                    if (nextTranslationTranscriptions.Count > 0)
                    {
                        do
                        {
                            if (first22)
                            {
                                writer.Write(" [");
                                remainingCornerWidth -= 2;
                                first22 = false;
                            }
                            next = nextTranslationTranscriptions.Dequeue();
                            writer.Write(next);
                            remainingCornerWidth -= next.Length;
                            if (remainingCornerWidth > 0)
                            {
                                writer.Write(" ");
                                remainingCornerWidth--;
                            }
                        }
                        while (nextTranslationTranscriptions.Count > 0 && remainingCornerWidth >= nextTranslationTranscriptions.Peek().Length);
                    }
                    writer.Write(new string(' ', remainingCornerWidth));
                    writer.WriteLine("║");
                }
                writer.Write("╚");
                writer.Write(new string('═', (int)VocabularyEnumConstants.CELLWIDTH));
                writer.Write("╧");
                writer.Write(new string('═', (int)VocabularyEnumConstants.CELLWIDTH));
                writer.Write("╧");
                writer.Write(new string('═', (int)VocabularyEnumConstants.CELLWIDTH));
                writer.Write("╧");
                writer.Write(new string('═', (int)VocabularyEnumConstants.CELLWIDTH));
                writer.WriteLine("╝");
            }
            catch (Exception e)
            {
                Console.WriteLine(ExceptionHandler.GetUnxpectedExceptionMessage(e));
                Console.ReadKey();
            }
        }
        public int PrintVocabularyAsMenu(string nav, Vocabulary vocabulary, List<string> menuItems)
        {
            return PrintVocabularyAsMenu(nav, null, vocabulary, new Func<KeyValuePair<ForeignWord, SortedSet<ForeignWord>>, bool> (word => true), false, menuItems);
        }
        public int PrintVocabularyAsMenu(string nav, string title, Vocabulary vocabulary, List<string> menuItems)
        {
            return PrintVocabularyAsMenu(nav, title, vocabulary, new Func<KeyValuePair<ForeignWord, SortedSet<ForeignWord>>, bool>(word => true), false, menuItems);
        }
        public int PrintVocabularyAsMenu(string nav, Vocabulary vocabulary, bool searchBarAvailable, List<string> menuItems)
        {
            return PrintVocabularyAsMenu(nav, null, vocabulary, new Func<KeyValuePair<ForeignWord, SortedSet<ForeignWord>>, bool>(word => true), searchBarAvailable, menuItems);
        }
        public int PrintVocabularyAsMenu(string nav, string title, Vocabulary vocabulary, bool searchBarAvailable, List<string> menuItems)
        {
            return PrintVocabularyAsMenu(nav, title, vocabulary, new Func<KeyValuePair<ForeignWord, SortedSet<ForeignWord>>, bool>(word => true), searchBarAvailable, menuItems);
        }
        public int PrintVocabularyAsMenu(string nav, Vocabulary vocabulary, Func<KeyValuePair<ForeignWord, SortedSet<ForeignWord>>, bool> filter, List<string> menuItems)
        {
            return PrintVocabularyAsMenu(nav, null, vocabulary, filter, false, menuItems);
        }
        public int PrintVocabularyAsMenu(string nav, string title, Vocabulary vocabulary, Func<KeyValuePair<ForeignWord, SortedSet<ForeignWord>>, bool> filter, List<string> menuItems)
        {
            return PrintVocabularyAsMenu(nav, title, vocabulary, filter, false, menuItems);
        }
        public int PrintVocabularyAsMenu(string nav, Vocabulary vocabulary, Func<KeyValuePair<ForeignWord, SortedSet<ForeignWord>>, bool> filter, bool searchBarAvailable, List<string> menuItems)
        {
            return PrintVocabularyAsMenu(nav, null, vocabulary, filter, searchBarAvailable, menuItems);
        }
        public int PrintVocabularyAsMenu(string nav, string title, Vocabulary vocabulary, Func<KeyValuePair<ForeignWord, SortedSet<ForeignWord>>, bool> filter, bool searchBarAvailable, List<string> menuItems)
        {
            try
            {
                int currentSelection = 0;
                foreach (var word in vocabulary)
                {
                    if (filter(word))
                    {
                        break;
                    }
                    currentSelection++;
                }
                int cellWidth;
                ConsoleKeyInfo keyinfo;
                StringBuilder desiredOption = new();
                string tempNav = nav;
                string _title;
                do
                {
                    Console.Clear();
                    Console.Write("\x1b[3J");
                    while (tempNav.Length > Console.WindowWidth)
                    {
                        string[] nav_arr = tempNav.Split(' ').Skip(1).ToArray();
                        if (nav_arr.Length <= 1)
                        {
                            break;
                        }
                        nav_arr[0] = "...";
                        tempNav = string.Join(" ", nav_arr);
                    }
                    Console.WriteLine($"\x1b[34m{tempNav}\x1b[0m");
                    if (!string.IsNullOrEmpty(title))
                    {
                        Console.WriteLine($"   \x1b[1m{title}\x1b[0m\n");
                    }
                    else
                    {
                        Console.WriteLine($"   \x1b[1m{LanguageNativeName.GetName(LanguageCode.GetLanguage(vocabulary.LanguaveCodeFrom))} to {LanguageNativeName.GetName(LanguageCode.GetLanguage(vocabulary.LanguaveCodeTo))} vocabulary: {vocabulary.Keys.Count} words, {vocabulary.Values.Select(set => set.Count).Sum()} translations\x1b[0m\n");
                    }
                    if (searchBarAvailable)
                    {
                        if (currentSelection == -1)
                        {
                            Console.WriteLine($"\x1b[3;96m-> [Search: {desiredOption}|]\x1b[0m");
                        }
                        else
                        {
                            Console.WriteLine($" \x1b[3;36m[Search: {desiredOption}]\x1b[0m");
                        }
                    }
                    // Перевірка мінімальної ширини консолі
                    int minimumConsoleWidth = 40;  // Мінімальна ширина для коректного відображення
                    if (Console.WindowWidth < minimumConsoleWidth)
                    {
                        Console.Clear();
                        Console.WriteLine("Please widen your console to at least " + minimumConsoleWidth + " characters.");
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        return -1; // Повертаємось до попереднього меню або перезапускаємо цикл
                    }

                    // Перевірка ширини колонок
                    cellWidth = (Console.WindowWidth - 8) / 4;
                    if (cellWidth <= 0)
                    {
                        Console.Clear();
                        Console.WriteLine("Please widen your console to fit the content.");
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        return -1; // Повертаємось до попереднього меню або перезапускаємо цикл
                    }

                    cellWidth = (Console.WindowWidth - 8) / 4;
                    Console.Write("   ╔");
                    Console.Write(new string('═', cellWidth));
                    Console.Write("╤");
                    Console.Write(new string('═', cellWidth));
                    Console.Write("╤");
                    Console.Write(new string('═', cellWidth));
                    Console.Write("╤");
                    Console.Write(new string('═', cellWidth));
                    Console.WriteLine("╗");
                    Console.Write("   ║");
                    _title = "Word";
                    Console.Write(_title.PadLeft((cellWidth - _title.Length) / 2 + _title.Length).PadRight(cellWidth));
                    Console.Write('│');
                    _title = "Transcription";
                    Console.Write(_title.PadLeft((cellWidth - _title.Length) / 2 + _title.Length).PadRight(cellWidth));
                    Console.Write('│');
                    _title = "Translation(s)";
                    Console.Write(_title.PadLeft((cellWidth - _title.Length) / 2 + _title.Length).PadRight(cellWidth));
                    Console.Write('│');
                    _title = "Tr. transcription(s)";
                    Console.Write(_title.PadLeft((cellWidth - _title.Length) / 2 + _title.Length).PadRight(cellWidth));
                    Console.WriteLine("║");
                    if (currentSelection != -1 && currentSelection < vocabulary.Count && EqualityComparer<KeyValuePair<ForeignWord, SortedSet<ForeignWord>>>.Default.Equals(vocabulary.ElementAt(currentSelection), vocabulary.FirstOrDefault(word => filter(word) && (IsContaining(word.Key.ToString(), desiredOption.ToString()) || word.Value.Any(w => IsContaining(w.ToString(), desiredOption.ToString()))))))
                    {
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                    }
                    Console.Write("   ╠");
                    Console.Write(new string('═', cellWidth));
                    Console.Write("╪");
                    Console.Write(new string('═', cellWidth));
                    Console.Write("╪");
                    Console.Write(new string('═', cellWidth));
                    Console.Write("╪");
                    Console.Write(new string('═', cellWidth));
                    Console.WriteLine("╣");
                    KeyValuePair<ForeignWord, SortedSet<ForeignWord>> foreignWord;
                    Queue<string> originalSpellings;
                    Queue<string> originalTranscriptions;
                    Queue<Queue<string>> translationsSpellings;
                    Queue<Queue<string>> translationsTranscriptions;
                    bool first11;
                    bool first12;
                    bool first21;
                    bool first22;
                    bool cellArrowed;
                    Queue<string> nextTranslationSpellings;
                    Queue<string> nextTranslationTranscriptions;
                    int remainingCornerWidth;
                    int nextCellIndex;
                    string next;
                    for (int i = 0; i < vocabulary.Count; i++)
                    {
                        if (filter(vocabulary.ElementAt(i)) && (IsContaining(vocabulary.ElementAt(i).Key.ToString(), desiredOption.ToString()) || vocabulary.ElementAt(i).Value.Any(word => IsContaining(word.ToString(), desiredOption.ToString()))))
                        {
                            Console.ResetColor();
                            foreignWord = vocabulary.ElementAt(i);
                            originalSpellings = new(foreignWord.Key.Spelling.Split(' '));
                            originalTranscriptions = new(foreignWord.Key.Transcribtion.Split(' ').Append("]"));
                            translationsSpellings = new(foreignWord.Value.Select(value => new Queue<string>(value.Spelling.Split(' '))));
                            translationsTranscriptions = new(foreignWord.Value.Select(value => new Queue<string>(value.Transcribtion.Split(' ').Append("]"))));
                            first11 = true;
                            first12 = true;
                            first21 = true;
                            first22 = true;
                            cellArrowed = false;
                            nextTranslationSpellings = new();
                            nextTranslationTranscriptions = new();
                            while (originalSpellings.Count > 0 || originalTranscriptions.Count > 0 || translationsSpellings.Count > 0 || translationsTranscriptions.Count > 0 || nextTranslationSpellings.Count > 0 || nextTranslationTranscriptions.Count > 0)
                            {
                                if (!cellArrowed && i == currentSelection)
                                {
                                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                                    Console.Write("-> ║");
                                    cellArrowed = true;
                                }
                                else if (i == currentSelection)
                                {
                                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                                    Console.Write("   ║");
                                }
                                else
                                {
                                    Console.Write("   ║");
                                }
                                remainingCornerWidth = cellWidth;
                                if (originalSpellings.Count > 0)
                                {
                                    do
                                    {
                                        if (first11)
                                        {
                                            Console.Write(" ");
                                            remainingCornerWidth--;
                                            first11 = false;
                                        }
                                        next = originalSpellings.Dequeue();
                                        Console.Write(next);
                                        remainingCornerWidth -= next.Length;
                                        if (remainingCornerWidth > 0)
                                        {
                                            Console.Write(" ");
                                            remainingCornerWidth--;
                                        }
                                    }
                                    while (originalSpellings.Count > 0 && remainingCornerWidth >= originalSpellings.Peek().Length);
                                }
                                Console.Write(new string(' ', remainingCornerWidth));
                                Console.Write("│");
                                remainingCornerWidth = cellWidth;
                                if (originalTranscriptions.Count > 0)
                                {
                                    do
                                    {
                                        if (first12)
                                        {
                                            Console.Write(" [");
                                            remainingCornerWidth -= 2;
                                            first12 = false;
                                        }
                                        next = originalTranscriptions.Dequeue();
                                        Console.Write(next);
                                        remainingCornerWidth -= next.Length;
                                        if (remainingCornerWidth > 0)
                                        {
                                            Console.Write(" ");
                                            remainingCornerWidth--;
                                        }
                                    }
                                    while (originalTranscriptions.Count > 0 && remainingCornerWidth >= originalTranscriptions.Peek().Length);
                                }
                                Console.Write(new string(' ', remainingCornerWidth));
                                Console.Write("│");
                                remainingCornerWidth = cellWidth;
                                if (nextTranslationSpellings.Count == 0 && nextTranslationTranscriptions.Count == 0)
                                {
                                    nextTranslationSpellings = (translationsSpellings.Count > 0) ? translationsSpellings.Dequeue() : new();
                                    nextTranslationTranscriptions = (translationsTranscriptions.Count > 0) ? translationsTranscriptions.Dequeue() : new();
                                    first21 = true;
                                    first22 = true;
                                }
                                if (nextTranslationSpellings.Count > 0)
                                {
                                    do
                                    {
                                        if (first21)
                                        {
                                            Console.Write(" ");
                                            remainingCornerWidth--;
                                            first21 = false;
                                        }
                                        next = nextTranslationSpellings.Dequeue();
                                        Console.Write(next);
                                        remainingCornerWidth -= next.Length;
                                        if (remainingCornerWidth > 0)
                                        {
                                            Console.Write(" ");
                                            remainingCornerWidth--;
                                        }
                                    }
                                    while (nextTranslationSpellings.Count > 0 && remainingCornerWidth >= nextTranslationSpellings.Peek().Length);
                                }
                                Console.Write(new string(' ', remainingCornerWidth));
                                Console.Write("│");
                                remainingCornerWidth = cellWidth;
                                if (nextTranslationTranscriptions.Count > 0)
                                {
                                    do
                                    {
                                        if (first22)
                                        {
                                            Console.Write(" [");
                                            remainingCornerWidth -= 2;
                                            first22 = false;
                                        }
                                        next = nextTranslationTranscriptions.Dequeue();
                                        Console.Write(next);
                                        remainingCornerWidth -= next.Length;
                                        if (remainingCornerWidth > 0)
                                        {
                                            Console.Write(" ");
                                            remainingCornerWidth--;
                                        }
                                    }
                                    while (nextTranslationTranscriptions.Count > 0 && remainingCornerWidth >= nextTranslationTranscriptions.Peek().Length);
                                }
                                Console.Write(new string(' ', remainingCornerWidth));
                                Console.WriteLine("║");
                            }
                            nextCellIndex = i + 1;
                            while (nextCellIndex < vocabulary.Count && !(filter(vocabulary.ElementAt(nextCellIndex)) && (IsContaining(vocabulary.ElementAt(nextCellIndex).Key.ToString(), desiredOption.ToString()) || vocabulary.ElementAt(nextCellIndex).Value.Any(word => IsContaining(word.ToString(), desiredOption.ToString())))))
                            {
                                nextCellIndex++;
                            }
                            if (nextCellIndex == currentSelection && nextCellIndex < vocabulary.Count)
                            {
                                Console.ForegroundColor = ConsoleColor.DarkCyan;
                            }
                            Console.Write("   ╟");
                            Console.Write(new string('─', cellWidth));
                            Console.Write("┼");
                            Console.Write(new string('─', cellWidth));
                            Console.Write("┼");
                            Console.Write(new string('─', cellWidth));
                            Console.Write("┼");
                            Console.Write(new string('─', cellWidth));
                            Console.WriteLine("╢");
                        }
                    }
                    Console.CursorTop--;
                    Console.Write("   ╚");
                    Console.Write(new string('═', cellWidth));
                    Console.Write("╧");
                    Console.Write(new string('═', cellWidth));
                    Console.Write("╧");
                    Console.Write(new string('═', cellWidth));
                    Console.Write("╧");
                    Console.Write(new string('═', cellWidth));
                    Console.WriteLine("╝");
                    Console.ResetColor();
                    for (int i = vocabulary.Count; i < vocabulary.Count + menuItems.Count; i++)
                    {
                        if (IsContaining(menuItems[i - vocabulary.Count], desiredOption.ToString()))
                        {
                            if (i == currentSelection)
                            {
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                Console.WriteLine("-> " + menuItems[i - vocabulary.Count]);
                                Console.ResetColor();
                            }
                            else
                            {
                                Console.WriteLine("  " + menuItems[i - vocabulary.Count]);
                            }
                        }
                    }
                    keyinfo = Console.ReadKey(true);
                    if (currentSelection == -1)
                    {
                        switch (keyinfo.Key)
                        {
                            case ConsoleKey.UpArrow:
                                currentSelection = vocabulary.Count + menuItems.Count;
                                do
                                {
                                    currentSelection = (currentSelection == 0) ? -1 : ((currentSelection - 1 + vocabulary.Count + menuItems.Count) % (vocabulary.Count + menuItems.Count));
                                } while (currentSelection != -1 && ((currentSelection < vocabulary.Count && (!filter(vocabulary.ElementAt(currentSelection)) || !(IsContaining(vocabulary.ElementAt(currentSelection).Key.ToString(), desiredOption.ToString()) || vocabulary.ElementAt(currentSelection).Value.Any(word => IsContaining(word.ToString(), desiredOption.ToString()))))) || (currentSelection >= vocabulary.Count && !IsContaining(menuItems[currentSelection - vocabulary.Count], desiredOption.ToString()))));
                                break;
                            case ConsoleKey.DownArrow:
                            case ConsoleKey.Enter:
                                do
                                {
                                    currentSelection = (currentSelection == vocabulary.Count + menuItems.Count - 1) ? -1 : ((currentSelection + 1) % (vocabulary.Count + menuItems.Count));
                                } while (currentSelection != -1 && ((currentSelection < vocabulary.Count && (!filter(vocabulary.ElementAt(currentSelection)) || !(IsContaining(vocabulary.ElementAt(currentSelection).Key.ToString(), desiredOption.ToString()) || vocabulary.ElementAt(currentSelection).Value.Any(word => IsContaining(word.ToString(), desiredOption.ToString()))))) || (currentSelection >= vocabulary.Count && !IsContaining(menuItems[currentSelection - vocabulary.Count], desiredOption.ToString()))));
                                keyinfo = default;
                                break;
                            case ConsoleKey.Backspace:
                                if (desiredOption.Length > 0)
                                {
                                    desiredOption.Remove(desiredOption.Length - 1, 1);
                                }
                                else
                                {
                                    do
                                    {
                                        currentSelection = (currentSelection == vocabulary.Count + menuItems.Count - 1) ? -1 : ((currentSelection + 1) % (vocabulary.Count + menuItems.Count));
                                    } while (currentSelection != -1 && ((currentSelection < vocabulary.Count && (!filter(vocabulary.ElementAt(currentSelection)) || !(IsContaining(vocabulary.ElementAt(currentSelection).Key.ToString(), desiredOption.ToString()) || vocabulary.ElementAt(currentSelection).Value.Any(word => IsContaining(word.ToString(), desiredOption.ToString()))))) || (currentSelection >= vocabulary.Count && !IsContaining(menuItems[currentSelection - vocabulary.Count], desiredOption.ToString()))));
                                }
                                break;
                            case ConsoleKey.Escape:
                                return -1;
                            default:
                                desiredOption.Append(keyinfo.KeyChar);
                                break;
                        }
                    }
                    else
                    {
                        switch (keyinfo.Key)
                        {
                            case ConsoleKey.UpArrow:
                            case ConsoleKey.W:
                                do
                                {
                                    currentSelection = (searchBarAvailable && currentSelection == 0) ? -1 : ((currentSelection - 1 + vocabulary.Count + menuItems.Count) % (vocabulary.Count + menuItems.Count));
                                } while (currentSelection != -1 && ((currentSelection < vocabulary.Count && (!filter(vocabulary.ElementAt(currentSelection)) || !(IsContaining(vocabulary.ElementAt(currentSelection).Key.ToString(), desiredOption.ToString()) || vocabulary.ElementAt(currentSelection).Value.Any(word => IsContaining(word.ToString(), desiredOption.ToString()))))) || (currentSelection >= vocabulary.Count && !IsContaining(menuItems[currentSelection - vocabulary.Count], desiredOption.ToString()))));
                                break;
                            case ConsoleKey.DownArrow:
                            case ConsoleKey.S:
                                do
                                {
                                    currentSelection = (currentSelection + 1) % (vocabulary.Count + menuItems.Count);
                                } while (currentSelection != -1 && ((currentSelection < vocabulary.Count && (!filter(vocabulary.ElementAt(currentSelection)) || !(IsContaining(vocabulary.ElementAt(currentSelection).Key.ToString(), desiredOption.ToString()) || vocabulary.ElementAt(currentSelection).Value.Any(word => IsContaining(word.ToString(), desiredOption.ToString()))))) || (currentSelection >= vocabulary.Count && !IsContaining(menuItems[currentSelection - vocabulary.Count], desiredOption.ToString()))));
                                break;
                            case ConsoleKey.Escape:
                                return -1;
                        }
                    }
                } while (keyinfo.Key != ConsoleKey.Enter);
                Console.WriteLine();
                return currentSelection;
            }
            catch (Exception e)
            {
                Console.WriteLine(ExceptionHandler.GetUnxpectedExceptionMessage(e));
                Console.ReadKey();
                return -1;
            }
        }
        static string ReadLineWirhSuggestion(string nav, string request, string suggestion)
        {
            StringBuilder sb = new(suggestion);
            ConsoleKeyInfo keyinfo;
            while (nav.Length > Console.WindowWidth)
            {
                string[] nav_arr = nav.Split(' ').Skip(1).ToArray();
                if (nav_arr.Length <= 1)
                {
                    break;
                }
                nav_arr[0] = "...";
                nav = string.Join(" ", nav_arr);
            }
            while (true)
            {
                Console.Clear();
                Console.Write("\x1b[3J");
                Console.WriteLine($"\x1b[34m{nav}\x1b[0m");
                Console.Write($"{request}: {sb}");
                keyinfo = Console.ReadKey();
                switch (keyinfo.Key)
                {
                    case ConsoleKey.Escape:
                        Console.CursorVisible = true;
                        Console.WriteLine();
                        return ConsoleKey.Escape.ToString();
                    case ConsoleKey.Backspace:
                        if (sb.Length > 0)
                        {
                            sb.Remove(sb.Length - 1, 1);
                        }
                        break;
                    case ConsoleKey.Enter:
                        Console.WriteLine();
                        return sb.ToString();
                    default:
                        sb.Append(keyinfo.KeyChar);
                        break;
                }
            }
        }
        static string ReadHiddenLine()
        {
            StringBuilder sb = new();
            ConsoleKeyInfo keyinfo;
            while (true)
            {
                keyinfo = Console.ReadKey(true);
                switch (keyinfo.Key)
                {
                    case ConsoleKey.Escape:
                        Console.CursorVisible = true;
                        Console.WriteLine();
                        return ConsoleKey.Escape.ToString();
                    case ConsoleKey.Backspace:
                        if (sb.Length > 0)
                        {
                            sb.Remove(sb.Length - 1, 1);
                        }
                        break;
                    case ConsoleKey.Enter:
                        Console.WriteLine();
                        return sb.ToString();
                    default:
                        sb.Append(keyinfo.KeyChar);
                        break;
                }
            }
        }
    }
}