using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Gun
{
    public int maxAmmo;
    public int currentAmmo;
    public float fireRate;
    public float reloadTime;
    public float hitForce;
    public int hitDamage;
    public float distance;
    public LayerMask mask;
    public SoundPlayer sound;
    public Text balas;

    public bool canShot;
    public bool reloading;

    public Animator animacion;
    private MonoBehaviour mono;

    public void Initialize(MonoBehaviour m)
    {
        mono = m;
        canShot = true;
        reloading = false;
        currentAmmo = maxAmmo;
        balas.text = currentAmmo.ToString();
    }

    public void Shot()
    {
        if (!canShot && !reloading) return;
        if (currentAmmo <= 0) return;

        sound.Play(0, 1);
        canShot = false;
        currentAmmo--;
        balas.text = currentAmmo.ToString();
        Debug.Log("Dispare");
        animacion.SetTrigger("Shot");
        RaycastHit hit = new RaycastHit();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, distance, mask, QueryTriggerInteraction.Collide))
        {
            //Debug.Log("Le di");
            //Debug.Log(hit.transform.name);
            //Debug.DrawRay (ray.origin, ray.direction * hit.distance, Color.red, 1);
            if (hit.transform.tag == "Enemy")
            {
                //hit.rigidbody.AddForce(ray.direction * hitForce, ForceMode.Impulse);
                //Debug.Log(hit.collider.name + "CHACHO DADO");
                hit.transform.GetComponent<CrawlerBehaviour>().Dmg(hitDamage);
            }
        }

        mono.StartCoroutine(WaitFireRate());
    }

    public void Reload()
    {
        if (reloading) return;

        reloading = true;
        animacion.SetTrigger("Record");
        mono.StartCoroutine(Reloading());
    }



    IEnumerator WaitFireRate()
    {
        //Debug.Log ("CORUTINA");
        /*float timeCounter = 0;
		while (timeCounter < fireRate) 
		{
			timeCounter += Time.deltaTime;
		}
		canShot = true;*/

        //Debug.Log("Empieza corutina");
        yield return new WaitForSeconds(fireRate);
        //Debug.Log("Termina corutina");
        canShot = true;
    }

    IEnumerator Reloading()
    {
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        reloading = false;
    }
}
