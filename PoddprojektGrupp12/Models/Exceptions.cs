namespace Models
{
    public class PodcastAppException : Exception
    {
        public PodcastAppException(string message) : base(message) { }
        public PodcastAppException(string message, Exception inner) : base(message, inner) { }
    }

    public class EntityNotFoundException : PodcastAppException
    {
        public EntityNotFoundException(string message) : base(message) { }
    }

    public class FeedException : PodcastAppException
    {
        public FeedException(string message, Exception inner) : base(message, inner) { }
    }
}