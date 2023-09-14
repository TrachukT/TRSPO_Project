using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFSport.Models.Entities
{
    public class Favorites:BaseModel
    {
        [JsonProperty("userId")]
        public string UserId { get; set; }
        [JsonProperty("favoriteArticles")]
        public HashSet<string> FavoriteArticles { get; set; }
    }
}
