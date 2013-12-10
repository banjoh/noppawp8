using Microsoft.Phone.Scheduler;
using NoppaClient.Resources;
using NoppaLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NoppaClient
{
    class BackgroundAgentController
    {
        private readonly string _taskName = "NoppaBackgroundAgentUniqueID";

        public BackgroundAgentController()
        {
            // Check if there is a background agent and update the status
            //BackgroundAgentEnabled = ScheduledActionService.Find(_taskName) as PeriodicTask != null;
        }
        /**
         * Background agent
         */
        public bool StartTaskAgent()
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

        public void RemoveTaskAgent()
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
    }
}
