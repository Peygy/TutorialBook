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



        public async Task<List<GeneralPart>> GetPartsAsync(int parentId, string table)
        {
            try
            {
                switch (table)
                {
                    // For Display parts
                    case "onload":
                        var sections = await data.Sections.ToListAsync();
                        return sections.Cast<GeneralPart>().ToList();

                    case "section":
                        var subsections = await data.Subsections.Where(s => s.ParentId == parentId).ToListAsync();
                        return subsections.Cast<GeneralPart>().ToList();

                    case "subsection":
                        var chapters = await data.Chapters.Where(s => s.ParentId == parentId).ToListAsync();
                        return chapters.Cast<GeneralPart>().ToList();

                    case "chapter":
                        var subchapters = await data.Subchapters.Where(s => s.ParentId == parentId).ToListAsync();
                        return subchapters.Cast<GeneralPart>().ToList();

                    case "subchapter":
                        var posts = await data.Posts.Where(p => p.ParentId == parentId).ToListAsync();
                        return posts.Cast<GeneralPart>().ToList();
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }

            return null;
        }

        public async Task<List<GeneralPart>> GetPartsParentsAsync(int partId, string table)
        {
            try
            {
                switch (table)
                {
                    // For Choosing new Parent
                    case "subsection":
                        var subsection = await data.Subsections.FirstOrDefaultAsync(s => s.Id == partId);
                        var sections = await data.Sections.Where(p => p.Id != subsection.ParentId).ToListAsync();
                        return sections.Cast<GeneralPart>().ToList();

                    case "chapter":
                        var chapter = await data.Chapters.FirstOrDefaultAsync(s => s.Id == partId);
                        var subsections = await data.Subsections.Where(p => p.Id != chapter.ParentId).ToListAsync();
                        return subsections.Cast<GeneralPart>().ToList();

                    case "subchapter":
                        var subchapter = await data.Subchapters.FirstOrDefaultAsync(s => s.Id == partId);
                        var chapters = await data.Chapters.Where(p => p.Id != subchapter.ParentId).ToListAsync();
                        return chapters.Cast<GeneralPart>().ToList();

                    case "post":
                        var post = await data.Posts.FirstOrDefaultAsync(s => s.Id == partId);
                        var subchapters = await data.Subsections.Where(p => p.Id != post.ParentId).ToListAsync();
                        return subchapters.Cast<GeneralPart>().ToList();
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }

            return null;
        }

        public async Task<GeneralPart> GetPartAsync(int partId, string table)
        {
            try
            {
                switch (table)
                {
                    case "section":
                        if (data.Sections.Any(s => s.Id == partId))
                        {
                            return await data.Sections.FirstOrDefaultAsync(s => s.Id == partId);
                        }
                        break;

                    case "subsection":
                        if (data.Subsections.Any(s => s.Id == partId))
                        { 
                            return await data.Subsections.FirstOrDefaultAsync(s => s.Id == partId);
                        }
                        break;

                    case "chapter":
                        if (data.Chapters.Any(s => s.Id == partId))
                        {
                            return await data.Chapters.FirstOrDefaultAsync(s => s.Id == partId);
                        }
                        break;

                    case "subchapter":
                        if (data.Subchapters.Any(s => s.Id == partId))
                        {
                            return await data.Subchapters.FirstOrDefaultAsync(s => s.Id == partId);
                        }
                        break;

                    case "post":
                        if (data.Posts.Any(s => s.Id == partId))
                        {
                            return await data.Posts.FirstOrDefaultAsync(s => s.Id == partId);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }

            return null;
        }



        //Add
        public async Task<bool> AddPartAsync(int parentId, string addName, string content, string table)
        {
            try
            {
                switch (table)
                {
                    case "onload":
                        if (!data.Sections.Any(s => s.Title == addName))
                        {
                            var section = new Section { Title = addName, CreatedDate = DateTime.UtcNow, Table = "section" };
                            await data.Sections.AddAsync(section);
                            await data.SaveChangesAsync();
                            return true;
                        }
                        break;

                    case "section":
                        if (!data.Subsections.Any(s => s.Title == addName))
                        {
                            var subsection = new Subsection { Title = addName, CreatedDate = DateTime.UtcNow, ParentId = parentId, Table = "subsection" };
                            await data.Subsections.AddAsync(subsection);
                            await data.SaveChangesAsync();
                            return true;
                        }
                        break;

                    case "subsection":
                        if (!data.Chapters.Any(s => s.Title == addName))
                        {
                            var chapter = new Chapter { Title = addName, CreatedDate = DateTime.UtcNow, ParentId = parentId, Table = "chapter" };
                            await data.Chapters.AddAsync(chapter);
                            await data.SaveChangesAsync();
                            return true;
                        }
                        break;

                    case "chapter":
                        if (!data.Subchapters.Any(s => s.Title == addName))
                        {
                            var subchapter = new Subchapter { Title = addName, CreatedDate = DateTime.UtcNow, ParentId = parentId, Table = "subchapter" };
                            await data.Subchapters.AddAsync(subchapter);
                            await data.SaveChangesAsync();
                            return true;
                        }
                        break;

                    case "subchapter":
                        if (!data.Posts.Any(s => s.Title == addName))
                        {
                            var post = new Post { Title = addName, Content = content, CreatedDate = DateTime.UtcNow, ParentId = parentId, Table = "post" };
                            await data.Posts.AddAsync(post);
                            await data.SaveChangesAsync();
                            return true;
                        }
                        break;
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
                            data.Sections.Update(section);
                            await data.SaveChangesAsync();
                            return true;
                        }
                        break;

                    case "subsection":
                        if (!data.Subsections.Any(s => s.Title == newName))
                        {
                            var subsection = await data.Subsections.FirstOrDefaultAsync(s => s.Id == partId);
                            subsection.Title = newName;

                            if (data.Sections.Any(s => s.Id == parentId))
                            {
                                subsection.ParentId = parentId;
                            }

                            subsection.CreatedDate = DateTime.UtcNow;
                            data.Subsections.Update(subsection);
                            await data.SaveChangesAsync();
                            return true;
                        }
                        break;

                    case "chapter":
                        if (!data.Chapters.Any(s => s.Title == newName))
                        {
                            var chapter = await data.Chapters.FirstOrDefaultAsync(s => s.Id == partId);
                            chapter.Title = newName;

                            if (data.Subsections.Any(s => s.Id == parentId))
                            {
                                chapter.ParentId = parentId;
                            }

                            chapter.CreatedDate = DateTime.UtcNow;
                            data.Chapters.Update(chapter);
                            await data.SaveChangesAsync();
                            return true;
                        }
                        break;

                    case "subchapter":
                        if (!data.Subchapters.Any(s => s.Title == newName))
                        {
                            var subchapter = await data.Subchapters.FirstOrDefaultAsync(s => s.Id == partId);
                            subchapter.Title = newName;

                            if (data.Chapters.Any(s => s.Id == parentId))
                            {
                                subchapter.ParentId = parentId;
                            }

                            subchapter.CreatedDate = DateTime.UtcNow;
                            data.Subchapters.Update(subchapter);
                            await data.SaveChangesAsync();
                            return true;
                        }
                        break;

                    case "post":
                        if (!data.Posts.Any(s => s.Title == newName))
                        {
                            var post = await data.Posts.FirstOrDefaultAsync(s => s.Id == partId);
                            post.Title = newName;
                            post.Content = content;

                            if (data.Subchapters.Any(s => s.Id == parentId))
                            {
                                post.ParentId = parentId;
                            }

                            post.CreatedDate = DateTime.UtcNow;
                            data.Posts.Update(post);
                            await data.SaveChangesAsync();
                            return true;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }

            return false;
        }



        //Delete part or post
        public async Task<GeneralPart> RemovePartAsync(int partId, string table)
        {
            try
            {
                switch (table)
                {
                    case "onload":
                        if (data.Sections.Any(s => s.Id == partId))
                        {
                            var section = await data.Sections.FirstOrDefaultAsync(s => s.Id == partId);
                            foreach(var subsection in await data.Subsections.Where(s => s.ParentId == partId).ToListAsync())
                            {
                                subsection.ParentId = null;
                                await data.SaveChangesAsync();
                            }

                            data.Sections.Remove(section);
                            await data.SaveChangesAsync();
                            return section;
                        }
                        break;

                    case "section":
                        if (data.Subsections.Any(s => s.Id == partId))
                        {
                            var subsection = await data.Subsections.FirstOrDefaultAsync(s => s.Id == partId);
                            foreach (var chapter in await data.Chapters.Where(s => s.ParentId == partId).ToListAsync())
                            {
                                chapter.ParentId = null;
                                await data.SaveChangesAsync();
                            }

                            data.Subsections.Remove(subsection);
                            await data.SaveChangesAsync();
                            return subsection;
                        }
                        break;

                    case "subsection":
                        if (data.Chapters.Any(s => s.Id == partId))
                        {
                            var chapter = await data.Chapters.FirstOrDefaultAsync(s => s.Id == partId);
                            foreach (var subchapter in await data.Subchapters.Where(s => s.ParentId == partId).ToListAsync())
                            {
                                subchapter.ParentId = null;
                                await data.SaveChangesAsync();
                            }

                            data.Chapters.Remove(chapter);
                            await data.SaveChangesAsync();
                            return chapter;
                        }
                        break;

                    case "chapter":
                        if (data.Subchapters.Any(s => s.Id == partId))
                        {
                            var subchapter = await data.Subchapters.FirstOrDefaultAsync(s => s.Id == partId);
                            foreach (var post in await data.Posts.Where(s => s.ParentId == partId).ToListAsync())
                            {
                                post.ParentId = null;
                                await data.SaveChangesAsync();
                            }

                            data.Subchapters.Remove(subchapter);
                            await data.SaveChangesAsync();
                            return subchapter;
                        }
                        break;

                    case "subchapter":
                        if (data.Posts.Any(s => s.Id == partId))
                        {
                            var post = await data.Posts.FirstOrDefaultAsync(s => s.Id == partId);
                            data.Posts.Remove(post);
                            await data.SaveChangesAsync();
                            return post;
                        }
                        break;
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
