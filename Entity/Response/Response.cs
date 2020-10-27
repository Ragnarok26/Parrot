namespace Entity.Response
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Response<T>
    {
        public string Status { get; set; }
        public bool Success { get { return Status == "ok"; } }
        public string Message { get; set; }
        public T Result { get; set; }
        public T Results { get; set; }

        public Response()
        {
            Status = string.Empty;
            Result = default;
            Results = default;
            Message = string.Empty;
        }
    }
}
