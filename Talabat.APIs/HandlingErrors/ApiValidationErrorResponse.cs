﻿namespace Talabat.APIs.HandlingErrors
{
    public class ApiValidationErrorResponse : ApiResponse
    {
        public IEnumerable<String> Errors { get; set; }
        public ApiValidationErrorResponse() : base(400)
        {
            Errors = new List<String>();
        }
    }
}
