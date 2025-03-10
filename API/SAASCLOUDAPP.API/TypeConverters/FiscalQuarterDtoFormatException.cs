using System;

namespace SAASCLOUDAPP.API.ModelBinding
{
    public class FiscalQuarterDtoFormatException : Exception
    {
        public FiscalQuarterDtoFormatException(string value)
            : base($"{value} is not a fiscal quarter. Expected format is yyyyQq (where q is the quarter number). For example: 2020Q1.")
        {
        }
    }
}