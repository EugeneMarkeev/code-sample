using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Infrastructure.Configs
{
    public interface ISpritesConfigService
    {
        UniTask Initialize();
        Sprite GetSprite(string id);
        Sprite GetMockSprite();
    }
}