using DTO;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL
{
    public class NewsRepository : INewsRepository
    {
        private readonly DBContext _context = null;

        public NewsRepository(MongoConfig config)
        {
            _context = new DBContext(config);
        }

        public async Task<IEnumerable<NewsDTO>> GetAllNewsAsync()
        {
            var documents = await _context.News.Find(_ => true).ToListAsync();
            return documents.Select(MapToNewDTO);
        }

        public async Task<NewsDTO> AddNews(NewsDTO item)
        {
            if (!(await _context.News.Find(q => q.Title.Equals(item.Title)).ToListAsync()).Any())
                await _context.News.InsertOneAsync(MapToNewEntity(item));

            return item;
        }

        public async Task<NewsDTO> GetNewsByIdAsync(string id)
        {
            var document = await _context.News.Find(_ => _.ID == id).SingleOrDefaultAsync();
            return MapToNewDTO(document);
        }

        private NewsEntity MapToNewEntity(NewsDTO news)
        {
            return new NewsEntity
            {
                ID = news.ID,
                Author = news.Author,
                DateOfPublication = news.DateOfPublication,
                Description = news.Description,
                Title = news.Title,
                Url = news.Url
            };
        }
        private NewsDTO MapToNewDTO(NewsEntity entity)
        {
            return new NewsDTO
            {
                ID = entity.ID,
                Author = entity.Author,
                DateOfPublication = entity.DateOfPublication,
                Description = entity.Description,
                Title = entity.Title,
                Url = entity.Url
            };
        }
    }

}
