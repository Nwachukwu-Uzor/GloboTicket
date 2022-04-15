using AutoMapper;
using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace GloboTicket.TicketManagement.Application.Features.Events.Commands.UpdateEvent
{
    public class UpdateEventCommandHandler : IRequestHandler<UpdateEventCommand>
    {
        private readonly IMapper _mapper;
        private readonly IAsyncRepository<Event> _eventsRepository;

        public UpdateEventCommandHandler(IMapper mapper, IAsyncRepository<Event> eventsRepository)
        {
            _mapper = mapper;
            _eventsRepository = eventsRepository;
        }
        public async Task<Unit> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
        {
            var eventToUpdate = await _eventsRepository.GetByIdAsync(request.EventId);

            _mapper.Map(eventToUpdate, request, typeof(UpdateEventCommand), typeof(Event));

            await _eventsRepository.UpdateAsync(eventToUpdate);

            return Unit.Value;
        }
    }
}
