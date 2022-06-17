using Moq;
using Microsoft.AspNetCore.Mvc;
using MainApp.Tests.Models.Repositories;
using MainApp.Tests.Models.Crew;

namespace MainApp.Tests.Controllers.Crew
{
    public class CrewControllerTests
    {
        // Interface with method imitations for controller testing
        // Mock is used to imitate objects or to create fake objects!!!
        private readonly Mock<ICrewRepository> mockRepo;
        // A imitation of the CrewController from the MainApp for testing isolation
        private readonly CrewController controller;

        public CrewControllerTests()
        {
            mockRepo = new Mock<ICrewRepository>();
            controller = new CrewController(mockRepo.Object);
        }



        [Fact]
        public void ViewUsers_Returns_AListOfUsers()
        {
            mockRepo.Setup(repo => repo.GetUsers())
                .Returns(GetTestUsers());

            var result = controller.ViewUsers();
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<User>>(viewResult.Model);

            Assert.Equal(GetTestUsers().Count, model.Count());
        }

        [Fact]
        public void ViewAdmins_Returns_AListOfAdmins()
        {
            mockRepo.Setup(repo => repo.GetAdmins())
                .Returns(GetTestAdmins());

            var result = controller.ViewAdmins();
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Admin>>(viewResult.Model);
            
            Assert.Equal(GetTestAdmins().Count, model.Count());
        }


        [Fact]
        public void AddAdmin_Get_Returns_ViewResult()
        {
            var result = controller.AddAdmin();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void AddAdmin_Post_Returns_AdminModelError()
        {
            controller.ModelState.AddModelError("Login", "Required");
            var newAdmin = new Admin();

            var result = controller.AddAdmin(newAdmin);
            var viewResult = Assert.IsType<ViewResult>(result);
            
            Assert.Equal(newAdmin, viewResult.Model);
        }

        [Fact]
        public void AddAdmin_Post_Returns_RedirectToAction()
        {
            var newAdmin = GetTestAdmins().FirstOrDefault(u => u.Id == 0);
            mockRepo.Setup(repo => repo.AddNewAdmin(newAdmin))
                .Returns(true);

            var result = controller.AddAdmin(newAdmin);
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            
            Assert.Null(redirectResult.ControllerName);
            Assert.Equal("ViewAdmins", redirectResult.ActionName);
        }

        [Fact]
        public void AddAdmin_Post_Returns_BadRequest()
        {
            var newAdmin = GetTestAdmins().FirstOrDefault(u => u.Id == 0);
            mockRepo.Setup(repo => repo.AddNewAdmin(newAdmin))
                .Returns(false);

            var result = controller.AddAdmin(newAdmin);
            
            Assert.IsType<BadRequestResult>(result);
        }


        [Fact]
        public void DeleteUsers_Returns_Json()
        {
            int userId = 1;
            mockRepo.Setup(repo => repo.RemoveUser(userId))
                .Returns(GetTestUsers().FirstOrDefault(u => u.Id == userId));

            var result = controller.DeleteUsers(userId);
            var jsonResult = Assert.IsType<JsonResult>(result);
            var model = (User)jsonResult.Value;

            Assert.IsType<User>(model);
            Assert.Equal(1, model.Id);
            Assert.Equal("Test1", model.Login);
            Assert.Equal("12345678", model.Password);
        }

        [Fact]
        public void DeleteUsers_Returns_NotFound()
        {
            int userId = 4;
            mockRepo.Setup(repo => repo.RemoveUser(userId))
                .Returns(GetTestUsers().FirstOrDefault(u => u.Id == userId));

            var result = controller.DeleteUsers(userId);
            
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void DeleteAdmins_Returns_Json()
        {
            int userId = 1;
            mockRepo.Setup(repo => repo.RemoveUser(userId))
                .Returns(GetTestUsers().FirstOrDefault(u => u.Id == userId));

            var result = controller.DeleteUsers(userId);
            var jsonResult = Assert.IsType<JsonResult>(result);
            var model = (User)jsonResult.Value;

            Assert.IsType<User>(model);
            Assert.Equal(1, model.Id);
            Assert.Equal("Test1", model.Login);
            Assert.Equal("12345678", model.Password);
        }

        [Fact]
        public void DeleteAdmins_Returns_NotFound()
        {
            int userId = 4;
            mockRepo.Setup(repo => repo.RemoveUser(userId))
                .Returns(GetTestUsers().FirstOrDefault(u => u.Id == userId));

            var result = controller.DeleteUsers(userId);
            
            Assert.IsType<BadRequestResult>(result);
        }




        // Additional methods for implementing testing
        private List<User> GetTestUsers()
        {
            return new List<User>
            {
                new User { Id=1, Login="Test1", Password="12345678" },
                new User { Id=1, Login="Test2", Password="12345678" },
                new User { Id=1, Login="Test3", Password="12345678" }
            };
        }
        private List<Admin> GetTestAdmins()
        {
            return new List<Admin>
            {
                new Admin { Id=1, Login="Test1", Password="12345678" },
                new Admin { Id=1, Login="Test2", Password="12345678" },
                new Admin { Id=1, Login="Test3", Password="12345678" }
            };
        }
    }
}
