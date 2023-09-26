using AutoMapper;
using TFSport.Models.DTOs;
using TFSport.Models.Entities;
using TFSport.Models.Exceptions;
using TFSport.Repository.Interfaces;
using TFSport.Services.Interfaces;

namespace TFSport.Services.Services
{
    public class TagsService : ITagsService
    {
        private readonly ITagsRepository _tagsRepository;
        private readonly IMapper _mapper;

        public TagsService(ITagsRepository tagsRepository, IMapper mapper)
        {
            _tagsRepository = tagsRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TagDto>> GetTopTagsAsync(int pageNumber, int pageSize)
        {
            try
            {
                var tags = await _tagsRepository.GetTagsPageAsync(tag => tag.ArticleCount > 0, pageNumber, pageSize);
                var sortedTags = tags.OrderByDescending(tag => tag.ArticleCount);

                var tagDtos = sortedTags.Select(_mapper.Map<TagDto>);
                return tagDtos;
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }

        public async Task CreateOrUpdateTagsAsync(HashSet<string> tagNames, string articleId, ArticleStatus articleStatus)
        {
            try
            {
                var existingTags = await _tagsRepository.GetTagsAsync(tagNames.ToHashSet());

                foreach (var tagName in tagNames)
                {
                    var existingTag = existingTags.FirstOrDefault(t => t.TagName == tagName);

                    if (existingTag != null)
                    {
                        existingTag.ArticleIds.Add(articleId);

                        if (articleStatus == ArticleStatus.Published)
                        {
                            existingTag.ArticleCount++;
                        }

                        await _tagsRepository.UpdateTagAsync(existingTag);
                    }
                    else
                    {
                        var newTag = new Tag
                        {
                            TagName = tagName,
                            ArticleIds = new HashSet<string> { articleId },
                            ArticleCount = articleStatus == ArticleStatus.Published ? 1 : 0
                        };
                        await _tagsRepository.CreateTagAsync(newTag);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }

        public async Task RemoveArticleTagsAsync(HashSet<string> tagNames, string articleId, ArticleStatus articleStatus)
        {
            try
            {
                foreach (var tagName in tagNames)
                {
                    var existingTag = await _tagsRepository.GetTagAsync(tagName);

                    if (existingTag != null)
                    {
                        existingTag.ArticleIds.Remove(articleId);

                        if (existingTag.ArticleIds.Count == 0)
                        {
                            await _tagsRepository.DeleteTagAsync(existingTag.Id);
                        }
                        else
                        {
                            if (articleStatus == ArticleStatus.Published)
                            {
                                existingTag.ArticleCount--;
                            }

                            await _tagsRepository.UpdateTagAsync(existingTag);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }
    }
}
