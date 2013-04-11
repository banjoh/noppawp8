using Microsoft.Phone.Shell;
using NoppaClient.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoppaClient
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
            var data = CreateTileData(course);

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

        private static ShellTileData CreateTileData(Course course)
        {
            // TODO: Get more content as parameters in this function,
            // in particular, don't call any async methods in here
            // because that information should already be available to
            // the caller. If it's not, when the new information is
            // available, it can check whether the tile is active and
            // update it then.
            return new FlipTileData()
            {
               Title = course.LongName,
               /*
               BackTitle = "[back of Tile title]",
               BackContent = "[back of medium Tile size content]",
               WideBackContent = "[back of wide Tile size content]",
               Count = [count],
               SmallBackgroundImage = [small Tile size URI],
               BackgroundImage = [front of medium Tile size URI],
               BackBackgroundImage = [back of medium Tile size URI],
               WideBackgroundImage = [front of wide Tile size URI],
               WideBackBackgroundImage = [back of wide Tile size URI],
                */
            };
        }

        private static Uri GetCourseUri(Course course)
        {
            return PhoneNavigationController.MakeCoursePageUri(course);
        }
    }
}
