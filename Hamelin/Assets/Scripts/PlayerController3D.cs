using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController3D : MonoBehaviour
{
    //deklarering av variablar s� jag kan se dem i Unity och buggfixa.
    public float acceleration;
    public Vector3 velocity;
    private Vector3 input;
    public Vector3 velocityXZ;

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

    private Vector3 normal;
  
    private bool jumping = false;

    //bugnet test
    float netRotationX = 0;
    public SphereCollider bugNet;
    float netRotationSpeed = -10f;
    Vector3 bugNetOffset = new Vector3(0, 4, 0);
    Vector3 bugNetStartOffset = new Vector3(1, 1, 0);
    private float netHoldMovementDecrease = 1.5f;
    private float newSwipeMovementDecrease = 4f;

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


    // Jump Respawn test
    public Transform jumpLocation;
    public GameObject respawnPoint;
    private bool falling;
    //

    private StateMachine StateMachine;
    public CapsuleCollider collider;
    void Awake() => collider = GetComponent<CapsuleCollider>();


    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;

        //grappling hook test
        rigid = GetComponent<Rigidbody>();
        pulling = false;
        //

        StateMachine = new StateMachine(this, States);

        startMaxSpeedXZ = maxSpeedXZ;
        Cursor.lockState = CursorLockMode.Confined;


    }

    private bool grounded = false;
    private float collisionMargin = 0.2f;
    // Update is called once per frame
    void FixedUpdate()
    {

   

        //Points på spelaren
        point1 = transform.position + collider.center + Vector3.up * (collider.height / 2 - collider.radius);
        point2 = transform.position + collider.center + Vector3.down * (collider.height / 2 - collider.radius);

        //Update  velocity

        RaycastHit hit;
        grounded = Physics.CapsuleCast(
            point1,
            point2,
            collider.radius,
            Vector3.down,
            out hit,
            groundCheckDistance + collisionMargin,
            collisionMask
        );

        // if hitting KillZone respawn
        if (hit.collider != null)
        {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("killZone") && respawnPoint)
            {
                Debug.Log("RESPAWN");
                velocity = new Vector3(-velocity.x * 3, 0, -velocity.z * 3);
                transform.position = jumpLocation.transform.position + new Vector3(0, 5, 0);
            }

        }
        //

        input = Vector3.right * Input.GetAxisRaw("Horizontal") + Vector3.forward* Input.GetAxisRaw("Vertical");
      
        if (input.magnitude > 1.0f) {
            input.Normalize();
        }

        float inputMagnitude = input.magnitude;

        
        if (GroundCheck(point2))
        {
            normal = GroundNormal(point2);
            falling = false;

        }
        else{
            normal = Vector3.up;
            //Set Respawn
            if (!falling && respawnPoint)
            {
                //jumpLocation = GameObject.Find("Player").transform;
                //Debug.Log(jumpLocation.transform.position.x);
                respawnPoint.transform.position = transform.position;
            }
            falling = true;
            //
        }

        input = Vector3.ProjectOnPlane(camera.transform.rotation * input, Vector3.Lerp(Vector3.up, normal, 0.5f)).normalized * inputMagnitude;


        velocity += input * acceleration * Time.deltaTime;


        velocity += Vector3.down * gravity * Time.deltaTime;


        // Ser till att man inte rör sig över max speed i X och Z. Y är separat eftersom att hoppet 
        velocityXZ = new Vector3(velocity.x, 0, velocity.z);

        if (velocityXZ.magnitude >= maxSpeedXZ)
        {
            velocityXZ = velocity.normalized * maxSpeedXZ;
            velocity = new Vector3(velocityXZ.x, velocity.y, velocityXZ.z);

        }
      
      
        // en egen maxspeed för y hastigheten
        if (velocity.y >= maxSpeedY)
        {
            velocity.x = maxSpeedY;

        }
        if (velocity.y <= -maxSpeedY)
        {
            velocity.z = -maxSpeedY;

        }

  


        ApplyAirResistance();



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



        //net
        if (netReady) {
            netIdle();
        }
        if (netHolding) {
            netHold();
        }
        if (netSwipe) {
            netSwiping();
            netReset();
        }
     

    
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }


            // Grappling hook 




            shootLocation.transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);


        /*

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
         //   DestroyHook();
        }
        
        if (pulling && hook != null)
        {
           
            Debug.Log("pulling");
            if (Vector3.Distance(transform.position, hook.transform.position) <= hookDistanceStop)
            {
                maxSpeedXZ = startMaxSpeedXZ;
                DestroyHook();
          
            }
            else
            {
          
                Vector3 newVector = (hook.transform.position - transform.position).normalized * grapplingSpeed;
                velocity += newVector;
                maxSpeedXZ = maxGrapplingSpeed;

                if (velocity.magnitude > maxGrapplingSpeed)
                {
                    velocity = Vector3.ClampMagnitude(velocity, maxGrapplingSpeed);
                }
            
            }
            
        }
        else {
        */
            // Deacceleration metoden här under anropas inte om grappling hooken är aktiv
            if (input.x == 0)
            {
                velocity.x *= 0.1f;
            }
            if (input.z == 0)
            {
                velocity.z *= 0.1f;
            }
      //  }
     

        //





        //Debug.Log(velocity.y);
    //    StateMachine.RunUpdate();
    }

    void Update()
    {


        if (Input.GetKeyDown(KeyCode.Space) && GroundCheck(point2))
        {
            jumping = true;
            
        }
        else {
            jumping = false;
        }

        if (jumping)
        {      
            velocity.y = 0;
            velocity += Vector3.up * jumpPowerVariable;
        }



        if (Input.GetMouseButtonDown(0)){
     
            if (netReady)
            {
                netHolding = true;

                netReady = false;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
        
            if (netHolding)
            {
                netSwipe = true;
                netHolding = false;
            }
        }  

    
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

    

    void netHold()
    {
        Vector3 netOffset = bugNet.transform.rotation * bugNetOffset;


        bugNet.transform.rotation = Quaternion.Euler(netRotationX, rotationY, 0);


        bugNet.transform.position = (netOffset + transform.position);

        maxSpeedXZ = startMaxSpeedXZ / netHoldMovementDecrease;
    }

    void netSwiping() {
        bugNet.isTrigger = false;

        maxSpeedXZ = startMaxSpeedXZ / newSwipeMovementDecrease;
        Vector3 netOffset = bugNet.transform.rotation * bugNetOffset;

        netRotationX -= netRotationSpeed;

        netRotationX = Mathf.Clamp(netRotationX, 0, 90);


        bugNet.transform.rotation = Quaternion.Euler(netRotationX, rotationY, 0);


        bugNet.transform.position = (netOffset + transform.position);


    }

    void netIdle() {
        bugNet.isTrigger = true;

        Vector3 netOffset = bugNet.transform.rotation * bugNetStartOffset;


        bugNet.transform.rotation = Quaternion.Euler(netRotationX, rotationY, 0);


        bugNet.transform.position = (netOffset + transform.position);

    }

    void netReset() {
        if (netRotationX >= 90)
        {
            bugNet.isTrigger = true;
            if (WaitTime(0.5f))
            {
                netRotationX = 0;

                maxSpeedXZ = startMaxSpeedXZ;

                netReady = true;
                netSwipe = false;
            }


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
