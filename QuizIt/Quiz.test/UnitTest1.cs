using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuizIt.Controllers;
using QuizIt.Models;
using QuizIt.Models.ViewModels;
using QuizIt.Services.Spotify;

namespace Quiz.test
{


    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void should_display_about_page()
        {
            HomeController controller = new HomeController();

            ViewResult result = controller.About() as ViewResult;

            Assert.AreEqual("About", result.ViewName);
        }

        [TestMethod]
        public void should_display_contact_page()
        {
            HomeController controller = new HomeController();

            ViewResult result = controller.Contact() as ViewResult;

            Assert.AreEqual("Contact", result.ViewName);
        }

    }

}
