﻿using Microsoft.EntityFrameworkCore.Query;
using RoomService.RoomModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RoomService.RoomRepository
{
    public interface IRepository<T, Tkey> where T : class where Tkey : struct
    {
        Task SaveChangeAsync();
        Task AddRoomAsync(T obj);
        Task<IQueryable<T>> GetAllAsync(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null);
        Task DeleteRoom(Tkey id);
        Task EditRoom(T room);
        Task<T> FindRoomAsync(Tkey id);
    }
}