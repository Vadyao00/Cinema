﻿namespace Cinema.Domain.RequestFeatures
{
    public class MovieParameters : RequestParameters
    {
        public MovieParameters() => OrderBy = "Title";
        public uint MinAgeRestriction { get; set; }
        public uint MaxAgeRestriction { get; set; } = int.MaxValue;

        public bool ValidAgeRestriction => MaxAgeRestriction > MinAgeRestriction;

        public string? searchTitle { get; set; }
        public string? searchProductionCompany { get; set; }
    }
}