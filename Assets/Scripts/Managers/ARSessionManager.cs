using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARSessionManager : MonoBehaviour
{
    private ARSession arSession;

    void Start()
    {
        arSession = FindObjectOfType<ARSession>();
        if (arSession != null)
        {
            arSession.enabled = true;
        }
    }

    public void ResetARSession()
    {
        if (arSession != null)
        {
            arSession.Reset();
        }
    }
}
