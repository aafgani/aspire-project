namespace App.Domain.Model
{
    public class Result
    {
        public bool IsSuccess { get; }
        public string[] Errors { get; set; }
        public static Result Success() => new(true, Array.Empty<string>());
        public static Result Failure(params string[] errors) => new Result(false, errors);
        public Result(bool isSuccess, string[] errors)
        {
            IsSuccess = isSuccess;
            Errors = errors;
        }
    }

    public class Result<T> : Result
    {
        public T? Value { get; }
        public static Result<T> Success(T value) => new (value, true, Array.Empty<string>());
        public new static Result<T> Failure(params string[] errors) => new(default, false, errors);
        private Result(T? value, bool isSuccess, params string[] errors) 
            : base(isSuccess, errors)
        {
            Value = value;
        }
    }
}
