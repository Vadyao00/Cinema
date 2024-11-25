using AutoMapper;
using Cinema.Application.Handlers.MoviesHandlers;
using Cinema.Application.Queries.MoviesQueries;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Entities;
using Cinema.Domain.RequestFeatures;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using Moq;

namespace TestHandlers.TestMovieHandlers
{
    public class GetMoviesHandlerTests
    {
        private readonly Mock<IRepositoryManager> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetMoviesHandler _handler;

        public GetMoviesHandlerTests()
        {
            _repositoryMock = new Mock<IRepositoryManager>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetMoviesHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_InvalidAgeRestriction_ReturnsAgeRestrictionBadRequestResponse()
        {
            var movieParameters = new MovieParameters
            {
                MinAgeRestriction = 18,
                MaxAgeRestriction = 16
            };
            var query = new GetMoviesQuery(movieParameters, false);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.IsType<AgeRestrictionBadRequestResponse>(result);
        }

        [Fact]
        public async Task Handle_ValidParameters_ReturnsMoviesWithMetaData()
        {
            var movieParameters = new MovieParameters
            {
                MinAgeRestriction = 13,
                MaxAgeRestriction = 18,
                searchTitle = "Action",
                PageSize = 2,
                PageNumber = 1
            };
            var query = new GetMoviesQuery(movieParameters, false);

            var movies = new List<Movie>
            {
                new Movie { MovieId = Guid.NewGuid(), Title = "Action Movie 1" },
                new Movie { MovieId = Guid.NewGuid(), Title = "Action Movie 2" }
            };

            var metaData = new MetaData
            {
                CurrentPage = 1,
                TotalPages = 1,
                PageSize = 2,
                TotalCount = 2
            };

            var pagedMovies = new PagedList<Movie>(movies, movies.Count, metaData.CurrentPage, metaData.PageSize);

            var moviesDto = new List<MovieDto>
            {
                new MovieDto { MovieId = movies[0].MovieId, Title = "Action Movie 1" },
                new MovieDto { MovieId = movies[1].MovieId, Title = "Action Movie 2" }
            };

            _repositoryMock.Setup(repo => repo.Movie.GetAllMoviesAsync(movieParameters, false))
                .ReturnsAsync(pagedMovies);

            _mapperMock.Setup(mapper => mapper.Map<IEnumerable<MovieDto>>(movies))
                .Returns(moviesDto);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.IsType<ApiOkResponse<(IEnumerable<MovieDto>, MetaData)>>(result);
            var apiResponse = result as ApiOkResponse<(IEnumerable<MovieDto>, MetaData)>;

            Assert.NotNull(apiResponse);
            Assert.NotNull(apiResponse.Result);
            Assert.Equal(2, apiResponse.Result.Item1.Count());
            Assert.Equal(metaData.TotalCount, apiResponse.Result.Item2.TotalCount);

            _repositoryMock.Verify(repo => repo.Movie.GetAllMoviesAsync(movieParameters, false), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<IEnumerable<MovieDto>>(movies), Times.Once);
        }
    }
}