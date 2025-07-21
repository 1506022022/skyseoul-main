using UnityEngine;

[CreateAssetMenu(fileName = "ScanData", menuName = "SO/Scanner")]
public class ScannerSO : ScriptableObject
{
    [field: SerializeField] public ScanBasicData scanBasicData { get; private set; }
}
