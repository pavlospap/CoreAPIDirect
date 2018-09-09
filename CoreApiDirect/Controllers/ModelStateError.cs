namespace CoreApiDirect.Controllers
{
    internal class ModelStateError
    {
        public string Field { get; }
        public string Message { get; }

        public ModelStateError(
            string field,
            string message)
        {
            Field = field;
            Message = message;
        }
    }
}
