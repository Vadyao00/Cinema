using AutoMapper;
using Cinema.Application.Commands.EventsCommands;
using Cinema.Domain.Entities;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using MediatR;

namespace Cinema.Application.Handlers.EventsHandlers
{
    internal sealed class UpdateEventHandler : IRequestHandler<UpdateEventCommand, ApiBaseResponse>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public UpdateEventHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ApiBaseResponse> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
        {
            var eevent = await _repository.Event.GetEventAsync(request.Id, request.TrackChanges);
            if (eevent is null)
                return new EventNotFoundResponse(request.Id);

            _mapper.Map(request.EventForUpdateDto, eevent);
            await _repository.SaveAsync();

            return new ApiOkResponse<Event>(eevent);
        }
    }
}