using AutoMapper;
using MyMusic.Api.Models;
using MyMusic.Core;
using MyMusic.Core.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyMusic.Api.App_Start
{
    public class AutoMapperConfig
    {
        public static IMapper Mapper { get; private set; }

        public static void RegisterMappings()
        {
            
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Artist, ArtistDto>()
                    .ForMember(d => d.Name, cng => cng.MapFrom(s => s.Name))
                    .ForMember(d => d.Country, cng => cng.MapFrom(s => s.Country))
                    .ForMember(d => d.Uid, cng => cng.MapFrom(s => s.UniqueId.ToString()))
                    .ForMember(d => d.Alias, cng => cng.MapFrom(s => s.AliasList));

                cfg.CreateMap<ArtistSearchResult, ArtistSearchResultDto>()
                .ForMember(d => d.NumberOfPages, cng => cng.MapFrom(s => s.NumberOfPages))
                .ForMember(d => d.NumberOfSearchResults, cng => cng.MapFrom(s => s.NoOfSearchResults))
                .ForMember(d => d.Page, cng => cng.MapFrom(s => s.Page))
                .ForMember(d => d.PageSize, cng => cng.MapFrom(s => s.PageSize))
                .ForMember(d => d.Results, cng => cng.MapFrom(s => s.Artists));

                cfg.CreateMap<OtherArtist, OtherArtistDto>();
                cfg.CreateMap<ArtistReleaseInfo, ArtistReleaseInfoDto>();
                cfg.CreateMap<ArtistReleaseInfoDto, FavouriteReleaseInfoDto>();

                cfg.CreateMap<FavouriteReleaseInfoDto, FavouriteReleaseInfo>();
                cfg.CreateMap<FavouriteReleaseInfo, FavouriteReleaseInfoDto>();
            });


            Mapper = config.CreateMapper();


        }
    }
}