using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Scheduler;
using NoppaClient.Resources;
using System.Windows;
using NoppaLib.DataModel;
using NoppaLib;

namespace NoppaClient
{
    public class Settings
    {
        public const string LanguageSettingKeyName = "LanguageSetting";
        private const string PrimaryTileKeyName = "ActivePrimaryTile";
        private const string _taskName = "NoppaBackgroundAgentUniqueID";
        
        private IsolatedStorageSettings _settings;

        public Settings()
        {
            _settings = IsolatedStorageSettings.ApplicationSettings;

            // Check if there is a background agent and update the status
            //BackgroundAgentEnabled = ScheduledActionService.Find(_taskName) as PeriodicTask != null;
        }

        /*
         * Application properties.
         */
        public Language Language
        {
            get
            {
                return GetValueOrDefault(LanguageSettingKeyName, Language.English);
            }
            set
            {
                SetValue(LanguageSettingKeyName, value);
            }
        }

        public bool PrimaryTileIsActive
        {
            // TODO: Clear and set the primary tile depending on agent status
            get
            {
                bool ret = GetValueOrDefault(PrimaryTileKeyName, false);
                System.Diagnostics.Debug.WriteLine("get PrimaryTileIsActive: {0}", ret);
                return ret;
            }
            set
            {
                bool v = value;
                if (v == true)
                {
                    v = StartTaskAgent();
                }
                else
                {
                    RemoveTaskAgent();
                }
                System.Diagnostics.Debug.WriteLine("set PrimaryTileIsActive: {0}", v);
                SetValue(PrimaryTileKeyName, v);
            }
        }

        /**
         * Background agent
         */
        private bool StartTaskAgent()
        {
            // If the task already exists and background agents are enabled for the
            // application, you must remove the task and then add it again to update 
            // the schedule
            RemoveTaskAgent();

            PeriodicTask periodicTask = new PeriodicTask(_taskName);

            // The description is required for periodic agents. This is the string that the user
            // will see in the background services Settings page on the device.
            //TODO: Verify if background agents get to see the resource files
            periodicTask.Description = AppResources.ApplicationTitle;

            // Place the call to Add in a try block in case the user has disabled agents.
            try
            {
                ScheduledActionService.Add(periodicTask);
                // If debugging is enabled, use LaunchForTest to launch the agent in one minute.
#if(DEBUG)
                ScheduledActionService.LaunchForTest(_taskName, TimeSpan.FromSeconds(10));
#endif
            }
            catch (InvalidOperationException exception)
            {
                if (exception.Message.Contains("BNS Error: The action is disabled"))
                {
                    //TODO: Localize
                    MessageBox.Show("Background agents for this application have been disabled by the user.");
                }
                else if (exception.Message.Contains("BNS Error: The maximum number of ScheduledActions of this type have already been added."))
                {
                    // No user action required. The system prompts the user when the hard limit of periodic tasks has been reached.
                }
                System.Diagnostics.Debug.WriteLine("Noppa Agent InvalidOperationException: {0}", exception.Message);
                return false;
            }
            catch (SchedulerServiceException e)
            {
                // No user action required.
                System.Diagnostics.Debug.WriteLine("Noppa Agent SchedulerServiceException: {0}", e.Message);
                return false;
            }
            catch (Exception e)
            {
                // No user action required.
                System.Diagnostics.Debug.WriteLine("Noppa Agent Exception: {0}", e.Message);
                return false;
            }
            System.Diagnostics.Debug.WriteLine("Noppa agent scheduled successfully :)");
            return true;
        }

        private void RemoveTaskAgent()
        {
            // Obtain a reference to the period task, if one exists
            PeriodicTask periodicTask = ScheduledActionService.Find(_taskName) as PeriodicTask;

            // Remove agent
            if (periodicTask != null)
            {
                try
                {
                    ScheduledActionService.Remove(_taskName);
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("Exception removing the Noppa Task Agent: {0}", e.Message);
                }
            }
            NoppaTiles.ClearAllTiles();
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
