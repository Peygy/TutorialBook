using MainApp.Tests.Models.Parts;

namespace MainApp.Tests.Models.Repositories
{
    // Interface with method imitations for controller testing
    public interface IPartRepository
    {
        public List<GeneralPart> GetParts(int id, string table);
        public List<Post> GetPosts(int id);
        public GeneralPart GetPart(int id, string table);
        public bool AddPartRepo(int parentId, string addName, string table);
        public bool AddPostRepo(int parentId, string postName, string content);
        public bool UpdatePartRepo(int partId, int parentId, string newName, string content, string table);
        public GeneralPart RemovePart(int id, string table);
        public Post RemovePost(int id);
    }
}
