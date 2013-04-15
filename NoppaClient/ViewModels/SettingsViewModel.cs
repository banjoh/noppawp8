using NoppaClient.Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoppaClient.ViewModels
{
    public class SettingsViewModel : BindableBase
    {
        public Settings Settings { get { return App.Settings; } }

        private IEnumerable<LanguageOption> _languages;
        public IEnumerable<LanguageOption> Languages { get { return _languages; } }

        /* Automatically update the settings when this value changes */
        private LanguageOption _language;
        public LanguageOption Language
        {
            get { return _language; }
            set 
            {
                if (Settings.SetValue(Settings.LanguageSettingKeyName, value.Language))
                {
                    _language = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool PrimaryTileIsActive
        {
            get { return Settings.PrimaryTileIsActive; }
            set 
            {
                Settings.PrimaryTileIsActive = value;
                NotifyPropertyChanged("PrimaryTileIsActive");
            }
        }

        public SettingsViewModel()
        {
            // if we start adding more languages, this will become quite unwieldy
            var finnish = new LanguageOption { Language = NoppaLib.DataModel.Language.Finnish, Name = AppResources.FinnishLanguage };
            var swedish = new LanguageOption { Language = NoppaLib.DataModel.Language.Swedish, Name = AppResources.SwedishLanguage };
            var english = new LanguageOption { Language = NoppaLib.DataModel.Language.English, Name = AppResources.EnglishLanguage };

            _languages = new LanguageOption[] {
                finnish,
                swedish,
                english
            };

            if (!System.ComponentModel.DesignerProperties.IsInDesignTool)
            {
                switch (Settings.Language)
                {
                    case NoppaLib.DataModel.Language.Finnish: _language = finnish; break;
                    case NoppaLib.DataModel.Language.Swedish: _language = swedish; break;
                    default: _language = english; break;
                }
            }
            else
            {
                _language = english;
            }
        }
    }

    public class LanguageOption
    {
        public string Name { get; set; }
        public NoppaLib.DataModel.Language Language { get; set; }
        
        public override string ToString()
        {
            return Name;
        }
    }
}
