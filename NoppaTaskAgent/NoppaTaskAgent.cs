using System.Diagnostics;
using System.Windows;
using Microsoft.Phone.Scheduler;
using Microsoft.Phone.Shell;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;
using System.Linq;
using System.IO.IsolatedStorage;
using System.IO;
using System.Threading.Tasks;

using NoppaLib.DataModel;
using NoppaLib;

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

            //TODO: To use the CACHE or not to use it?

            PinnedCourses pc = new PinnedCourses();
            Task<ObservableCollection<string>> t = pc.GetCodesAsync();
            t.Wait();
            ObservableCollection<string> codes = t.Result;

            // Clear all the tiles
            if (codes == null || codes.Count <= 0)
            {
                ClearTiles();
            }
            // Update tiles with data
            else
            {
                Task<List<CourseNews>>[] asyncOps = (from code in codes select GetCourseNewsAsync(code)).ToArray();
                try
                {
                    Task.WaitAll(asyncOps, new TimeSpan(0, 0, 20));   // Wait for 20 seconds max. Might throw a timeout exception
                    int count = 0;
                    foreach (Task<List<CourseNews>> op in asyncOps)
                    {
                        List<CourseNews> news = op.Result;
                        count += news.Count;
                    }

                    // Tile update
                    ShellTile primaryTile = ShellTile.ActiveTiles.First();

                    //If tile was found then update the tile
                    if (primaryTile != null)
                    {
                        //TODO: Get content from the network
                        IconicTileData data = new IconicTileData
                        {
                            Count = count,
                            WideContent1 = "Distributed Systems",
                            WideContent2 = "Lecture has been updated",
                            WideContent3 = "T1 Building"
                        };

                        primaryTile.Update(data);
                    }
                }
                catch
                { 
                    // Exception caught. Do nothing
                }
            }

            NotifyComplete();
            System.Diagnostics.Debug.WriteLine("Noppa background agent ended successfully");
        }

        private Task<List<CourseNews>> GetCourseNewsAsync(string course_id)
        {
            return NoppaImpl.GetInstance().GetObject<List<CourseNews>>(Cache.PolicyLevel.BypassCache, "/courses/{0}/news?key={1}", course_id, APIConfigHolder.Key);
        }

        private void ClearTiles()
        {
            // Empty data
            IconicTileData empty = new IconicTileData { Count = 0 };
            foreach (ShellTile tile in ShellTile.ActiveTiles)
            {
                if (tile != null)
                    tile.Update(empty);
            }
        }
    }
}