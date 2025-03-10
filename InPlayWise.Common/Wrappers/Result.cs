namespace InPlayWiseCommon.Wrappers
{
    public class Result<T>
    {
        public T Items { get; set; }
        public bool IsSuccess { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public Result(int statusCode, bool isSuccess, string message, T items)
        {
            this.StatusCode = statusCode;
            this.IsSuccess = isSuccess;
            this.Message = message;
            this.Items = items;
        }


        public Result() {

        }


		public static Result<T> Success(string msg = "", T item = default(T))
		{
			return new Result<T>()
			{
				StatusCode = 200,
				IsSuccess = true,
				Items = item,
				Message = string.IsNullOrEmpty(msg) ? "Success" : msg
			};
		}



		public static Result<T> InternalServerError(string msg = "")
        {
            return new Result<T>()
            {
                StatusCode = 500,
                IsSuccess = false,
                Items = default(T),
                Message = string.IsNullOrEmpty(msg) ? "Internal Server Error" : msg
            };
        }

		public static Result<T> BadRequest(string msg = "")
		{
			return new Result<T>()
			{
				StatusCode = 400,
				IsSuccess = false,
				Items = default(T),
				Message = string.IsNullOrEmpty(msg) ? "Bad Request" : msg
			};
		}


		public static Result<T> Unauthorized(string msg = "")
		{
			return new Result<T>()
			{
				StatusCode = 401,
				IsSuccess = false,
				Items = default(T),
				Message = string.IsNullOrEmpty(msg) ? "Unauthorized Access" : msg
			};
		}

		public static Result<T> NotFound(string msg = "")
		{
			return new Result<T>()
			{
				StatusCode = 404,
				IsSuccess = false,
				Items = default(T),
				Message = string.IsNullOrEmpty(msg) ? "Not Found" : msg
			};
		}

		public static Result<T> Forbidden(string msg = "")
		{
			return new Result<T>()
			{
				StatusCode = 403,
				IsSuccess = false,
				Items = default(T),
				Message = string.IsNullOrEmpty(msg) ? "Forbidden" : msg
			};
		}

		public static Result<T> Conflict(string msg = "")
		{
			return new Result<T>()
			{
				StatusCode = 409,
				IsSuccess = false,
				Items = default(T),
				Message = string.IsNullOrEmpty(msg) ? "Conflict" : msg
			};
		}

	}
}