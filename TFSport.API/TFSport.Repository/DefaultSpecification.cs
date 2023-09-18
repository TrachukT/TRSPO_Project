using Microsoft.Azure.CosmosRepository.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TFSport.Models.Entities;
using TFSport.Models;

namespace TFSport.Repository
{
    public class DefaultArticleSpecification : DefaultSpecification<Article>
    {
        public DefaultArticleSpecification(int pageNumber, int pageSize, string orderBy,
            Expression<Func<Article, bool>> predicate, HashSet<string> articleIds)
        {
            var query = this.Query;
            if (predicate != default)
            {
                this.Query.Where(predicate);
            }

            if (articleIds != default && articleIds.Any())
            {
                this.Query.Where(x => articleIds.Contains(x.Id));
            }

            switch (orderBy)
            {
                case OrderType.byCreatedDateAsc:
                    this.Query.OrderBy(x => x.CreatedTimeUtc);
                    break;
                case OrderType.topRated:
                    this.Query.OrderByDescending(x => x.LikeCount);
                    break;
                default:
                    this.Query.OrderByDescending(x => x.CreatedTimeUtc);
                    break;
            }

            this.Query.PageSize(pageSize)
                .PageNumber(pageNumber);
        }
    }
}
