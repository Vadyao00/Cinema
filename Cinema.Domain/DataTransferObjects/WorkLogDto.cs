﻿namespace Cinema.Domain.DataTransferObjects
{
    public record WorkLogDto
    {
        public Guid WorkLogId { get; init; }

        public string? EmployeeName { get; init; }

        public int WorkExperience { get; init; }

        public DateOnly StartDate { get; init; }

        public decimal WorkHours { get; init; }
    }
}