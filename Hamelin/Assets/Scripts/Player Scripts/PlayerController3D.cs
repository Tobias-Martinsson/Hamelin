using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Main Authors: Tobias Martinsson, Henrik Rudén, Tim Agélii
public class PlayerController3D : MonoBehaviour
{
    //Declaring variables 
    [Header("Movement Variables")]
    [SerializeField] private float acceleration;
    [SerializeField] private float maxSpeedXZ;
    [SerializeField] private float startMaxSpeedXZ;
    [SerializeField] private float maxSpeedY;
    [SerializeField] private float gravity;
    [SerializeField] private float jumpPowerVariable;
    [SerializeField] private float deceleration;
    private Vector3 velocity;
    private Vector3 input;
    private Vector3 inputVelocity;
    private Vector3 velocityXZ;
    private Vector3 gravityVelocity;
    private Vector3 jumpingVelocity;
    private float gravityBonus = 1.4f;
    private float gravityFallBonus = 2.4f;
    private bool upOnRoof = false;

    [Header("Collision")]
    [SerializeField] private float skinWidth;
    [SerializeField] private LayerMask collisionMask;
    [SerializeField] private RaycastHit hit;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float staticFrictionCoefficient;
    [SerializeField] private float kineticFrictionCoefficient;
    [SerializeField] private float airResistance;
    [SerializeField] private RaycastHit groundHit;
    private Collider[] collidingObjects;
    private Vector3 normal;
    private float collisionMargin = 0.2f;
    private new CapsuleCollider collider;


    [Header("Camera Variables")]
    [SerializeField] private float mouseSensitivity;
    [SerializeField] private new Camera camera;

    [SerializeField] private State[] States;

    [Header("Player Variables")]
    [SerializeField] private float invincibleTime = 1;
    [SerializeField] private float dashPower = 5f;
    [SerializeField] private int maxHealth = 3;
    private bool damageDealt;
    public Vector3 point1;
    public Vector3 point2;
    public int health;
    private bool invincible = false;
    private bool dashing = false;
    private bool climbReady = false;
    private bool ladderStartPointBottom;
    public bool climbing = false;
    private bool onGround;
    public int currentScene;
    public bool loaded;

    [Header("UI Elements")]
    public GameObject health1;
    public GameObject health2;
    public GameObject health3;
    private Scene scene;

    [Header("Ladder Variables")]
    private Vector3 ladderpointTop;
    private Vector3 ladderpointBottom;
    private Vector3 ladderpointEnd;
    private float ladderRotationY;

    [Header("Bugnet Variables")]
    public SphereCollider bugNet;
    private float netHoldMovementDecrease = 1.5f;
    private float newSwipeMovementDecrease = 4f;
    private bool netReady = true;
    private bool netHolding = false;
    private bool netSwipe = false;
    private bool catchCheck = false;

    [Header("Timers")]
    private float netTimer = 0;
    private float damageTimer = 0;
    private bool startDamageTimer = false;
    private float dashTimer = 0;
    private float dashTime = 0.25f;
    private float dashCoolDown = 1f;
    private bool dashAllowed = true;

    [Header("Player Mesh")]
    [SerializeField] private GameObject playerMesh;

    //TEMP FOR TESTING OF SAVING
    public GameObject myRatPrefab;
    public GameObject myBirdPrefab;

    [Header("Killzone Respawn Variables")]
    public Transform jumpLocation;
    public GameObject respawnPoint;
    private bool falling;

    void Awake() => collider = GetComponent<CapsuleCollider>();

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        currentScene = SceneManager.GetActiveScene().buildIndex;
        startMaxSpeedXZ = maxSpeedXZ;
        Cursor.lockState = CursorLockMode.Confined;
        health1.SetActive(true);
        health2.SetActive(true);
        health3.SetActive(true);

        if (PlayerPrefs.HasKey("loaded") == false){
            PlayerPrefs.SetInt("loaded", 0);
        }

        if (PlayerPrefs.GetInt("loaded") == 1)
        {
            LoadGame();
        }
        else{
            Debug.Log(health);
            health = maxHealth;

            SaveGame();
        }

        scene = SceneManager.GetActiveScene();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MovementSetup();

        AbilityHandler();

        UpdateMovement();

        DamageHandler();

    }

    void Update()
    {
        onGround = GroundCheck(point2);

        InputAbilities(onGround);
    }

    public void SaveGame() {
        PlayerPrefs.SetInt("loaded", 1);
        SaveSystem.SavePlayer(this);

        AllAgents.SaveTransforms();
    }
    private void LoadGame() {
        PlayerData data = SaveSystem.LoadPlayer();
        foreach (GameObject a in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Destroy(a);
        }

        health = data.health;
        Vector3 position;

        position.x = data.position[0];
        position.y = data.position[1];
        position.z = data.position[2];
        transform.position = position;

        upOnRoof = data.onRoof;
        GetComponentInChildren<BugNetController>().setScore(data.score);
        SetUIHealth();

        foreach (EnemySaveData e in data.enemySaveData)
        {
            Vector3 enemyPosition;
            enemyPosition.x = e.position[0];
            enemyPosition.y = e.position[1];
            enemyPosition.z = e.position[2];

            Quaternion enemyRotation;
            enemyRotation.w = e.rotation[0];
            enemyRotation.x = e.rotation[1];
            enemyRotation.y = e.rotation[2];
            enemyRotation.z = e.rotation[3];

            if (e.name.Contains("Variant"))
            {
                Instantiate(myRatPrefab, enemyPosition, enemyRotation);
            }

            else if (e.name.Contains("Bird"))
            {
                Instantiate(myBirdPrefab, enemyPosition, enemyRotation);
            }
        }
    }
    private void InputAbilities(bool onGround)
    {
      
        //dash
        if (Input.GetKeyDown(KeyCode.LeftShift) && onGround)
        {
            DodgeDash();
        }
        //climb
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (climbReady && !climbing && onGround)
            {
                if (ladderStartPointBottom)
                {
                    transform.position = ladderpointBottom;
      
                }
                else
                {
                    transform.position = ladderpointTop;
                
                }
                SetClimbing(true);
            }
        }
        //jump
        if (Input.GetKeyDown(KeyCode.Space) && onGround)
        {

            velocity.y = 0;
            velocity += jumpingVelocity;
        }
        //Net
        if (Input.GetMouseButtonDown(0))
        {

            if (netReady)
            {
                netHolding = true;

                netReady = false;
            }
        }
    }

    private void DamageHandler()
    {
        if (damageDealt)
        {

            PlayerTakesDamage();
            if (startDamageTimer)
            {
                DamageWaitTime();
            }
        }
    }
    private void UpdateMovement()
    {
        StopInputDuringDash();

        velocity += inputVelocity;
        velocity += gravityVelocity;

        LimitMaxSpeed();

        ApplyAirResistance();

        //collision
        collidingObjects = Physics.OverlapCapsule(point1,
                            point2,
                            collider.radius, collisionMask);

        if (!(collidingObjects.Length == 0))
        {
            PreventCollision(collidingObjects);
        }

<<<<<<< Updated upstream
        transform.position += velocity;


       
=======
        if(health > 0)
        {
            transform.position += velocity;
        }
>>>>>>> Stashed changes
    }

    private void StopInputDuringDash() {
        if (!dashing)
        {
            if (input.x == 0)
            {
                velocity.x *= 0.1f;
            }
            if (input.z == 0)
            {
                velocity.z *= 0.1f;
            }
        }
    }

    private void LimitMaxSpeed() {
        // Ser till att man inte rör sig över max speed i X och Z.

        if (velocityXZ.magnitude >= maxSpeedXZ)
        {
            velocityXZ = velocity.normalized * maxSpeedXZ;
            velocity = new Vector3(velocityXZ.x, velocity.y, velocityXZ.z);

        }

        // en egen maxspeed för y hastigheten
        if (velocity.y >= maxSpeedY)
        {
            velocity.y = maxSpeedY;

        }
        if (velocity.y <= -maxSpeedY)
        {
            velocity.y = -maxSpeedY;

        }
    }

    private void AbilityHandler()
    {
        // dashState sätts här innan input velocity updateras eftersom den ska stänga av input under dash
        if (dashing)
        {
            DashState();
        }
        if (!dashAllowed)
        {
            if (DashWaitTime(dashCoolDown))
            {
                dashAllowed = true;
            }
        }

        // state för climbimng
        if (climbing)
        {
            ClimbingState();

        }

        if (netReady)
        {
            NetIdle();
        }
        if (netHolding)
        {
            NetHold();
        }
        if (netSwipe)
        {
            NetSwiping();
        }
    }

    private void MovementSetup()
    {
        //Points på spelaren
        point1 = transform.position + collider.center + Vector3.up * (collider.height / 2 - collider.radius);
        point2 = transform.position + collider.center + Vector3.down * (collider.height / 2 - collider.radius);

        //Update velocity
        grounded = Physics.CapsuleCast(
            point1,
            point2,
            collider.radius,
            Vector3.down,
            groundCheckDistance + collisionMargin,
            collisionMask
        );


        // Rotate player. To keep or not to keep
        if (climbing == false)
        {
            playerMesh.transform.rotation = Quaternion.Euler(0, camera.GetComponent<CameraFollowScript>().rotationY, 0);
        }
        else {
          
          
        }
        input = Vector3.right * Input.GetAxisRaw("Horizontal") + Vector3.forward * Input.GetAxisRaw("Vertical");

        if (input.magnitude > 1.0f)
        {
            input.Normalize();
        }

        float inputMagnitude = input.magnitude;

        UpdateGroundNormal();

        input = Vector3.ProjectOnPlane(camera.transform.rotation * input, Vector3.Lerp(Vector3.up, normal, 0.5f)).normalized * inputMagnitude;

        inputVelocity = input * acceleration * Time.deltaTime;

        velocityXZ = new Vector3(velocity.x, 0, velocity.z);


        if (velocity.y < 0.1 && !onGround)
        {
            gravityVelocity = Vector3.down * gravity * Time.deltaTime * gravityFallBonus;
        }
        else {
            gravityVelocity = Vector3.down * gravity * Time.deltaTime * gravityBonus;
        }

        jumpingVelocity = Vector3.up * jumpPowerVariable;

    }
    
    public void KillZoneCollision() {

        Debug.Log("RESPAWN");
        SetDamageDealt(true);
        velocity = new Vector3(-velocity.x * 3, 0, -velocity.z * 3);
        transform.position = jumpLocation.transform.position;

    }
    
    private void UpdateGroundNormal() {

        if (GroundCheck(point2))
        {
            normal = GroundNormal(point2);
            falling = false;

        }
        else
        {
            normal = Vector3.up;
            //Set Respawn
            if (!falling && respawnPoint)
            {
                //jumpLocation = GameObject.Find("Player").transform;
                //Debug.Log(jumpLocation.transform.position.x);
                respawnPoint.transform.position = transform.position;
            }
            falling = true;
        }
    }

    private bool DamageWaitTime()
    {
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

    private bool NetWaitTime(float seconds)
    {

        netTimer += Time.deltaTime;

        if (netTimer >= seconds)
        {

            netTimer = 0;
            return true;

        }

        return false;
    }

    private bool DashWaitTime(float seconds)
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

    void NetHold()
    {

        if (!dashing)
        {
            maxSpeedXZ = startMaxSpeedXZ / netHoldMovementDecrease;

        }

        if (NetWaitTime(0.50f)) {
            netSwipe = true;
            netHolding = false;
        }
    }

    void NetSwiping()
    {
        bugNet.isTrigger = catchCheck;

        if (!dashing)
        {
            maxSpeedXZ = startMaxSpeedXZ / newSwipeMovementDecrease;
        }

        if (NetWaitTime(0.30f))
        {
            NetReset();
        }
    }
    void NetIdle()
    {
        bugNet.isTrigger = true;

    }

    void NetReset()
    {
        maxSpeedXZ = startMaxSpeedXZ;
        bugNet.isTrigger = true;
        catchCheck = false;
        netReady = true;
        netSwipe = false;

    }

    public void SetDamageDealt(bool b)
    {
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

    public void SetCatchCheckTrue()
    {
        catchCheck = true;

    }

    private void PlayerTakesDamage()
    {

        if (!invincible)
        {
            health = health - 1;
<<<<<<< Updated upstream

            Debug.Log("took damage,current health: " + health);
            if (health <= 0)
            {
=======
            
            Debug.Log("took damage,current health: " + health);
            if (health <= 0)
            {
                
                playerAnimator.SetBool("Dead", true);
                StartCoroutine(DeathWaitTime(2f));
>>>>>>> Stashed changes
                PlayerPrefs.SetInt("loaded", 0);
                SceneManager.LoadScene(scene.name);
            }

            invincible = true;

            startDamageTimer = true;
            SetUIHealth();
        }

    }

    public void SetUIHealth()
    {
        if(health == 3)
        {
            health1.SetActive(true);
            health2.SetActive(true);
            health3.SetActive(true);
        }
        else if (health == 2)
        {
            health3.SetActive(false);
            health2.SetActive(true);
            health1.SetActive(true);

        }
        else if (health == 1)
        {
            health3.SetActive(false);
            health2.SetActive(false);
            health1.SetActive(true);
        }
    }

    public bool GetOnGround() {
        return onGround;
    }

    public void SetLadderRotation(float y) {

        ladderRotationY = y;
        Debug.Log(ladderRotationY);
    }

    public void SetClimbing(bool b)
    {

        climbing = b;

    }

    void ClimbingState()
    {
        inputVelocity = new Vector3(0, 0, 0);
        gravityVelocity = new Vector3(0, 0, 0);
        jumpingVelocity = new Vector3(0, 0, 0);

        playerMesh.transform.rotation = Quaternion.Euler(0, ladderRotationY, 0);
        Debug.Log(ladderRotationY);
        Debug.Log(playerMesh.transform.rotation);

        if (ladderStartPointBottom)
        {

            velocity = Vector3.up * 4f * Time.deltaTime;

            if (transform.position.y >= ladderpointTop.y)
            {
                velocity = new Vector3(0, 0, 0);
                transform.position = ladderpointEnd;
                upOnRoof = true;
                ExitClimb();
            }

        }
        else
        {
            
            velocity = Vector3.down * 4f * Time.deltaTime;
            upOnRoof = false;
            if (transform.position.y <= ladderpointBottom.y)
            {
                ExitClimb();
            }

        }

        void ExitClimb()
        {

            climbing = false;
        }

    }

    void DodgeDash()
    {
        if (dashAllowed)
        {

            maxSpeedXZ = startMaxSpeedXZ * 3f;



            velocity += input.normalized * dashPower;
            dashing = true;
        }
    }
    void DashState()
    {

        inputVelocity = new Vector3(0, 0, 0);
        velocity.y = velocity.y * 0.9f;


        if (DashWaitTime(dashTime))
        {
            maxSpeedXZ = startMaxSpeedXZ;
            dashing = false;
            dashAllowed = false;
        }

    }
    public void SetClimbReady(bool b)
    {
        climbReady = b;
    }

    public void SetLadderPointBottom(Vector3 p)
    {
        ladderpointBottom = p;
    }
    public void SetLadderPointTop(Vector3 p)
    {
        ladderpointTop = p;
    }

    public void SetLadderPointEnd(Vector3 p)
    {
        ladderpointEnd = p;
    }

    public void SetLadderStartPointBottom(bool b) {
        if (climbing == false) {
            ladderStartPointBottom = b;
        }
    }

    public bool GroundCheck(Vector3 point2)
    {

        return Physics.Raycast(point2, Vector3.down, collider.radius + skinWidth + groundCheckDistance, collisionMask);

    }

    public bool getUpOnRoof() {

        return upOnRoof;
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
            Vector3 normalForce = CalculateNormalForce(velocity, separationVector.normalized);
            velocity += normalForce;

            ApplyFriction(normalForce);

        }
    }
}
