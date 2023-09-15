using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFSport.Models.Entities;

namespace TFSport.Models.DTOModels.Articles
{
    public class SportsDTO
    {
        public SportType Name { get; set; }
        public string Description { get; set; }
    }
}
