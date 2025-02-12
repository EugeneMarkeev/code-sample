using Cysharp.Threading.Tasks;

namespace Infrastructure.LoadingTasks
{
    public interface ILoadingTask
    {
        UniTask LoadAsync();
    }
}