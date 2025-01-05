using application.Server.Models;

namespace application.Server.Services
{
    public interface IFileOperationsService
    {
        Task<Node> GetJsonFileContent(string path);
    }
}
