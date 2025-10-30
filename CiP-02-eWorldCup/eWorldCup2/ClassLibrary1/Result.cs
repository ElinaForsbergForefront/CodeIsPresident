using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace eWorldCup2.Domain
{
    public readonly struct Result
    {
        public bool IsSuccess { get; } 
        public Error Error { get; } 
        private Result(bool isSuccess, Error error)
        {
            IsSuccess = isSuccess;
            Error = error;
        }
        public static Result Success() => new Result(true, Error.None);
        public static Result Failure(Error error) => new Result(false, error);
    }
    public readonly struct Result<T>
    {
        public bool IsSuccess { get; }
        public T? Value { get; } 
        public Error Error { get; } 
        private Result(bool isSuccess, T? value, Error error)
        {
            IsSuccess = isSuccess;
            Value = value;
            Error = error;
        }
        public static Result<T> Success(T value) => new Result<T>(true, value, Error.None);
        public static Result<T> Failure(Error error) => new Result<T>(false, default, error);
    }
}
