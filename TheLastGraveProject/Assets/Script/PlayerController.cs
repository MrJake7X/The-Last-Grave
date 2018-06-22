using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Vector2 axis;
    private CharacterController controller;

    private Vector3 moveDirection;
    public float speed;
    private float forceToGround = Physics.gravity.y;
    public float gravityMagnitude;

    private bool jump;
    public float jumpSpeed;
    private SoundPlayer sound;
    public UIControler hud;

    [Header("Life System")]
    public int life;
    public Slider sliderLife;
    public int damageTaked;
    private int damageCounter = 0;
    private bool isAttacked; 

    [Header("View System")]
    public float distance;
    public LayerMask mask;
    public bool angelDetected;
    public CrawlerAngel angel;

    [Header("GUN")]
    public Gun gun;
    public Animator gunAnim;

    [Header("Farolillo")]
    public Animator farolAnim;

    public Animator camaraAnim;

    // Use this for initialization
    void Start ()
    {
        angelDetected = false;
        controller = GetComponent<CharacterController>();
        sound = GetComponentInChildren<SoundPlayer>();
        gun.Initialize(this);
    }
	
	// Update is called once per frame
	void Update ()
    {
       
        if (controller.isGrounded && !jump)//Dice si el controler esta tocando el suelo
        {
            moveDirection.y = forceToGround;
        }
        else
        {
            jump = false;
            moveDirection.y += Physics.gravity.y * gravityMagnitude * Time.deltaTime;
        }
        //transforma el movimiento del moundo al del local
        Vector3 tranformDirection = axis.x * transform.right + axis.y * transform.forward;

        
        moveDirection.x = tranformDirection.x * speed;
        moveDirection.z = tranformDirection.z * speed;
        controller.Move(moveDirection * Time.deltaTime);//Mueve el controller

        if(moveDirection.x > 0 ||moveDirection.z > 0)
        {
            farolAnim.SetBool("isWalking", true);
            gunAnim.SetBool("isWalking", true);
            camaraAnim.SetBool("isWalking", true);
        }
        else
        {
            farolAnim.SetBool("isWalking", false);
            gunAnim.SetBool("isWalking", false);
            camaraAnim.SetBool("isWalking", false);
        }

        //Update Slider Player Life
        if (isAttacked)
        {
            UpdateLife();
        }
        sliderLife.value = life;
	}
    private void FixedUpdate()
    {
        RaycastHit hit = new RaycastHit();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, distance, mask))
        {
            //Debug.Log("Le di");
            //Debug.Log(hit.transform.name);
            Debug.DrawRay (ray.origin, ray.direction * hit.distance, Color.red, 1);
            if (hit.transform.name == "Angel")
            {
                if(angelDetected == false)
                {
                    angelDetected = true;
                    Debug.Log("1");
                    angel = hit.transform.GetComponent<CrawlerAngel>();
                }
                Debug.Log("2");
                hit.transform.GetComponent<CrawlerAngel>().SetDetected(true);
            }
            else
            {
                Debug.Log("3");
                if (angelDetected == true)
                {
                    Debug.Log("4");
                    angel.SetDetected(false);
                    angelDetected = false;
                }
            }
        }
    }

    public void SetAxis(Vector2 naxis)
    {
        axis = naxis;
    }

    public void StartJump()
    {
        if(!controller.isGrounded) return;
        jump = true;
        moveDirection.y = jumpSpeed;
    }

    public void TryShot()
    {
        //Pueden haber situaciones en las que no se pueda disparar.
        gun.Shot();
    }
    public void TryReload()
    {
        //gun.Reload();
    }
    public void Dmg(int dmg)
    {
        if(life <= 0)
        {
            Dead();
        }
        damageTaked = dmg;
        isAttacked = true;
        //Debug.Log(life);
    }

    public void UpdateLife()
    {
        if (damageCounter < damageTaked)
        {
            life --;
            damageCounter++;
        }
        else
        {
            damageCounter = 0;
            isAttacked = false;
        }
    }

    public void Dead()
    {
        hud.OpenLosePanel();
        Debug.Log("MUELTO");
    }
}
