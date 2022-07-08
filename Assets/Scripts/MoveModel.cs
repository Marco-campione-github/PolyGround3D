using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveModel : MonoBehaviour
{
    //Initial transform values
    private Vector3 initialModelPosition;
    private Quaternion initialModelRotation;

    //Settings
    private float touchSensitivity; // {0.1; 1}
    private float zoomSensitivity; // {0.1; 1}

    //GameObjects
    public GameObject xAxisIndicator;
    public GameObject yAxisIndicator;
    public GameObject zAxisIndicator;

    private void Start()
    {
        //Save initial transform
        initialModelPosition = transform.position;
        initialModelRotation = transform.rotation;

        //Get player settings and adjust them to make them useable
        PlayerSettings data = SaveSystem.LoadSettings();
        touchSensitivity = data.touchSensitivity / 5f; // {0,02; 0.2}
        zoomSensitivity = data.zoomSensitivity / 200f; // {0,0005; 0,005}

    }
    void LateUpdate()
    {
        if(!UIManager.arePanelsOpen)
            TouchManager();
    }

    private void TouchManager()
    {
        
        //Rotation of the model around the center point
        if (Input.touchCount == 1 && !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                transform.RotateAround(transform.position, Vector3.down,
                    touch.deltaPosition.x * touchSensitivity);
                transform.RotateAround(transform.position, Vector3.right,
                    touch.deltaPosition.y * touchSensitivity);
            }
            DrawAxis();
        }

        
        if (Input.touchCount == 2 &&
            !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) &&
            !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(1).fingerId))
        {
            //Get Input 
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);
            Vector2 touchZeroPrevPosition = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPosition = touchOne.position - touchOne.deltaPosition;

            //Movement of the model in the world space
            Vector2 zeroMovement = touchZeroPrevPosition - touchZero.position;
            Vector2 oneMovement = touchOnePrevPosition - touchOne.position;
            Vector2 movement = (zeroMovement + oneMovement) / 2 * (touchSensitivity / 40f);
            transform.position = transform.position - new Vector3(movement.x, movement.y, 0);

            //Scaling of the model by pinching with two fingers
            float prevMagnitude = (touchZeroPrevPosition - touchOnePrevPosition).magnitude;
            float currMagnitude = (touchZero.position - touchOne.position).magnitude;
            float difference = (currMagnitude - prevMagnitude) * zoomSensitivity;
            transform.localScale = new Vector3(
                Mathf.Clamp(gameObject.transform.localScale.x + difference, 0.5f, 10f),
                Mathf.Clamp(gameObject.transform.localScale.y + difference, 0.5f, 10f),
                Mathf.Clamp(gameObject.transform.localScale.z + difference, 0.5f, 10f));

            DrawAxis();
        }

        //Rotation with 2 fingers to rotate around the axis that goes into the screens
        if (Input.touchCount == 2 &&
            !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) &&
            !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(1).fingerId))
        {
            Touch touchZero = Input.GetTouch(0);
            switch (touchZero.phase)
            {
                case TouchPhase.Stationary:
                    break;
                case TouchPhase.Moved:
                    break;
                default:
                    return;
            }

            Touch touchOne = Input.GetTouch(1);
            switch (touchOne.phase)
            {
                case TouchPhase.Stationary:
                    break;
                case TouchPhase.Moved:
                    break;
                default:
                    return;
            }

            Vector2 pos1 = touchZero.position;
            Vector2 pos2 = touchOne.position;
            Vector2 pos1b = touchZero.position - touchZero.deltaPosition;
            Vector2 pos2b = touchOne.position - touchOne.deltaPosition;

            transform.RotateAround(transform.position, Vector3.forward,
                Vector3.SignedAngle(pos2b - pos1b, pos2 - pos1, Vector3.forward));
            
        }

    }

    //Reset centerpoint position, rotation and scaling.
    public void ResetPosition()
    {
        transform.position = initialModelPosition;
        transform.rotation = initialModelRotation;
        transform.localScale = new Vector3(1f, 1f, 1f);
    }

    //Tells each AxisIndicator to draw the axis
    private void DrawAxis()
    {
        xAxisIndicator.GetComponent<AxisIndicator>().SetDrawingAxis();
        yAxisIndicator.GetComponent<AxisIndicator>().SetDrawingAxis();
        zAxisIndicator.GetComponent<AxisIndicator>().SetDrawingAxis();
    }
}
