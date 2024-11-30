using AutoMapper;
using Cinema.Application.Queries.WorkLogsQueries;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.RequestFeatures;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using MediatR;

namespace Cinema.Application.Handlers.WorkLogsHandlers
{
    public sealed class GetWorkLogsHandler : IRequestHandler<GetWorkLogsQuery, ApiBaseResponse>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public GetWorkLogsHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ApiBaseResponse> Handle(GetWorkLogsQuery request, CancellationToken cancellationToken)
        {
            var workLogsWithMetaData = await _repository.WorkLog.GetAllWorkLogsAsync(request.WorkLogParameters, request.TrackChanges);
            var workLogsDto = _mapper.Map<IEnumerable<WorkLogDto>>(workLogsWithMetaData);

            return new ApiOkResponse<(IEnumerable<WorkLogDto>, MetaData)>((workLogsDto, workLogsWithMetaData.MetaData));
        }
    }
}