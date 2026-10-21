using System;

namespace Models
{
    // Gemensam basklass så att formuläret kan skilja på "våra" fel och oväntade fel
    public class PodcastAppException : Exception
    {
        public PodcastAppException(string message) : base(message) { }
        public PodcastAppException(string message, Exception inner) : base(message, inner) { }
    }

    public class EntityNotFoundException : PodcastAppException
    {
        public EntityNotFoundException(string message) : base(message) { }
    }

    public class CategoryInUseException : PodcastAppException
    {
        public CategoryInUseException(string message) : base(message) { }
    }

    public class FeedException : PodcastAppException
    {
        public FeedException(string message, Exception inner) : base(message, inner) { }
    }

    public class DatabaseConfigException : PodcastAppException
    {
        public DatabaseConfigException(string message) : base(message) { }
    }
}