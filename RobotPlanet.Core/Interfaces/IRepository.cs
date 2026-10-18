using System;
using System.Collections.Generic;
using RobotPlanet.Core.Models;

namespace RobotPlanet.Core.Interfaces
{
    public interface IRepository<T> where T : Robot
    {
        event Action<T>? Added;
        event Action<T>? Removed;
        event Action<T, string>? CrazyActionPerformed;
        event Action<T, string>? ReactionOccurred;

        void Add(T item);
        void Remove(T item);
        IEnumerable<T> GetAll();
        T? Find(Func<T, bool> predicate);
        void TriggerCrazyAction(T item, string actionDescription);
    }
}