using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController3D : MonoBehaviour
{
    //deklarering av variablar så jag kan se dem i Unity och buggfixa.
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
    private Vector3 jumpPower;
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


    }

    // Update is called once per frame
    void Update()
    {
        jumpPower = Vector3.up * jumpPowerVariable;
        gravityPower = Vector3.down * gravity * Time.deltaTime;



        //Variablar som behöver sättas varje update.
        point1 = transform.position + collider.center + Vector3.up * (collider.height / 2 - collider.radius);
        point2 = transform.position + collider.center + Vector3.down * (collider.height / 2 - collider.radius);

        //input och gravitation / hoppkraft
        //input = Vector3.right * Input.GetAxisRaw("Horizontal") + Vector3.forward * Input.GetAxisRaw("Vertical");


        //förflyttning av kameran. Bara fått det att fungera någolunda med en dynamisk kamera, men har problem att raycasta mot föremål jag nuddar. 

        rotationX -= Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
        rotationY += Input.GetAxisRaw("Mouse X") * mouseSensitivity;
        Vector3 offset = camera.transform.rotation * cameraOffset;
        rotationX = Mathf.Clamp(rotationX, -90, 90);


        camera.transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);
        shootLocation.transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);

        Debug.DrawLine(transform.position, transform.position + velocity, Color.red);
        Debug.DrawLine(transform.position, camera.transform.position);
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
        //kommentera ut det ovanför om det behövs testa med en simpel kamera. 
        camera.transform.position = (offset + transform.position);





        // bugnet   offset funkar inte riktigt

        //Debug.Log(bugNetOffset);

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

        inputX = (Vector3.right * Input.GetAxisRaw("Horizontal")) * acceleration * Time.deltaTime;
        inputZ = (Vector3.forward * Input.GetAxis("Vertical")) * acceleration * Time.deltaTime;



        inputX = (Quaternion.Euler(0, rotationY, 0)) * inputX;
        inputZ = (Quaternion.Euler(0, rotationY, 0)) * inputZ;
        Vector3.ProjectOnPlane(inputZ, GroundNormal(point2));



        velocity += inputX;
        velocity += inputZ;

        velocity += gravityPower;

        if (Input.GetKeyDown(KeyCode.Space) && GroundCheck(point2))
        {
            velocity += jumpPower;
        }

        //En array av alla object som min overlapcapsule returnerar, alltså de kolliderade med. 
        collidingObjects = Physics.OverlapCapsule(point1,
                            point2,
                            collider.radius, collisionMask);

        //om någonting kolliderades med så checkar den att karaktären inte åker igenom det. 
        if (!(collidingObjects.Length == 0))
        {
            PreventCollision(collidingObjects);
        }

        if (velocity.x > maxSpeedXZ)
        {
            velocity.x = maxSpeedXZ;
        }
        if (velocity.x < -maxSpeedXZ)
        {
            velocity.x = -maxSpeedXZ;
        }

        // har inte max speed för y positivt för att annars kan man inte hoppa 
        if (velocity.y < -maxSpeedY)
        {
            velocity.y = -maxSpeedY;
        }
        if (velocity.z > maxSpeedXZ)
        {
            velocity.z = maxSpeedXZ;
        }
        if (velocity.z < -maxSpeedXZ)
        {
            velocity.z = -maxSpeedXZ;
        }


        // Grappling hook test
        //if (hook == null && Input.GetMouseButtonDown(1))
        if (hook == null && Input.GetKeyDown(KeyCode.G))
        {
            StopAllCoroutines();
            pulling = false;
            hook = Instantiate(hookPrefab, shootLocation.position, Quaternion.identity).GetComponent<Hook>();
            hook.Initialize(this, shootLocation);
            StartCoroutine(DestroyHookAfterLifetime());
        }
        else if (hook != null && Input.GetMouseButtonDown(1))
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
                //rigid.AddForce((hook.transform.position - transform.position).normalized * pullSpeed, ForceMode.VelocityChange);



                //Accelerate(velocity);

                //Vector3 newVector = hookPrefab.transform.position - transform.position;
                Vector3 newVector = (hook.transform.position - transform.position).normalized * grapplingSpeed;
                velocity += newVector;

                if (velocity.magnitude > maxGrapplingSpeed)
                {
                    velocity = Vector3.ClampMagnitude(velocity, maxGrapplingSpeed);
                    //velocity += velocity.normalized* maxSpeed * Time.deltaTime;
                }

                //velocity -= velocity.normalized * normalForce.magnitude * kineticFrictionCoefficient;

            }
        }
        else
        {
            Debug.Log("not pulling");
        }

        //

        transform.position += velocity;



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






    //kalkylerar normalkraften med hjälp av normalen från overlapcapsule-kollisionerna.
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
    //applicerar friktion på karaktären.
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

    bool GroundCheck(Vector3 point2)
    {

        return Physics.Raycast(point2, Vector3.down, collider.radius + skinWidth + groundCheckDistance, collisionMask);

    }


    Vector3 GroundNormal(Vector3 point2)
    {
        RaycastHit hit;
        Physics.Raycast(point2, velocity.normalized, out hit, collider.radius + skinWidth + groundCheckDistance, collisionMask);
        return hit.normal;
    }


    //ser till att karaktären inte åker igenom något, tvingar den att stanna och dödar dens momentum vid kontakt.
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
