using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Xml.Schema;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private const float Lane_Distance = 2.0f;
    private const float Turn_Speed = 0.05f;

    //
    private bool isRunning = false;

    //Animationen
    private Animator anim;

    //Movement variablen
    private CharacterController cc;   
    public float jumpForce = 4.0f; //Sprungkraft   
    private float gravity = 12.0f; //Gravitation    
    private float verticalVelocity; //vertikale Geschwindigkeit       
    //Die Linien wo sich der Spieler befindet
    private int desiredLane = 1; // 0 = Links, 1 = Mitte, 2 = Rechts

    //Speed modifier
    private float originalSpeed = 7.0f;
    private float speed; 
    private float speedIncreaseLastTick;
    private float speedIncreaseTime = 2.5f;
    private float speedIncreaseAmount = 0.1f;

    private void Start()
    {
        speed = originalSpeed;
        cc = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!isRunning)
            return;

        if(Time.time - speedIncreaseLastTick > speedIncreaseTime)
        {
            speedIncreaseLastTick = Time.time;
            speed += speedIncreaseAmount;
            GameManager.Instance.UpdateModifier(speed - originalSpeed);

        }

        //Inputs die zeigen auf welcher Linie der Spieler sein muss
        if (MobileInputs.Instance.SwipeLeft) //Nach Links
            MoveLane(false);
        if (MobileInputs.Instance.SwipeRight) //Nach Rechts
            MoveLane(true);

        //Berechnungen die zeigen auf welcher Linie wir in der Zukunft sein werden
        Vector3 targetPosition = transform.position.z * Vector3.forward;
        if (desiredLane == 0)
            targetPosition += Vector3.left * Lane_Distance;
        else if (desiredLane == 2)
            targetPosition += Vector3.right * Lane_Distance;

        //Bewegungsdelta berechnen
        Vector3 moveVector = Vector3.zero;
        moveVector.x = (targetPosition - transform.position).normalized.x * speed;

        bool IsGrounded = isGrounded(); 
            anim.SetBool("Grounded",IsGrounded);

        // Y berechnen
        if (isGrounded())
        {
            verticalVelocity = -0.1f;
           

            if (MobileInputs.Instance.SwipeUp)
            {
                //Springen
                anim.SetTrigger("Jump");
                verticalVelocity = jumpForce;
            }
            else if (MobileInputs.Instance.SwipeDown)
            {
                //Slide
                StartSliding();
                Invoke("StopSliding", 1.0f);
            }
        }
        else
        {
            verticalVelocity -= (gravity * Time.deltaTime);

            //Schnelles runterfallen 
            if (MobileInputs.Instance.SwipeDown)
            {
                verticalVelocity = -jumpForce;
            }
        }

        moveVector.y = verticalVelocity;
        moveVector.z = speed;

        //Den Spieler bewegen
        cc.Move(moveVector * Time.deltaTime);

        //Den Spieler drehen, wo er hingeht
        Vector3 dir = cc.velocity;
        if(dir != Vector3.zero)
        {
            dir.y = 0;
            transform.forward = Vector3.Lerp(transform.forward, dir, Turn_Speed);
        }
        
            
    }

    private void StartSliding()
    {
        anim.SetBool("Sliding", true);
        cc.height /= 2;
        cc.center = new Vector3(cc.center.x, cc.center.y / 2, cc.center.z);
    }

    private void StopSliding()
    {
        anim.SetBool("Sliding", false);
        cc.height *= 2;
        cc.center = new Vector3(cc.center.x, cc.center.y * 2, cc.center.z);
    }

    private void MoveLane(bool goingRight)
    {
        desiredLane += (goingRight) ? 1 : -1;
        desiredLane = Mathf.Clamp(desiredLane, 0, 2);
    }

    private bool isGrounded()
    {
        Ray groundRay = new Ray(new Vector3(cc.bounds.center.x,(cc.bounds.center.y - cc.bounds.extents.y) + 0.2f,cc.bounds.center.z),Vector3.down);
        Debug.DrawRay(groundRay.origin, groundRay.direction, Color.cyan,1.0f);

        return Physics.Raycast(groundRay, 0.2f + 0.1f);
           
    }

    public void StartRunning()
    {
        isRunning = true;
    }

    private void Crash()
    {
        anim.SetTrigger("Death");
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        switch (hit.gameObject.tag)
        {
            case "Obstacle":
                Crash();
            break;
        }
    }
}
