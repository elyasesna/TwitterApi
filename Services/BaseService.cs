using AutoMapper;
using TwitterApi.Data;

namespace TwitterApi.Services
{
   public class BaseService
   {
      protected readonly IUnitOfWork _unitOfWork;
      protected readonly IMapper _mapper;

      public BaseService(IUnitOfWork unitOfWork, IMapper mapper)
      {
         _unitOfWork = unitOfWork;
         _mapper = mapper;
      }
   }
}
