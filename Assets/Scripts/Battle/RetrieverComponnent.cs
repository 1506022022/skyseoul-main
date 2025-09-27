using Character;
using Effect;
using System.Collections;
using UnityEngine;

public class RetrieverComponent : MonoBehaviour, IRetriever
{
    [SerializeField] float duration;
    [SerializeField] Vector3 offset;
    [SerializeField] Vector3 rotation;

    public float Duration => duration;
    public Vector3 Offset => offset;
    public Vector3 Rotation => rotation;

    public void Retrieve(Transform actor) => StartCoroutine(RetrieveRoutine(actor));

    private IEnumerator RetrieveRoutine(Transform actor)
    { 
        actor.TryGetComponent<IAppearance>(out var appearance);

        appearance?.InvokeDissolve();

        yield return new WaitForSeconds(GetWaitTime(appearance));

        actor.gameObject.SetActive(false);  

        RepositionActor(actor);

        actor.gameObject.SetActive(true);

        appearance?.InvokeAppear();
    }

    private float GetWaitTime(IAppearance appearance) => appearance != null ? appearance.Duration : Duration;

    private void RepositionActor(Transform actor)
    {
        actor.position = transform.position + Offset;
        actor.rotation = transform.rotation * Quaternion.Euler(Rotation);
    }
}
