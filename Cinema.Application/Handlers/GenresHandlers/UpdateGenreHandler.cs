using AutoMapper;
using Cinema.Application.Commands.GenresCommands;
using Cinema.Domain.Entities;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using MediatR;

namespace Cinema.Application.Handlers.GenresHandlers
{
    internal sealed class UpdateGenreHandler : IRequestHandler<UpdateGenreCommand, ApiBaseResponse>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public UpdateGenreHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ApiBaseResponse> Handle(UpdateGenreCommand request, CancellationToken cancellationToken)
        {
            var genre = await _repository.Genre.GetGenreAsync(request.Id, request.TrackChanges);
            if (genre is null)
                return new GenreNotFoundResponse(request.Id);

            _mapper.Map(request.GenreForUpdateDto, genre);
            await _repository.SaveAsync();

            return new ApiOkResponse<Genre>(genre);
        }
    }
}