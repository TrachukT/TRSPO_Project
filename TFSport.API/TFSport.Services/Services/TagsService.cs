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

        public async Task<IEnumerable<TagDto>> GetTopTagsAsync()
        {
            try
            {
                var tags = await _tagsRepository.GetAllTagsAsync();
                var sortedTags = tags.OrderByDescending(tag => tag.Count);
                var tagDtos = sortedTags.Select(tag => _mapper.Map<TagDto>(tag));
                return tagDtos;
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }

        public async Task CreateOrUpdateTagsAsync(HashSet<string> tagNames, string articleId)
        {
            try
            {
                var existingTags = await _tagsRepository.GetTagsAsync(tagNames.ToList());

                foreach (var tagName in tagNames)
                {
                    var existingTag = existingTags.FirstOrDefault(t => t.TagName == tagName);

                    if (existingTag != null)
                    {
                        existingTag.ArticleIds.Add(articleId);
                        existingTag.Count++;
                        await _tagsRepository.UpdateTagAsync(existingTag);
                    }
                    else
                    {
                        var newTag = new Tag
                        {
                            TagName = tagName,
                            ArticleIds = new HashSet<string> { articleId },
                            Count = 1
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

        public async Task RemoveArticleTagsAsync(HashSet<string> tagNames, string articleId)
        {
            try
            {
                foreach (var tagName in tagNames)
                {
                    var existingTag = await _tagsRepository.GetTagAsync(tagName);

                    if (existingTag != null)
                    {
                        existingTag.ArticleIds.Remove(articleId);
                        existingTag.Count--;

                        if (existingTag.ArticleIds.Count == 0 && existingTag.Count == 0)
                        {
                            await _tagsRepository.DeleteTagAsync(existingTag.Id);
                        }
                        else
                        {
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
