using application.Server.Models;
using Newtonsoft.Json;

namespace application.Server.Services
{
    public class FileOperationsService : IFileOperationsService
    {
        public async Task<Node> GetJsonFileContent(string path)
        {
            return JsonConvert.DeserializeObject<Node>(await File.ReadAllTextAsync(path));
        }
    }
}
