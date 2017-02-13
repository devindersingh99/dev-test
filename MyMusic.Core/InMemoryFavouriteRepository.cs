using MyMusic.Core.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMusic.Core
{
    public class InMemoryFavouriteRepository : IFavouriteRepository
    {
        static Dictionary<string, List<FavouriteReleaseInfo>> favourites = new Dictionary<string, List<FavouriteReleaseInfo>>();
        static string _default = "Default List";
        static InMemoryFavouriteRepository()
        {
            if (!favourites.ContainsKey("Default List"))
                favourites.Add(_default, new List<FavouriteReleaseInfo>());

        }
        public void AddToFavourite(string name, FavouriteReleaseInfo releaseInfo)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException($"{nameof(name)} can't be empty or null");
            if (releaseInfo == null)
                throw new ArgumentNullException($"{nameof(releaseInfo)} can't be null");

            if (favourites.ContainsKey(name))
            {
                var fav = favourites[name];
                foreach (var item in fav)
                {
                    if (item.ReleaseId == releaseInfo.ReleaseId)
                        return;
                }
                fav.Add(releaseInfo);
            }
            else
            {
                throw new ArgumentException($"{name} does not exists in favourites");
            }
        }

        public void CreateFavouriteList(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException($"{nameof(name)} can't be empty or null");

            if (favourites.ContainsKey(name))
                throw new ArgumentException($"{nameof(name)} already exists");

            favourites.Add(name, new List<FavouriteReleaseInfo>());

        }

        public void DeleteFavouriteList(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException($"{nameof(name)} can't be empty or null");

            if (favourites.ContainsKey(name) && !name.Equals(_default))
                favourites.Remove(name);
        }

        public bool FavouriteExists(string name)
        {
            return favourites.ContainsKey(name);
        }

        public List<string> FavouriteNameFromReleaseInfoId(string releaseInfoId)
        {
            if (string.IsNullOrEmpty(releaseInfoId))
                throw new ArgumentNullException($"{nameof(releaseInfoId)} can't be empty or null");

            return FindFavourite(releaseInfoId);

        }

        private List<string> FindFavourite(string releaseInfoId)
        {
            List<string> names = new List<string>();
            foreach (var item in favourites)
            {
                foreach (var val in item.Value)
                {
                    if (val.ReleaseId == releaseInfoId)
                    {
                        if (!names.Contains(item.Key))
                            names.Add(item.Key);
                    }
                }
            }
            return names;
        }

        public List<FavouriteReleaseInfo> GetFavourite(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException($"{nameof(name)} can't be empty or null");
            if (favourites.ContainsKey(name))
                return favourites[name];

            throw new ArgumentException($"{nameof(name)} not found in favourites");
        }

        public Dictionary<string, List<FavouriteReleaseInfo>> GetFavourites()
        {
            return favourites;
        }

        public void RemoveFromFavourite(string name, string releaseInfoId)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException($"{nameof(name)} can't be empty or null");

            if (string.IsNullOrEmpty(releaseInfoId))
                throw new ArgumentNullException($"{nameof(releaseInfoId)} can't be empty or null");

            if (favourites.ContainsKey(name))
            {
                var lst = favourites[name];
                var item = lst.Find(info => info.ReleaseId == releaseInfoId);
                lst.Remove(item);
            }


        }

        public List<string> GetFavouriteListName()
        {
            return favourites.Keys.ToList();
        }
    }
}
