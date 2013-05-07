using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoppaClient.ViewModels
{
    public static class Empty<T>
    {
        public static Task<T> Task { get { return _task; } }

        private static readonly Task<T> _task = System.Threading.Tasks.Task.FromResult(default(T));
    }

    public class AdditionalPageViewModel : CourseContentViewModel
    {
        public string Content { get { return "Hello, world"; } }

        public override Task<CourseContentViewModel> LoadDataAsync(string code)
        {
            return Empty<CourseContentViewModel>.Task;
        }
    }
}
