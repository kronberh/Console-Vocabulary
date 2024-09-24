using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace n_ForeignWord
{
    public record ForeignWord([Required][XmlElement] string Spelling, [XmlElement] string Transcribtion) : IComparable<ForeignWord>
    {
        public ForeignWord() : this(null, null) { }
        public override string ToString()
        {
            return this.Spelling + (string.IsNullOrEmpty(this.Transcribtion) ? "" : $" [{this.Transcribtion}]");
        }
        public int CompareTo(ForeignWord other)
        {
            return string.Compare(this.ToString(), other.ToString());
        }
        public static bool operator >(ForeignWord left, ForeignWord right)
        {
            return left.CompareTo(right) > 0;
        }
        public static bool operator <(ForeignWord left, ForeignWord right)
        {
            return left.CompareTo(right) < 0;
        }
        public static bool operator >=(ForeignWord left, ForeignWord right)
        {
            return left.CompareTo(right) >= 0;
        }
        public static bool operator <=(ForeignWord left, ForeignWord right)
        {
            return left.CompareTo(right) <= 0;
        }
    }
}