
using UnityEngine;
using Cinemachine;
public class ChinemachineShakeManager : Singleton<ChinemachineShakeManager>
{
    CinemachineImpulseSource source;
    protected override void Awake()
    {
        base.Awake();
        source = GetComponent<CinemachineImpulseSource>();
    }
    public void Shake()
    {
        source.GenerateImpulse();
    }
}
