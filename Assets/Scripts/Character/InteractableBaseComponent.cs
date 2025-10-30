using Battle;
using Character;
using System;
using UnityEngine;

public class InteractableBaseComponent : MonoBehaviour, IInteractable
{
    [Header("Base Interaction Settings")]
    [SerializeField, Range(0f, 10f)] protected float requiredHoldTime = 2f;
    [SerializeField, Range(0f, 10f)] protected float releaseDecaySpeed = 2f;
    [SerializeField, Range(0f, 10f)] protected float interactionRange = 3f;
    [SerializeField, Range(0f, 10f)] protected float cancelTimeOut = 3f;     
    [SerializeField] protected Transform anchor;

    protected float heldTime;
    protected float cancelTimer;
    protected bool isActive;
    protected IActor currentActor;

    public float Progress => Mathf.Clamp01(heldTime / Mathf.Max(0.01f, requiredHoldTime));

    public event Action<float> OnProgress;
    public event Action OnCompleted, OnBegin, OnCancel;

    protected virtual void Awake()
    {
        if (anchor == null)
            anchor = transform;
    }

    public virtual bool CanBegin(IActor actor)
    {
        if (actor is Component comp && comp.TryGetComponent<Transform>(out var t))
        {
            float dist = Vector3.Distance(anchor.position, t.position);
            return dist <= interactionRange;
        }

        return false;
    }

    public virtual void Begin(IActor actor)
    {
        if (!CanBegin(actor)) return;
        Debug.Log("Begin");
        currentActor = actor;
        isActive = true;
        cancelTimer = 0f;
        OnBegin?.Invoke();
    }

    public virtual void Tick(IActor actor, float deltaTime)
    {
        if (!isActive) return;

        heldTime += deltaTime;
        cancelTimer = 0f; 
        OnProgress?.Invoke(Progress);
        Debug.Log(Progress);
        if (heldTime >= requiredHoldTime)
            CompleteInteraction(actor);
    }

    public virtual void Cancel(IActor actor)
    {
        if (!isActive) return;

        isActive = false;
        cancelTimer = 0f; 
        OnCancel?.Invoke();
    }

    protected virtual void Update()
    {
        if (!isActive && heldTime > 0f)
        {
            heldTime = Mathf.Max(0f, heldTime - Time.deltaTime * releaseDecaySpeed);
            OnProgress?.Invoke(Progress);

           
            if (cancelTimer >= cancelTimeOut)
            {
                ResetState();
            }
        }
    }

    protected virtual void CompleteInteraction(IActor actor)
    {
        OnCompleted?.Invoke();
        ResetState();
    }

    protected virtual void ResetState()
    {
        isActive = false;
        heldTime = 0f;
        currentActor = null;
        cancelTimer = 0f;
        OnProgress?.Invoke(Progress);
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(anchor ? anchor.position : transform.position, interactionRange);
    }
}
