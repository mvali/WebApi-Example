namespace Entities.Contracts
{
    public interface IAliment
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Line { get; set; }

        public string Platform { get; set; }
    }
}
