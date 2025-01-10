using serverApp.Models;

namespace serverApp.Services
{
    public interface IFileOperationsService
    {
        Task<Node> GetJsonFileContent(string path);
    }
}
