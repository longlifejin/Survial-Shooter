using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public static readonly string moveV = "Vertical";
    public static readonly string moveH= "Horizontal";
    public static readonly string rotateName = "Mouse X";
    public static readonly string fireName = "Fire1";

    public float moveVertical { get; private set; }
    public float moveHorizontal { get; private set; }
    public float rotate { get; private set; }
    public bool fire { get; private set; }

    private void Update()
    {
        moveVertical = Input.GetAxis(moveV);
        moveHorizontal = Input.GetAxis(moveH);
        rotate = Input.GetAxis(rotateName);
        fire = Input.GetButton(fireName);
    }
}
