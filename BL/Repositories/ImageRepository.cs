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
    public interface IImageRepository
    {
        IEnumerable<BLImage> GetAll();
        BLImage GetById(int id);
        BLImage Add(BLImage image);
        BLImage Update(int id, BLImage image);
        void Delete(int id);
    }

    public class ImageRepository : IImageRepository
    {
        private readonly RwaMoviesContext _dbContext;
        private readonly IMapper _mapper;

        public ImageRepository(RwaMoviesContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IEnumerable<BLImage> GetAll()
        {
            var dbImages = _dbContext.Images;
            var blImages = _mapper.Map<IEnumerable<BLImage>>(dbImages);

            return blImages;
        }

        public BLImage GetById(int id)
        {
            var dbImage = _dbContext.Images.FirstOrDefault(s => s.Id == id);
            var blImage = _mapper.Map<BLImage>(dbImage);

            return blImage;
        }

        public BLImage Add(BLImage image)
        {
            var newDbImage = _mapper.Map<Image>(image);
            newDbImage.Id = 0;
            _dbContext.Images.Add(newDbImage);
            _dbContext.SaveChanges();

            var newBlImage = _mapper.Map<BLImage>(newDbImage);
            return newBlImage;
        }

        public BLImage Update(int id, BLImage image)
        {
            var dbImage = _dbContext.Images.FirstOrDefault(s => s.Id == id);
            if (dbImage == null)
            {
                throw new InvalidOperationException("Image not found");
            }

            dbImage.Content = image.Content;
        

            _dbContext.SaveChanges();

            var updatedBlImage = _mapper.Map<BLImage>(dbImage);
            return updatedBlImage;
        }

        public void Delete(int id)
        {
            var dbImage = _dbContext.Images.FirstOrDefault(s => s.Id == id);
            if (dbImage == null)
            {
                throw new InvalidOperationException("Image not found");
            }

            _dbContext.Images.Remove(dbImage);
            _dbContext.SaveChanges();
        }
    }

}
