﻿namespace Cinema.Domain.Exceptions
{
    public sealed class IdParametrBadRequestException : BadRequestException
    {
        public IdParametrBadRequestException() : base("Parametr ids is null")
        {
        }
    }
}