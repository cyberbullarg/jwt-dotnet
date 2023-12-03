namespace AuthAPIService.Shared
{
    public class Result<T> where T : class
    {
        public bool Failure { get; set; }
        public string Message => DefaultMessgae;
        public T? Data { get; set; }

        private const string DefaultMessgae = "Success";
    }
}
