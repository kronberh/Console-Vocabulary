using System;
using System.IO;
using n_VocabularyMenu;
using n_ExceptionHandler;

namespace Exam_Vocabulary
{
    internal static class Program
    {
        public static void Main()
        {
            try
            {
                const string languageFilename = "_language.txt";
                if (!File.Exists(languageFilename))
                {
                    File.Create(languageFilename).Close();
                }
                VocabularyAppMenu app = new(File.ReadAllText(languageFilename));
                app.MainMenu();
                File.WriteAllText(languageFilename, app.UILanguage);
            }
            catch (Exception e)
            {
                Console.WriteLine(ExceptionHandler.GetUnxpectedExceptionMessage(e));
                Console.ReadKey();
            }
        }
    }
}