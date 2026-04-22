using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



public class MobileJoystick : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private RectTransform joystickOutline;
    [SerializeField] private RectTransform joystickKnob;


    [Header("Settings")]
    [SerializeField] private float moveFactor;
    private bool canControl;
    private Vector3 clickedPosition;
    private Vector3 move;

    // Start is called before the first frame update
    void Start()
    {
        HideJoystick();
    }

    // Update is called once per frame
    void Update()
    {
        if (canControl)
            ControlJoystick();
    }

    public void ClickedOnJoystickZoneCallBack()
    {
        clickedPosition = Input.mousePosition;
        joystickOutline.position = clickedPosition;

        ShowJoystick();
    }

    private void ShowJoystick()
    {
        joystickOutline.gameObject.SetActive(true);

        canControl = true;
    }
    private void HideJoystick()
    {
        joystickOutline.gameObject.SetActive(false);
        canControl = false;

        move = Vector3.zero;
    }

    private void ControlJoystick()
    {
        Vector3 currentPosition = Input.mousePosition;
        Vector3 direction = currentPosition - clickedPosition;

        joystickKnob.position = joystickOutline.position+direction;

        float moveMagnitude = direction.magnitude*moveFactor /Screen.width;

        moveMagnitude =Mathf.Min(moveMagnitude,joystickOutline.rect.width/2);

        move =direction.normalized*moveMagnitude;

        Vector3 targetPosition = clickedPosition + move;

        joystickKnob.position = targetPosition;

        if (Input.GetMouseButtonUp(0))
            HideJoystick();
    }
    public Vector3 GetMoveVector()
    {
        return move;
    }
}
