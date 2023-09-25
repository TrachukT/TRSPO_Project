using Microsoft.Azure.CosmosRepository;
using Microsoft.Azure.CosmosRepository.Extensions;
using System.Linq.Expressions;
using TFSport.Models.Entities;
using TFSport.Repository.Interfaces;

namespace TFSport.Repository.Repositories
{
    public class TagsRepository : ITagsRepository
    {
        private readonly IRepository<Tag> _repository;

        public TagsRepository(IRepository<Tag> repository)
        {
            _repository = repository;
        }

        public async Task<Tag> GetTagAsync(string tagName)
        {
            return await _repository.GetAsync(x => x.TagName == tagName).FirstOrDefaultAsync();
        }

        public async Task<HashSet<Tag>> GetTagsAsync(HashSet<string> tagNames)
        {
            var tags = await _repository.GetAsync(x => tagNames.Contains(x.TagName));
            return tags.ToHashSet();
        }

        public async Task<IEnumerable<Tag>> GetTagsPageAsync(Expression<Func<Tag, bool>> predicate, int pageNumber, int pageSize)
        {
            var tags = await _repository.PageAsync(predicate, pageNumber, pageSize);
            return tags.Items.ToList();
        }

        public async Task<HashSet<Tag>> GetTagsByArticleIdAsync(string articleId)
        {
            var tags = await _repository.GetAsync(tag => tag.ArticleIds.Contains(articleId));
            return tags.ToHashSet();
        }

        public async Task CreateTagAsync(Tag tag)
        {
            tag.PartitionKey = tag.Id;
            await _repository.CreateAsync(tag);
        }

        public async Task UpdateTagAsync(Tag tag)
        {
            await _repository.UpdateAsync(tag);
        }

        public async Task DeleteTagAsync(string tagId)
        {
            await _repository.DeleteAsync(tagId);
        }
    }
}
