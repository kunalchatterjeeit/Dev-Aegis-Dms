using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Background.Dms
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Entity.File> files = new BusinessLayer.File().File_GetPendingContentSave();
            Console.WriteLine("Started at: " + DateTime.Now.ToString());
            foreach (Entity.File file in files)
            {
                string filename = file.FileOriginalName + file.FileExtension;
                Console.WriteLine("Processing file: " + filename);
                string response = UpdateFileContent(file);
                Console.WriteLine("Processed file: " + filename + " Response: " + response);
            }

            ClearAllDirectories();

            Console.WriteLine("Ended at: " + DateTime.Now.ToString());
        }

        public static string UpdateFileContent(Entity.File file)
        {
            var body = JsonConvert.SerializeObject(file);
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri((string)new AppSettingsReader().GetValue("BaseUrl", typeof(string)));
            var response = client.PostAsync("api/file/UpdateFileContent", new StringContent(body, Encoding.UTF8, "application/json")).Result;
            return response.ToString();
        }

        public static string ClearAllDirectories()
        {
            var body = "";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri((string)new AppSettingsReader().GetValue("BaseUrl", typeof(string)));
            var response = client.PostAsync("api/file/ClearAllDirectories", new StringContent(body, Encoding.UTF8, "application/json"));
            return response.ToString();
        }
    }
}
