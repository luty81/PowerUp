namespace PowerUp.Database
{
    public class BatchOperation
    {
        public string CommandSQL { get; set; }
        public object Entity { get; set; }

        public BatchOperation(string sql, object entity)
        {
            CommandSQL = sql;
            Entity = entity;
        }
    }
}
