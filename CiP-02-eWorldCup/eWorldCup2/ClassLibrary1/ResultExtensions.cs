using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace eWorldCup2.Domain
{
    public static class ResultExtensions
    {
        public static Result<U> Map<T, U>(this Result<T> result, Func<T, U> map)
            => result.IsSuccess
               ? Result<U>.Success(map(result.Value!))
               : Result<U>.Failure(result.Error);

        public static Result<U> Bind<T, U>(this Result<T> result, Func<T, Result<U>> bind)
            => result.IsSuccess
               ? bind(result.Value!)
               : Result<U>.Failure(result.Error);

        public static Result<T> Tap<T>(this Result<T> result, Action<T> action)
        {
            if (result.IsSuccess) action(result.Value!);
            return result;
        }

        public static Result<T> Ensure<T>(this Result<T> result, Func<T, bool> predicate, Error error)
            => result.IsSuccess && !predicate(result.Value!)
               ? Result<T>.Failure(error)
               : result;
        
        public static Result<T> FromNullable<T>(T? value, Error error)
            => value is null ? Result<T>.Failure(error) : Result<T>.Success(value);
    }
}
