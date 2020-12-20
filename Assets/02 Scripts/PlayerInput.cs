using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public string moveAxisName = "Vertical";
    public string rotateAxisName = "Horizontal";
    public string fireButtonName = "Fire1";
    public string reloadButtonName = "Reload";
    //public string moveSideName = "Horizontal";

    public float move { get; private set; }
    public float rotate { get; private set; }
    public bool fire { get; private set; }
    public bool reload { get; private set; }
    //public float moveSide { get; private set; }

    void Start()
    {
        
    }

    void Update()
    {
        /*if(GameManager.instance.isGameover )
        {
            move = 0;
            rotate = 0f;
             moveSide = 0f;
            reload = false;
            fire = false;
            return;
        }*/
        move = Input.GetAxis(moveAxisName);
        //rotate = Input.GetAxis(rotateAxisName);
        fire = Input.GetButton(fireButtonName);
        reload = Input.GetButtonDown(reloadButtonName);
        //moveSide = Input.GetAxis(moveSideName);
    }
}
