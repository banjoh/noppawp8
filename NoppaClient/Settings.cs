using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoppaClient
{
    public class Settings
    {
        public const string LanguageSettingKeyName = "LanguageSetting";
                
        private IsolatedStorageSettings _settings;

        /*
         * Application properties.
         */
        public DataModel.Language Language
        {
            get
            {
                return GetValueOrDefault(LanguageSettingKeyName, DataModel.Language.English);
            }
            set
            {
                SetValue(LanguageSettingKeyName, value);
            }
        }

        public Settings()
        {
            _settings = IsolatedStorageSettings.ApplicationSettings;
        }

        public bool SetValue(string key, Object value)
        {
            bool valueChanged = false;

            if (_settings.Contains(key))
            {
                if (_settings[key] != value)
                {
                    _settings[key] = value;
                    valueChanged = true;
                }
            }
            else
            {
                _settings.Add(key, value);
                valueChanged = true;
            }
            return valueChanged;
        }

        public T GetValueOrDefault<T>(string key, T defaultValue)
        {
            return _settings.Contains(key) ? (T)_settings[key] : defaultValue;
        }
    }
}
