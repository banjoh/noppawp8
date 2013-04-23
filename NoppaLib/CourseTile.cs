using Microsoft.Phone.Shell;
using NoppaLib.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace NoppaLib
{
    public static class CourseTile
    {
        public static bool Exists(Course course)
        {
            return course != null && GetActiveTile(course) != null;
        }

        // Make this a single function to avoid multiple tiles of the same course
        public static void CreateOrUpdate(Course course)
        {
            // TODO: Add additional information to this call to populate more interesting data
            var data = CreateTileData(course, 0);

            var tile = GetActiveTile(course);
            if (tile != null)
            {
                tile.Update(data);
            }
            else
            {
                ShellTile.Create(GetCourseUri(course), data, false);
            }
        }

        public static void Update(string courseId, int newsCount)
        {
            Course course = new Course();
            course.Id = courseId;
            // TODO: Add additional information to this call to populate more interesting data
            var data = CreateTileData(course, newsCount);

            var tile = GetActiveTile(course);
            if (tile != null)
            {
                tile.Update(data);
            }
        }

        public static bool Delete(Course course)
        {
            var tile = GetActiveTile(course);
            if (tile == null)
            {
                return false;
            }

            tile.Delete();
            return true;
        }

        private static ShellTile GetActiveTile(Course course)
        {
            var uri = GetCourseUri(course);
            var tiles = ShellTile.ActiveTiles;
            foreach (var tile in tiles)
            {
                if (tile.NavigationUri.Equals(uri))
                {
                    return tile;
                }
            }
            return null;
        }

        private static ShellTileData CreateTileData(Course course, int newsCount)
        {
            // TODO: Get more content as parameters in this function,
            // in particular, don't call any async methods in here
            // because that information should already be available to
            // the caller. If it's not, when the new information is
            // available, it can check whether the tile is active and
            // update it then.
            return new IconicTileData()
            {
               Title = course.Id,
               Count = newsCount
            };
        }

        private static Uri GetCourseUri(Course course)
        {
            return NoppaUtility.MakeUri("/CoursePage.xaml", "id", course.Id);
        }

        public static string GetCourseCode(Uri uri)
        {
            string pattern = @"^/CoursePage.xaml\?id=(..*)$";
            Match m = Regex.Match(uri.ToString(), pattern);

            if (m.Success)
            {
                return m.Groups[1].ToString();
            }

            return null;
        }
    }
}
