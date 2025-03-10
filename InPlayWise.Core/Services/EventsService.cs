//using AutoMapper;
//using Chat.Data.IUOW;
//using InPlayWiseCommon.Wrappers;
//using InPlayWiseCore.IServices;
//using InPlayWiseData.Entities;
//using Microsoft.AspNetCore.Http;

//namespace InPlayWiseCore.Services
//{
//    public class EventsService : IEventsService
//    {
//        private readonly IMapper _mapper;
//        private readonly IUnitOfWork _unitOfWork;
//        public EventsService(IMapper mapper, IUnitOfWork unitOfWork)
//        {
//            _mapper = mapper;
//            _unitOfWork = unitOfWork;
//        }
//        public void Create(EventDTO entity)
//        {
//            var newEvent = _mapper.Map<EventDTO, EventModel>(entity);
//            newEvent.CreatedDate = DateTime.Now;
//            _unitOfWork.Events.Create(newEvent);
//            _unitOfWork.Save();
//        }

//        public async Task<Result<List<EventDTO>>> FindAll()
//        {
//            var result = _unitOfWork.Events.FindAll();
//            if (result is not null)
//            {
//                return new Result<List<EventDTO>>
//                {
//                    Items = null, // need to convert type cast
//                    IsSuccess = true,
//                    StatusCode = StatusCodes.Status200OK,
//                    Message = "List of Events"
//                };
//            }
//            else
//            {
//                return new Result<List<EventDTO>>
//                {
//                    Items = null,
//                    IsSuccess = true,
//                    StatusCode = StatusCodes.Status200OK,
//                    Message = "List of Events"
//                };
//            }
//        }

//        Task<Result<EventDTO>> IEventsService.Delete(string entity)
//        {
//            throw new NotImplementedException();
//        }

//        public async Task<Result<EventDTO>> FindByCondition(string id)
//        {
//            var data = _unitOfWork.Events.FindByCondition(id);
//            var result = _mapper.Map<EventDTO>(data);

//            return new Result<EventDTO>
//            {
//                Items = result,
//                IsSuccess = true,
//                StatusCode = StatusCodes.Status200OK,
//                Message = "event find "
//            };
//        }

//        public async Task<Result<EventDTO>> Update(EventDTO newentity, string id)
//        {
//            var oldEntity = await FindByCondition(id);
//            if (oldEntity is not null)
//            {
//                var newEvent = _mapper.Map<EventDTO, EventModel>(newentity);
//                newEvent.ModifiedDate = DateTime.Now;
//                _unitOfWork.Events.Update(newEvent);
//                _unitOfWork.Save();
//                return new Result<EventDTO>
//                {
//                    Items = _mapper.Map<EventModel, EventDTO>(newEvent),
//                    IsSuccess = true,
//                    StatusCode = StatusCodes.Status200OK,
//                    Message = "Event Update"
//                };
//            }
//            else
//            {
//                return new Result<EventDTO>
//                {
//                    Items = null,
//                    IsSuccess = true,
//                    StatusCode = StatusCodes.Status200OK,
//                    Message = "No event Exits"
//                };
//            }
//        }
//    }
//}