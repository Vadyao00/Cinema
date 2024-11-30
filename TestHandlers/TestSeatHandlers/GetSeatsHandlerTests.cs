using AutoMapper;
using Cinema.Application.Queries.SeatsQueries;
using Cinema.Application.Handlers.SeatsHandlers;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Entities;
using Cinema.Domain.RequestFeatures;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using Moq;

namespace TestHandlers.TestSeatHandlers
{
    public class GetSeatsHandlerTests
    {
        private readonly Mock<IRepositoryManager> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetSeatsHandler _handler;

        public GetSeatsHandlerTests()
        {
            _repositoryMock = new Mock<IRepositoryManager>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetSeatsHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_SeatsFound_ReturnsApiOkResponseWithSeatDtoAndMetaData()
        {
            var seatParameters = new SeatParameters { PageNumber = 1, PageSize = 10 };
            var seats = new List<Seat>
            {
                new Seat { SeatId = Guid.NewGuid(), SeatNumber = 1 },
                new Seat { SeatId = Guid.NewGuid(), SeatNumber = 2 }
            };

            var metaData = new MetaData
            {
                TotalCount = 2,
                PageSize = 10,
                CurrentPage = 1,
                TotalPages = 1
            };

            var seatsDto = new List<SeatDto>
            {
                new SeatDto { SeatId = seats[0].SeatId, SeatNumber = seats[0].SeatNumber },
                new SeatDto { SeatId = seats[1].SeatId, SeatNumber = seats[1].SeatNumber }
            };

            var seatsWithMetaData = new PagedList<Seat>(seats, metaData.TotalCount, seatParameters.PageNumber, seatParameters.PageSize);

            var command = new GetSeatsQuery(seatParameters, TrackChanges: false);

            _repositoryMock.Setup(repo => repo.Seat.GetAllSeatsAsync(seatParameters, false))
                .ReturnsAsync(seatsWithMetaData);

            _mapperMock.Setup(m => m.Map<IEnumerable<SeatDto>>(seats)).Returns(seatsDto);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsType<ApiOkResponse<(IEnumerable<SeatDto>, MetaData)>>(result);
            var apiResponse = result as ApiOkResponse<(IEnumerable<SeatDto>, MetaData)>;
            Assert.Equal(2, apiResponse.Result.Item1.Count());
            Assert.Equal(metaData.TotalCount, apiResponse.Result.Item2.TotalCount);
            Assert.Equal(metaData.PageSize, apiResponse.Result.Item2.PageSize);
            Assert.Equal(metaData.CurrentPage, apiResponse.Result.Item2.CurrentPage);
            Assert.Equal(metaData.TotalPages, apiResponse.Result.Item2.TotalPages);

            _repositoryMock.Verify(repo => repo.Seat.GetAllSeatsAsync(seatParameters, false), Times.Once);
            _mapperMock.Verify(m => m.Map<IEnumerable<SeatDto>>(seats), Times.Once);
        }

        [Fact]
        public async Task Handle_NoSeatsFound_ReturnsApiOkResponseWithEmptyListAndMetaData()
        {
            var seatParameters = new SeatParameters { PageNumber = 1, PageSize = 10};
            var seats = new List<Seat>();

            var metaData = new MetaData
            {
                TotalCount = 0,
                PageSize = 10,
                CurrentPage = 1,
                TotalPages = 0
            };

            var seatsDto = new List<SeatDto>();

            var seatsWithMetaData = new PagedList<Seat>(seats, metaData.TotalCount, seatParameters.PageNumber, seatParameters.PageSize);

            var command = new GetSeatsQuery(seatParameters, TrackChanges: false);

            _repositoryMock.Setup(repo => repo.Seat.GetAllSeatsAsync(seatParameters, false))
                .ReturnsAsync(seatsWithMetaData);

            _mapperMock.Setup(m => m.Map<IEnumerable<SeatDto>>(seats)).Returns(seatsDto);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsType<ApiOkResponse<(IEnumerable<SeatDto>, MetaData)>>(result);
            var apiResponse = result as ApiOkResponse<(IEnumerable<SeatDto>, MetaData)>;
            Assert.Empty(apiResponse.Result.Item1);
            Assert.Equal(metaData.TotalCount, apiResponse.Result.Item2.TotalCount);
            Assert.Equal(metaData.PageSize, apiResponse.Result.Item2.PageSize);
            Assert.Equal(metaData.CurrentPage, apiResponse.Result.Item2.CurrentPage);
            Assert.Equal(metaData.TotalPages, apiResponse.Result.Item2.TotalPages);

            _repositoryMock.Verify(repo => repo.Seat.GetAllSeatsAsync(seatParameters, false), Times.Once);
            _mapperMock.Verify(m => m.Map<IEnumerable<SeatDto>>(seats), Times.Once);
        }
    }
}