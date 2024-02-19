using TwitterApi.Data;

namespace TwitterApi.Services
{
   public class BaseService
   {
      protected readonly IUnitOfWork _unitOfWork;

      public BaseService(IUnitOfWork unitOfWork)
      {
         _unitOfWork = unitOfWork;
      }
   }
}
