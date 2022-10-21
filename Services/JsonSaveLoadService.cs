using DataAccess.Models;
using RTK_HMI.Views.DialogWindows;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Windows;

namespace RTK_HMI.Services
{
    internal static class JsonSaveLoadService
    {

        public const string FolderName = "SavedData";
        public static void Save(IEnumerable<Parameter> parameters)
        {
            var name = GetName();
            if (string.IsNullOrEmpty(name)) return;
            var jsonFormatter = new DataContractJsonSerializer(typeof(IEnumerable<Parameter>));
            Directory.CreateDirectory(Path.GetDirectoryName($"{FolderName}/{name}.json"));
            using (var file = new FileStream($"{FolderName}/{name}.json", FileMode.OpenOrCreate))
            {
                jsonFormatter.WriteObject(file, parameters);
            }

        }

        static string GetName()
        {
            SaveToJsonWindow window = new SaveToJsonWindow();
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

        
    }
}
