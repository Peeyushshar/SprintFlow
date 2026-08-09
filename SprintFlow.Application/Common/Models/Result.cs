using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SprintFlow.Application.Common.Models
{
    public class Result
    {
        public bool IsSuccess { get; }

        public bool IsFailure => !IsSuccess;

        public Error Error { get; }

        protected Result(bool isSuccess, Error error)
        {
            if (isSuccess && error != Error.None)
                throw new ArgumentException("Invalid Error.");

            if (!isSuccess && error == Error.None)
                throw new ArgumentException("Invalid Error.");

            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result Success()
            => new(true, Error.None);

        public static Result Failure(Error error)
            => new(false, error);

        public static Result<T> Failure<T>(Error error)
            => Result<T>.Failure(error);
    }

    public class Result<T> : Result
    {
        public T? Value { get; }

        private Result(T? value, bool isSuccess, Error error)
            : base(isSuccess, error)
        {
            Value = value;
        }

        public static Result<T> Success(T value)
            => new(value, true, Error.None);

        public static new Result<T> Failure(Error error)
            => new(default, false, error);

        public static implicit operator Result<T>(T value)
            => Success(value);
    }
}
