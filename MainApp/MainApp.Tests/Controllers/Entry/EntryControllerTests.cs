using Moq;
using Microsoft.AspNetCore.Mvc;
using MainApp.Tests.Models.Repositories;
using MainApp.Tests.Models.Crew;

namespace MainApp.Tests.Controllers.Entry
{
    public class EntryControllerTests
    {
        // Interface with method imitations for controller testing
        // Mock is used to imitate objects or to create fake objects!!!
        private readonly Mock<IEntryRepository> mockRepo;
        // A imitation of the EntryController from the MainApp for testing isolation
        private readonly EntryController controller;

        public EntryControllerTests()
        {
            mockRepo = new Mock<IEntryRepository>();
            controller = new EntryController(mockRepo.Object);
        }



        [Fact]
        public void UserRegistration_Returns_CreateUserModelError()
        {
            controller.ModelState.AddModelError("Login", "Required");
            var newUser = new CreateUserModel();

            var result = controller.UserRegistration(newUser);
            var viewResult = Assert.IsType<ViewResult>(result);
            
            Assert.Equal(newUser, viewResult.Model);
        }

        [Fact]
        public void UserRegistration_Returns_AvailabilityCheckFalse()
        {
            var newUser = new CreateUserModel 
            { 
                Login = "Test", 
                Password = "12345678"
            };
            mockRepo.Setup(repo => repo.AvailabilityCheck(newUser.Login))
                .Returns(false);

            var result = controller.UserRegistration(newUser);
            var viewResult = Assert.IsType<ViewResult>(result);

            Assert.Equal(viewResult.ViewData["Error"], "Пользователь с таким логином уже существует");
            Assert.Equal(newUser, viewResult.Model);
        }

        [Fact]
        public void UserRegistration_Returns_RedirectToAction()
        {
            var newUser = new CreateUserModel
            {
                Login = "Test",
                Password = "12345678"
            };
            mockRepo.Setup(repo => repo.AvailabilityCheck(newUser.Login))
                .Returns(true);

            var result = controller.UserRegistration(newUser);
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            
            mockRepo.Verify(repo => repo.AddUser(newUser.Login, newUser.Password));
            Assert.Equal("Study", redirectResult.ActionName);
            Assert.Equal("Page", redirectResult.ControllerName);
        }


        [Fact]
        public void UserLogin_Get_Returns_ViewResult()
        {
            var user = new User { Login = null };
            mockRepo.Setup(repo => repo.GetUserInfo())
                .Returns(user);

            var result = controller.UserLogin();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void UserLogin_Get_Returns_RedirectToAction()
        {
            mockRepo.Setup(repo => repo.GetUserInfo())
                .Returns(new User { Login = "Test"});

            var result = controller.UserLogin();
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal("Study", redirectResult.ActionName);
            Assert.Equal("Page", redirectResult.ControllerName);
        }

        [Fact]
        public void UserLogin_Post_Returns_UserModelError()
        {
            controller.ModelState.AddModelError("Login", "Required");
            var user = new User();

            var result = controller.UserLogin(user, true);
            var viewResult = Assert.IsType<ViewResult>(result);

            Assert.Equal(user, viewResult.Model);
        }

        [Fact]
        public void UserLogin_Post_Returns_UserAuthenticationFalse()
        {
            var user = new User { Login = "Test", Password = "12345678" };
            mockRepo.Setup(repo => repo.UserAuthentication(user.Login, user.Password))
                .Returns(false);

            var result = controller.UserLogin(user, true);
            var viewResult = Assert.IsType<ViewResult>(result);

            Assert.Equal(viewResult.ViewData["Error"], "Логин или пароль неверны!");
            Assert.Equal(user, viewResult.Model);
        }

        [Fact]
        public void UserLogin_Post_Returns_RedirectToAction()
        {
            var user = new User { Login = "Test", Password = "12345678" };
            mockRepo.Setup(repo => repo.UserAuthentication(user.Login, user.Password))
                .Returns(true);

            var result = controller.UserLogin(user, true);
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);

            mockRepo.Verify(repo => repo.UserAuthorization(user.Login, true));
            Assert.Equal("Study", redirectResult.ActionName);
            Assert.Equal("Page", redirectResult.ControllerName);
        }


        [Fact]
        public void Logout_Returns_RedirectToAction()
        {
            var result = controller.Logout();
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);

            mockRepo.Verify(repo => repo.Logout());
            Assert.Equal("Welcome", redirectResult.ActionName);
            Assert.Equal("Page", redirectResult.ControllerName);
        }


        [Fact]
        public void CrewLogin_Get_Returns_Null()
        {
            var user = new User { Role = null };
            mockRepo.Setup(repo => repo.GetUserInfo()).Returns(user);

            var result = controller.CrewLogin();
            Assert.Null(result);
        }

        [Theory]
        [MemberData("ReturnNewRole")]
        public void CrewLogin_Get_Returns_RedirectOrViewResult(string role)
        {
            var user = new User { Role = role };
            mockRepo.Setup(repo => repo.GetUserInfo()).Returns(user);

            var result = controller.CrewLogin();

            switch (user.Role)
            {
                case "admin" or "editor":
                    {
                        var redirectResult = Assert.IsType<RedirectToActionResult>(result);

                        Assert.Equal("ViewParts", redirectResult.ActionName);
                        Assert.Equal("Part", redirectResult.ControllerName);
                        break;
                    }

                case "user":
                    {
                        var redirectResult = Assert.IsType<RedirectToActionResult>(result);

                        Assert.Equal("Study", redirectResult.ActionName);
                        Assert.Equal("Page", redirectResult.ControllerName);
                        break;
                    }
                case "null":
                    {
                        Assert.IsType<ViewResult>(result);
                        break;
                    }
            }
        }

        [Fact]
        public void CrewLogin_Post_Returns_AdminModelError()
        {
            controller.ModelState.AddModelError("Login", "Required");
            var admin = new Admin();

            var result = controller.CrewLogin(admin);
            var viewResult = Assert.IsType<ViewResult>(result);

            Assert.Equal(admin, viewResult.Model);
        }

        [Fact]
        public void CrewLogin_Post_Returns_AuthenticationFalse()
        {
            var admin = new Admin { Login = "Test", Password = "12345678"};
            mockRepo.Setup(repo => repo.AdmAuthentication(admin.Login, admin.Password))
                .Returns(false);

            var result = controller.CrewLogin(admin);
            var viewResult = Assert.IsType<ViewResult>(result);

            Assert.Equal(viewResult.ViewData["Error"], "Логин или пароль неверны!");
            Assert.Equal(admin, viewResult.Model);
        }

        [Fact]
        public void CrewLogin_Post_Returns_RedirectToAction()
        {
            var admin = new Admin { Login = "Test", Password = "12345678" };
            mockRepo.Setup(repo => repo.AdmAuthentication(admin.Login, admin.Password))
                .Returns(true);

            var result = controller.CrewLogin(admin);
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal("ViewParts", redirectResult.ActionName);
            Assert.Equal("Part", redirectResult.ControllerName);
        }




        // Additional methods for implementing testing
        public static IEnumerable<object[]> ReturnNewRole()
        {
            yield return new object[] { "admin" };
            yield return new object[] { "editor" };
            yield return new object[] { "user" };
            yield return new object[] { "null" };
        }
    }
}
