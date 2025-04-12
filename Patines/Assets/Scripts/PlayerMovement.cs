using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //referencia al rigidbody
   public Rigidbody _rb;

    [Header("Movement")]
    //fuerza de inpulso por cada vez que se pulsa la tecla
    public float force;
    //limitador de velocidad máxima
    public float maxSpeed;

    [Header("Rotation")]
    public float stepRotation = 20;
    //velocidad del smooth de la rotacion
    public float smoothRotation = 0.2f;

    //angulo objetivo de rotacion del player
    private float _targetYAngle;
    //variable necesaria para el smooth de la rotacion
    private Vector3 _rotateSoothVelocity;

    [Header("Fly")]
    //fuerza del player mientras esta volando
    public float flyForce;
    public float glideTime = 2;
    private float _currentGlideTimer;

    [Header("Jump")]
    //fuerza de salto
    public float jumpForce;
    public float jumpFloatFactor;

    private float _currentJumpFloatFactor = 1;
    [HideInInspector]
    public Vector3 _velocity;

    [Header("Force Back")]
    public float breakForce = 0.2f;
    public float backForce = 0.1f;

    [Header("Drag")]
    public float dragIntensity;

    [Header("Gravity")]
    //gravedad a aplicar cuando el objeto salta
    public float gravityForce = 9.8f;

    [Header("Check Ground")]
    public Transform groundCheckCenter;
    public Vector3 groundCheckSize;
    public LayerMask groundLayer;
    public Collider[] colliders;

    //[Header("Check Ground")]
    //public Vector3 groundRayOffset;
    //public float groundRayLenght;
    //public LayerMask groundLayer;

    [Header("Check Front")]
    public Transform frontRayOrigin;
    public float frontRayLenght;
    public LayerMask frontLayer;

    //detecta si el jugador está en el suelo
    private bool _isGrounded;
    public bool _canDrag = true;
    public bool _canGlide = true;
    public bool _playerCanMove = true;
    public bool isAgainstWall = false;

    public bool grounded {
        get { 
            return _isGrounded; 
        } 
    }


    void Start() {
        //recuperamos el material del rigidbody
        _rb = GetComponent<Rigidbody>();

        _targetYAngle = transform.rotation.eulerAngles.y;

        _currentGlideTimer = glideTime;
    }

    private void Update() {
        ApplyRotation();
        Drag();

        //si no esta en el suelo...
        if (!_isGrounded) {
            ApplyGravity();
            //GlideTimer();
        //si no...
        } 

        if (_currentJumpFloatFactor != 1) {
            _canDrag = false;
        }

        if (_isGrounded) {
            _canDrag = true;
            _canGlide = true;
            _currentGlideTimer = glideTime;
        }              
    }

    void FixedUpdate() {
        CheckGround();
        isAgainstWall = CheckFront();
        //_velocity = transform.rotation * _velocity;
        //if (_velocity.y == 0) {
        //    _velocity = transform.forward * _velocity.magnitude;
        //}

        if (CheckFront() && !Input.GetButton("ForceBack")) {
            _velocity.x = 0;
            _velocity.z = 0;
        }
        if (!_playerCanMove) {
            _velocity = Vector3.zero;        
        }
        _rb.velocity = _velocity;
    }

    /// <summary>
    /// Metodo que chequea los inputs
    /// </summary>
    public void CheckGroundInputs() {
        //la rotacion en y rotara cada vez que la tecla se presione
        //además de aplicar la fuerza de movimiento cada vez que se presione.
        if(Input.GetButtonDown("RightImpulse")) {
            Move(transform.forward, force);
            Rotate(stepRotation);
        } else if(Input.GetButtonDown("LeftImpulse")) {
            Move(transform.forward, force);
            Rotate(-stepRotation);
        }
        if (Input.GetButton("ForceBack")) {
            MoveBack();
        }
    }
    public void CheckFlyImputs() {
        float horizontal = Input.GetAxis("Horizontal");
        Fly(horizontal);
        if (Input.GetButton("ForceBack")) {
            MoveBack();
        }
    }
    public void CheckJumpInputs() {
        //si el jugador èsta pulsando el espacio, el factor de flotabilidad se modificará
        if (Input.GetButton("Float")) {
            SetFloatFactor(jumpFloatFactor);
        }
        else {
            SetFloatFactor(1);
        }
    }

    /// <summary>
    /// Método que controla y clamea el movimiento
    /// </summary>
    public void Move(Vector3 direction, float force) {
        //PRUEBA
        //Vector3 moveDirection = new Vector3(Mathf.Cos(_targetYAngle + 90), 0, Mathf.Sin(_targetYAngle + 90));
        //Debug.Log(_targetYAngle);
        //Debug.Log(moveDirection);
        //Quaternion rot = Quaternion.Euler(transform.rotation.eulerAngles.x, stepRotation, transform.rotation.eulerAngles.z);
        //Añadimos la fuerza 
        _velocity += direction * force;
        //limitamos la velocidad segun maxspeed
        _velocity = new Vector3(Mathf.Clamp(_velocity.x, -maxSpeed, maxSpeed),
                                   Mathf.Clamp(_velocity.y, -maxSpeed, maxSpeed),
                                   Mathf.Clamp(_velocity.z, -maxSpeed, maxSpeed));
        //_velocity = rot * _velocity;
    }
    public void Fly(float direction) {
        //_velocity = transform.right * direction * flyForce;
        float forwardMagnitud = Vector3.Dot(_velocity, transform.forward);

        Vector3 forwardVelocity = transform.forward * forwardMagnitud;
        Vector3 lateralVelocity = transform.right * flyForce * direction;
        Vector3 velocidadFinal = forwardVelocity + lateralVelocity;
        velocidadFinal.y = _velocity.y;
        _velocity = velocidadFinal;
    }
    /// <summary>
    /// Método que gestiona la rotación
    /// </summary>
    public void Rotate(float yRotation) {
        //le damos el valor al angulo de target
        _targetYAngle += yRotation;
    }

    /// <summary>
    /// Metodo que aplica la rotacion al player
    /// </summary>
    public void ApplyRotation() {
        //Interpola suavemente la rotación actual hacia el ángulo objetivo
        float smoothYAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetYAngle, ref _rotateSoothVelocity.y, smoothRotation);

        //Aplica la rotación al jugador
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, smoothYAngle, transform.rotation.eulerAngles.z);
    }

    /// <summary>
    /// Método que gestiona el salto
    /// </summary>
    public void Jump() {
        //aplicamos la velocidad de salto al pulsar el boton de salto
        if(Input.GetButtonDown("Jump")) {
            _velocity += transform.up * jumpForce;
            _canGlide = true;
        }
    }

    /// <summary>
    /// Metodo que modifica el factor de flotabilidad
    /// </summary>
    /// <param name="floatfactor"></param>
    public void SetFloatFactor(float floatfactor) {
        _currentJumpFloatFactor = floatfactor;
    }

    /// <summary>
    /// Metodo que aplica la fuerza de la gravedad
    /// </summary>
    private void ApplyGravity() {
        //aplicamos la gravedad
        _velocity -= Vector3.up * gravityForce * Time.deltaTime;
        //siempre que el jugador este cayendo, se le aplicará el factor de flotabilidad
        if (_velocity.y < 0) {
            _velocity.y /= _currentJumpFloatFactor;
        }
    }

    /// <summary>
    /// Metodo que chequea si el jugador esta tocando el suelo
    /// </summary>
    private void CheckGround() {
        //generamos el array de colisiones con el overlapBox
        Collider[] collisions = Physics.OverlapBox(groundCheckCenter.position, groundCheckSize / 2f, groundCheckCenter.rotation, groundLayer);

        //si el overlap detecta colisiones...
        if(collisions != null && collisions.Length > 0) {
            //seleccionamos el offset del raycast
            Vector3 rayOffset = new Vector3(0, 0.5f, 0);
            Ray ray = new Ray(transform.position + rayOffset, Vector3.down);
            Debug.DrawRay(transform.position + rayOffset, Vector3.down, Color.red);
            if(Physics.Raycast(ray, out RaycastHit hit, 1f)) {
                float angle = Mathf.Abs(Vector3.Angle(hit.normal, transform.up));
                _isGrounded = angle < 1f;
                if(_velocity.y <= 0) {
                   _velocity.y = 0f;
                }
            } else {
                _isGrounded = false;
            }
            return;
        }
        _isGrounded = false;
    }
    //private void CheckGround() {
    //    Ray ray = new Ray(transform.position + groundRayOffset, Vector3.down * groundRayLenght);
    //    if (Physics.Raycast(ray, out RaycastHit hit, 1f, groundLayer)) {
    //        _isGrounded = true;
    //        return;
    //    }
    //    _isGrounded = false;
    //}

    //private void CheckGround() {
    //    Collider[] forgardCollisions = Physics.OverlapBox(groundCheckCenter[0].position, groundCheckSize[0] / 2f, groundCheckCenter[0].rotation, groundLayer);
    //    Collider[] rightCollisions = Physics.OverlapBox(groundCheckCenter[1].position, groundCheckSize[1] / 2f, groundCheckCenter[1].rotation, groundLayer);
    //    Collider[] backCollisions = Physics.OverlapBox(groundCheckCenter[2].position, groundCheckSize[2] / 2f, groundCheckCenter[2].rotation, groundLayer);
    //    Collider[] leftCollisions = Physics.OverlapBox(groundCheckCenter[3].position, groundCheckSize[3] / 2f, groundCheckCenter[3].rotation, groundLayer);
    //}

    private bool CheckFront() {
        Ray ray = new Ray(frontRayOrigin.transform.position, transform.forward * frontRayLenght);
        if (Physics.Raycast(ray, out RaycastHit hit, frontRayLenght, frontLayer)) {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Metodo que gestiona la friccion del judador 
    /// </summary>
    void Drag() {
        if (_canDrag && _velocity.magnitude > 0) {
            _velocity += -_velocity.normalized * dragIntensity * Time.deltaTime;
        }
    }

    public void Break() {
        _velocity += -_velocity.normalized * breakForce * Time.deltaTime;
        //_rb.AddForce(-_velocity.normalized * breakForce);
    }
    public void MoveBack () {
        Move(-transform.forward, backForce);
    }
    /// <summary>
    /// Rebota al jugador en una determinada dirección
    /// </summary>
    public void Bounce(float bounceForce){
        _velocity += transform.up * bounceForce;
    }

    public void BounceBack(float bounceForce) {
        _velocity = -transform.forward * bounceForce;
    }

    public void BounceDirection(Vector3 dir, float bounceForce)
    {
        _velocity += dir.normalized * bounceForce;
    }

    public void GlideTimer()
    {
        _currentGlideTimer -= Time.deltaTime;

        if(_currentGlideTimer <= 0)
        {
            _canGlide = false;
            _currentGlideTimer = glideTime;
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        //Gizmos.DrawLine(transform.position + rayOffset, transform.position + rayOffset + Vector3.down * rayLeght);
        //if(groundCheckCenter != null) {
            //Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(groundCheckCenter.position, groundCheckSize);
            Gizmos.DrawLine(frontRayOrigin.transform.position, frontRayOrigin.transform.position + transform.forward * frontRayLenght);
            //Gizmos.DrawLine(transform.position + groundRayOffset, transform.position + groundRayOffset + Vector3.down * groundRayLenght);

            //Gizmos.DrawWireCube(groundCheckCenter[0].position, groundCheckSize[0]);
            //Gizmos.DrawWireCube(groundCheckCenter[1].position, groundCheckSize[1]);
            //Gizmos.DrawWireCube(groundCheckCenter[2].position, groundCheckSize[2]);
            //Gizmos.DrawWireCube(groundCheckCenter[3].position, groundCheckSize[3]);
        //}
    }
}
