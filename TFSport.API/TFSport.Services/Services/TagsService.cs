using TFSport.Models.Entities;
using TFSport.Models.Exceptions;
using TFSport.Repository.Interfaces;
using TFSport.Services.Interfaces;

namespace TFSport.Services.Services
{
    public class TagsService : ITagsService
    {
        private readonly ITagsRepository _tagsRepository;

        public TagsService(ITagsRepository tagsRepository)
        {
            _tagsRepository = tagsRepository;
        }

        public async Task CreateNewTagsAsync(List<string> tagNames, string articleId)
        {
            try
            {
                foreach (var tagName in tagNames)
                {
                    var existingTag = await _tagsRepository.GetTagAsync(tagName);

                    if (existingTag == null)
                    {
                        var newTag = new Tag
                        {
                            Tags = tagName,
                            ArticleId = new List<string> { articleId },
                            Count = 1
                        };
                        newTag.PartitionKey = newTag.Id;
                        await _tagsRepository.CreateTagAsync(newTag);
                    }
                    else
                    {
                        if (!existingTag.ArticleId.Contains(articleId))
                        {
                            existingTag.ArticleId.Add(articleId);
                            existingTag.Count++;
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

        public async Task UpdateExistingTagsAsync(List<string> tagNames, string articleId)
        {
            try
            {
                var existingTags = await _tagsRepository.GetTagsAsync(tagNames);

                var tagsToRemove = existingTags.Where(t => !tagNames.Contains(t.Tags)).ToList();
                foreach (var tagToRemove in tagsToRemove)
                {
                    tagToRemove.ArticleId.Remove(articleId);
                    tagToRemove.Count--;

                    if (tagToRemove.ArticleId.Count == 0)
                    {
                        if (tagToRemove.Count == 0)
                        {
                            await _tagsRepository.DeleteTagAsync(tagToRemove.Id);
                        }
                        else
                        {
                            await _tagsRepository.UpdateTagAsync(tagToRemove);
                        }
                    }
                    else
                    {
                        await _tagsRepository.UpdateTagAsync(tagToRemove);
                    }
                }

                foreach (var tagName in tagNames)
                {
                    var existingTag = existingTags.FirstOrDefault(t => t.Tags == tagName);

                    if (existingTag != null)
                    {
                        if (!existingTag.ArticleId.Contains(articleId))
                        {
                            existingTag.ArticleId.Add(articleId);
                            existingTag.Count++;
                            await _tagsRepository.UpdateTagAsync(existingTag);
                        }
                    }
                    else
                    {
                        var newTag = new Tag
                        {
                            Tags = tagName,
                            ArticleId = new List<string> { articleId },
                            Count = 1
                        };
                        newTag.PartitionKey = newTag.Id;
                        await _tagsRepository.CreateTagAsync(newTag);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }

        public async Task RemoveArticleTagsAsync(List<string> tagNames, string articleId)
        {
            try
            {
                foreach (var tagName in tagNames)
                {
                    var existingTag = await _tagsRepository.GetTagAsync(tagName);

                    if (existingTag != null && existingTag.ArticleId.Contains(articleId))
                    {
                        existingTag.ArticleId.Remove(articleId);
                        existingTag.Count--;

                        if (existingTag.ArticleId.Count == 0 && existingTag.Count == 0)
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
