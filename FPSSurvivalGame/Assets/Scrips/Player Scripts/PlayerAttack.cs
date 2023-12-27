using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    private WeaponManager weapon_Manager;

    public float fireRate = 15f;
    private float nextTimeToFire;
    public float damage = 20f;

    private Animator zoomCameraAnim;
    private bool zoomed;

    private Camera mainCam;

    private GameObject crosshair;

    private bool is_Aiming;

    private Transform lookRoot;

    private void Awake()
    {
        weapon_Manager = GetComponent<WeaponManager>();

        lookRoot = transform.GetChild(0);
        zoomCameraAnim = lookRoot.GetChild(0).GetComponent<Animator>();

        //zoomCameraAnim = transform.Find(Tags.LOOK_ROOT).transform.Find(Tags.ZOOM_CAMERA).GetComponent<Animator>();

        crosshair = GameObject.FindWithTag(Tags.CROSSHAIR);

        mainCam = Camera.main;
    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        WeaponShoot();
        ZoomInAndOut();
    }

    void WeaponShoot()
    {
        //assult rifle
        if (weapon_Manager.GetCurrentSelectedWeapon().fireType == WeaponFireType.MULTIPLE)
        {
            if(Input.GetMouseButton(0) && Time.time > nextTimeToFire)
            {
                nextTimeToFire = Time.time + 1f / fireRate;

                weapon_Manager.GetCurrentSelectedWeapon().ShootAnimation();


            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                //axe
                if (weapon_Manager.GetCurrentSelectedWeapon().tag == Tags.AXE_TAG)
                {
                    weapon_Manager.GetCurrentSelectedWeapon().ShootAnimation();
                }

                // bullettype is bullet weapons
                if (weapon_Manager.GetCurrentSelectedWeapon().bulletType == WeaponBulletType.BULLET)
                {
                    weapon_Manager.GetCurrentSelectedWeapon().ShootAnimation();


                }

                // arrow and spear
                else
                {
                    if (is_Aiming)
                    {
                        weapon_Manager.GetCurrentSelectedWeapon().ShootAnimation();

                        if(weapon_Manager.GetCurrentSelectedWeapon().bulletType == WeaponBulletType.ARROW)
                        {

                        }

                        else if (weapon_Manager.GetCurrentSelectedWeapon().bulletType == WeaponBulletType.SPEAR)
                        {

                        }
                    } 
                }
            }
        }
    } // weapon shoot func

    void ZoomInAndOut()
    {
        if (weapon_Manager.GetCurrentSelectedWeapon().weapon_Aim == WeaponAim.AIM)
        {
            if (Input.GetMouseButtonDown(1))
            {
                zoomCameraAnim.Play(AnimationTags.ZOOM_IN_ANIM);

                crosshair.SetActive(false);
            }

            if (Input.GetMouseButtonUp(1))
            {
                zoomCameraAnim.Play(AnimationTags.ZOOM_OUT_ANIM);

                crosshair.SetActive(true);
            }
        }

        if(weapon_Manager.GetCurrentSelectedWeapon().weapon_Aim  == WeaponAim.SELF_AIM)
        {
            if (Input.GetMouseButtonDown(1))
            {
                weapon_Manager.GetCurrentSelectedWeapon().Aim(true);

                is_Aiming = true;
            }

            if (Input.GetMouseButtonUp(1))
            {
                weapon_Manager.GetCurrentSelectedWeapon().Aim(false);

                is_Aiming = false;
            }

        }
    }// zoom in and out func
}
