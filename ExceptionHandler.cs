using System;

namespace n_ExceptionHandler
{
    internal static class ExceptionHandler
    {
        public static string GetUnxpectedExceptionMessage(Exception e)
        {
            return $"\x1b[4;31m{e.GetType()}\x1b[0m\x1b[31m: \x1b[91m{e.Message}\x1b[0m"
                 + $"\n\x1b[31mCode: \x1b[31m{e.HResult}\x1b[0m"
                 + (e.InnerException == null ? "" : $"\n\x1b[31mInner exception: \x1b[91m{e.InnerException.GetType()} ({e.InnerException.Message})\x1b[0m")
                 + (e.Source == null ? "" : $"\n\x1b[31mSource: \x1b[91m{e.Source}")
                 + (e.TargetSite == null ? "" : $"\n\x1b[31mTarget site: \x1b[91m{e.TargetSite}\x1b[0m")
                 + (e.StackTrace == null ? "" : $"\n\x1b[31mStack trace: \n\x1b[91m{e.StackTrace}\x1b[0m")
                 + (e.HelpLink == null ? "" : $"\n\x1b[31mVisit \x1b[91m{e.HelpLink}\x1b[31m for further information.\x1b[0m");
        }
    }
    public class VocabularyException : Exception
    {
        public VocabularyException() : base("An error has occured while running Vocabulary app.") { }
        public VocabularyException(string message) : base(message) { }
        public VocabularyException(string message, Exception inner) : base(message, inner) { }
    }
    public class AccessViolationException : VocabularyException
    {
        public AccessViolationException() : base("An access violation was discovered while running Vocabulary app.") { }
        public AccessViolationException(string message) : base(message) { }
        public AccessViolationException(string message, Exception inner) : base(message, inner) { }
    }
    public class ConfirmationFailedException : VocabularyException
    {
        public ConfirmationFailedException() : base("Confirmation has failed.") { }
        public ConfirmationFailedException(string message) : base(message) { }
        public ConfirmationFailedException(string message, Exception inner) : base(message, inner) { }
    }
    public class EmptySpellingException : VocabularyException
    {
        public EmptySpellingException() : base("Word's spelling is empty.") { }
        public EmptySpellingException(string message) : base(message) { }
        public EmptySpellingException(string message, Exception inner) : base(message, inner) { }
    }
    public class DublicateWordException : VocabularyException
    {
        public DublicateWordException() : base("Attempted to add a dublicate of an already existing word.") { }
        public DublicateWordException(string message) : base(message) { }
        public DublicateWordException(string message, Exception inner) : base(message, inner) { }
    }
}