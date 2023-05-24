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

        //public IEnumerable<BLVideo> GetFiltered(string searchName, string sortBy)
        //{
        //    var query = _dbContext.Videos.AsQueryable();

        //    if (!string.IsNullOrEmpty(searchName))
        //    {
        //        query = query.Where(v => v.Name.Contains(searchName));
        //    }

        //    //switch (sortBy)
        //    //{
        //    //    case "id":
        //    //        query = query.OrderBy(v => v.Id);
        //    //        break;
        //    //    case "name":
        //    //        query = query.OrderBy(v => v.Name);
        //    //        break;
        //    //    case "totalTime":
        //    //        query = query.OrderBy(v => v.TotalSeconds);
        //    //        break;
        //    //    default:
        //    //        query = query.OrderBy(v => v.Id);
        //    //        break;
        //    //}

        //    var dbVideos = query.ToList();
        //    var blVideos = _mapper.Map<IEnumerable<BLVideo>>(dbVideos);

        //    return blVideos;
        //}

        public BLVideo GetById(int id)
        {
            var dbVideo = _dbContext.Videos.Find(id);
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
            var dbVideo = _dbContext.Videos.Find(id);
            if (dbVideo == null)
            {
                throw new InvalidOperationException("Video not found");
            }

            _mapper.Map(video, dbVideo);
            _dbContext.SaveChanges();

            var updatedBlVideo = _mapper.Map<BLVideo>(dbVideo);
            return updatedBlVideo;
        }

        public void Delete(int id)
        {
            var dbVideo = _dbContext.Videos.Find(id);
            if (dbVideo == null)
            {
                throw new InvalidOperationException("Video not found");
            }

            _dbContext.Videos.Remove(dbVideo);
            _dbContext.SaveChanges();
        }
    }

}
