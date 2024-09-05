using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine.InputSystem;//Need this for the new input system.
using UnityEngine;

public class MovementKeyboard : MonoBehaviour
{
    //Define keys but allow them to be accessed when using the player settings.
    [SerializeField] private KeyCode KeyLeft = KeyCode.A;
    [SerializeField] private KeyCode KeyRight = KeyCode.D;
    [SerializeField] private KeyCode Jump = KeyCode.Space;
    [SerializeField] private KeyCode DashKey = KeyCode.M;

    //Values for movement
    [SerializeField] private float f_moveSpeed = 3f;
    [SerializeField] private float f_jumpAmount = 20f;
    [SerializeField] private float f_dashAmount = 1.25f;

    //Amount of time for the measurement of the jump height.
    [SerializeField] private float f_jumpTime;
    [SerializeField] private float f_buttonTime = 0.3f;

    //Check to see if player is jumping.
    [SerializeField] private bool b_jump;

    //Get the reference to the players rigidbody so that we can use physics.
    [SerializeField] private Rigidbody2D rigidBody;

    //Get the reference to the players sprite so that we can manipulate it as neccessary. 
    [SerializeField] private SpriteRenderer m_SpriteRenderer;

    //Variables to be used for the ground check.
    [SerializeField] private Vector3 v3_boxSize;
    [SerializeField] private float f_maxDistance;
    [SerializeField] private LayerMask layerMask;

    //Variables to reference the animator
    [SerializeField] private Animator animator;
    private bool b_check;

    //Variables to control the firing of the spells.
    [SerializeField] private Shoot spell;
    [SerializeField] private Transform firePoint;

    void Start()
    {
        //Fetch the SpriteRenderer from the GameObject
        m_SpriteRenderer = GetComponent<SpriteRenderer>();

        rigidBody = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update()
    {
        ////Animation process for the walk, jump and dash.
        //if (Input.GetKey(KeyLeft) | Input.GetKey(KeyRight) && !Input.GetKey(DashKey) & GroundCheck())
        //{
        //    animator.SetBool("Walk", true);
        //    animator.SetBool("Jump", false);
        //}
        //else if (Input.GetKey(KeyLeft) | Input.GetKey(KeyRight) && Input.GetKey(DashKey) & GroundCheck())
        //{
        //    animator.SetBool("Dash", true);
        //    animator.SetBool("Jump", false);
        //}
        //else if (!GroundCheck())
        //{
        //    animator.SetBool("Jump", true);
        //    animator.SetBool("Walk", false);
        //}
        //else
        //{
        //    animator.SetBool("Walk", false);
        //    animator.SetBool("Jump", false);
        //    animator.SetBool("Dash", false);
        //}

        //Use the keboard to move the character left or right.
        if (Input.GetKey(KeyRight))
        {
            //Check if the dash key is pressed
            if (Input.GetKey(DashKey))
            {
                transform.position += Vector3.right * (f_moveSpeed * f_dashAmount) * Time.deltaTime;

            }
            else if (!Input.GetKey(DashKey))
            {
                transform.position += Vector3.right * f_moveSpeed * Time.deltaTime;
            }

            //Rotate the character when the left / right key are pressed. 
            m_SpriteRenderer.flipX = false;
        }
        else if (Input.GetKey(KeyLeft))
        {
            //Check if the dash key is pressed
            if (Input.GetKey(DashKey))
            {
                transform.position += Vector3.right * (-f_moveSpeed * f_dashAmount) * Time.deltaTime;

            }
            else if (!Input.GetKey(DashKey))
            {
                transform.position += Vector3.right * -f_moveSpeed * Time.deltaTime;
            }
            //Rotate the character when the left / right key are pressed. 
            m_SpriteRenderer.flipX = true;
        }

        //Firstly check if the character is grounded, then if the space bar is pressed the character will jump. 
        //The length of time that the player holds the key influences the hieght of the jump. This is resstricted so that there isn't an infinite jump.
        if (Input.GetKeyDown(KeyCode.Space) && GroundCheck())
        {
            b_jump = true;
            f_jumpTime = 0;
        }
        if (b_jump)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, f_jumpAmount);
            f_jumpTime += Time.deltaTime;
        }
        if (Input.GetKeyUp(Jump) | f_jumpTime > f_buttonTime)
        {
            b_jump = false;
        }

        //By pressing the p key, a spell is instantiated (created on the screen during rather than at the start of the game).
        //It checks which way the player is facing to instantiate the spell. 
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (m_SpriteRenderer.flipX == false)
            {
                Debug.Log(firePoint.position);
                Instantiate(spell, firePoint.position, transform.rotation);

            }
            else if (m_SpriteRenderer.flipX == true)
            {
                Debug.Log(firePoint.position * -1);
                Instantiate(spell, firePoint.position * -1, transform.rotation);
            }

        }

    }

    //Draws a box to see how far the jump condition is. 
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position - transform.up * f_maxDistance, v3_boxSize);
    }

    //Checks using a boxcast the a specfic layermask. It returns a true boolean if it is range.
    private bool GroundCheck()
    {
        b_check = Physics2D.BoxCast(transform.position, v3_boxSize, 0, -transform.up, f_maxDistance, layerMask);
        return b_check;
    }
}
