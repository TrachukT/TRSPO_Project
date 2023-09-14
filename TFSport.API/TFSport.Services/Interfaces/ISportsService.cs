using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFSport.Models.DTOModels.Articles;
using TFSport.Models.Entities;

namespace TFSport.Services.Interfaces
{
    public interface ISportsService
    {
        public Task<List<GetSportsDTO>> GetSportTypes();

        public string GetDescription(SportType GenericEnum);
    }
}
