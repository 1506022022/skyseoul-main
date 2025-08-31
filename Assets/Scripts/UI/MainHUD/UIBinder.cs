using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameUI
{
    public class UIBinder : MonoBehaviour
    {
        protected Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();

        protected void Bind<T, TEnum>()
            where T : UnityEngine.Object
            where TEnum : Enum
        {
            string[] names = Enum.GetNames(typeof(TEnum));
            UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
            _objects[typeof(TEnum)] = objects;

            for (int i = 0; i < names.Length; i++)
            {
                if (typeof(T) == typeof(GameObject))
                    objects[i] = Utils.FindChild(gameObject, names[i], true);
                else
                    objects[i] = Utils.FindChild<T>(gameObject, names[i], true);

                if (objects[i] == null)
                    Debug.LogWarning($"[UIBinder] Failed to bind ({names[i]}) in {typeof(TEnum).Name}");
            }
        }

        protected T Get<T, TEnum>(TEnum key)
            where T : UnityEngine.Object
            where TEnum : Enum
        {
            if (_objects.TryGetValue(typeof(TEnum), out var objects) == false)
                return null;

            int idx = Convert.ToInt32(key);
            return objects[idx] as T;
        }
    }
}