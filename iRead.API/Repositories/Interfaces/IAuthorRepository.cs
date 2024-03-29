﻿namespace iRead.API.Repositories
{
    public interface IAuthorRepository
    {
        Task<IEnumerable<Author>> GetAuthors();
        Task<Author> GetAuthor(int id);
        Task<IEnumerable<Author>> GetMultipleAuthors(IEnumerable<int> ids);
    }
}
