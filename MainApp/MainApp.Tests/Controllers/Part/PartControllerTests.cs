using Moq;
using Microsoft.AspNetCore.Mvc;
using MainApp.Tests.Models.Repositories;
using MainApp.Tests.Models.Parts;


// controller.ViewBag.Title ---> UpdatePart
namespace MainApp.Tests.Controllers.Part
{
    public class PartControllerTests
    {
        // Interface with method imitations for controller testing
        // Mock is used to imitate objects or to create fake objects!!!
        private readonly Mock<IPartRepository> mockRepo;
        // A imitation of the PartController from the MainApp for testing isolation
        private readonly PartController controller;

        public PartControllerTests()
        {
            mockRepo = new Mock<IPartRepository>();
            controller = new PartController(mockRepo.Object);
        }



        [Fact]
        public void ViewParts_Returns_ViewResult_OnLoad()
        {
            var id = 0;
            var parentName = "test";
            mockRepo.Setup(repo => repo.GetParts(id, "onload"))
                .Returns(GetListOfParts());

            var result = controller.ViewParts(id, parentName, null);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<GeneralPart>>(viewResult.Model);

            Assert.Equal(GetListOfParts().Count(), model.Count());
            Assert.Equal("section", viewResult.ViewData["Name"]);
        }

        [Fact]
        public void ViewParts_Returns_BadRequest()
        {
            var id = 0;
            var parentName = "test";
            var table = "test"; 
            var list = new List<GeneralPart>();
            mockRepo.Setup(repo => repo.GetParts(id, table))
                .Returns((List<GeneralPart>)null);

            var result = controller.ViewParts(id, parentName, table);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void ViewParts_Returns_ListOfParts()
        {
            var id = 0;
            var parentName = "test";
            var table = "test";
            mockRepo.Setup(repo => repo.GetParts(id, table))
                .Returns(GetListOfParts());

            var result = controller.ViewParts(id, parentName, table);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<GeneralPart>>(viewResult.Model);

            Assert.Equal(GetListOfParts().Count, model.Count());
            Assert.Equal(parentName, viewResult.ViewData["Name"]);
        }

        [Fact]
        public void ViewParts_Returns_ListOfPosts()
        {
            var id = 0;
            var parentName = "test";
            var table = "subchapter";
            mockRepo.Setup(repo => repo.GetPosts(id))
                .Returns(GetListOfPosts());

            var result = controller.ViewParts(id, parentName, table);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Post>>(viewResult.Model);

            Assert.Equal(GetListOfPosts().Count, model.Count());
            Assert.Equal(parentName, viewResult.ViewData["Name"]);
        }

        [Fact]
        public void ViewPost_Returns_BadRequest()
        {
            var id = 0;
            var table = "subchapter";
            mockRepo.Setup(repo => repo.GetPart(id, table))
                .Returns((Post)null);

            var result = controller.ViewPost(id);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void ViewPost_Returns_Post()
        {
            var id = 0;
            var table = "subchapter";
            var post = GetListOfPosts().FirstOrDefault(p => p.Id == id);
            mockRepo.Setup(repo => repo.GetPart(id, table))
                .Returns(post);

            var result = controller.ViewPost(id);
            var viewResult = Assert.IsType<ViewResult>(result);

            Assert.Equal(post, viewResult.Model);
        }



        [Fact]
        public void AddPart_Get_Returns_ListOfObjects()
        {
            var parentId = 0;
            var parentName = "test";
            var table = "test";
            var part = new List<object[]> { new object[] { parentId, parentName, table } };

            var result = controller.AddPart(parentId, parentName, table);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<object[]>>(viewResult.Model);
            
            Assert.Equal(part.Count(), model.Count());
        }

        [Fact]
        public void AddPart_Post_Returns_BadRequest()
        {
            var partName = "test";
            var parentId = 0;
            var table = "test";
            mockRepo.Setup(repo => repo.AddPartRepo(parentId, partName, table))
                .Returns(false);

            var result = controller.AddPart(partName, parentId, table);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void AddPart_Post_Returns_RedirectToAction()
        {
            var partName = "test";
            var parentId = 0;     
            var table = "test";
            var list = new List<object[]> { new object[] { parentId, table } };
            mockRepo.Setup(repo => repo.AddPartRepo(parentId, partName, table))
                .Returns(true);

            var result = controller.AddPart(partName, parentId, table);
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal("ViewParts", redirectResult.ActionName);
            Assert.Equal(list.Count(), redirectResult.RouteValues?
                .Values.ElementAtOrDefault(1));
        }


        [Fact]
        public void AddPost_Get_Returns_ListOfObjects()
        {
            var parentId = 0;
            var parentName = "test";
            var part = new List<object[]> { new object[] { parentId, parentName } };

            var result = controller.AddPost(parentId, parentName);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<object[]>>(viewResult.Model);

            Assert.Equal(part.Count(), model.Count());
        }

        [Fact]
        public void AddPost_Post_Returns_BadRequest()
        {
            var postName = "test";
            var content = "test";
            var parentId = 0;
            mockRepo.Setup(repo => repo.AddPostRepo(parentId, postName, content))
                .Returns(false);

            var result = controller.AddPost(postName, content, parentId);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void AddPost_Post_Returns_RedirectToAction()
        {
            var postName = "test";
            var content = "test";
            var parentId = 0;
            var list = new List<object[]> { new object[] { parentId, "subchapter" } };
            mockRepo.Setup(repo => repo.AddPostRepo(parentId, postName, content))
                .Returns(true);

            var result = controller.AddPost(postName, content, parentId);
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal("ViewParts", redirectResult.ActionName);
            Assert.Equal(list.Count(), redirectResult.RouteValues?
                .Values.ElementAtOrDefault(1));
        }



        [Fact]
        public void UpdatePart_Get_Returns_OnlyGeneralPart()
        {
            var id = 0;
            var table = "section";
            var part = GetListOfParts().FirstOrDefault(p => p.Id == id);
            mockRepo.Setup(repo => repo.GetPart(id, table))
                .Returns(part);

            var result = controller.UpdatePart(id, table);
            var viewResult = Assert.IsType<ViewResult>(result);

            Assert.Null(viewResult.ViewData["Parents"]);
            Assert.Equal(part, viewResult.Model);
        }

        [Fact]
        public void UpdatePart_Get_Returns_GeneralPart_And_ViewBag()
        {
            var id = 0;
            var table = "test";
            var part = GetListOfParts().FirstOrDefault(p => p.Id == id);
            mockRepo.Setup(repo => repo.GetPart(id, table))
                .Returns(part);
            mockRepo.Setup(repo => repo.GetParts(id, table + "parents"))
                .Returns(GetListOfParts());

            var result = controller.UpdatePart(id, table);
            var viewResult = Assert.IsType<ViewResult>(result);
            var viewBag = Assert.IsAssignableFrom<IEnumerable<GeneralPart>>
                (viewResult.ViewData["Parents"]);

            Assert.Equal(viewBag.Count(), GetListOfParts().Count());
            Assert.Equal(part, viewResult.Model);
        }

        [Fact]
        public void UpdatePart_Post_Returns_BadRequest()
        {
            var newName = "test";
            var partId = 0;
            var parentId = 0;
            var table = "test";
            mockRepo.Setup(
                repo => repo.UpdatePartRepo(
                    partId, parentId, newName, String.Empty, table))
                .Returns(false);

            var result = controller.UpdatePart(newName, partId, parentId, table);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void UpdatePart_Post_Returns_RedirectToAction()
        {
            var newName = "test";
            var partId = 0;
            var parentId = 0;
            var table = "test";
            var list = new List<object[]> { new object[] { parentId, table } };
            mockRepo.Setup(
                repo => repo.UpdatePartRepo(
                    partId, parentId, newName, String.Empty, table))
                .Returns(true);

            var result = controller.UpdatePart(newName, partId, parentId, table);
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal("ViewParts", redirectResult.ActionName);
            Assert.Equal(list.Count(), redirectResult.RouteValues?
                .Values.ElementAtOrDefault(1));
        }


        [Fact]
        public void UpdatePost_Get_Returns_GeneralPart_And_ViewBag()
        {
            var id = 0;
            var post = GetListOfParts().FirstOrDefault(p => p.Id == id);
            mockRepo.Setup(repo => repo.GetPart(id, "post"))
                .Returns(post);
            mockRepo.Setup(repo => repo.GetParts(id, "postparents"))
                .Returns(GetListOfParts());

            var result = controller.UpdatePost(id);
            var viewResult = Assert.IsType<ViewResult>(result);
            var viewBag = Assert.IsAssignableFrom<IEnumerable<GeneralPart>>
                (viewResult.ViewData["Parents"]);

            Assert.Equal(viewBag.Count(), GetListOfParts().Count());
            Assert.Equal(post, viewResult.Model);
        }

        [Fact]
        public void UpdatePost_Post_Returns_BadRequest()
        {
            var newName = "test";
            var content = "test";
            var partId = 0;
            var parentId = 0;
            var table = "test";
            mockRepo.Setup(
                repo => repo.UpdatePartRepo(
                    partId, parentId, newName, content, table))
                .Returns(false);

            var result = controller.UpdatePost(newName, content, partId, parentId, table);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void UpdatePost_Post_Returns_RedirectToAction()
        {
            var newName = "test";
            var content = "test";
            var partId = 0;
            var parentId = 0;
            var table = "test";
            var list = new List<object[]> { new object[] { parentId, table } };
            mockRepo.Setup(
                repo => repo.UpdatePartRepo(
                    partId, parentId, newName, content, table))
                .Returns(true);

            var result = controller.UpdatePost(newName, content, partId, parentId, table);
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal("ViewParts", redirectResult.ActionName);
            Assert.Equal(list.Count(), redirectResult.RouteValues?
                .Values.ElementAtOrDefault(1));
        }



        [Fact]
        public void DeletePart_Returns_BadRequest()
        {
            var id = 0;
            var table = "test";
            mockRepo.Setup(repo => repo.RemovePart(id, table))
                .Returns((GeneralPart)null);

            var result = controller.DeletePart(id, table);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void DeletePart_Returns_JsonResult()
        {
            var id = 0;
            var table = "test";
            var part = new GeneralPart();
            mockRepo.Setup(repo => repo.RemovePart(id, table))
                .Returns(part);

            var result = controller.DeletePart(id, table);

            var jsonResult = Assert.IsType<JsonResult>(result);
        }

        [Fact]
        public void DeletePost_Returns_BadRequest()
        {
            var id = 0;
            mockRepo.Setup(repo => repo.RemovePost(id))
                .Returns((Post)null);

            var result = controller.DeletePost(id);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void DeletePost_Returns_JsonResult()
        {
            var id = 0;
            var post = new Post();
            mockRepo.Setup(repo => repo.RemovePost(id))
                .Returns(post);

            var result = controller.DeletePost(id);

            var jsonResult = Assert.IsType<JsonResult>(result);
        }




        // Additional methods for implementing testing
        private List<GeneralPart> GetListOfParts()
        {
            return new List<GeneralPart>
            {
                new GeneralPart { Title = "Test1", Table = "testTable" },
                new GeneralPart { Title = "Test2", Table = "testTable" },
                new GeneralPart { Title = "Test3", Table = "testTable" },
                new GeneralPart { Title = "Test4", Table = "testTable" }
            };
        }

        private List<Post> GetListOfPosts()
        {
            return new List<Post>
            {
                new Post { Title = "Test1", Content = "testTable" },
                new Post { Title = "Test2", Content = "testTable" },
                new Post { Title = "Test3", Content = "testTable" },
                new Post { Title = "Test4", Content = "testTable" }
            };
        }
    }
}
