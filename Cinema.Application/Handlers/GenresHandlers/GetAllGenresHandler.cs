using AutoMapper;
using Cinema.Application.Queries.GenresQueries;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using MediatR;

namespace Cinema.Application.Handlers.GenresHandlers
{
    public sealed class GetAllGenresHandler : IRequestHandler<GetAllGenresQuery, ApiBaseResponse>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public GetAllGenresHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ApiBaseResponse> Handle(GetAllGenresQuery request, CancellationToken cancellationToken)
        {
            var genres = await _repository.Genre.GetAllGenresWithoutMetaAsync(request.TrackChanges);
            var genresDto = _mapper.Map<IEnumerable<GenreDto>>(genres);

            return new ApiOkResponse<IEnumerable<GenreDto>>(genresDto);
        }
    }
}