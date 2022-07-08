using UnityEngine;

public class CameraController : MonoBehaviour
{
    //Initial transform values
    [SerializeField] private Transform target; 
    private Transform initialCameraTransform;
    private Vector3 initialTargetPosition;
    private float initialOrtSize;

    private Color originalBackColor;
    private string currentBackgrund;

    void Start()
    {
        //Save initial transform
        initialCameraTransform = transform;
        initialOrtSize = gameObject.GetComponent<Camera>().orthographicSize;
        initialTargetPosition = target.transform.position;

        //Save initial color background
        originalBackColor = GetComponent<Camera>().backgroundColor;
        currentBackgrund = "original";

        //Lock Camera view to the center
        transform.LookAt(target); 
    }

    //reset camera position, rotation, lookAtPoint and orthograpic size.
    public void ResetPosition()
    {
        transform.position = initialCameraTransform.position;
        transform.rotation = initialCameraTransform.rotation;
        transform.LookAt(initialTargetPosition);
        gameObject.GetComponent<Camera>().orthographicSize = initialOrtSize;
    }

    public void CameraProjectionChange()
    {
        if(!gameObject.GetComponent<Camera>().orthographic)
            gameObject.GetComponent<Camera>().orthographic = true;
        else
            gameObject.GetComponent<Camera>().orthographic = false;
    }

    public void ChangeBackground()
    {

        if(currentBackgrund == "original")
        {
            GetComponent<Camera>().backgroundColor = Color.black;
            currentBackgrund = "black";
        }
        else if (currentBackgrund == "black")
        {
            GetComponent<Camera>().backgroundColor = Color.red;
            currentBackgrund = "red";
        }
        else if (currentBackgrund == "red")
        {
            GetComponent<Camera>().backgroundColor = Color.green;
            currentBackgrund = "green";
        }
        else if (currentBackgrund == "green")
        {
            GetComponent<Camera>().backgroundColor = Color.blue;
            currentBackgrund = "blue";
        }
        else if (currentBackgrund == "blue")
        {
            GetComponent<Camera>().backgroundColor = originalBackColor;
            currentBackgrund = "original";
        }
    }
}
