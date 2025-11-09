using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
namespace Battle
{
    public class FieldComponent : MonoBehaviour, IInitializable, IDisposable, IActor
    {
        public List<IEnemy> enemys = new();
        bool isDispose;
        [SerializeField] bool initOnAwake;

        public void Add(IEnemy enemy)
        {
            enemys.Add(enemy);
            if (isDispose)
            {
                if (enemy is IGameObject tActor) tActor.transform.gameObject.SetActive(false);
                if (enemy is IDisposable disposable) disposable.Dispose();
            }
        }
        public void Remove(IEnemy enemy)
        {
            enemys.Remove(enemy);
        }
        
        void Awake()
        {
            if (initOnAwake) Initialize();
        }
        public void Initialize()
        {
            isDispose = false;

            for (int i = 0; i < enemys.Count; i++)
            {
                if (enemys[i] is IGameObject tActor) tActor.transform.gameObject.SetActive(true);
                if (enemys[i] is IInitializable initializable) initializable.Initialize();
            }
        }
        public void Dispose()
        {
            isDispose = true;
            for (int i = 0; i < enemys.Count; i++)
            {
                if (enemys[i] is IGameObject tActor) tActor.transform.gameObject.SetActive(false);
                if (enemys[i] is IDisposable disposable) disposable.Dispose();
            }
        }
    }
}
