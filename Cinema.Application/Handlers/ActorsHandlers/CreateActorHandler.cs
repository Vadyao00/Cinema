using AutoMapper;
using Cinema.Application.Commands.ActorsCommands;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Entities;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using MediatR;

namespace Cinema.Application.Handlers.ActorsHandlers
{
    public sealed class CreateActorHandler : IRequestHandler<CreateActorCommand, ActorDto>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public CreateActorHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ActorDto> Handle(CreateActorCommand request, CancellationToken cancellationToken)
        {
            var actorEntity = _mapper.Map<Actor>(request.ActorDto);

            actorEntity.Movies.Clear();

            var movies = await _repository.Movie.GetMoviesByIdsAsync(request.ActorDto.MoviesIds, false);

            if(movies is not null)
                foreach( var movie in movies)
                {
                    _repository.Movie.Attach(movie);
                    actorEntity.Movies.Add(movie);
                }

            _repository.Actor.CreateActor(actorEntity);
            await _repository.SaveAsync();

            var actorToReturn = _mapper.Map<ActorDto>(actorEntity);

            return actorToReturn;
        }
    }
}