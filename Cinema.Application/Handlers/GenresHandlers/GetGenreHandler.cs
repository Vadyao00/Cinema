using AutoMapper;
using Cinema.Application.Queries.GenresQueries;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using MediatR;

namespace Cinema.Application.Handlers.GenresHandlers
{
    public sealed class GetGenreHandler : IRequestHandler<GetGenreQuery, ApiBaseResponse>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public GetGenreHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ApiBaseResponse> Handle(GetGenreQuery request, CancellationToken cancellationToken)
        {
            var genre = await _repository.Genre.GetGenreAsync(request.Id, request.TrackChanges);
            if (genre is null)
                return new GenreNotFoundResponse(request.Id);

            var genreDto = _mapper.Map<GenreDto>(genre);

            return new ApiOkResponse<GenreDto>(genreDto);
        }
    }
}