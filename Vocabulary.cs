using System;
using System.Collections.Generic;
using n_ForeignWord;
using n_FileManager;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using System.IO;
using n_ExceptionHandler;
using System.Xml;
using System.Xml.Schema;
using System.Linq;

namespace n_Vocabulary
{
    public class Vocabulary(string languageCodeFrom, string languageCodeTo) : IDictionary<ForeignWord, SortedSet<ForeignWord>>, IXmlSerializable
    {
        public Vocabulary() : this(null, null) { }
        [Required]
        readonly string languageCodeFrom = languageCodeFrom;
        [Required]
        readonly string languageCodeTo = languageCodeTo;
        readonly SortedDictionary<ForeignWord, SortedSet<ForeignWord>> words_dict = [];
        public string LanguaveCodeFrom { get => languageCodeFrom; }
        public string LanguaveCodeTo { get => languageCodeTo; }
        public string Name { get => languageCodeFrom + '-' + languageCodeTo; }
        public SortedDictionary<ForeignWord, SortedSet<ForeignWord>> WordsDict { get => new(words_dict); }
        public int Count => ((ICollection<KeyValuePair<ForeignWord, SortedSet<ForeignWord>>>)words_dict).Count;
        public bool IsReadOnly => ((ICollection<KeyValuePair<ForeignWord, SortedSet<ForeignWord>>>)words_dict).IsReadOnly;
        public ICollection<ForeignWord> Keys => ((IDictionary<ForeignWord, SortedSet<ForeignWord>>)words_dict).Keys;
        public ICollection<SortedSet<ForeignWord>> Values => ((IDictionary<ForeignWord, SortedSet<ForeignWord>>)words_dict).Values;
        public SortedSet<ForeignWord> this[ForeignWord key] { get => ((IDictionary<ForeignWord, SortedSet<ForeignWord>>)words_dict)[key]; set => ((IDictionary<ForeignWord, SortedSet<ForeignWord>>)words_dict)[key] = value; }
        public void Save()
        {
            if (!File.Exists(this.Name.ToXmlFile()))
            {
                File.Create(this.Name.ToXmlFile()).Close();
            }
            using StreamWriter writer = new(this.Name.ToXmlFile());
            new XmlSerializer(typeof(Vocabulary)).Serialize(writer, this);
        }
        public void Load()
        {
            SortedDictionary<ForeignWord, SortedSet<ForeignWord>> savepoint = new(this.words_dict);
            try
            {
                Vocabulary loaded;
                if (!File.Exists(this.Name.ToXmlFile()))
                {
                    this.Save();
                    return;
                }
                using (StreamReader reader = new(this.Name.ToXmlFile()))
                {
                    loaded = (Vocabulary)new XmlSerializer(typeof(Vocabulary)).Deserialize(reader);
                }
                this.words_dict.Clear();
                foreach (KeyValuePair<ForeignWord, SortedSet<ForeignWord>> element in loaded.words_dict)
                {
                    this.words_dict.Add(element.Key, new SortedSet<ForeignWord>(element.Value));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(ExceptionHandler.GetUnxpectedExceptionMessage(e));
                foreach (KeyValuePair<ForeignWord, SortedSet<ForeignWord>> element in savepoint)
                {
                    this.words_dict.Add(element.Key, element.Value);
                }
                Console.ReadKey();
            }
        }
        public void Add(ForeignWord key, ForeignWord value) => ((IDictionary<ForeignWord, SortedSet<ForeignWord>>)words_dict).Add(key, [value]);
        public void Add(ForeignWord key, SortedSet<ForeignWord> value) => ((IDictionary<ForeignWord, SortedSet<ForeignWord>>)words_dict).Add(key, value);
        public void Add(KeyValuePair<ForeignWord, ForeignWord> item) => ((ICollection<KeyValuePair<ForeignWord, SortedSet<ForeignWord>>>)words_dict).Add(KeyValuePair.Create(item.Key, new SortedSet<ForeignWord> { item.Value }));
        public void Add(KeyValuePair<ForeignWord, SortedSet<ForeignWord>> item) => ((ICollection<KeyValuePair<ForeignWord, SortedSet<ForeignWord>>>)words_dict).Add(item);
        public bool ContainsKey(ForeignWord key) => ((IDictionary<ForeignWord, SortedSet<ForeignWord>>)words_dict).ContainsKey(key);
        public bool Remove(ForeignWord key) => ((IDictionary<ForeignWord, SortedSet<ForeignWord>>)words_dict).Remove(key);
        public bool Remove(KeyValuePair<ForeignWord, ForeignWord> item) => ((ICollection<KeyValuePair<ForeignWord, SortedSet<ForeignWord>>>)words_dict).Remove(KeyValuePair.Create(item.Key, new SortedSet<ForeignWord> { item.Value }));
        public bool Remove(KeyValuePair<ForeignWord, SortedSet<ForeignWord>> item) => ((ICollection<KeyValuePair<ForeignWord, SortedSet<ForeignWord>>>)words_dict).Remove(item);
        public bool TryGetValue(ForeignWord key, [MaybeNullWhen(false)] out SortedSet<ForeignWord> value) => ((IDictionary<ForeignWord, SortedSet<ForeignWord>>)words_dict).TryGetValue(key, out value);
        public void Clear() => ((ICollection<KeyValuePair<ForeignWord, SortedSet<ForeignWord>>>)words_dict).Clear();
        public bool Contains(KeyValuePair<ForeignWord, ForeignWord> item) => ((ICollection<KeyValuePair<ForeignWord, SortedSet<ForeignWord>>>)words_dict).Contains(KeyValuePair.Create(item.Key, new SortedSet<ForeignWord> { item.Value }));
        public bool Contains(KeyValuePair<ForeignWord, SortedSet<ForeignWord>> item) => ((ICollection<KeyValuePair<ForeignWord, SortedSet<ForeignWord>>>)words_dict).Contains(item);
        public void CopyTo(KeyValuePair<ForeignWord, SortedSet<ForeignWord>>[] array, int arrayIndex) => ((ICollection<KeyValuePair<ForeignWord, SortedSet<ForeignWord>>>)words_dict).CopyTo(array, arrayIndex);
        public IEnumerator<KeyValuePair<ForeignWord, SortedSet<ForeignWord>>> GetEnumerator() => ((IEnumerable<KeyValuePair<ForeignWord, SortedSet<ForeignWord>>>)words_dict).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)words_dict).GetEnumerator();
        public void WriteXml(XmlWriter writer)
        {
            XmlSerializer keySerializer = new(typeof(ForeignWord));
            XmlSerializer valueSerializer = new(typeof(List<ForeignWord>));
            foreach (var pair in this.words_dict)
            {
                writer.WriteStartElement("Word");
                writer.WriteStartElement("Original");
                keySerializer.Serialize(writer, pair.Key);
                writer.WriteEndElement();
                writer.WriteStartElement("Trandlation");
                valueSerializer.Serialize(writer, pair.Value.ToList());
                writer.WriteEndElement();
                writer.WriteEndElement();
            }
        }
        public void ReadXml(XmlReader reader)
        {
            reader.ReadStartElement(typeof(Vocabulary).Name);
            XmlSerializer keySerializer = new(typeof(ForeignWord));
            XmlSerializer valueSerializer = new(typeof(List<ForeignWord>));
            while (reader.IsStartElement("Word"))
            {
                reader.ReadStartElement("Word");
                reader.ReadStartElement("Original");
                ForeignWord key = (ForeignWord)keySerializer.Deserialize(reader);
                reader.ReadEndElement();
                reader.ReadStartElement("Trandlation");
                List<ForeignWord> value = (List<ForeignWord>)valueSerializer.Deserialize(reader);
                reader.ReadEndElement();
                this.words_dict.Add(key, new SortedSet<ForeignWord>(value));
                reader.ReadEndElement();
            }
            if (this.words_dict.Count > 0)
            {
                reader.ReadEndElement();
            }
        }
        public XmlSchema GetSchema() => null;
    }
}