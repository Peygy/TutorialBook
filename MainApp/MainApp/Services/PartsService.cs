using Microsoft.EntityFrameworkCore;
using MainApp.Models;

namespace MainApp.Services
{
    public class PartsService
    {
        private TopicsContext topicsData;
        private ILogger<PartsService> logger;


        //Parts actions 
        //Get
        public async Task<Array> GetPartsAsync_Db(int id, string table)
        {
            try
            {
                switch (table)
                {
                    // For View
                    case "onload":
                        var sections = await topicsData.Sections.ToListAsync();
                        if (sections == null)
                        {
                            return null;
                        }
                        return sections.ToArray();

                    case "section":
                        var section = await topicsData.Sections.FirstOrDefaultAsync(s => s.Id == id);
                        if (section == null)
                        {
                            return null;
                        }
                        var subsections = await topicsData.SubSections.Where(s => s.Section == section).ToListAsync();

                        return subsections.ToArray();

                    case "subsection":
                        var subsection = await topicsData.SubSections.FirstOrDefaultAsync(s => s.Id == id);
                        if (subsection == null)
                        {
                            return null;
                        }
                        var chapters = await topicsData.Chapters.Where(s => s.Subsection == subsection).ToListAsync();

                        return chapters.ToArray();

                    case "chapter":
                        var chapter = await topicsData.Chapters.FirstOrDefaultAsync(s => s.Id == id);
                        if (chapter == null)
                        {
                            return null;
                        }
                        var subchapters = await topicsData.SubChapters.Where(s => s.Chapter == chapter).ToListAsync();

                        return subchapters.ToArray();

                    case "subchapter":
                        var subchapter = await topicsData.SubChapters.FirstOrDefaultAsync(s => s.Id == id);
                        if (subchapter == null)
                        {
                            return null;
                        }
                        var posts = await topicsData.Posts.Where(s => s.Subchapter == subchapter).ToListAsync();

                        return posts.ToArray();


                    // For Choosing
                    case "sections":
                        subsection = await topicsData.SubSections.FirstOrDefaultAsync(s => s.Id == id);
                        if (subsection == null)
                        {
                            return null;
                        }

                        sections = await topicsData.Sections.Where(p => p != subsection.Section).ToListAsync();
                        return sections.ToArray();

                    case "subsections":
                        chapter = await topicsData.Chapters.FirstOrDefaultAsync(s => s.Id == id);
                        if (chapter == null)
                        {
                            return null;
                        }

                        subsections = await topicsData.SubSections.Where(p => p != chapter.Subsection).ToListAsync();
                        return subsections.ToArray();

                    case "chapters":
                        subchapter = await topicsData.SubChapters.FirstOrDefaultAsync(s => s.Id == id);
                        if (subchapter == null)
                        {
                            return null;
                        }

                        chapters = await topicsData.Chapters.Where(p => p != subchapter.Chapter).ToListAsync();
                        return chapters.ToArray();

                    case "subchapters":
                        var post = await topicsData.Posts.FirstOrDefaultAsync(s => s.Id == id);
                        if (post == null)
                        {
                            return null;
                        }

                        subchapters = await topicsData.SubChapters.Where(p => p != post.Subchapter).ToListAsync();
                        return subchapters.ToArray();
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }

            return null;
        }

        public async Task<object> GetPartAsync_Db(int id, string table)
        {
            try
            {
                switch (table)
                {
                    case "onload":
                        if (topicsData.Sections.Any(s => s.Id == id))
                        {
                            return await topicsData.Sections.FirstOrDefaultAsync(s => s.Id == id);
                        }
                        return null;

                    case "section":
                        if (topicsData.SubSections.Any(s => s.Id == id))
                        {
                            return await topicsData.SubSections.FirstOrDefaultAsync(s => s.Id == id);
                        }
                        return null;

                    case "subsection":
                        if (topicsData.Chapters.Any(s => s.Id == id))
                        {
                            return await topicsData.Chapters.FirstOrDefaultAsync(s => s.Id == id);
                        }
                        return null;

                    case "chapter":
                        if (topicsData.SubChapters.Any(s => s.Id == id))
                        {
                            return await topicsData.SubChapters.FirstOrDefaultAsync(s => s.Id == id);
                        }
                        return null;

                    case "subchapter":
                        if (topicsData.Posts.Any(s => s.Id == id))
                        {
                            return await topicsData.Posts.FirstOrDefaultAsync(s => s.Id == id);
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
        public async Task<bool> AddPartAsync_Db(int parentId, string addName, string table)
        {
            try
            {
                switch (table)
                {
                    case "section":
                        if (!topicsData.Sections.Any(s => s.Title == addName))
                        {
                            var section = new Section { Title = addName, CreatedDate = DateTime.UtcNow };
                            await topicsData.Sections.AddAsync(section);
                            await topicsData.SaveChangesAsync();
                            return true;
                        }
                        return false;

                    case "subsection":
                        if (!topicsData.SubSections.Any(s => s.Title == addName))
                        {
                            var section = await topicsData.Sections.FirstOrDefaultAsync(s => s.Id == parentId);
                            var subsection = new Subsection { Title = addName, CreatedDate = DateTime.UtcNow, Section = section };
                            await topicsData.SubSections.AddAsync(subsection);
                            await topicsData.SaveChangesAsync();
                            return true;
                        }
                        return false;

                    case "chapter":
                        if (!topicsData.Chapters.Any(s => s.Title == addName))
                        {
                            var subsection = await topicsData.SubSections.FirstOrDefaultAsync(s => s.Id == parentId);
                            var chapter = new Chapter { Title = addName, CreatedDate = DateTime.UtcNow, Subsection = subsection };
                            await topicsData.Chapters.AddAsync(chapter);
                            await topicsData.SaveChangesAsync();
                            return true;
                        }
                        return false;

                    case "subchapter":
                        if (!topicsData.SubChapters.Any(s => s.Title == addName))
                        {
                            var chapter = await topicsData.Chapters.FirstOrDefaultAsync(s => s.Id == parentId);
                            var subchapter = new Subchapter { Title = addName, CreatedDate = DateTime.UtcNow, Chapter = chapter };
                            await topicsData.SubChapters.AddAsync(subchapter);
                            await topicsData.SaveChangesAsync();
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

        public async Task<bool> AddPostAsync_Db(int parentId, string postName, string content)
        {
            try
            {
                if (!topicsData.Posts.Any(p => p.Title == postName && p.Subchapter.Id == parentId))
                {
                    var subchapter = await topicsData.SubChapters.FirstOrDefaultAsync(s => s.Id == parentId);
                    var post = new Post { Title = postName, Content = content, CreatedDate = DateTime.UtcNow, Subchapter = subchapter };
                    await topicsData.Posts.AddAsync(post);
                    await topicsData.SaveChangesAsync();
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
        public async Task<bool> UpdatePartAsync_Db(int partId, int parentId, string newName, string content, string table)
        {
            try 
            {
                switch (table)
                {
                    case "section":
                        if (!topicsData.Sections.Any(s => s.Title == newName))
                        {
                            var section = await topicsData.Sections.FirstOrDefaultAsync(s => s.Id == partId);
                            section.Title = newName;
                            section.CreatedDate = DateTime.UtcNow;
                            await topicsData.SaveChangesAsync();
                            return true;
                        }
                        return false;

                    case "subsection":
                        if (!topicsData.SubSections.Any(s => s.Title == newName))
                        {
                            var subsection = await topicsData.SubSections.FirstOrDefaultAsync(s => s.Id == partId);
                            var section = await topicsData.Sections.FirstOrDefaultAsync(s => s.Id == parentId);
                            subsection.Title = newName;

                            if (section != null)
                            {
                                subsection.Section = section;
                            }

                            subsection.CreatedDate = DateTime.UtcNow;
                            await topicsData.SaveChangesAsync();
                            return true;
                        }
                        return false;

                    case "chapter":
                        if (!topicsData.Chapters.Any(s => s.Title == newName))
                        {
                            var chapter = await topicsData.Chapters.FirstOrDefaultAsync(s => s.Id == partId);
                            var subsection = await topicsData.SubSections.FirstOrDefaultAsync(s => s.Id == parentId);
                            chapter.Title = newName;

                            if (subsection != null)
                            {
                                chapter.Subsection = subsection;
                            }

                            chapter.CreatedDate = DateTime.UtcNow;
                            await topicsData.SaveChangesAsync();
                            return true;
                        }
                        return false;

                    case "subchapter":
                        if (!topicsData.SubChapters.Any(s => s.Title == newName))
                        {
                            var subchapter = await topicsData.SubChapters.FirstOrDefaultAsync(s => s.Id == partId);
                            var chapter = await topicsData.Chapters.FirstOrDefaultAsync(s => s.Id == parentId);
                            subchapter.Title = newName;

                            if (chapter != null)
                            {
                                subchapter.Chapter = chapter;
                            }

                            subchapter.CreatedDate = DateTime.UtcNow;
                            await topicsData.SaveChangesAsync();
                            return true;
                        }
                        return false;

                    case "post":
                        if (!topicsData.Posts.Any(s => s.Title == newName))
                        {
                            var post = await topicsData.Posts.FirstOrDefaultAsync(s => s.Id == partId);
                            var subchapter = await topicsData.SubChapters.FirstOrDefaultAsync(s => s.Id == parentId);
                            post.Title = newName;
                            post.Content = content;

                            if (subchapter != null)
                            {
                                post.Subchapter = subchapter;
                            }

                            post.CreatedDate = DateTime.UtcNow;
                            await topicsData.SaveChangesAsync();
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
        public async Task<JsonContent> RemovePartAsync_Db(int id, string table)
        {
            try
            {
                switch (table)
                {
                    case "onload":
                        if (topicsData.Sections.Any(s => s.Id == id))
                        {
                            var section = await topicsData.Sections.FirstOrDefaultAsync(s => s.Id == id);
                            topicsData.Sections.Remove(section);
                            await topicsData.SaveChangesAsync();
                            return JsonContent.Create(section);
                        }
                        return null;

                    case "section":
                        if (topicsData.SubSections.Any(s => s.Id == id))
                        {
                            var subsection = await topicsData.SubSections.FirstOrDefaultAsync(s => s.Id == id);
                            topicsData.SubSections.Remove(subsection);
                            await topicsData.SaveChangesAsync();
                            return JsonContent.Create(subsection);
                        }
                        return null;

                    case "subsection":
                        if (topicsData.Chapters.Any(s => s.Id == id))
                        {
                            var chapter = await topicsData.Chapters.FirstOrDefaultAsync(s => s.Id == id);
                            topicsData.Chapters.Remove(chapter);
                            await topicsData.SaveChangesAsync();
                            return JsonContent.Create(chapter);
                        }
                        return null;

                    case "chapter":
                        if (topicsData.SubChapters.Any(s => s.Id == id))
                        {
                            var subchapter = await topicsData.SubChapters.FirstOrDefaultAsync(s => s.Id == id);
                            topicsData.SubChapters.Remove(subchapter);
                            await topicsData.SaveChangesAsync();
                            return JsonContent.Create(subchapter);
                        }
                        return null;

                    case "subchapter":
                        if (topicsData.SubChapters.Any(s => s.Id == id))
                        {
                            var post = await topicsData.Posts.FirstOrDefaultAsync(s => s.Id == id);
                            topicsData.Posts.Remove(post);
                            await topicsData.SaveChangesAsync();
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
