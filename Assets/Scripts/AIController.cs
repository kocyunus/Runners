using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    #region Variables
    #region MoveProperties
    private Vector3 _moveProperties;
    [Header("Player Move Properties")]
    public float speed, jumpForce;
    [Header("SuperJumpe multiple jumpForce")]
    public float superJumpForce;
    float _gravity;
    float _verctialVelocity;
    float jumpDistance = 8f;
    #endregion
    #region state
    bool lateJump;
    bool aiCanJump;
    private bool _wallSlide;
    #endregion
    private CharacterController charController;
    private Animator anim;
    #endregion

    #region Unity Funcs
    void Awake()
    {
        charController = GetComponent<CharacterController>();
        anim = transform.GetChild(0).GetComponent<Animator>();
    }
    private void Update()
    {

        
        if (GameManager._MyInstance.finish)
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Dance"))
            {
                anim.SetTrigger("Dance");
                transform.eulerAngles = Vector3.up * 90;
            }
            return;
        }
        if (!GameManager._MyInstance.start)
        {
            return;
        }
        _verctialVelocity = Mathf.Clamp(_verctialVelocity, -40f, 100f);
        SetMoveProperties();
        MoveBot();
        if (charController.isGrounded  || aiCanJump)
        {
            _wallSlide = false;
            lateJump = true;
            _verctialVelocity -= _gravity * 0.1f * Time.deltaTime;
            aiCanJump = false;
            RayCasting();
        }
        if (!_wallSlide)
        {
            Fall();
        }
        else
        {
            WallSideFall();
        }
        anim.SetBool("WallSlide", _wallSlide);
        anim.SetBool("Grounded", charController.isGrounded);
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.tag == "Wall")
        {
            if (lateJump)
            {
                float random = Random.Range(0.15f, 0.45f);
                StartCoroutine(LateJump(random));
            }
            if (_verctialVelocity < 0)
            {
                _wallSlide = true;
            }
        }
        else if (hit.collider.tag == "Slide" && charController.isGrounded)
        {
            TurnBot();
        }
        else if (hit.collider.tag == "Slide")
        {
            _wallSlide = true;
        }
        if (hit.collider.tag == "AiGround")
        {
            aiCanJump = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Trombolin")
        {
            SuperJump();
        }
    }
    #endregion
    #region AI Move Method

    void RayCasting() 
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit , jumpDistance))
        {
            if (hit.collider.tag == "Wall" )
            {
                Jump();
            }
          
        }
    
    }
    void Jump()
    {
        _verctialVelocity = jumpForce;
        anim.SetTrigger("Jump");
    }
    void SuperJump() 
    {
        _verctialVelocity = jumpForce * superJumpForce;
      
        anim.SetTrigger("Jump");

        Debug.Log("Superjump  aktif ve verticalvelocity =" + _verctialVelocity);
    }
    void Fall()
    {
        _gravity = 40;
        _verctialVelocity -= _gravity * Time.deltaTime;
    }
    void WallSideFall()
    {
        _gravity = 20;
        _verctialVelocity -= _gravity * Time.deltaTime;
    }
    void TurnBot()
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + 180, transform.eulerAngles.z);
    }
    void SetMoveProperties()
    {
        _moveProperties = Vector3.zero;
        _moveProperties = transform.forward;
        _moveProperties.Normalize();
        _moveProperties *= speed;
        _moveProperties.y = _verctialVelocity;
    }
    void MoveBot()
    {
        charController.Move(_moveProperties * Time.deltaTime);
    }
    IEnumerator LateJump(float time) 
    {
        lateJump = false;
        _wallSlide = true;
        yield return new WaitForSeconds(time);
            if (!charController.isGrounded)
            {
                TurnBot();
                Jump();
            }
        lateJump = true;
        _wallSlide = false;
    }
    #endregion
}
