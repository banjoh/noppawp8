using System.Diagnostics;
using System.Windows;
using Microsoft.Phone.Scheduler;
using Microsoft.Phone.Shell;
using System.Collections.Generic;
using System;
using System.Linq;

namespace NoppaTaskAgent
{
    public class NoppaTaskAgent : ScheduledTaskAgent
    {
        /// <remarks>
        /// ScheduledAgent constructor, initializes the UnhandledException handler
        /// </remarks>
        static NoppaTaskAgent()
        {
            // Subscribe to the managed exception handler
            Deployment.Current.Dispatcher.BeginInvoke(delegate
            {
                Application.Current.UnhandledException += UnhandledException;
            });
        }

        /// Code to execute on Unhandled Exceptions
        private static void UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                Debugger.Break();
            }

            if (e != null && e.ExceptionObject != null && e.ExceptionObject.Message != null)
            {
                System.Diagnostics.Debug.WriteLine("Background agent exception: {0}", e.ExceptionObject.Message);
            }
        }

        /// <summary>
        /// Agent that runs a scheduled task
        /// </summary>
        /// <param name="task">
        /// The invoked task
        /// </param>
        /// <remarks>
        /// This method is called when a periodic or resource intensive task is invoked
        /// </remarks>
        protected override void OnInvoke(ScheduledTask task)
        {
            System.Diagnostics.Debug.WriteLine("Noppa background agent started");

            // Tile update
            ShellTile primaryTile = ShellTile.ActiveTiles.First();

            //If tile was found then update the tile
            if (primaryTile != null)
            {
                //TODO: Get content from the network
                IconicTileData data = new IconicTileData
                {
                    Count = new Random().Next(1, 100),
                    WideContent1 = "Distributed Systems",
                    WideContent2 = "Lecture has been updated",
                    WideContent3 = "T1 Building"
                };

                primaryTile.Update(data);
            }

            NotifyComplete();
            System.Diagnostics.Debug.WriteLine("Noppa background agent ended successfully");
        }
    }
}