using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.Windows;


namespace DraftWayfinder.Models
{
    [DataContract]
    public class Set
    {
        private static readonly string SetsUrl = "https://api.magicthegathering.io/v1/sets";
        private static readonly string ExecPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        private static readonly string DatabaseDir = $@"{ExecPath}\databases";
        private static readonly string SetsFilepath = $@"{ExecPath}\databases\sets.json";
        private static readonly string SetsFileTmp = $@"{ExecPath}\databases\sets.json.tmp";

        public static IEnumerable<Set> AllSets()
        {
            if (!File.Exists(SetsFilepath) ||
                (DateTime.Now - File.GetLastAccessTime(SetsFilepath)) > TimeSpan.FromDays(1))
            {
                if (!Directory.Exists(DatabaseDir))
                {
                    Directory.CreateDirectory(DatabaseDir);
                }

                if (File.Exists(SetsFileTmp))
                {
                    File.Delete(SetsFileTmp);
                }

                using (var writer = File.Create(SetsFileTmp))
                using (var stream = new HttpClient().GetStreamAsync(SetsUrl).Result)
                {                    
                    stream.CopyTo(writer);
                }

                if (File.Exists(SetsFilepath))
                {
                    File.Delete(SetsFilepath);
                }

                File.Copy(SetsFileTmp, SetsFilepath);
            }

            using (var reader = new FileStream(SetsFilepath, FileMode.Open))
            {
                var serializer = new DataContractJsonSerializer(typeof(Sets));
                var deserialized = (Sets) serializer.ReadObject(reader);
                var allSets = deserialized.Body
                                          .Where(s => s.Type == "core" || s.Type == "expansion")
                                          .OrderByDescending(s => s.ReleaseDate)
                                          .ToList();
                return allSets;
            }
        }

        [DataMember(Name="code")]
        public string Code { get; set; }
        [DataMember(Name="name")]
        public string Name { get; set; }
        [DataMember(Name = "releaseDate")]
        public string ReleaseDate { get; set; }
        [DataMember(Name = "type")]
        public string Type { get; set; }

        public IEnumerable<Card> GetCards()
        {
            return null;
        }

        public override string ToString()
        {
            return Name;
        }
    }

    [DataContract]
    public class Sets
    {
        [DataMember(Name = "sets")]
        public List<Set> Body { get; set; }
    }
}
