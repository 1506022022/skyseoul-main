using Battle;
using Character;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour, IActor
{
    [Header("Interaction Settings")]
    [SerializeField] private float rayDistance = 5f;
    [SerializeField] private LayerMask interactMask = ~0;
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    Camera playerCamera;
    IInteractable currentTarget;
    RaycastHit hitInfo;

    public Vector3 Position => transform.position;
    public Vector3 Forward => transform.forward;

    void Awake()
    {
        if (playerCamera == null)
            playerCamera = Camera.main;
    }

    void Update()
    {
        HandleInteractionRay();
        HandleInput();
    }

    void HandleInteractionRay()
    {
        IInteractable hitTarget = null;

        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward,
     out hitInfo, rayDistance, interactMask))
        {
            hitTarget = hitInfo.collider.GetComponentInParent<IInteractable>();
        }


        if (currentTarget != hitTarget)
        {
            if (currentTarget != null)
                currentTarget.Cancel(this);

            currentTarget = hitTarget;
        }
    }

    void HandleInput()
    {
        if (currentTarget == null) return;


        if (Input.GetKeyDown(interactKey))
        {
            if (currentTarget.CanBegin(this))
                currentTarget.Begin(this);
        }

        if (Input.GetKey(interactKey))
        {
            currentTarget.Tick(this, Time.deltaTime);
        }

        if (Input.GetKeyUp(interactKey))
        {
            currentTarget.Cancel(this);
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if (playerCamera == null)
            return;

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(playerCamera.transform.position,
                        playerCamera.transform.position + playerCamera.transform.forward * rayDistance);
    }
#endif
}
