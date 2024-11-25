using AutoMapper;
using Cinema.Application.Commands.GenresCommands;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Entities;
using Contracts.IRepositories;
using MediatR;

namespace Cinema.Application.Handlers.GenresHandlers
{
    public sealed class CreateGenreHandler : IRequestHandler<CreateGenreCommand, GenreDto>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public CreateGenreHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<GenreDto> Handle(CreateGenreCommand request, CancellationToken cancellationToken)
        {
            var genreEntity = _mapper.Map<Genre>(request.GenreDto);

            _repository.Genre.CreateGenre(genreEntity);
            await _repository.SaveAsync();

            var genreToReturn = _mapper.Map<GenreDto>(genreEntity);

            return genreToReturn;
        }
    }
}