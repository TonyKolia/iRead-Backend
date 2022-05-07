namespace iRead.API.Repositories.Interfaces
{
    public interface IPublisherRepository
    {
        Task<Publisher> GetPublisher(int id);
        Task<IEnumerable<Publisher>> GetPublishers();
    }
}
