﻿namespace Cinema.Domain.RequestFeatures
{
    public class TicketParameters : RequestParameters
    {
        public TicketParameters() => OrderBy = "PurchaseDate";
        public uint MinSeatNumber { get; set; }
        public uint MaxSeatNumber { get; set; } = int.MaxValue;

        public bool ValidSeatNumber => MaxSeatNumber > MinSeatNumber;
    }
}