namespace Glowee.Application.Exceptions
{
    class NotFoundException : Exception
    {
        public NotFoundException(string name, object key) : base($"{name} ({key}) was not found") { }
    }
}
