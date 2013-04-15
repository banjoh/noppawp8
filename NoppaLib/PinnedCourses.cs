using System;
using System.Collections.Generic;
using System.IO;

namespace NoppaLib
{
    public class PinnedCourses
    {
        public static readonly string CourseFile = "MyCourses.txt";
        private List<string> _codes = new List<string>();
        public List<string> Codes { get { return _codes; } }

        public void Add(string CourseCode)
        {
            if (_codes.Contains(CourseCode) == false)
            {
                _codes.Add(CourseCode);
            }
        }

        public void Remove(string CourseCode)
        {
            if (_codes.Contains(CourseCode))
            {
                _codes.Remove(CourseCode);
            }
        }

        public void Serialize(Stream s)
        {
            try
            {
                using (var writer = new StreamWriter(s))
                {
                    // Save codes only if there is any
                    if (_codes.Count > 0)
                    {
                        string codes = "";

                        foreach (string c in _codes)
                        {
                            codes += c + ";";
                        }
                        codes = codes.Substring(0, codes.Length - 1);

                        writer.Write(codes);
                    }
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("PinnedCourse::SaveCodesToFile::{0}", e);
            }
        }


        public void Deserialize(Stream s)
        {
            try
            {
                using (var reader = new StreamReader(s))
                {
                    string fileContent = reader.ReadToEnd();
                    if (fileContent != "")
                    {
                        string[] codes = fileContent.Split(';');
                        _codes.Clear();
                        foreach (string c in codes)
                        {
                            _codes.Add(c);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("PinnedCourse::ReadCodesFromFile::{0}", e);
            }
        }
    }
}
