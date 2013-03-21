using NoppaClient.DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoppaClient.ViewModels
{
    class NewsViewModel : CourseContentViewModel
    {
        private ObservableCollection<NewsItem> _news = new ObservableCollection<NewsItem>();
        public ObservableCollection<NewsItem> News { get { return _news; } }

        public NewsViewModel()
        {
        }

        public NewsViewModel(Course course)
        {
            Index = 2;
            Title = "News";

            _news.Add(new NewsItem { 
                Date = DateTime.Parse("2013-03-13T19:05"),
                Title = "Want to work in a startup? Check out Startup Sauna Internship Helsinki",
                Content = "The Startup Sauna Internship 2013 program offers a chance for the best computer science students to work in the most exciting startups in Finland. The program offers programmer interships from Helsinki area startups for the summer of 2013. This is an opportunity for you to build a network of entrepreneurial minded people and learn by doing with the most driven entrepreneurs in Finland. Take a leap and apply to have the time of your life!\n\nSee info and open positions at startupsauna.com/internship\n"
            });

            _news.Add(new NewsItem { 
                Date = DateTime.Parse("2013-02-11T22:02"),
                Title = "A clarification to the Niksula user account applying process and contact information",
                Content = "In order to speed up the user account generation process if you don't already have a Niksula account, we will do it in a more streamlined fashion - send an e-mail to the me (Olli) with your Aalto user account name (this will be enough), and I will send a combined list to the admins who will enable the Niksula accounts for you.\n\nAlso, the correct contact information for questions regarding GitLab is in fact guru (at) niksula.hut.fi."
            });

            _news.Add(new NewsItem {
                Date = DateTime.Parse("2013-02-11T21:51"),
                Title = "Niksula GitLab now available for use",
                Content = "The GitLab service provided by Niksula, the CSE department's own computer lab environment, is now available for use at https://git.niksula.hut.fi/ - though you need to have a Niksula user account in order to use it. If you don't have one, you can apply for one at the Niksula administration office B210 during the 12:15-14:00 daily office hours. If you have a question regarding the service which you can't find an answer to from the GitLab's own help system, you can ask the admins - guru ( at ) cs.hut.fi",
                Links = new NewsItemLink[] { 
                    new NewsItemLink { Title = "GitLab sign-in page", Description = "", Url = new Uri("https://git.niksula.hut.fi/users/sign_in") }, 
                    new NewsItemLink { Title="Niksula home page", Description="", Url = new Uri("http://www.niksula.hut.fi/") }
                }.ToList()
            });

            _news.Add(new NewsItem {
                Date = DateTime.Parse("2013-01-30T14:14"),
                Title = "Optional GIT lecture tomorrow and other updates",
                Content = "On Thursday, January 31st there will be a GIT Introduction lecture with some practical information how to use the GIT system. The lecture is not mandatory but highly recommended for people who are not experienced with GIT. The lecture will be given in the Main Building (left entrance), U414 from 16-18.\n\nThe project groups page has also been updated with the current information, and the lecture slides from today's Android lecture will be uploaded later today."
            });
            
            _news.Add(new NewsItem {
                Date = DateTime.Parse("2013-01-29T17:42"),
                Title = "Summer Internships in Data Communications Software research group",
                Content ="We have summer internship positions again this summer. Application deadline is end of January. Please read more from separate announcement. (Sorry about several mails if you are attending many of our courses.)",
                Links = new NewsItemLink[] { 
                    new NewsItemLink { 
                        Title = "Summer Internship announcement", Description = "", Url = new Uri("http://cse.aalto.fi/2013/01/15/summer-internships-in-data-communications-software-2/")
                    }
                }.ToList()
            });

            _news.Add(new NewsItem {
                Date = DateTime.Parse("2013-01-23T14:50"),
                Title = "More slides and the next assignment published",
                Content = "Slides from the second lecture can now be found on the Lectures page. Note that the group slides include the person responsible for gathering the first meeting and further instructions!\n\nAlso, I updated the Assignments page with instructions regarding the project plan."
            });
            
            _news.Add(new NewsItem {
                Date = DateTime.Parse("2013-01-07T15:20"),
                Title = "Details for the course registration added to the Assignments page",
                Content = "Registration details are now on the Assignments page. More information will also be provided during the introduction lecture on Wednesday, 16th January."
            });
            
            _news.Add(new NewsItem {
                Date = DateTime.Parse("2012-12-21T17:03"),
                Title = "Course in Spring 2013",
                Content = "The course T-110.5130 Mobile Systems Programming will start on Wednesday, 16th January at 12:15 in lecture room T2. The participants will be assigned to groups for project work, so registration is mandatory via e-mail. Details on the registration will be added later in early 2013. The theme of the Spring 2013 course is innovative mobile applications. The focus is in a practical programming project work, which involves groups of 4-5 students implementing specific mobile applications. The course will also include mandatory lectures on mobile programming, the first lecture covers the practicalities of the course, including the topics of the group projects. After the first lecture, the students can express their topic preferences."
            });
        }
    }
}
