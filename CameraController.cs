using UnityEngine;
using UnityEngine.Rendering;

public class CameraController : MonoBehaviour {

    [SerializeField]
    private Camera _activeCamera;
    [SerializeField]
    private Camera _hiddenCamera;

    void Awake() {
        var rt = new RenderTexture(Screen.width, Screen.height, 24);
        Shader.SetGlobalTexture("_ParallelTexture", rt);
        _hiddenCamera.targetTexture = rt;
    }

    public void SwapCameras() {
        _activeCamera.targetTexture = _hiddenCamera.targetTexture;
        _hiddenCamera.targetTexture = null;

        var swapCamera = _activeCamera;
        _activeCamera = _hiddenCamera;
        _hiddenCamera = swapCamera;
    }
}