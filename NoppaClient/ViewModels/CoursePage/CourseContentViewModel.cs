using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoppaClient.ViewModels
{
    /*
     * The CourseContentViewModel is a base class for different "pages" of a course
     * for instance Overview, Exercises and Lectures. Each of these pages can have
     * some additional text above and below the page.
     */
    public abstract class CourseContentViewModel : BindableBase
    {
        // Title of the "sub-page"
        private string _title = "";
        public string Title { get { return _title; } protected set { SetProperty(ref _title, value); } }

        // The index in the list of pages, i.e. importance (Overview should be first, etc.)
        private int _index = 100;
        public int Index { get { return _index; } protected set { SetProperty(ref _index, value); } }

        private bool _isEmpty = false;
        public bool IsEmpty { get { return _isEmpty; } protected set { SetProperty(ref _isEmpty, value); } }

        // Top and bottom text meant to be this: Additional texts in pages (/courses/<course_id>/texts)
        private string _topText = "";
        public string TopText { get { return _topText; } protected set { SetProperty(ref _topText, value); } }

        private string _bottomText = "";
        public string BottomText { get { return _bottomText; } protected set { SetProperty(ref _bottomText, value); } }

        public abstract Task<CourseContentViewModel> LoadDataAsync(string code);
    }
}
