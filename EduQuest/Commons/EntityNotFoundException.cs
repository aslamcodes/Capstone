namespace EduQuest.Commons
{
    [Serializable]
    internal class EntityNotFoundException : Exception
    {
        private readonly string _message;
        public EntityNotFoundException()
        {
            _message = "Entity not found";
        }

        public EntityNotFoundException(string? message) : base(message)
        {
            _message = message;
        }

        public override string Message => _message;
    }
}