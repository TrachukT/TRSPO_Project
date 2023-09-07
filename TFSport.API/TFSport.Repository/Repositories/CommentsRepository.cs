using Microsoft.Azure.CosmosRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFSport.Models.Entities;
using TFSport.Repository.Interfaces;

namespace TFSport.Repository.Repositories
{
    public class CommentsRepository : ICommentsRepository
    {
        private readonly IRepository<Comment> _repository;

        public CommentsRepository(IRepository<Comment> repository)
        {
            _repository = repository;
        }

    }
}
