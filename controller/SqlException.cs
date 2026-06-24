namespace InjuryLogs.controller
{
    [Serializable]
    internal class SqlException : Exception
    {
        public SqlException()
        {
        }

        public SqlException(string? message) : base(message)
        {
        }

        public SqlException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}