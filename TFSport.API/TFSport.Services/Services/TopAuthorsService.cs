using AutoMapper;
using TFSport.Models.DTOModels.Users;
using TFSport.Models.Exceptions;
using TFSport.Repository.Interfaces;
using TFSport.Services.Interfaces;

namespace TFSport.Services.Services
{
    public class TopAuthorsService : ITopAuthorsService
    {
        private readonly IAuthorStatisticsRepository _authorStatisticsRepository;
        private readonly IUsersRepository _usersRepository;
        private readonly IMapper _mapper;

        public TopAuthorsService(IAuthorStatisticsRepository authorStatisticsRepository, IMapper mapper, IUsersRepository usersRepository)
        {
            _authorStatisticsRepository = authorStatisticsRepository;
            _mapper = mapper;
            _usersRepository = usersRepository;
        }

        public async Task<IEnumerable<AuthorDTO>> GetTopAuthorsAsync(int pageNumber, int pageSize)
        {
            try
            {
                var authors = await _authorStatisticsRepository.GetAuthorsPageAsync(author => author.ArticleCount > 0, pageNumber, pageSize);
                var sortedAuthors = authors.OrderByDescending(author => author.ArticleCount);

                var authorDtos = await Task.WhenAll(sortedAuthors.Select(async authorStatistics =>
                {
                    var user = await _usersRepository.GetUserById(authorStatistics.AuthorId);

                    var authorDto = new AuthorDTO
                    {
                        AuthorId = authorStatistics.AuthorId,
                        FirstName = user.FirstName,
                        LastName = user.LastName
                    };

                    return authorDto;
                }));

                return authorDtos;
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }
    }
}
