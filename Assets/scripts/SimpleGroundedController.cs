using System.Collections.Generic;
using UnityEngine;

public class SimpleGroundedController : MonoBehaviour
{
    public float JumpImpulse = 20f;
    public float SideSpeed = 10f;

    public ContactFilter2D ContactFilter;

    private Rigidbody2D m_Rigidbody;
    private bool m_ShouldJump;
    private float m_SideSpeed;

    // We can check to see if there are any contacts given our contact filter
    // which can be set to a specific layer and normal angle.
    public bool IsGrounded => m_Rigidbody.IsTouching(ContactFilter);

    private bool FaceRight = true;
    private Animator animHero;


    private void flip()
    {
        FaceRight = !FaceRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.x);
    }

    private bool LeftArrow = false;
    private bool RightArrow = false;

    private SpriteRenderer sprite;


    private void Awake()
    {
        animHero=GetComponent<Animator>();  

    }

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    public enum States
    {
        Idle,Run,Jump
    }

    private States State
    {
        get { return (States)animHero.GetInteger("StateHero"); }
        set { animHero.SetInteger("StateHero", (int) value); }


    }



    void Update()
    {
        // Set jump/
        if (Input.GetKeyDown(KeyCode.Space))
            m_ShouldJump = true;

        LeftArrow = Input.GetKey(KeyCode.LeftArrow);
        RightArrow = Input.GetKey(KeyCode.RightArrow);
        m_SideSpeed = ( LeftArrow  ? -SideSpeed : 0f) + (RightArrow ? SideSpeed : 0f);


        if (IsGrounded) State = States.Idle;
        if ((LeftArrow || RightArrow ) && IsGrounded) State = States.Run;
        if (!IsGrounded) State = States.Jump;

        if (LeftArrow && (FaceRight == true))
                {
                    flip();
                }
        else if (RightArrow  && (FaceRight != true))
                {
                    flip();
                }
    }


   


    void FixedUpdate()
    {
        // Handle jump.
        // NOTE: If instructed to jump, we'll check if we're grounded.
        if (m_ShouldJump && IsGrounded)
            m_Rigidbody.AddForce(Vector2.up * JumpImpulse, ForceMode2D.Impulse);

        // Set sideways velocity.
        m_Rigidbody.velocity = new Vector2(m_SideSpeed, m_Rigidbody.velocity.y);






        // Reset movement.
        m_ShouldJump = false;
        m_SideSpeed = 0f;
    }
}
