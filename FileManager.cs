using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace n_FileManager
{
    static internal partial class FileManager
    {
        [GeneratedRegex(@"\.txt$")]
        private static partial Regex TextFileRegex();
        [GeneratedRegex(@"\.dat$")]
        private static partial Regex BinaryFileRegex();
        [GeneratedRegex(@"\.xml$")]
        private static partial Regex XmlFileRegex();
        public static string ToTextFile(this string name)
        {
            return TextFileRegex().IsMatch(name) ? name : name + ".txt";
        }
        public static string ToBinaryFile(this string name)
        {
            return BinaryFileRegex().IsMatch(name) ? name : name + ".dat";
        }
        public static string ToXmlFile(this string name)
        {
            return XmlFileRegex().IsMatch(name) ? name : name + ".xml";
        }
        public static List<string> GetXmlFiles()
        {
            return GetXmlFiles(Directory.GetCurrentDirectory());
        }
        public static List<string> GetXmlFiles(string directory)
        {
            return Directory.GetFiles(directory).Where(path => XmlFileRegex().IsMatch(path)).Select(path => Path.GetFileName(path)).ToList();
        }
        public static bool IsFileExisting(string path)
        {
            return File.Exists(path);
        }
        public static void WriteStringFromBinaryFile(string path, string str)
        {
            if (!File.Exists(path.ToBinaryFile()))
            {
                return;
            }
            using FileStream fs = File.Open(path.ToBinaryFile(), FileMode.Create);
            using BinaryWriter writer = new(fs);
            writer.Write(str);
        }
        public static string ReadStringFromBinaryFile(string path)
        {
            if (!File.Exists(path.ToBinaryFile()))
            {
                return null;
            }
            using FileStream fs = File.Open(path.ToBinaryFile(), FileMode.Open);
            using BinaryReader reader = new(fs);
            return reader.ReadString();
        }
    }
}