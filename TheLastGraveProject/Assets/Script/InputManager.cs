using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private PlayerController PC;
    private LookRotation lookRotation;

    private float sensitivity = 3.0f;

    private MouseCursor mouse;
    public UIControler hud;

    // Use this for initialization
    void Start ()
    {
        PC = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        lookRotation = PC.GetComponent<LookRotation>();

        mouse = new MouseCursor();
        mouse.Hide();
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector2 inputAxis = Vector2.zero;
        //Pause
        if (hud.paused || hud.winLose)
        {
            mouse.Show();
            inputAxis.x = 0;
            inputAxis.y = 0;
            lookRotation.SetMouseAxis(Vector2.zero);

            PC.SetAxis(inputAxis);
            if (hud.paused)if (Input.GetKeyDown(KeyCode.P)) hud.ClosePausePanel();
        }
        else
        {
            mouse.Hide();
            if (Input.GetKeyDown(KeyCode.P)) hud.OpenPausePanel();
            //Player
            

            inputAxis.x = Input.GetAxis("Horizontal");
            inputAxis.y = Input.GetAxis("Vertical");

            PC.SetAxis(inputAxis);
            //Jump
            /* if(Input.GetButton("Jump"))
             {
                 PC.StartJump();
             }*/

            //Mouse
            Vector2 mouseAxis = Vector2.zero;
            mouseAxis.x = Input.GetAxis("Mouse X") * sensitivity;
            mouseAxis.y = Input.GetAxis("Mouse Y") * sensitivity; //Recogemos el Y para mover el X de la camara;
            lookRotation.SetMouseAxis(mouseAxis);

            if (Input.GetMouseButtonDown(0)) mouse.Hide();
            if (Input.GetKeyDown(KeyCode.Escape)) mouse.Hide();


            //DISPARO
            if (Input.GetButton("Fire1")) PC.TryShot();


            if (Input.GetMouseButtonDown(0)) mouse.Hide();
            if (Input.GetKeyDown(KeyCode.Escape)) mouse.Show();
        }
        
    }
}
