using Newtonsoft.Json;
using NoppaLib.DataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Threading;
using System.Threading.Tasks;

namespace NoppaLib
{
    public class PinnedCourses
    {
        public static readonly string CourseFile = "MyCourses.txt";

        private SemaphoreSlim _codesLock = new SemaphoreSlim(1, 1);
        private List<Course> _courses = new List<Course>();

        public void Add(Course course)
        {
            _courses.Add(course);
        }

        public void Remove(Course course)
        {
            _courses.Remove(course);
        }

        public bool Contains(Course course)
        {
            return _courses.Contains(course);
        }

        public async Task<List<Course>> GetCoursesAsync()
        {
            await _codesLock.WaitAsync();
            await Task.Run(() =>
            {
                try
                {
                    using (var fileStorage = IsolatedStorageFile.GetUserStoreForApplication())
                    {
                        if (fileStorage.FileExists(PinnedCourses.CourseFile))
                        {
                            using (var stream = new IsolatedStorageFileStream(PinnedCourses.CourseFile, FileMode.Open, FileAccess.Read, fileStorage))
                            {
                                Deserialize(stream);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("GetCodesAsync: Error accessing the course list file in the IsolatedStorage.\n{0}", e.StackTrace);
                }
            });

            _codesLock.Release();
            return _courses;
        }

        public void Serialize(Stream s)
        {
            try{
                using (var writer = new StreamWriter(s))
                {
                    // Save codes only if there is any
                    if (_courses.Count > 0)
                    {
                        var serialized = JsonConvert.SerializeObject(_courses);
                        writer.Write(serialized);
                    }
                }
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine("PinnedCourse::SaveCodesToFile::{0}",e);          
            }
        }

        public void Deserialize(Stream s)
        {
            try 
            {
                using (var reader = new StreamReader(s))
                {
                    string serialized = reader.ReadToEnd();
                    
                    if (serialized != null)
                    {
                        _courses.AddRange(JsonConvert.DeserializeObject<List<Course>>(serialized));
                    }
                }
            }
            catch (Exception e) 
            {
                System.Diagnostics.Debug.WriteLine("PinnedCourse::ReadCodesFromFile::{0}",e);
            }
        }
    }
}
