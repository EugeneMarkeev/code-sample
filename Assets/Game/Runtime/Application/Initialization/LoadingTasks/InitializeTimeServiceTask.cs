using Cysharp.Threading.Tasks;
using Infrastructure.LoadingTasks;
using Infrastructure.Time;
using UnityEngine.Scripting;

namespace Application.Initialization.LoadingTasks
{
    public class InitializeTimeServiceTask : ILoadingTask
    {
        private readonly LocalTimeService _localTimeService;

        [Preserve]
        public InitializeTimeServiceTask(LocalTimeService localTimeService)
        {
            _localTimeService = localTimeService;
        }

        public UniTask LoadAsync()
        {
            _localTimeService.Initialize();
            return UniTask.CompletedTask;
        }
    }
}