using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using System.IO;
using System.Windows;
using System.Collections.ObjectModel;
using NoppaClient.ViewModels;
using System.IO.IsolatedStorage;
using System.Threading;

namespace NoppaClient
{
    public class PinnedCourses
    {
        public static readonly string CourseFile = "MyCourses.txt";

        private SemaphoreSlim _codesLock = new SemaphoreSlim(1, 1);
        private ObservableCollection<string> _codes;

        public async Task AddAsync(string code)
        {
            var codes = await GetCodesAsync();
            if (!codes.Contains(code))
            {
                codes.Add(code);
            }
        }

        public async Task RemoveAsync(string code)
        {
            var codes = await GetCodesAsync();
            codes.Remove(code);
        }

        public async Task<bool> ContainsAsync(string code)
        {
            var codes = await GetCodesAsync();
            return codes.Contains(code);
        }

        public async Task<ObservableCollection<string>> GetCodesAsync()
        {
            await _codesLock.WaitAsync();
            if (_codes == null)
            {
                _codes = new ObservableCollection<string>();
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
            }
            _codesLock.Release();
            return _codes;
        }

        public void Serialize(Stream s)
        {
            try{
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
                    string fileContent = reader.ReadToEnd();
                    if (fileContent != "")
                    {
                        string[] codes = fileContent.Split(';');
                        foreach (string c in codes)
                        {
                            _codes.Add(c);
                        }
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
