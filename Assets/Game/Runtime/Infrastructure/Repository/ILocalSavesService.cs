using Cysharp.Threading.Tasks;

namespace Infrastructure.Repository
{
    public interface ILocalSavesService
    {
        UniTask LoadAll();
        UniTask SaveAll();
        UniTask DeleteAll();
    }
}