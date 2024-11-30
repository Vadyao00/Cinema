using AutoMapper;
using Cinema.Application.Queries.WorkLogsQueries;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using MediatR;

namespace Cinema.Application.Handlers.WorkLogsHandlers
{
    public sealed class GetWorkLogHandler : IRequestHandler<GetWorkLogQuery, ApiBaseResponse>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public GetWorkLogHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ApiBaseResponse> Handle(GetWorkLogQuery request, CancellationToken cancellationToken)
        {
            var workLogDb = await _repository.WorkLog.GetWorkLogAsync(request.Id, request.TrackChanges);
            if (workLogDb is null)
                return new WorkLogNotFoundResponse(request.Id);

            var workLogDto = _mapper.Map<WorkLogDto>(workLogDb);
            return new ApiOkResponse<WorkLogDto>(workLogDto);
        }
    }
}