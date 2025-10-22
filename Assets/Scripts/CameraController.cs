
using UnityEngine;
using Cinemachine;

public class CameraController : Singleton<CameraController>
{
    CinemachineVirtualCamera cinemachineVirtualCamera;

    
   private void Start()
    {
      
        cinemachineVirtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
    }
    public void SetFollow(Transform subjectToFollow)
    {
        if (cinemachineVirtualCamera == null)
            cinemachineVirtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        cinemachineVirtualCamera.Follow = subjectToFollow;
    }


    /*public IEnumerator DampingHandler()
    {
        float defaultDamp = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_XDamping;
        cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_XDamping = 0;
        yield return new WaitForSeconds(1f);
        cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_XDamping = defaultDamp;
    }*/

}
