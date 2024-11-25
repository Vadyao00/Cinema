using AutoMapper;
using Cinema.Application.Commands.GenresCommands;
using Cinema.Domain.Entities;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using MediatR;

namespace Cinema.Application.Handlers.GenresHandlers
{
    public sealed class DeleteGenreHandler : IRequestHandler<DeleteGenreCommand, ApiBaseResponse>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public DeleteGenreHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ApiBaseResponse> Handle(DeleteGenreCommand request, CancellationToken cancellationToken)
        {
            var genre = await _repository.Genre.GetGenreAsync(request.Id, request.TrackChanges);
            if (genre is null)
                return new GenreNotFoundResponse(request.Id);

            _repository.Genre.DeleteGenre(genre);
            await _repository.SaveAsync();

            return new ApiOkResponse<Genre>(genre);
        }
    }
}