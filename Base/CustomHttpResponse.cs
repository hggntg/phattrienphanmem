using System;
using System.Collections.Generic;
namespace Base
{
    public abstract class BaseResult
    {
        public abstract string Type { get; }
    }

    public class SingleResult <T>: BaseResult
    {
        public override string Type {
            get
            {
                return "Single";
            }
        }
        public T Result { get; set; }

        public SingleResult()
        {
        
        }
    }

    public class ListResult<T> : BaseResult
    {
        public override string Type {
            get
            {
                return "List";
            }
        }
        public int Page { get; set; }
        public int RecordPerPage { get; set; }
        public int Length { get; set; }
        public int Total { get; set; }
        public bool IsEnd { get; set; }
        public List<T> Results { get; set; }
    }

    public class StateResult : BaseResult
    {
        public override string Type {
            get
            {
                return "State";
            }
        }
        public string Status { get; set; }
        public string Message { get; set; }
        
        public StateResult()
        {

        }

        public StateResult(string Status, string Message)
        {
            this.Status = Status;
            this.Message = Message;
        }
    }

    public static class CustomHttpResponse
    {
        public static SingleResult<T> Single<T>()
        {
            return new SingleResult<T>();
        }
        public static ListResult<T> List<T>()
        {
            return new ListResult<T>();
        }
        public static StateResult State()
        {
            return new StateResult();
        }
    }
}
