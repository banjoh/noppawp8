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
namespace NoppaClient
{
    public class PinnedCourses
    {

        private List<string> _codes = new  List<string>();
        public List<string> Codes { get { return _codes; } }

        public async Task Add(string CourseCode)
        {
            if (_codes.Contains(CourseCode) == false)
            {
                _codes.Add(CourseCode);
                await SaveCodesToFile();
            }
        }

        public async Task Remove(string CourseCode)
        {
            if (_codes.Contains(CourseCode))
            {
                _codes.Remove(CourseCode);
                await SaveCodesToFile();
            }
        }

        private async Task SaveCodesToFile()
        {
            try{
                // Get the local folder.
                StorageFolder local = Windows.Storage.ApplicationData.Current.LocalFolder;

                // Create MyCourses file, delete old file if exist.
                var file = await local.CreateFileAsync("MyCourses.txt", CreationCollisionOption.ReplaceExisting);

                // Save codes only if there is any
                if (_codes.Count > 0)
                {
                    string codes = "";

                    foreach (string c in _codes)
                    {
                        codes += c + ";";
                    }
                    codes = codes.Substring(0, codes.Length - 1);
                    byte[] fileBytes = System.Text.Encoding.UTF8.GetBytes(codes.ToCharArray());

                    // Write codes to file
                    using (var s = await file.OpenStreamForWriteAsync())
                    {
                        s.Write(fileBytes, 0, fileBytes.Length);
                    }
                }
            }catch(Exception e){
                System.Diagnostics.Debug.WriteLine("PinnedCourse::SaveCodesToFile::{0}",e);          
            }
        }


        public async Task ReadCodesFromFile()
        {
            // Get the local folder.
            StorageFolder local = Windows.Storage.ApplicationData.Current.LocalFolder;

            if (local != null)
            {
                try
                {
                    // Get MyCourses file.
                    var file = await local.OpenStreamForReadAsync("MyCourses.txt");

                    // Read MyCourses.
                    using (StreamReader streamReader = new StreamReader(file))
                    {
                        string fileContent = streamReader.ReadToEnd();
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
                }catch(Exception e){
                    System.Diagnostics.Debug.WriteLine("PinnedCourse::ReadCodesFromFile::{0}",e);
                }
            }
        }
    }
}
