using Battle;
using UnityEngine;
public struct ParryContext
{
    public IActor Defender;   
    public IActor Attacker;  
    public Vector3 HitPoint;  
    public float InputTime;   
    public float AttackTime;  

    public float TimingDelta => Mathf.Abs(InputTime - AttackTime);
}
public class ParryController : MonoBehaviour
{
 
    void Start()
    {
        
    }

  
    void Update()
    {
        
    }
}
