using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMusic.Core.Repo
{
    public interface IFavouriteRepository
    {
        void CreateFavouriteList(string name);
        void AddToFavourite(string name, FavouriteReleaseInfo releaseInfo);
        void RemoveFromFavourite(string name, string releaseInfoId);
        void DeleteFavouriteList(string name);
        Dictionary<string, List<FavouriteReleaseInfo>> GetFavourites();
        List<FavouriteReleaseInfo> GetFavourite(string name);
        bool FavouriteExists(string name);
        List<string> FavouriteNameFromReleaseInfoId(string releaseInfoId);
        List<string> GetFavouriteListName();
    }
}
