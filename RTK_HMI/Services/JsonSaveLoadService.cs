using DataAccess.Models;
using RTK_HMI.Views.DialogWindows;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Windows;

namespace RTK_HMI.Services
{
    internal static class JsonSaveLoadService
    {
        static JsonSaveLoadService()
        {
            if(!Directory.Exists(FolderName))
            {
                Directory.CreateDirectory(FolderName);
            }
        }


        public const string FolderName = "SavedData";
        public static void Save(IEnumerable<Parameter> parameters, JsonData fileInfo)
        {
            var name = GetName(fileInfo);
            if (string.IsNullOrEmpty(name)) return;
            var jsonFormatter = new DataContractJsonSerializer(typeof(IEnumerable<Parameter>));
            Directory.CreateDirectory(Path.GetDirectoryName($"{FolderName}/{name}.json"));
            using (var file = new FileStream($"{FolderName}/{name}.json", FileMode.OpenOrCreate))
            {
                jsonFormatter.WriteObject(file, parameters);
            }

        }

        static string GetName(JsonData fileInfo)
        {
            SaveToJsonWindow window = new SaveToJsonWindow(fileInfo is null ? string.Empty : fileInfo.Name);
            if (window.ShowDialog() == true)
            {
               return window.Name;
            }
            else
            {
                MessageBox.Show("Сохранение не выполнено");
                return string.Empty;
            }
        }

        public static List<JsonData> GetFilesInfo()
        {
            if (Directory.Exists(FolderName))
            {                
                return new DirectoryInfo(FolderName).EnumerateFiles()
                   .Where(fi => fi.Extension.ToLower() == ".json")                   
                   .Select(fi => new JsonData {Name=fi.Name.Split(new char[] {'.'}).FirstOrDefault(), ChangeTime=fi.LastWriteTime }).ToList();
            }
            return new List<JsonData>();
        }


        public static IEnumerable<Parameter> LoadFromJson(string name)
        {
            var jsonFormatter = new DataContractJsonSerializer(typeof(IEnumerable<Parameter>));
            var parameters = Enumerable.Empty<Parameter>();
            using (var file = new FileStream($"{FolderName}/{name}.json", FileMode.OpenOrCreate))
            {
                parameters = jsonFormatter.ReadObject(file) as IEnumerable<Parameter>;
                
            }
            return parameters;
        }


        
    }
}
