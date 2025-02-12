namespace Domain.Common
{
    public interface ISnapshotable<T>
    {
        public T GetSnapshot();
        public void RestoreFromSnapshot(T snapshot);
    }
}