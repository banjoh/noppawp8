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
        // ScheduledAgent constructor, initializes the UnhandledException handler
        static NoppaTaskAgent()
        {
            // Subscribe to the managed exception handler
            Deployment.Current.Dispatcher.BeginInvoke(delegate
            {
                Application.Current.UnhandledException += UnhandledException;
            });
        }

        // Code to execute on Unhandled Exceptions
        private static void UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                Debugger.Break();
            }
        }

        // This method is called when a periodic or resource intensive task is invoked by the system
        protected override void OnInvoke(ScheduledTask task)
        {
            System.Diagnostics.Debug.WriteLine("Noppa background agent started");

            try
            {
                // Clear all tiles first
                NoppaTiles.ClearAllTiles();

                Collection<string> codes = GetCourseCodes();
                
                if (codes.Count > 0)
                {
                    // Update tiles with data
                    Dictionary<string, Task<List<CourseNews>>> asyncOps = new Dictionary<string, Task<List<CourseNews>>>();
                    foreach (string code in codes)
                    {
                        asyncOps.Add(code, NoppaAPI.GetCourseNews(code));
                    }
                    
                    Task.WaitAll(asyncOps.Values.ToArray(), new TimeSpan(0, 0, 20));   // Wait for 20 seconds max. Might throw a timeout exception
                    
                    int count = 0;
                    DateTime dt = default(DateTime);
                    CourseNews latest = null;

                    foreach (string code in asyncOps.Keys)
                    {
                        List<CourseNews> news = asyncOps[code].Result;
                        NoppaTiles.Update(code, news.Count);
                        count += news.Count;

                        // Get the latest piece of news
                        foreach (CourseNews n in news)
                        {
                            if (n.Date > dt)
                            {
                                dt = n.Date;
                                latest = n;
                            }
                        }
                    }

                    UpdatePrimaryTile(latest, count);
                }

                System.Diagnostics.Debug.WriteLine("Noppa background agent ended successfully");
            }
            catch (Exception e)
            {
                // Catch all exceptions. The agent should never throw an exception
                System.Diagnostics.Debug.WriteLine("Background agent exception: {0}", e.Message);
                
                if (Debugger.IsAttached)
                {
                    Debugger.Break();
                }
            }

            NotifyComplete();
        }

        private Collection<string> GetCourseCodes()
        {
            Task<ObservableCollection<string>> t = new PinnedCourses().GetCodesAsync();
            t.Wait();

            ObservableCollection<string> codes = t.Result;

            foreach (ShellTile tile in ShellTile.ActiveTiles)
            {
                string code = NoppaTiles.GetCourseCode(tile.NavigationUri);
                if (code != null && codes.Contains(code) == false)
                {
                    codes.Add(code);
                }
            }

            return t.Result;
        }

        private void UpdatePrimaryTile(CourseNews news, int count)
        {
            // Tile update
            ShellTile primaryTile = ShellTile.ActiveTiles.First();

            //If tile was found then update the tile
            if (primaryTile != null)
            {
                //TODO: Get content from the network
                IconicTileData data = news == null 
                    ? new IconicTileData { Count = 0 }
                    : new IconicTileData
                    {
                        Count = count,
                        // TODO: Adjust length of the text
                        WideContent1 = news.Title.Substring(0, Math.Min(news.Title.Length, 26)),
                        WideContent2 = news.Content.Substring(0, Math.Min(news.Content.Length, 33)),
                        WideContent3 = news.Date.ToShortDateString() + " " + news.Date.ToShortTimeString()
                    };

                primaryTile.Update(data);
            }
        }
    }
}