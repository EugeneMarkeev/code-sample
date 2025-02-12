using Cysharp.Threading.Tasks;

namespace Infrastructure.Configs
{
    public interface IConfigsService
    {
        UniTask Initialize();
        T Get<T>();
    }
}