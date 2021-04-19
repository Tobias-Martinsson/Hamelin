using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController3D : MonoBehaviour
{
    //deklarering av variablar s� jag kan se dem i Unity och buggfixa.
    public float acceleration;
    public Vector3 velocity;
    private Vector3 inputX;
    private Vector3 inputZ;
    private Vector3 inputCameraAdjust;
    public float maxSpeedXZ;
    private float startMaxSpeedXZ;
    public float maxSpeedY;
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

    //

    //Grapple Test

    public float grapplingSpeed = 0.5f;
    public float maxGrapplingSpeed = 20.0f;
    public float hookDistanceStop = 4f;
    public GameObject hookPrefab;
    public Transform shootLocation;

    Hook hook;
    bool pulling;
    Rigidbody rigid;
    //


    private StateMachine StateMachine;
    public CapsuleCollider collider;
    void Awake() => collider = GetComponent<CapsuleCollider>();


    // Start is called before the first frame update
    void Start()
    {
        //grappling hook test
        rigid = GetComponent<Rigidbody>();
        pulling = false;
        //

        StateMachine = new StateMachine(this, States);

        startMaxSpeedXZ = maxSpeedXZ;
        Cursor.lockState = CursorLockMode.Confined;


    }

    // Update is called once per frame
    void Update()
    {
      
        //Points på spelaren
        point1 = transform.position + collider.center + Vector3.up * (collider.height / 2 - collider.radius);
        point2 = transform.position + collider.center + Vector3.down * (collider.height / 2 - collider.radius);

        //Update  velocity
        jumpPower = Vector3.up * jumpPowerVariable;
        gravityPower = Vector3.down * gravity * Time.deltaTime;

        inputX = (Vector3.right * Input.GetAxisRaw("Horizontal")) * acceleration * Time.deltaTime;
        inputZ = (Vector3.forward * Input.GetAxis("Vertical")) * acceleration * Time.deltaTime;



        inputX = (Quaternion.Euler(0, rotationY, 0)) * inputX;
        inputZ = (Quaternion.Euler(0, rotationY, 0)) * inputZ;
        Vector3.ProjectOnPlane(inputZ, GroundNormal(point2));



        velocity += inputX;
        velocity += inputZ;

        velocity += gravityPower;



        //Kollision
        collidingObjects = Physics.OverlapCapsule(point1,
                            point2,
                            collider.radius, collisionMask);

        if (!(collidingObjects.Length == 0))
        {
            PreventCollision(collidingObjects);
        }

        transform.position += velocity;



        //hämtad kamera rotation
        rotationY = camera.GetComponent<CameraFollowScript>().rotationY;
        rotationX = camera.GetComponent<CameraFollowScript>().rotationX;


        // Grappling hook 



        shootLocation.transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);

        if (hook == null && Input.GetKeyDown(KeyCode.G))
        {
            StopAllCoroutines();
            pulling = false;
            hook = Instantiate(hookPrefab, shootLocation.position, Quaternion.identity).GetComponent<Hook>();
            hook.Initialize(this, shootLocation);
            StartCoroutine(DestroyHookAfterLifetime());
        }
        //else if (hook != null && Input.GetMouseButtonDown(1))
        else if (hook != null && Input.GetKeyDown(KeyCode.G))
        {
            DestroyHook();
        }
        if (pulling && hook != null)
        {
            Debug.Log("pulling");
            if (Vector3.Distance(transform.position, hook.transform.position) <= hookDistanceStop)
            {
                DestroyHook();
            }
            else
            {

                Vector3 newVector = (hook.transform.position - transform.position).normalized * grapplingSpeed;
                velocity += newVector;

                if (velocity.magnitude > maxGrapplingSpeed)
                {
                    velocity = Vector3.ClampMagnitude(velocity, maxGrapplingSpeed);
                }

            }
        }
        else
        {
            Debug.Log("not pulling");
        }

        //




        if (netReady)
        {
            Vector3 netOffset = bugNet.transform.rotation * bugNetStartOffset;


            bugNet.transform.rotation = Quaternion.Euler(netRotationX, rotationY, 0);


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


            bugNet.transform.rotation = Quaternion.Euler(netRotationX, rotationY, 0);


            bugNet.transform.position = (netOffset + transform.position);

            maxSpeedXZ = startMaxSpeedXZ / 5;


            if (Input.GetMouseButtonUp(0))
            {
                netSwipe = true;
                netHolding = false;
            }

        }


        if (netSwipe)
        {

            maxSpeedXZ = 0;
            Vector3 netOffset = bugNet.transform.rotation * bugNetOffset;

            netRotationX -= netRotationSpeed;

            netRotationX = Mathf.Clamp(netRotationX, 0, 90);


            bugNet.transform.rotation = Quaternion.Euler(netRotationX, rotationY, 0);


            bugNet.transform.position = (netOffset + transform.position);


        }

        if (netRotationX >= 90)
        {
            if (WaitTime(0.5f))
            {
                netRotationX = 0;

                maxSpeedXZ = startMaxSpeedXZ;

                netReady = true;
                netSwipe = false;
            }


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





    void ApplyAirResistance()
    {

        //airResistance
        velocity *= Mathf.Pow(airResistance, Time.deltaTime);

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

    
    public bool GroundCheck(Vector3 point2)
    {

        return Physics.Raycast(point2, Vector3.down, collider.radius + skinWidth + groundCheckDistance, collisionMask);

    }
    


    Vector3 GroundNormal(Vector3 point2)
    {
        RaycastHit hit;
        Physics.Raycast(point2, velocity.normalized, out hit, collider.radius + skinWidth + groundCheckDistance, collisionMask);
        return hit.normal;
    }


    //ser till att karakt�ren inte �ker igenom n�got, tvingar den att stanna och d�dar dens momentum vid kontakt.
    public void PreventCollision(Collider[] collidingObjects)
    {


        Vector3 separationVector;
        foreach (Collider col in collidingObjects)
        {

            Physics.ComputePenetration(collider, transform.position, transform.rotation, col, col.transform.position, col.transform.rotation, out separationVector, out float distance);


            //  velocity += separationVector.normalized * skinWidth;
            Vector3 normalForce = CalculateNormalForce(velocity, separationVector.normalized);
            velocity += normalForce;

            ApplyFriction(normalForce);
            ApplyAirResistance();

        }


        /*
        transform.position +=
        separationVector + separationVector.normalized * skinWidth;
        velocity += CalculateNormalForce(velocity, separationVector.normalized);
        */

    }

    public void StartPull()
    {
        pulling = true;
    }

    private void DestroyHook()
    {
        if (hook == null) return;

        pulling = false;
        Destroy(hook.gameObject);
        hook = null;
    }

    private IEnumerator DestroyHookAfterLifetime()
    {
        yield return new WaitForSeconds(8f);

        DestroyHook();
    }
}
