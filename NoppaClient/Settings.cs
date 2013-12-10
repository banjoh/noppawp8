using Microsoft.Phone.Scheduler;
using Newtonsoft.Json;
using NoppaClient.Resources;
using NoppaLib;
using NoppaLib.DataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Windows;

namespace NoppaClient
{
    public class Settings
    {
        private readonly static string _settingsFilename = "shared/settings.json";

        private Settings()
        {
            Language = Language.English;
            PinnedCourses = new List<Course>();
            PrimaryTileIsActive = false;
        }

        /*
         * Application properties.
         */
        public Language Language
        {
            get; set;
        }

        public List<Course> PinnedCourses
        {
            get; set;
        }

        public bool PrimaryTileIsActive
        {
            get; set;
        }

        public static Settings LoadFromStorage()
        {
            var storage = IsolatedStorageFile.GetUserStoreForApplication();

            if (storage.FileExists(_settingsFilename) == false)
            {
                return new Settings();
            }

            using (var stream = storage.OpenFile(_settingsFilename, FileMode.Open, FileAccess.Read))
            using (var reader = new StreamReader(stream))
            {
                string json = reader.ReadToEnd();
                if (string.IsNullOrEmpty(json) == false)
                {
                    var settings = JsonConvert.DeserializeObject<Settings>(json);

                    if (settings.PinnedCourses == null)
                        settings.PinnedCourses = new List<Course>();

                    return settings;
                }
            }

            return new Settings();
        }

        public void Save()
        {
            var storage = IsolatedStorageFile.GetUserStoreForApplication();
            if (storage.FileExists(_settingsFilename))
                storage.DeleteFile(_settingsFilename);

            using (var fileStream = storage.CreateFile(_settingsFilename))
            using (var writer = new StreamWriter(fileStream))
            {
                var json = JsonConvert.SerializeObject(this);
                writer.WriteLine(json);
            }
        }
    }
}
