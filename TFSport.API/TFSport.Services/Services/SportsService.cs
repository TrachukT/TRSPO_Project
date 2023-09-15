using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TFSport.Models.DTOModels.Articles;
using TFSport.Models.Entities;
using TFSport.Models.Exceptions;
using TFSport.Services.Interfaces;

namespace TFSport.Services.Services
{
    public class SportsService : ISportsService
    {
        private readonly IMemoryCache _memoryCache;

        public SportsService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public async Task<List<SportsDTO>> GetSportTypes()
        {
            try
            {
                var isCached = _memoryCache.TryGetValue(nameof(SportType), out List<SportsDTO> sportsList);
                if (!isCached)
                {
                    sportsList = new List<SportsDTO>();
                    var sportsValues = Enum.GetValues(typeof(SportType));
                    foreach (var value in sportsValues)
                    {
                        sportsList.Add(new SportsDTO { Name = (SportType)value, Description = GetDescription((SportType)value) });
                    }
                    _memoryCache.Set(nameof(SportType), sportsList, new MemoryCacheEntryOptions()
                        .SetSize(5)
                        .SetAbsoluteExpiration(TimeSpan.FromMinutes(1)));
                }
                return sportsList;
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }

        public string GetDescription(SportType GenericEnum)
        {
            Type genericEnumType = GenericEnum.GetType();
            MemberInfo[] memberInfo = genericEnumType.GetMember(GenericEnum.ToString());
            if ((memberInfo != null && memberInfo.Length > 0))
            {
                var _Attribs = memberInfo[0].GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
                if ((_Attribs != null && _Attribs.Count() > 0))
                {
                    return ((System.ComponentModel.DescriptionAttribute)_Attribs.ElementAt(0)).Description;
                }
            }
            return GenericEnum.ToString();
        }
    }
}
