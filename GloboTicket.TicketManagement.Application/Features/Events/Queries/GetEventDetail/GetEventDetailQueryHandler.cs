using AutoMapper;
using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace GloboTicket.TicketManagement.Application.Features.Events.Queries.GetEventDetail
{
    class GetEventDetailQueryHandler : IRequestHandler<GetEventDetailQuery, EventDetailVm>
    {
        private readonly IMapper _mapper;
        private readonly IAsyncRepository<Event> _eventsRepository;
        private readonly IAsyncRepository<Category> _categoriesRepository;

        public GetEventDetailQueryHandler(IMapper mapper, IAsyncRepository<Event> eventsRepository, IAsyncRepository<Category> categoriesRepository)
        {
            _mapper = mapper;
            _eventsRepository = eventsRepository;
            _categoriesRepository = categoriesRepository;
        }
        public async Task<EventDetailVm> Handle(GetEventDetailQuery request, CancellationToken cancellationToken)
        {
            var @event = await _eventsRepository.GetByIdAsync(request.Id);
            var eventDetailDto = _mapper.Map<EventDetailVm>(@event);

            var category = await _categoriesRepository.GetByIdAsync(@event.CategoryId);
            eventDetailDto.Category = _mapper.Map<CategoryDto>(category);

            return eventDetailDto;
        }
    }
}
