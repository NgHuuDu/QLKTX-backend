using AutoMapper;
using DormitoryManagementSystem.BUS.Interfaces;
using DormitoryManagementSystem.DAO.Interfaces;
using DormitoryManagementSystem.DTO.News;
using DormitoryManagementSystem.Entity;

namespace DormitoryManagementSystem.BUS.Implementations
{
    public class NewsBUS : INewsBUS
    {
        private readonly INewsDAO _newsDAO;
        private readonly IUserDAO _userDAO;
        private readonly IMapper _mapper;

        public NewsBUS(INewsDAO newsDAO, IUserDAO userDAO, IMapper mapper)
        {
            _newsDAO = newsDAO;
            _userDAO = userDAO;
            _mapper = mapper;
        }

        public async Task<IEnumerable<NewsReadDTO>> GetAllNewsAsync()
        {
            IEnumerable<News> newsList = await _newsDAO.GetAllNewsAsync();
            return _mapper.Map<IEnumerable<NewsReadDTO>>(newsList);
        }

        public async Task<IEnumerable<NewsReadDTO>> GetAllNewsIncludingInactivesAsync()
        {
            IEnumerable<News> newsList = await _newsDAO.GetAllNewsIncludingInactivesAsync();
            return _mapper.Map<IEnumerable<NewsReadDTO>>(newsList);
        }

        public async Task<NewsReadDTO?> GetNewsByIDAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("News ID không thể để trống");

            News? news = await _newsDAO.GetNewsByIDAsync(id);
            if (news == null) return null;

            return _mapper.Map<NewsReadDTO>(news);
        }

        public async Task<string> AddNewsAsync(NewsCreateDTO dto)
        {
            var existingNews = await _newsDAO.GetNewsByIDAsync(dto.NewsID);
            if (existingNews != null)
                throw new InvalidOperationException($"News với ID {dto.NewsID} đã tồn tại.");

            if (!string.IsNullOrEmpty(dto.AuthorID))
            {
                User? author = await _userDAO.GetUserByIDAsync(dto.AuthorID);

                if (author == null)
                    throw new KeyNotFoundException($"Không có tác giả (User) với ID {dto.AuthorID}.");

                if (!author.IsActive)
                    throw new InvalidOperationException($"Không thể đăng tin tức vì tài khoản tác giả {dto.AuthorID} không hoạt động/đã bị xóa.");

                if (author.Role != "Admin")
                    throw new UnauthorizedAccessException("Chỉ có Admin mới được phép đăng tin tức.");
            }

            News newsEntity = _mapper.Map<News>(dto);

            // Set Published Date if not handled by DB default (Good practice to be explicit)
            if (newsEntity.Publisheddate == null)
                newsEntity.Publisheddate = DateTime.Now;

            await _newsDAO.AddNewsAsync(newsEntity);

            return newsEntity.Newsid;
        }

        public async Task UpdateNewsAsync(string id, NewsUpdateDTO dto)
        {
            News? newsEntity = await _newsDAO.GetNewsByIDAsync(id);
            if (newsEntity == null)
                throw new KeyNotFoundException($"Không có news với ID {id}.");

            _mapper.Map(dto, newsEntity);
            newsEntity.Newsid = id;

            await _newsDAO.UpdateNewsAsync(newsEntity);
        }

        public async Task DeleteNewsAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("News ID không thể để trống");

            var newsEntity = await _newsDAO.GetNewsByIDAsync(id);
            if (newsEntity == null)
                throw new KeyNotFoundException($"Không có news với ID {id}.");

            await _newsDAO.DeleteNewsAsync(id);
        }

        // Mới thêm - Lấy danh sách tóm tắt tin tức (chỉ gồm Newsid, Title, Publisheddate)
        public async Task<IEnumerable<NewsSummaryDTO>> GetNewsSummariesAsync()
        {
            var newsList = await _newsDAO.GetNewsSummariesAsync();
            var result = newsList.Select(n => new NewsSummaryDTO
            {
                NewsID = n.Newsid, 
                Title = n.Title,
                PublishedDate = n.Publisheddate ?? DateTime.MinValue
            });

            return result;
        }
    }
}