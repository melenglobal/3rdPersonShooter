using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Vector2 moveDirection;
    Vector2 lookDirection;
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float maxForwardSpeed = 8f;
    [SerializeField] float turnSpeed = 100f;
    float desiredSpeed;
    float forwardSpeed;

    const float groundAccel = 5f;
    const float groundDecel = 25f;
    Animator _anim;

    public bool isDead = false;
    
    bool isMoveInput
    {
        get 
        {
            return !Mathf.Approximately(moveDirection.sqrMagnitude, 0f);
        }
    }

    [SerializeField] Transform weapon;
    [SerializeField] Transform hand;
    [SerializeField] Transform hip;

    //public void PickUpGun()
    //{
    //    weapon.SetParent(hand);
    //    weapon.localPosition = new Vector3(0.2088f, 0.4167f, -0.1280f);
    //    weapon.localRotation = Quaternion.Euler(121.50f, 139.84f, 139.55f);
    //    weapon.localScale = new Vector3(1f, 1f, 1f);

    //}

    //public void PutDownGun()
    //{
    //    weapon.SetParent(hip);
    //    weapon.localPosition = new Vector3(0.2088f, 0.4167f, -0.1280f);
    //    weapon.localRotation = Quaternion.Euler(121.50f, 139.84f, 139.55f);
    //    weapon.localScale = new Vector3(1f, 1f, 1f);
    //}
    
    public void OnMove(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>();
    }

    bool firing = false;

    // Press to Q key from your keyboard for Fire
    public void OnFire(InputAction.CallbackContext context)
    {
        firing = false;
        if ((int)context.ReadValue<float>()==1 && _anim.GetBool("Armed"));
      
        _anim.SetTrigger("Fire");
        firing = true;
    }
    public void OnArmed(InputAction.CallbackContext context)
    {
        _anim.SetBool("Armed", !_anim.GetBool("Armed") );
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        lookDirection = context.ReadValue<Vector2>();
    }

    void Move(Vector2 direction)
    {
        float turnAmount = direction.x;
        float fDirection = direction.y;
        if (direction.sqrMagnitude > 1f)
        {
            direction.Normalize();
        }
            desiredSpeed = direction.magnitude * maxForwardSpeed * Mathf.Sign(fDirection);

            float acceleration = isMoveInput ? groundAccel : groundDecel;

            forwardSpeed = Mathf.MoveTowards(forwardSpeed, desiredSpeed, acceleration * Time.deltaTime);

            _anim.SetFloat("ForwardSpeed", forwardSpeed);

            transform.Rotate(0,turnAmount * turnSpeed * Time.deltaTime, 0);

            transform.Translate(direction.x * moveSpeed * Time.deltaTime, 0, direction.y * moveSpeed * Time.deltaTime);
    }

    // Start is called before the first frame update
    void Start()
    {
           _anim = this.GetComponent<Animator>();
    }
    public GameObject crosshair;
    public LineRenderer laser;
    public GameObject crossLight;
    // Update is called once per frame
    void Update()
    {
        if (_anim.GetBool("Armed"))
        {
            laser.gameObject.SetActive(true);
            //crosshair.gameObject.SetActive(true);
            RaycastHit laserHit;
            Ray laserRay = new Ray(laser.transform.position, laser.transform.forward);

            if (Physics.Raycast(laserRay, out laserHit))
            {
                laser.SetPosition(1, laser.transform.InverseTransformPoint(laserHit.point));
                Vector3 crosshairLocation = Camera.main.WorldToScreenPoint(laserHit.point);
                //crosshair.transform.position = crosshairLocation;
                crossLight.transform.localPosition = new Vector3(0,0,laser.GetPosition(1).z *0.9f);
                if (firing && laserHit.collider.gameObject.tag == "Orb")
                {
                    laserHit.collider.gameObject.GetComponent<AIController>().BlowUp();
                }
            }
            else
            {
                //crosshair.gameObject.SetActive(false);
            }
        }
        else {
            //crosshair.gameObject.SetActive(false);
            laser.gameObject.SetActive(false);
        }
            

        Move(moveDirection);
    }

    public Transform spine;
    Vector2 lastLookDirection;
    float xSensivity = 0.5f;
    float ySensivity = 0.5f;

    void LateUpdate()
    {
        lastLookDirection += new Vector2(-lookDirection.y*ySensivity, lookDirection.x*xSensivity);
        lastLookDirection.x = Mathf.Clamp(lastLookDirection.x, -30, 30);
        lastLookDirection.y = Mathf.Clamp(lastLookDirection.y, -30, 60);

        spine.localEulerAngles = lastLookDirection;
    }


}
