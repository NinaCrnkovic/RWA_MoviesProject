using AutoMapper;
using BL.BLModels;
using BL.DALModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Repositories
{
    public interface IVideoRepository
    {
        IEnumerable<BLVideo> GetAll();
        //IEnumerable<BLVideo> GetFiltered(string searchName, string sortBy);
        BLVideo GetById(int id);
        BLVideo Add(BLVideo video);
        BLVideo Update(int id, BLVideo video);
        void Delete(int id);

        IEnumerable<BLVideo> GetFilteredData(string term);
        IEnumerable<BLVideo> GetFilteredDataNameAndGenre(string name, string genre);
        IEnumerable<BLVideo> GetPagedData(int page, int size, string orderBy, string direction);
        IEnumerable<BLVideo> GetPagedDataAdmin(int page, int size, string orderBy, string direction, IEnumerable<BLVideo> filteredVideos);
        int GetTotalCount();
    }

    public class VideoRepository : IVideoRepository
    {
        private readonly RwaMoviesContext _dbContext;
        private readonly IMapper _mapper;

        public VideoRepository(RwaMoviesContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IEnumerable<BLVideo> GetAll()
        {
            var dbVideos = _dbContext.Videos;
            var blVideos = _mapper.Map<IEnumerable<BLVideo>>(dbVideos);

            return blVideos;
        }



        public int GetTotalCount() => _dbContext.Videos.Count();

        public IEnumerable<BLVideo> GetFilteredData(string term)
        {
            // It seems more flexible to check both name or description for the search term
            var dbVideos = _dbContext.Videos.Where(x =>
                x.Name.Contains(term) );

            var blVideos = _mapper.Map<IEnumerable<BLVideo>>(dbVideos);

            return blVideos;
        }

        public IEnumerable<BLVideo> GetFilteredDataNameAndGenre(string name, string genre)
        {
            // Filtriraj video zapise prema imenu, opisu i žanrovima
            var dbVideos = _dbContext.Videos.AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                dbVideos = dbVideos.Where(x => x.Name.Contains(name) || x.Description.Contains(name));
            }

            if (!string.IsNullOrEmpty(genre))
            {
                var genreIds = _dbContext.Genres
                    .Where(x => x.Name.Contains(genre))
                    .Select(x => x.Id);

                dbVideos = dbVideos.Where(x => genreIds.Contains(x.GenreId));
            }

            var blVideos = _mapper.Map<IEnumerable<BLVideo>>(dbVideos);

            return blVideos;
        }




        public IEnumerable<BLVideo> GetPagedData(int page, int size, string orderBy, string direction)
        {
            // All of this should go to repository
            IEnumerable<Video> dbVideo = _dbContext.Videos.AsEnumerable();

      

            // Ordering
            if (string.Compare(orderBy, "id", true) == 0)
            {
                dbVideo = dbVideo.OrderBy(x => x.Id);
            }
            else if (string.Compare(orderBy, "name", true) == 0)
            {
                dbVideo = dbVideo.OrderBy(x => x.Name);
            }
            else if (string.Compare(orderBy, "description", true) == 0)
            {
                dbVideo = dbVideo.OrderBy(x => x.Description);
            }
            else
            {
                // default: order by Id
                dbVideo = dbVideo.OrderBy(x => x.Id);
            }

            // For descending order we just reverse it
            if (string.Compare(direction, "desc", true) == 0)
            {
                dbVideo = dbVideo.Reverse();
            }

            // Now we can page the correctly ordered items
            dbVideo = dbVideo.Skip(page * size).Take(size);

            var blVideo = _mapper.Map<IEnumerable<BLVideo>>(dbVideo);

            return blVideo;
        }

        public IEnumerable<BLVideo> GetPagedDataAdmin(int page, int size, string orderBy, string direction, IEnumerable<BLVideo> filteredVideos)
        {
            // Apply ordering
            if (string.Compare(orderBy, "id", true) == 0)
            {
                filteredVideos = filteredVideos.OrderBy(x => x.Id);
            }
            else if (string.Compare(orderBy, "name", true) == 0)
            {
                filteredVideos = filteredVideos.OrderBy(x => x.Name);
            }
            else if (string.Compare(orderBy, "description", true) == 0)
            {
                filteredVideos = filteredVideos.OrderBy(x => x.Description);
            }
            else
            {
                // Default: order by Id
                filteredVideos = filteredVideos.OrderBy(x => x.Id);
            }

            // Apply descending order
            if (string.Compare(direction, "desc", true) == 0)
            {
                filteredVideos = filteredVideos.Reverse();
            }

            // Apply pagination
            filteredVideos = filteredVideos.Skip(page * size).Take(size);

            var blVideo = _mapper.Map<IEnumerable<BLVideo>>(filteredVideos);

            return blVideo;
        }



        public BLVideo GetById(int id)
        {
            var dbVideo = _dbContext.Videos.FirstOrDefault(s => s.Id == id);
            var blVideo = _mapper.Map<BLVideo>(dbVideo);

            return blVideo;
        }

        public BLVideo Add(BLVideo video)
        {
            var newDbVideo = _mapper.Map<Video>(video);
            newDbVideo.Id = 0;
            _dbContext.Videos.Add(newDbVideo);
            _dbContext.SaveChanges();

            var newBlVideo = _mapper.Map<BLVideo>(newDbVideo);
            return newBlVideo;
        }

        public BLVideo Update(int id, BLVideo video)
        {
            var dbVideo = _dbContext.Videos.FirstOrDefault(s => s.Id == id);
            if (dbVideo == null)
            {
                throw new InvalidOperationException("Video not found");
            }
       
            dbVideo.Name = video.Name;
            dbVideo.Description = video.Description;
            dbVideo.StreamingUrl = video.StreamingUrl;
            dbVideo.GenreId = video.GenreId;
            dbVideo.ImageId = video.ImageId;
            dbVideo.TotalSeconds = video.TotalSeconds;

            _dbContext.SaveChanges();

            var updatedBlVideo = _mapper.Map<BLVideo>(dbVideo);
            return updatedBlVideo;
        }

        public void Delete(int id)
        {
            var dbVideo = _dbContext.Videos.FirstOrDefault(s => s.Id == id);
            if (dbVideo == null)
            {
                throw new InvalidOperationException("Video not found");
            }

            _dbContext.Videos.Remove(dbVideo);
            _dbContext.SaveChanges();
        }
    }

}
