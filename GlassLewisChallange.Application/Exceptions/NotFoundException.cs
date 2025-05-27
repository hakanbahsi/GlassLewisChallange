namespace GlassLewisChallange.Application.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException()
            : base("Record was not found.") { }

        public NotFoundException(string name, object key)
            : base($"{name} with key '{key}' was not found.") { }
    }
}
