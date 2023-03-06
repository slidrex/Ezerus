using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform firstPos, secondPos;
    private Camera cam;
    private bool xz;
    private void Start()
    {
        cam = GetComponent<Camera>();
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F5) && xz == false)
        {
            cam.transform.position = secondPos.position;
            xz = !xz;
        }
        else if(Input.GetKeyDown(KeyCode.F5) && xz == true)
        {
            cam.transform.position = firstPos.position;
            xz = !xz;
        }
    }
}
