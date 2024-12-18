﻿namespace Cinema.Domain.DataTransferObjects
{
    public record EmployeeDto
    {
        public Guid EmployeeId { get; init; }

        public string? Name { get; init; }

        public string? Role { get; init; }
        public string? Events { get; init; }
        public string? Showtimes { get; init; }
    }
}