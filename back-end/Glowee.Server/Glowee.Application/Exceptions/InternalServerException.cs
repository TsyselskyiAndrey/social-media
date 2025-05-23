namespace Glowee.Application.Exceptions
{
    public class InternalServerException : Exception
    {
        public InternalServerException() : base("Internal server error.") { }
    }
}
