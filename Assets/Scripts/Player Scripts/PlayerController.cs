using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Variables
    #region Move Properties
    private Vector3 _moveProperties;
    [Header("Player Move Properties")]
    public float speed, jumpForce;
    [Header("SuperJumpe multiple jumpForce")]
    public float superJumpForce;
    float _gravity;
    float   _verctialVelocity;
    #endregion
    #region state of player
    private bool _wallSlide, _turn,_canDoubleJump,touched;
    #endregion

    private CharacterController _charController;
    private Animator _anim;
    #endregion
    #region Unity Funcs
    void Awake()
    {
        _charController = GetComponent<CharacterController>();
        _anim = transform.GetChild(0).GetComponent<Animator>();
        gameObject.name = PlayerPrefs.GetString("PlayerName", "Player");
        _wallSlide = true;
      
    }
    private void Start()
    {
        _wallSlide = true;
        
    }
    void Update()
    {
        if (GameManager._MyInstance.finish)
        {
            _moveProperties = Vector3.zero;
            if (!_charController.isGrounded)
            {
                Fall();
            }
            else
            {
                _verctialVelocity = 0;
            }
            _moveProperties.y = _verctialVelocity;
            _charController.Move(new Vector3(0, _moveProperties.y * Time.deltaTime, 0));
            if (!_anim.GetCurrentAnimatorStateInfo(0).IsName("Dance"))
            {
                _anim.SetTrigger("Dance");
                transform.eulerAngles = Vector3.up * 90;
            }
            return;
        }
        if (!GameManager._MyInstance.start)
        {
            return;
        }
        SetMoveProperties();
        MovePlayer();
        _verctialVelocity = Mathf.Clamp(_verctialVelocity, -40f, 100f);
        if (_charController.isGrounded)
        {
            _verctialVelocity -= _gravity* 0.1f*Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.Space) || touched)
            {
                Jump();  
                _canDoubleJump = true;
                touched = false;
            }
            if (_turn)
            {
                _turn = false;
                TurnPlayer();

            }
            _wallSlide = false;
        }
        else if (!_wallSlide)
        {
            Fall();
            if (Input.GetKeyDown(KeyCode.Space) && _canDoubleJump )
            {
                DoubleJump();
                _canDoubleJump = false;
                touched = false;
            }
        }
        else
        {
            WallSideFall();
        }
        _anim.SetBool("WallSlide", _wallSlide);
        _anim.SetBool("Grounded", _charController.isGrounded);
       
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!_charController.isGrounded)
        {
            if (hit.gameObject.tag == "Wall" || hit.gameObject.tag == "Slide")
            {
                if (_verctialVelocity < 0)
                {
                    _wallSlide = true;
                }
                if (Input.GetKeyDown(KeyCode.Space) || touched)
                {
                    touched = false;
                    _canDoubleJump = false;
                    _wallSlide = false;
                    Jump();
                    TurnPlayer();
                }
            }
        }
        else
        {
            if (transform.forward != hit.collider.transform.up && hit.collider.tag == "Ground" && !_turn)
            {
                _turn = true;
            }
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
    #region Move Method
    void Jump()
    {
        _verctialVelocity = jumpForce;
        _anim.SetTrigger("Jump");
    }
    void SuperJump()
    {
        _verctialVelocity = jumpForce * superJumpForce;
        _anim.SetTrigger("Jump");
    }
    void DoubleJump() 
    {
        _verctialVelocity += jumpForce*0.6f;
        _anim.SetTrigger("Jump");
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
    void TurnPlayer() 
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
    void MovePlayer() 
    {
        _charController.Move(_moveProperties * Time.deltaTime);
    }

    //  for mobile  devices GamePlayUI Button invisible 
    public void Touched() 
    {
        touched = true;
    }
    #endregion
}
