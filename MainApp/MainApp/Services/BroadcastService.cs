using Microsoft.EntityFrameworkCore;
using MainApp.Models;

namespace MainApp.Services
{
    public class BroadcastService
    {
        private UserContext userData;
        private TopicsContext topicsData;



        //Parts actions 
        //Give
        public async Task<JsonContent> GetPartAsync_Db(int id, string name, string table)
        {
            switch (table)
            {
                case "onload":
                    var sections = await topicsData.Sections.ToListAsync();
                    if (sections == null)
                    {
                        return null;
                    }
                    return JsonContent.Create(sections);

                case "section":
                    var section = await topicsData.Sections.FirstOrDefaultAsync(s => s.Id == id && s.Title == name);
                    if (section == null)
                    {
                        return null;
                    }
                    var subSections = await topicsData.SubSections.Where(s => s.Section == section).ToListAsync();

                    return JsonContent.Create(subSections);

                case "subsection":
                    var subSection = await topicsData.SubSections.FirstOrDefaultAsync(s => s.Id == id && s.Title == name);
                    if (subSection == null)
                    {
                        return null;
                    }
                    var chapters = await topicsData.Chapters.Where(s => s.Subsection == subSection).ToListAsync();

                    return JsonContent.Create(chapters);

                case "chapter":
                    var chapter = await topicsData.Chapters.FirstOrDefaultAsync(s => s.Id == id && s.Title == name);
                    if (chapter == null)
                    {
                        return null;
                    }
                    var subChapters = await topicsData.SubChapters.Where(s => s.Chapter == chapter).ToListAsync();

                    return JsonContent.Create(subChapters);

                case "post":
                    var subChapter = await topicsData.SubChapters.FirstOrDefaultAsync(s => s.Id == id && s.Title == name);
                    if (subChapter == null)
                    {
                        return null;
                    }
                    var posts = await topicsData.Posts.Where(s => s.Subchapter == subChapter).ToListAsync();

                    return JsonContent.Create(posts);
            }

            return null;
        }



        //Add
        public async Task<JsonContent> AddPartAsync_Db(string name, int parentPartId, string parentPartName, string content, string table)
        {
            switch (table)
            {
                case "section":
                    if (!await topicsData.Sections.AnyAsync(s => s.Title == name))
                    {
                        var section = new Section { Title = name, CreatedDate = DateTime.UtcNow };
                        await topicsData.Sections.AddAsync(section);
                        await topicsData.SaveChangesAsync();
                        return JsonContent.Create(section);
                    }
                    else
                    {
                        return null;
                    }

                case "subsection":
                    if (!await topicsData.SubSections.AnyAsync(s => s.Title == name))
                    {
                        var section = await topicsData.Sections.FirstOrDefaultAsync(s => s.Id == parentPartId && s.Title == parentPartName);
                        var subsection = new Subsection { Title = name, CreatedDate = DateTime.UtcNow, Section = section};
                        await topicsData.SubSections.AddAsync(subsection);
                        await topicsData.SaveChangesAsync();
                        return JsonContent.Create(subsection);
                    }
                    else
                    {
                        return null;
                    }

                case "chapter":
                    if (!await topicsData.Chapters.AnyAsync(s => s.Title == name))
                    {
                        var subsection = await topicsData.SubSections.FirstOrDefaultAsync(s => s.Id == parentPartId && s.Title == parentPartName);
                        var chapter = new Chapter { Title = name, CreatedDate = DateTime.UtcNow, Subsection = subsection };
                        await topicsData.Chapters.AddAsync(chapter);
                        await topicsData.SaveChangesAsync();
                        return JsonContent.Create(chapter);
                    }
                    else
                    {
                        return null;
                    }

                case "subchapter":
                    if (!await topicsData.SubChapters.AnyAsync(s => s.Title == name))
                    {
                        var chapter = await topicsData.Chapters.FirstOrDefaultAsync(s => s.Id == parentPartId && s.Title == parentPartName);
                        var subchapter = new Subchapter { Title = name, CreatedDate = DateTime.UtcNow, Chapter = chapter };
                        await topicsData.SubChapters.AddAsync(subchapter);
                        await topicsData.SaveChangesAsync();
                        return JsonContent.Create(subchapter);
                    }
                    else
                    {
                        return null;
                    }

                case "post":
                    if (!await topicsData.Posts.AnyAsync(s => s.Title == name))
                    {
                        var subchapter = await topicsData.SubChapters.FirstOrDefaultAsync(s => s.Id == parentPartId && s.Title == parentPartName);
                        var post = new Post { Title = name, Content = content, CreatedDate = DateTime.UtcNow, Subchapter = subchapter };
                        await topicsData.Posts.AddAsync(post);
                        await topicsData.SaveChangesAsync();
                        return JsonContent.Create(post);
                    }
                    else
                    {
                        return null;
                    }
            }
            return null;
        }



        //Update
        public async Task<JsonContent> UpdatePartAsync_Db(int id, string name, string newName, string newContent, string table)
        {
            switch (table)
            {
                case "section":
                    if (!await topicsData.Sections.AnyAsync(s => s.Id == id && s.Title == name))
                    {
                        var section = await topicsData.Sections.FirstOrDefaultAsync(s => s.Id == id && s.Title == name);
                        section.Title = newName;
                        section.CreatedDate = DateTime.UtcNow;
                        await topicsData.SaveChangesAsync();
                        return JsonContent.Create(section);
                    }
                    else
                    {
                        return null;
                    }

                case "subsection":
                    if (!await topicsData.SubSections.AnyAsync(s => s.Id == id && s.Title == name))
                    {
                        var subsection = await topicsData.SubSections.FirstOrDefaultAsync(s => s.Id == id && s.Title == name);
                        subsection.Title = newName;
                        subsection.CreatedDate = DateTime.UtcNow;
                        await topicsData.SaveChangesAsync();
                        return JsonContent.Create(subsection);
                    }
                    else
                    {
                        return null;
                    }

                case "chapter":
                    if (!await topicsData.Chapters.AnyAsync(s => s.Id == id && s.Title == name))
                    {
                        var chapter = await topicsData.Chapters.FirstOrDefaultAsync(s => s.Id == id && s.Title == name);
                        chapter.Title = newName;
                        chapter.CreatedDate = DateTime.UtcNow;
                        await topicsData.SaveChangesAsync();
                        return JsonContent.Create(chapter);
                    }
                    else
                    {
                        return null;
                    }

                case "subchapter":
                    if (!await topicsData.SubChapters.AnyAsync(s => s.Id == id && s.Title == name))
                    {
                        var subchapter = await topicsData.SubChapters.FirstOrDefaultAsync(s => s.Id == id && s.Title == name);
                        subchapter.Title = newName;
                        subchapter.CreatedDate = DateTime.UtcNow;
                        await topicsData.SaveChangesAsync();
                        return JsonContent.Create(subchapter);
                    }
                    else
                    {
                        return null;
                    }

                case "post":
                    if (!await topicsData.Posts.AnyAsync(s => s.Id == id && s.Title == name))
                    {
                        var post = await topicsData.Posts.FirstOrDefaultAsync(s => s.Id == id && s.Title == name);
                        post.Title = newName;
                        post.Content = newContent;
                        post.CreatedDate = DateTime.UtcNow;
                        await topicsData.SaveChangesAsync();
                        return JsonContent.Create(post);
                    }
                    else
                    {
                        return null;
                    }
            }
            return null;
        }



        //Delete
        public async Task<JsonContent> RemovePartAsync_Db(int id, string name, string table)
        {
            switch (table)
            {
                case "section":
                    if (!await topicsData.Sections.AnyAsync(s => s.Id == id && s.Title == name))
                    {
                        var section = await topicsData.Sections.FirstOrDefaultAsync(s => s.Id == id && s.Title == name);
                        topicsData.Sections.Remove(section);
                        await topicsData.SaveChangesAsync();
                        return JsonContent.Create(section);
                    }
                    else
                    {
                        return null;
                    }

                case "subsection":
                    if (!await topicsData.SubSections.AnyAsync(s => s.Id == id && s.Title == name))
                    {
                        var subsection = await topicsData.SubSections.FirstOrDefaultAsync(s => s.Id == id && s.Title == name);
                        topicsData.SubSections.Remove(subsection);
                        await topicsData.SaveChangesAsync();
                        return JsonContent.Create(subsection);
                    }
                    else
                    {
                        return null;
                    }

                case "chapter":
                    if (!await topicsData.Chapters.AnyAsync(s => s.Id == id && s.Title == name))
                    {
                        var chapter = await topicsData.Chapters.FirstOrDefaultAsync(s => s.Id == id && s.Title == name);
                        topicsData.Chapters.Remove(chapter);
                        await topicsData.SaveChangesAsync();
                        return JsonContent.Create(chapter);
                    }
                    else
                    {
                        return null;
                    }

                case "subchapter":
                    if (!await topicsData.SubChapters.AnyAsync(s => s.Id == id && s.Title == name))
                    {
                        var subchapter = await topicsData.SubChapters.FirstOrDefaultAsync(s => s.Id == id && s.Title == name);
                        topicsData.SubChapters.Remove(subchapter);
                        await topicsData.SaveChangesAsync();
                        return JsonContent.Create(subchapter);
                    }
                    else
                    {
                        return null;
                    }

                case "post":
                    if (!await topicsData.Posts.AnyAsync(s => s.Id == id && s.Title == name))
                    {
                        var post = await topicsData.Posts.FirstOrDefaultAsync(s => s.Id == id && s.Title == name);
                        topicsData.Posts.Remove(post);
                        await topicsData.SaveChangesAsync();
                        return JsonContent.Create(post);
                    }
                    else
                    {
                        return null;
                    }
            }
            return null;
        }





















        public List<User> GiveUsers()
        {
            try
            {
                List<User> users = userData.Users.Where(u => u.Role == "user").ToList();
                return users;
            }
            catch (DbUpdateException ex)
            {
                Console.Clear();
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
            return null;
        }

    }
}
