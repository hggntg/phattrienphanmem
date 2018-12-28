using Infrastructure;
using Microsoft.AspNetCore.Http;
using FileService.Model;
using Service;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace FileService.Business
{
    public class FileBusiness
    {
        private readonly FileContext FileContextInstance;
        private readonly AccountService AccountServiceInstance;
        public FileBusiness(FileContext FileContextInstance, AccountService AccountServiceInstance)
        {
            this.FileContextInstance = FileContextInstance;
            this.AccountServiceInstance = AccountServiceInstance;
        }
        public async Task<File> UploadFile(HttpRequest Request, string Token, string FilePath)
        {
            var Handler = new JwtSecurityTokenHandler();
            Token = Token.Replace("Bearer", "").Trim();
            var JsonToken = Handler.ReadJwtToken(Token);
            string Id = JsonToken.Claims.First(c => c.Type == "id").Value;

            User User = this.FileContextInstance.Users.SingleOrDefault(u => u.Id == Id);

            if (User == null)
            {
                User = await this.AccountServiceInstance.Detail("Bearer " + Token);

                this.FileContextInstance.Users.Add(User);

                this.FileContextInstance.SaveChanges();
            }

            File File = await FileStreamingHelper.StreamFile(Request, Id, FilePath);
            this.FileContextInstance.Files.Add(File);
            this.FileContextInstance.SaveChanges();
            return File;
        }

        public File GetFile(string UniqueName)
        {
            File File = this.FileContextInstance.Files.SingleOrDefault(f => f.UniqueName == UniqueName);
            return File;
        }
    }
}
