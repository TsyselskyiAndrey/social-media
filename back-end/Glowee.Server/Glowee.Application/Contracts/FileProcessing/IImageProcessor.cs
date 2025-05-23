namespace Glowee.Application.Contracts.FileProcessing
{
    public interface IImageProcessor
    {
        Task<MemoryStream> ProcessSmallImageAsync(Stream stream, int size = 256);
    }
}
