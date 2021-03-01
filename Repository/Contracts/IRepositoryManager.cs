namespace Entities.Contracts
{
    public interface IRepositoryManager
    {
        IAlimentRepository AlimentRepo { get; }
        ICartRepository CartRepo { get; }
        void Save();
    }
}
