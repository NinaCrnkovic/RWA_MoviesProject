//using BL.DALModels;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;

//namespace IntegrationModule.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class NotificationController : ControllerBase
//    {
//        private readonly RwaMoviesContext _dbContext;

//        public NotificationController(RwaMoviesContext dbContext)
//        {
//            _dbContext = dbContext;
//        }



//        [HttpGet("[action]")]
//        public bool TestConnection()
//        {
//            return _dbContext.Database.CanConnect();
//        }

//        [HttpGet("[action]")]
//        public ActionResult<IEnumerable<Notification>> GetAll()
//        {
//            try
//            {
//                return _dbContext.Notifications;
//            }
//            catch (Exception)
//            {
//                return StatusCode(
//                    StatusCodes.Status500InternalServerError,
//                    "There has been a problem while fetching the data you requested");
//            }
//        }
//    }
//}
