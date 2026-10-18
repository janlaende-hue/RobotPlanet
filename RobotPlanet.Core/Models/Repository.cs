using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RobotPlanet.Core.Interfaces;

namespace RobotPlanet.Core.Models
{
    public class Repository<T> : IRepository<T> where T : Robot
    {
        private readonly List<T> items = new();

        public event Action<T>? Added;
        public event Action<T>? Removed;
        public event Action<T, string>? CrazyActionPerformed;
        public event Action<T, string>? ReactionOccurred;

        public void Add(T item)
        {
            foreach (var existing in items.ToList())
            {
                var msg = existing.OnOtherAdded(item);
                if (!string.IsNullOrEmpty(msg))
                    ReactionOccurred?.Invoke(existing, msg);
            }

            items.Add(item);
            Added?.Invoke(item);
        }

        public void Remove(T item)
        {
            var removed = items.Remove(item);
            if (!removed) return;

            foreach (var remaining in items.ToList())
            {
                var msg = remaining.OnOtherRemoved(item);
                if (!string.IsNullOrEmpty(msg))
                    ReactionOccurred?.Invoke(remaining, msg);
            }

            Removed?.Invoke(item);
        }

        public IEnumerable<T> GetAll() => items.ToList();

        public T? Find(Func<T, bool> predicate) => items.FirstOrDefault(predicate);

        public void TriggerCrazyAction(T item, string actionDescription)
        {
            if (!items.Contains(item)) return;

            CrazyActionPerformed?.Invoke(item, actionDescription);

            foreach (var other in items.Where(x => !ReferenceEquals(x, item)).ToList())
            {
                var msg = other.OnOtherCrazyAction(item, actionDescription);
                if (!string.IsNullOrEmpty(msg))
                    ReactionOccurred?.Invoke(other, msg);
            }
        }
    }
}