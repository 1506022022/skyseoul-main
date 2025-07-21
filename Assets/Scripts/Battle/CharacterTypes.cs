using UnityEngine;

namespace Battle
{
    public interface IEntity { }
    public interface IActor { }
    public interface IGameObject { Transform transform { get; } }
    public interface IPlayable { }
    public interface IEnemy { }
    public interface IProp { }
}