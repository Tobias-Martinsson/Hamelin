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
    private Vector3 inputVelocity;
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
    private int health;
    private int maxHealth = 3;
    private bool invincible = false;
    private float invincibleTime = 1;
    private Scene scene;
    private bool damageDealt;
    private float dashPower = 5f;
    private bool dashing = false;

    private bool onGround;
   

    //bugnet test
    float netRotationX = 0;
    public SphereCollider bugNet;
    float netRotationSpeed = -10f;
    Vector3 bugNetOffset = new Vector3(0.2f, 1.5f, 0);
    Vector3 bugNetStartOffset = new Vector3(0.2f, 1, 0);
    private float netHoldMovementDecrease = 1.5f;
    private float newSwipeMovementDecrease = 4f;

    bool netReady = true;
    bool netHolding = false;
    bool netSwipe = false;

    private bool catchCheck = false;

    //timers
    private float netTimer = 0;
    private float damageTimer = 0;
    private bool startDamageTimer = false;

    private float dashTimer = 0;
    private float dashTime = 0.25f;
    private float dashCoolDown = 1f;
    private bool dashAllowed = true;


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
        // Application.targetFrameRate = 60;
        health = maxHealth;

        scene = SceneManager.GetActiveScene();

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

        input = Vector3.right * Input.GetAxisRaw("Horizontal") + Vector3.forward * Input.GetAxisRaw("Vertical");

        if (input.magnitude > 1.0f) {
            input.Normalize();
        }

        float inputMagnitude = input.magnitude;


        if (GroundCheck(point2))
        {
            normal = GroundNormal(point2);
            falling = false;

        }
        else {
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

        inputVelocity = input * acceleration * Time.deltaTime;

        velocityXZ = new Vector3(velocity.x, 0, velocity.z);


        // dashState sätts här innan input velocity updateras eftersom den ska stänga av input under dash
        if (dashing)
        {
            dashState();
        }
        if (!dashAllowed) {
            if (dashWaitTime(dashCoolDown)){
                dashAllowed = true;
            }
        }

        velocity += inputVelocity;

        velocity += Vector3.down * gravity * Time.deltaTime;


        // Ser till att man inte rör sig över max speed i X och Z.

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



        if (damageDealt) {

            playerTakesDamage();
            if (startDamageTimer)
            {
                damageWaitTime();

            }
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

        bool onGround= GroundCheck(point2);


        if (Input.GetKeyDown(KeyCode.LeftShift) && onGround)
        {
         dodgeDash();
        }
      

        if (Input.GetKeyDown(KeyCode.Space) && onGround)
        {
         
            velocity.y = 0;
            velocity += Vector3.up * jumpPowerVariable;
        }



        if (Input.GetMouseButtonDown(0)) {

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


    private bool damageWaitTime() {
        Debug.Log("WAIT TIME CALLED");

        damageTimer += Time.deltaTime;

        if (damageTimer >= invincibleTime)
        {
            Debug.Log("TIMER COMPLETE AFTER: " + invincibleTime);
            damageTimer = 0;
            invincible = false;
            startDamageTimer = false;
            damageDealt = false;
            return true;

        }

        return false;
    }
    private bool netWaitTime(float seconds)
    {




        netTimer += Time.deltaTime;

        if (netTimer >= seconds)
        {

            netTimer = 0;
            return true;

        }

        return false;
    }

    private bool dashWaitTime(float seconds)
    {

        dashTimer += Time.deltaTime;

        if (dashTimer >= seconds)
        {
        
            dashTimer = 0;
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
     
        if (!dashing) {
            maxSpeedXZ = startMaxSpeedXZ / netHoldMovementDecrease;
    
        }
   
    }

    void netSwiping() {
        bugNet.isTrigger = catchCheck;
    
        if (!dashing)
        {
            maxSpeedXZ = startMaxSpeedXZ / newSwipeMovementDecrease;
        }
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
            if (netWaitTime(0.5f))
            {
                netRotationX = 0;

                maxSpeedXZ = startMaxSpeedXZ;

                catchCheck = false;
                netReady = true;
                netSwipe = false;
            }


        }

    }

    public void setDamageDealt(bool b) {
        damageDealt = b;

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

    public void setCatchCheckTrue(){
        catchCheck = true;

}

    private void playerTakesDamage()
    {
       
        if (!invincible)
        {
            health = health - 1;

            Debug.Log("took damage,current health: " + health);
                if (health <= 0)
                {
                  Debug.Log("RESPAWN");
                }

            invincible = true;

            startDamageTimer = true;
        }
        
    }


    void dodgeDash() {
        if (dashAllowed)
        {
          
                maxSpeedXZ = startMaxSpeedXZ * 3f;
       


            velocity += input.normalized * dashPower;
            dashing = true;
        }
    }
    void dashState() {
      
        inputVelocity = new Vector3(0, 0, 0);
        velocity.y = velocity.y * 0.9f;


        if (dashWaitTime(dashTime))
        {
            maxSpeedXZ = startMaxSpeedXZ;
            dashing = false;
            dashAllowed = false;
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
