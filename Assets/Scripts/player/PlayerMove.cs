using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{

	public int playerSpeed = 10;
	public int playerJumpPower = 1250;
	private float moveX;
	public bool grounded = false;
	public bool canMove = true;
	private bool canWalk = true;
	//for coyote time;
	private float timer = 99999999;
	public float coyoteTime = 0.05f;
	private bool storedJump = false;
	public float storedJumpTime = 0.05f;
	//for stored jump
	private float timer2 = 999999999;
	public bool doubleJump = false;
	public GameObject doubleJumpPlatform;
	[SerializeField]
	private bool doubleJumpStored = true;
	private Rigidbody2D rb;
	public GameObject spriteChild;
	private Animator anim;
	private bool landed = true;
	private float lastVel = 0;
	[SerializeField]
	private float ungroundedTime;
	private bool setUngTime = false;

	// assign the actions asset to this field in the inspector:
	public InputActionAsset actions;

	// private field to store move action reference
	public InputAction moveAction;

	private void Start()
    {
		doubleJump = FindObjectOfType<dontDestroySave>().itemsGotten[2];
		rb = GetComponent<Rigidbody2D>();
		anim = spriteChild.GetComponent<Animator>();

		// find the "move" action, and keep the reference to it, for use in Update
		moveAction = actions.FindActionMap("gameplay").FindAction("move");

		// for the "jump" action, we add a callback method for when it is performed
		actions.FindActionMap("gameplay").FindAction("jump").performed += Jump;
	}

    private void OnDisable()
    {
		// for the "jump" action, we add a callback method for when it is performed
		actions.FindActionMap("gameplay").FindAction("jump").performed -= Jump;
	}

    // Update is called once per frame
    void Update()
    {
		//stretchAndSquish
		if (!grounded && !Mathf.Approximately(rb.velocity.y, 0f) && (Time.time - ungroundedTime) > 0.05f)
		{
			float stretch = Mathf.Pow(1.0003f, -(rb.velocity.y * rb.velocity.y));
			spriteChild.gameObject.transform.localScale = new Vector3(stretch, 1 / stretch);
			landed = false;
			lastVel = rb.velocity.y;
		}
		else if (grounded && !landed && (Time.time - ungroundedTime) > 0.1f)
		{
			float stretch = Mathf.Pow(1.0003f, -(lastVel * lastVel));
			if (stretch != 1)
            {
			spriteChild.gameObject.transform.localScale = new Vector3(1 / stretch, stretch);
			landed = true;
            }
		}
		else
		{
			spriteChild.gameObject.transform.localScale = Vector3.MoveTowards(spriteChild.gameObject.transform.localScale, new Vector3(1, 1, 0), 4f * Time.deltaTime);
		}

		GroundDetection();
		if(canMove)
		{
        	Move();
		}
		else
		{
			rb.velocity = new Vector2(0, rb.velocity.y);
			moveX = 0;
			anim.SetBool("isWalking", false);
		}
		if(!grounded)
		{
			anim.SetBool("isAirborne", true);
		} else
		{
			anim.SetBool("isAirborne", false);
		}
		if(storedJump && timer2 <= Time.time - storedJumpTime)
        {
			storedJump = false;
        }


	}

    void Move()
    {
    	//CONTROLS
    	if(canWalk)
		{
			moveX = moveAction.ReadValue<Vector2>().x;
			if (grounded && storedJump)
			{
				Jump();
				storedJump = false;
			}
		}
    	//ANIMATIONS
		if(moveX != 0)
		{
			anim.SetBool("isWalking", true);
		} else 
		{
			anim.SetBool("isWalking", false);
		}
    	//PLAYER DIRECTION
    	if (moveX < 0.0f)
    	{
    		gameObject.transform.localScale = new Vector2(-1, 1);
    	}
    	else if (moveX > 0.0f)
    	{
    		gameObject.transform.localScale = new Vector2(1, 1);
    	}
    	//PHYSICS
    	rb.velocity = new Vector2(moveX * playerSpeed, rb.velocity.y);
    }

	void GroundDetection()
	{
		float extraHeightTest = 0.5f;
		RaycastHit2D raycastHit = Physics2D.BoxCast(gameObject.GetComponent<BoxCollider2D>().bounds.center, gameObject.GetComponent<BoxCollider2D>().bounds.size, 0f, Vector2.down, extraHeightTest);
		if(raycastHit.collider != null && raycastHit.collider.tag == "ground")
		{
			grounded = true;
			timer = Time.time;
			setUngTime = false;
		} else if(timer >= Time.time - coyoteTime)
		{
			grounded = true;
			doubleJumpStored = true;
			setUngTime = false;
		}
        else
        {
			if (!setUngTime)
            {
				ungroundedTime = Time.time;
				setUngTime = true;
            }
			grounded = false;
        }
	}

    private void Jump(InputAction.CallbackContext context = new InputAction.CallbackContext())
    {
		if (grounded && !storedJump)
		{
			//JUMPING CODE
			rb.velocity = new Vector2(rb.velocity.x, 0);
			rb.AddForce(Vector2.up * playerJumpPower);
		}
		else if (!grounded && !storedJump)
		{
			storedJump = true;
			timer2 = Time.time;
		}
		
		if (!grounded && doubleJumpStored && doubleJump)
		{
			//JUMPING CODE
			rb.velocity = new Vector2(rb.velocity.x, 0);
			rb.AddForce(Vector2.up * playerJumpPower);

			doubleJumpStored = false;
			Instantiate(doubleJumpPlatform, transform.position - new Vector3(0f, 0.5f), Quaternion.identity);
		}
    }

	public void Immobilize()
    {
		canMove = false;
		gameObject.GetComponent<playerAttack>().enabled = false;
		gameObject.GetComponent<playerFire>().enabled = false;
    }
	public void Unimmobilize()
	{
		canMove = true;
		gameObject.GetComponent<playerAttack>().enabled = true;
		gameObject.GetComponent<playerFire>().enabled = true;
	}
}
