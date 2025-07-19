using System.Collections.Generic;
using UnityEngine;

public class RandomProperty<T> : StateMachineBehaviour
{
    [SerializeField] string property;
    [SerializeField] List<T> Values = new List<T>();
    [SerializeField] bool stateEnter = true;
    [SerializeField] bool stateExit = false;
    [SerializeField] bool stateMove = false;

    void SetRandomValue(Animator animator)
    {
        if (string.IsNullOrEmpty(property) || Values == null || Values.Count == 0)
            return;

        int index = Random.Range(0, Values.Count);
        T value = Values[index];

        if (typeof(T) == typeof(int))
        {
            animator.SetInteger(property, (int)(object)value);
        }
        else if (typeof(T) == typeof(float))
        {
            animator.SetFloat(property, (float)(object)value);
        }
        else if (typeof(T) == typeof(bool))
        {
            animator.SetBool(property, (bool)(object)value);
        }
        else
        {
            Debug.LogWarning($"Unsupported parameter type: {typeof(T)}");
        }
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateEnter)
        {
            SetRandomValue(animator);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateExit)
        {
            SetRandomValue(animator);
        }
    }

    public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateMove)
        {
            SetRandomValue(animator);
        }
    }
}
