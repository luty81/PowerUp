namespace PowerUp.Tests.SQL.FakeEntities
{
    public class SampleType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; }
        private double Internal { get; set; }
    }
}