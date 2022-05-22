using Microsoft.EntityFrameworkCore;
using MainApp.Models;

namespace MainApp.Services
{
    // Service for fetching parts from the database and passing them to display
    public class PartsService
    {
        // Data context for parts
        private TopicsContext data;
        // Logger for exceptions

        private ILogger<PartsService> logger;
        public PartsService(TopicsContext _db, ILogger<PartsService> _logger)
        {
            data = _db;
            logger = _logger;
        }



        public async Task<List<GeneralPart>> GetPartsAsync(int id, string table)
        {
            try
            {
                switch (table)
                {
                    // For Display parts
                    case "onload":
                        var sections = await data.Sections.ToListAsync();
                        if (sections == null)
                        {
                            return null;
                        }
                        return sections;

                    case "section":
                        var section = await data.Sections.FirstOrDefaultAsync(s => s.Id == id);
                        if (section == null)
                        {
                            return null;
                        }
                        var subsections = await data.Subsections.Where(s => s.Parent == section).ToListAsync();

                        return subsections;

                    case "subsection":
                        var subsection = await data.Subsections.FirstOrDefaultAsync(s => s.Id == id);
                        if (subsection == null)
                        {
                            return null;
                        }

                        return await data.Chapters.Where(s => s.Parent == subsection).ToListAsync();

                    case "chapter":
                        var chapter = await data.Chapters.FirstOrDefaultAsync(s => s.Id == id);
                        if (chapter == null)
                        {
                            return null;
                        }

                        return await data.Subchapters.Where(s => s.Parent == chapter).ToListAsync();


                    // For Choosing new Parent
                    case "subsectionparents":
                        subsection = await data.Subsections.FirstOrDefaultAsync(s => s.Id == id);
                        if (subsection == null)
                        {
                            return null;
                        }
                        return await data.Sections.Where(p => p != subsection.Parent).ToListAsync();

                    case "chapterparents":
                        chapter = await data.Chapters.FirstOrDefaultAsync(s => s.Id == id);
                        if (chapter == null)
                        {
                            return null;
                        }
                        return await data.Subsections.Where(p => p != chapter.Parent).ToListAsync();

                    case "subchapterparents":
                        var subchapter = await data.Subchapters.FirstOrDefaultAsync(s => s.Id == id);
                        if (subchapter == null)
                        {
                            return null;
                        }
                        return await data.Chapters.Where(p => p != subchapter.Parent).ToListAsync();

                    case "postparents":
                        var post = await data.Posts.FirstOrDefaultAsync(s => s.Id == id);
                        if (post == null)
                        {
                            return null;
                        }
                        return await data.Subchapters.Where(p => p != post.Parent).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }

            return null;
        }

        public async Task<List<Post>> GetPostsAsync(int id)
        {
            var subchapter = await data.Subchapters.FirstOrDefaultAsync(s => s.Id == id);
            if (subchapter == null)
            {
                return null;
            }

            return await data.Posts.Where(s => s.Parent == subchapter).ToListAsync();
        }


        public async Task<GeneralPart> GetPartAsync(int id, string table)
        {
            try
            {
                switch (table)
                {
                    case "section":
                        if (data.Sections.Any(s => s.Id == id))
                        {
                            return await data.Sections.FirstOrDefaultAsync(s => s.Id == id);
                        }
                        return null;

                    case "subsection":
                        if (data.Subsections.Any(s => s.Id == id))
                        { 
                            return await data.Subsections.FirstOrDefaultAsync(s => s.Id == id);
                        }
                        return null;

                    case "chapter":
                        if (data.Chapters.Any(s => s.Id == id))
                        {
                            return await data.Chapters.FirstOrDefaultAsync(s => s.Id == id);
                        }
                        return null;

                    case "subchapter":
                        if (data.Subchapters.Any(s => s.Id == id))
                        {
                            return await data.Subchapters.FirstOrDefaultAsync(s => s.Id == id);
                        }
                        return null;

                    case "post":
                        if (data.Posts.Any(s => s.Id == id))
                        {
                            return await data.Posts.FirstOrDefaultAsync(s => s.Id == id);
                        }
                        return null;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }

            return null;
        }




        //Add
        public async Task<bool> AddPartAsync(int parentId, string addName, string table)
        {
            try
            {
                switch (table)
                {
                    case "section":
                        if (!data.Sections.Any(s => s.Title == addName))
                        {
                            var section = new GeneralPart { Title = addName, CreatedDate = DateTime.UtcNow, Table = "section" };
                            await data.Sections.AddAsync(section);
                            await data.SaveChangesAsync();
                            return true;
                        }
                        return false;

                    case "subsection":
                        if (!data.Subsections.Any(s => s.Title == addName))
                        {
                            var section = await data.Sections.FirstOrDefaultAsync(s => s.Id == parentId);
                            var subsection = new GeneralPart { Title = addName, CreatedDate = DateTime.UtcNow, Parent = section, Table = "subsection" };
                            await data.Subsections.AddAsync(subsection);
                            await data.SaveChangesAsync();
                            return true;
                        }
                        return false;

                    case "chapter":
                        if (!data.Chapters.Any(s => s.Title == addName))
                        {
                            var subsection = await data.Subsections.FirstOrDefaultAsync(s => s.Id == parentId);
                            var chapter = new GeneralPart { Title = addName, CreatedDate = DateTime.UtcNow, Parent = subsection, Table = "chapter" };
                            await data.Chapters.AddAsync(chapter);
                            await data.SaveChangesAsync();
                            return true;
                        }
                        return false;

                    case "subchapter":
                        if (!data.Subchapters.Any(s => s.Title == addName))
                        {
                            var chapter = await data.Chapters.FirstOrDefaultAsync(s => s.Id == parentId);
                            var subchapter = new GeneralPart { Title = addName, CreatedDate = DateTime.UtcNow, Parent = chapter, Table = "subchapter" };
                            await data.Subchapters.AddAsync(subchapter);
                            await data.SaveChangesAsync();
                            return true;
                        }
                        return false;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }

            return false;
        }

        public async Task<bool> AddPostAsync(int parentId, string postName, string content)
        {
            try
            {
                if (!data.Posts.Any(p => p.Title == postName && p.ParentId == parentId))
                {
                    var subchapter = await data.Subchapters.FirstOrDefaultAsync(s => s.Id == parentId);
                    var post = new Post { Title = postName, Content = content, CreatedDate = DateTime.UtcNow, Parent = subchapter, Table = "post" };
                    await data.Posts.AddAsync(post);
                    await data.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }

            return false;
        }



        //Update
        public async Task<bool> UpdatePartAsync(int partId, int parentId, string newName, string content, string table)
        {
            try 
            {
                switch (table)
                {
                    case "section":
                        if (!data.Sections.Any(s => s.Title == newName))
                        {
                            var section = await data.Sections.FirstOrDefaultAsync(s => s.Id == partId);
                            section.Title = newName;
                            section.CreatedDate = DateTime.UtcNow;
                            await data.SaveChangesAsync();
                            return true;
                        }
                        return false;

                    case "subsection":
                        if (!data.Subsections.Any(s => s.Title == newName))
                        {
                            var subsection = await data.Subsections.FirstOrDefaultAsync(s => s.Id == partId);
                            var section = await data.Sections.FirstOrDefaultAsync(s => s.Id == parentId);
                            subsection.Title = newName;

                            if (section != null)
                            {
                                subsection.Parent = section;
                            }

                            subsection.CreatedDate = DateTime.UtcNow;
                            await data.SaveChangesAsync();
                            return true;
                        }
                        return false;

                    case "chapter":
                        if (!data.Chapters.Any(s => s.Title == newName))
                        {
                            var chapter = await data.Chapters.FirstOrDefaultAsync(s => s.Id == partId);
                            var subsection = await data.Subsections.FirstOrDefaultAsync(s => s.Id == parentId);
                            chapter.Title = newName;

                            if (subsection != null)
                            {
                                chapter.Parent = subsection;
                            }

                            chapter.CreatedDate = DateTime.UtcNow;
                            await data.SaveChangesAsync();
                            return true;
                        }
                        return false;

                    case "subchapter":
                        if (!data.Subchapters.Any(s => s.Title == newName))
                        {
                            var subchapter = await data.Subchapters.FirstOrDefaultAsync(s => s.Id == partId);
                            var chapter = await data.Chapters.FirstOrDefaultAsync(s => s.Id == parentId);
                            subchapter.Title = newName;

                            if (chapter != null)
                            {
                                subchapter.Parent = chapter;
                            }

                            subchapter.CreatedDate = DateTime.UtcNow;
                            await data.SaveChangesAsync();
                            return true;
                        }
                        return false;

                    case "post":
                        if (!data.Posts.Any(s => s.Title == newName))
                        {
                            var post = await data.Posts.FirstOrDefaultAsync(s => s.Id == partId);
                            var subchapter = await data.Subchapters.FirstOrDefaultAsync(s => s.Id == parentId);
                            post.Title = newName;
                            post.Content = content;

                            if (subchapter != null)
                            {
                                post.Parent = subchapter;
                            }

                            post.CreatedDate = DateTime.UtcNow;
                            await data.SaveChangesAsync();
                            return true;
                        }
                        return false;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }

            return false;
        }

      



        //Delete
        public async Task<JsonContent> RemovePartAsync(int id, string table)
        {
            try
            {
                switch (table)
                {
                    case "onload":
                        if (data.Sections.Any(s => s.Id == id))
                        {
                            var section = await data.Sections.FirstOrDefaultAsync(s => s.Id == id);
                            data.Sections.Remove(section);
                            await data.SaveChangesAsync();
                            return JsonContent.Create(section);
                        }
                        return null;

                    case "section":
                        if (data.Subsections.Any(s => s.Id == id))
                        {
                            var subsection = await data.Subsections.FirstOrDefaultAsync(s => s.Id == id);
                            data.Subsections.Remove(subsection);
                            await data.SaveChangesAsync();
                            return JsonContent.Create(subsection);
                        }
                        return null;

                    case "subsection":
                        if (data.Chapters.Any(s => s.Id == id))
                        {
                            var chapter = await data.Chapters.FirstOrDefaultAsync(s => s.Id == id);
                            data.Chapters.Remove(chapter);
                            await data.SaveChangesAsync();
                            return JsonContent.Create(chapter);
                        }
                        return null;

                    case "chapter":
                        if (data.Subchapters.Any(s => s.Id == id))
                        {
                            var subchapter = await data.Subchapters.FirstOrDefaultAsync(s => s.Id == id);
                            data.Subchapters.Remove(subchapter);
                            await data.SaveChangesAsync();
                            return JsonContent.Create(subchapter);
                        }
                        return null;

                    case "subchapter":
                        if (data.Subchapters.Any(s => s.Id == id))
                        {
                            var post = await data.Posts.FirstOrDefaultAsync(s => s.Id == id);
                            data.Posts.Remove(post);
                            await data.SaveChangesAsync();
                            return JsonContent.Create(post);
                        }
                        return null;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }

            return null;
        }
    }
}
