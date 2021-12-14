using UnityEngine;
using Unity.Netcode;

//[RequireComponent(typeof(NetworkObject))]
public class Character2DController : NetworkBehaviour {

  [SerializeField]
  public float movementSpeed = 4;

  [SerializeField]
  public float jumpForce = 4;

  [SerializeField]
  private NetworkVariable<Vector2> networkInputDirection = new NetworkVariable<Vector2>();

  [SerializeField]
  private NetworkVariable<PlayerState> networkPlayerState = new NetworkVariable<PlayerState>();

  [SerializeField]
  private Rigidbody2D _rigidbody;

  [SerializeField]
  private SpriteRenderer _spriteRenderer;

  private Character2DController _character2DController;
  private Animator _animator;

  private float inputDirX;
  private float inputDirY;
  private bool inputJump;

  private void Awake() {
    _character2DController = GetComponent<Character2DController>();
    _animator = GetComponent<Animator>();
    _rigidbody = GetComponent<Rigidbody2D>();
    _spriteRenderer = GetComponent<SpriteRenderer>();
  }

  // Start is called before the first frame update
  private void Start() {
    if (IsClient && IsOwner) {
      transform.position = new Vector2(0, 0);
    }
  }

  // Update is called once per frame
  private void Update() {
    /* orginal client side movement
    var movement = Input.GetAxisRaw("Horizontal");
    transform.position += new Vector3(movement, 0, 0) * Time.deltaTime * movementSpeed;

    if (Input.GetButtonDown("Jump") && Mathf.Abs(_rigidbody.velocity.y) < 0.001) {
      _rigidbody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
    }
    */

    if (IsClient && IsOwner) {
      ClientInput();
    }

    ClientMove();
    ClientVisuals();
  }

  private void ClientInput() {
    //player input
    inputDirX = Input.GetAxisRaw("Horizontal");
    inputDirY = Input.GetAxisRaw("Vertical");
    inputJump = Input.GetButtonDown("Jump");

    Vector2 inputDir = Time.deltaTime * movementSpeed * new Vector2(inputDirX, inputDirY);
    UpdateClientPositionAndFacingServerRpc(inputDir, inputJump);

    //player state change based on input
    if (inputDirX != 0) {
      UpdatePlayerStateServerRpc(PlayerState.running);
    } else {
      UpdatePlayerStateServerRpc(PlayerState.idle);
    }
  }

  private void ClientMove() {
    if (networkInputDirection.Value != Vector2.zero) {
      _rigidbody.position += new Vector2(networkInputDirection.Value.x, 0);
    }
  }

  private void ClientVisuals() {
    if (networkPlayerState.Value == PlayerState.running) {
      _animator.SetBool("idle", false);
      _animator.SetBool("running", true);
      // we can check networkInputDirection.Value for facing
      if (inputDirX > 0f) {
        _spriteRenderer.flipX = false;
      } else if (inputDirX < 0f) {
        _spriteRenderer.flipX = true;
      }
    } else if (networkPlayerState.Value == PlayerState.idle) {
      _animator.SetBool("running", false);
      _animator.SetBool("idle", true);
    }
  }

  [ServerRpc]
  public void UpdateClientPositionAndFacingServerRpc(Vector2 newInputDir, bool jump) {
    networkInputDirection.Value = newInputDir;
  }

  [ServerRpc]
  public void UpdatePlayerStateServerRpc(PlayerState state) {
    networkPlayerState.Value = state;
  }
}
