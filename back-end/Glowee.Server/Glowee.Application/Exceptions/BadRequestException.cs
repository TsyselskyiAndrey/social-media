namespace Glowee.Application.Exceptions
{
    class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message) { }
    }
}
