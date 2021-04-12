using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController3D : MonoBehaviour
{
    //deklarering av variablar s� jag kan se dem i Unity och buggfixa.
    public float acceleration;
    public Vector3 velocity;
    public Vector3 input;
    public float maxSpeed;
    private float startMaxSpeed;
    public float skinWidth;
    public LayerMask collisionMask;
    public RaycastHit hit;
    public float gravity;
    public float jumpPowerVariable;
    public float groundCheckDistance;
    public float deceleration;
    public float rotationX;
    public float rotationY;
    public float mouseSensitivity;
    public Camera camera;

    public Vector3 cameraOffset;
    public float staticFrictionCoefficient;
    public float kineticFrictionCoefficient;
    public float airResistance;
    public RaycastHit groundHit;
    public State[] States;
    public Vector3 point1;
    public Vector3 point2;
    public Vector3 jumpPower;
    public Vector3 gravityPower;
    public Collider[] collidingObjects;
    RaycastHit hitInfo3;

    //bugnet test
    float netRotationX = 0;
    public SphereCollider bugNet;
    float netRotationSpeed = -0.8f;
    Vector3 bugNetOffset = new Vector3(0, 4, 0);
    Vector3 bugNetStartOffset = new Vector3(1, 1, 0);

    bool netReady = true;
    bool netHolding = false;
    bool netSwipe = false;

    float timer = 0;


    private StateMachine StateMachine;
    public CapsuleCollider collider;
    void Awake() => collider = GetComponent<CapsuleCollider>();
   

    // Start is called before the first frame update
    void Start()
    {
        StateMachine = new StateMachine(this, States);
        jumpPower = Vector3.up * jumpPowerVariable;
        startMaxSpeed = maxSpeed;

    }

    // Update is called once per frame
    void Update()
    {

        //Variablar som beh�ver s�ttas varje update.
        point1 = transform.position + collider.center + Vector3.up * (collider.height / 2 - collider.radius);
        point2 = transform.position + collider.center + Vector3.down * (collider.height / 2 - collider.radius);

        //input och gravitation / hoppkraft
        gravityPower = Vector3.down * gravity * Time.deltaTime;
        velocity += gravityPower;

        //Debug.DrawLine(transform.position, transform.position + velocity, Color.red);
        //Debug.DrawLine(transform.position, camera.transform.position);
        /*
        if (Physics.SphereCast(transform.position, 2f,offset + camera.transform.position.normalized, out hitInfo3,offset.magnitude,collisionMask))
        {
            Debug.DrawLine(transform.position, hitInfo3.point, Color.red);
            camera.transform.position = hitInfo3.point + hitInfo3.normal * 2;

        }
        else
        {
            camera.transform.position = (offset + transform.position);
        }

        */


        // bugnet   offset funkar inte riktigt

        //Debug.Log(bugNetOffset);

        if (netReady)
        {
            Vector3 netOffset = bugNet.transform.rotation * bugNetStartOffset;


            bugNet.transform.rotation = Quaternion.Euler(netRotationX, camera.GetComponent<CameraFollowScript>().rotationY, 0);


            bugNet.transform.position = (netOffset + transform.position);

        }

        if (Input.GetMouseButtonDown(0))
        {
           
                netHolding = true;
           
                netReady = false;
            
        }

        if (netHolding)
        {
            Vector3 netOffset = bugNet.transform.rotation * bugNetOffset;


            bugNet.transform.rotation = Quaternion.Euler(netRotationX, camera.GetComponent<CameraFollowScript>().rotationY, 0);


            bugNet.transform.position = (netOffset + transform.position);

            maxSpeed = startMaxSpeed / 5;

            
                if (Input.GetMouseButtonUp(0))
                {
                    netSwipe = true;
                    netHolding = false;

                }
            
        }


        if (netSwipe)
        {

            maxSpeed = 0;
            Vector3 netOffset = bugNet.transform.rotation * bugNetOffset;

            netRotationX -= netRotationSpeed;

            netRotationX = Mathf.Clamp(netRotationX, 0, 90);


            bugNet.transform.rotation = Quaternion.Euler(netRotationX, camera.GetComponent<CameraFollowScript>().rotationY, 0);


            bugNet.transform.position = (netOffset + transform.position);


        }

        if (netRotationX >= 90)
        {
            if (WaitTime(0.5f))
            {
                netRotationX = 0;

                maxSpeed = startMaxSpeed;

                netReady = true;
                netSwipe = false;
            }


        }
        //

        //En array av alla object som min overlapcapsule returnerar, allts� de kolliderade med. 
        collidingObjects = Physics.OverlapCapsule(point1,
                            point2,
                            collider.radius, collisionMask);

        //om n�gonting kolliderades med s� checkar den att karakt�ren inte �ker igenom det. 
        if (!(collidingObjects.Length == 0))
        {
            PreventCollision(collidingObjects);
        }

        StateMachine.RunUpdate();
    }


   


    bool WaitTime(float seconds)
    {

        timer += Time.deltaTime;

        if (timer >= seconds)
        {
            timer = 0;
            return true;

        }

        return false;
    }

    //3 funktioner f�r att hantera acceleration
    public void AccelerateOrDecelerate(Vector3 input)
    {
        if (input.magnitude != 0)
            Accelerate(input);
        else
            Decelerate();
    }

    void Accelerate(Vector3 input)
    {

        velocity += input * acceleration * Time.deltaTime;

        if (velocity.magnitude > maxSpeed)
        {
            velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
            //velocity += velocity.normalized* maxSpeed * Time.deltaTime;
        }

    }

    void Decelerate()
    {

        if (deceleration > Mathf.Abs(velocity.x))
        {
            velocity.x = 0;
        }

        if (deceleration > Mathf.Abs(velocity.y))
        {
            velocity.y = 0;
        }



        Vector3 projection = new Vector3(velocity.x, 0.0f, velocity.z).normalized;
        velocity -= projection * deceleration * Time.deltaTime;


    }

    //kalkylerar normalkraften med hj�lp av normalen fr�n overlapcapsule-kollisionerna.
    Vector3 CalculateNormalForce(Vector3 velocity, Vector3 normal)
    {
        if (Vector3.Dot(velocity, normal) > 0)
        {
            return Vector3.zero;
        }
        else
        {
            Vector3 projection = Vector3.Dot(velocity, normal) * normal;
            return -projection;
        }

    }

    //applicerar friktion p� karakt�ren.
    void ApplyFriction(Vector3 normalForce)
    {

        if (velocity.magnitude < normalForce.magnitude * staticFrictionCoefficient)
        {
            velocity = Vector3.zero;
        }
        else
        {
            velocity -= velocity.normalized * normalForce.magnitude *
                kineticFrictionCoefficient;
        }


    }

    //ser till att karakt�ren inte �ker igenom n�got, tvingar den att stanna och d�dar dens momentum vid kontakt.
    public void PreventCollision(Collider[] collidingObjects)
    {

        Vector3 separationVector;
        foreach (Collider col in collidingObjects)
        {

            Physics.ComputePenetration(collider, transform.position, transform.rotation, col, col.transform.position, col.transform.rotation, out separationVector, out float distance);

            velocity += separationVector.normalized * skinWidth;

            Vector3 normalForce = CalculateNormalForce(velocity, separationVector.normalized);
            ApplyFriction(normalForce);

            velocity += normalForce;
        }


        /*
        transform.position +=
        separationVector + separationVector.normalized * skinWidth;
        velocity += CalculateNormalForce(velocity, separationVector.normalized);
        */

    }
}
