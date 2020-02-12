using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UploadFile.WebApi.Repositories.Interfaces;
using UploadFile.WebApi.Context;
using Microsoft.EntityFrameworkCore;

namespace UploadFile.WebApi.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly UploadFileDbContext _db;

        public BaseRepository(UploadFileDbContext db)
        {
            _db = db;
        }

        public async Task<List<T>> GetAll()
        {
            return await _db.Set<T>().AsNoTracking().ToListAsync();
        }

        public async Task<T> Get(object id)
        {
            var entity =  await _db.Set<T>().FindAsync(id);

            _db.Entry(entity).State = EntityState.Detached;

            return entity;
        }

        public async Task<T> Save(T entity)
        {
            await _db.Set<T>().AddAsync(entity);

            await _db.SaveChangesAsync();

            return entity;
        }

        public async Task<List<T>> SaveRange(List<T> entity)
        {
            await _db.Set<T>().AddRangeAsync(entity);

            await _db.SaveChangesAsync();

            return entity;
        }

        public async Task Update(T entity)
        {
            _db.Set<T>().Update(entity);

            await _db.SaveChangesAsync();
        }

        public async Task UpdateRange(List<T> entity)
        {
            _db.Set<T>().UpdateRange(entity);

            await _db.SaveChangesAsync();
        }

        public async Task Delete(long id)
        {
            var entity = await Get(id);

            _db.Set<T>().Remove(entity);

            await _db.SaveChangesAsync();
        }

        public async Task DeleteRange(List<T> entity)
        {
            _db.Set<T>().RemoveRange(entity);

            await _db.SaveChangesAsync();
        }
    }
}