using UnityEngine;

public class AirHockeyPlayerController : MonoBehaviour {

    public float m_MaxMoveSpeed = 5;

	private bool m_WasJustClicked = true;
	private bool m_CanMove;
    //Vector2 playerSize;
    public GameObject particle;
    public float m_RayLength = 200;
    public Transform m_PlayerStriker;
    public Rigidbody m_PlayerRB;

    // Bit shift the index of the layer (8) to get a bit mask
    private int m_PlayGroundLayerMask = 1 << 9;

    public Transform m_BoundaryHolder;

    private Boundary m_PlayerBoundary;
    private Vector3 m_TargetPosition;

    public bool m_ControlsEnabled = true;

    void Start () {

        //PONER LA Y SEGUN LA ALTURA DEL JUGADOR (preguntar antes de empezar el juego)

        //importante el orden de los hijos
        m_PlayerBoundary = new Boundary(m_BoundaryHolder.GetChild(0).position.z, //up
                                        m_BoundaryHolder.GetChild(1).position.z, //down
                                        m_BoundaryHolder.GetChild(2).position.x, //left
                                        m_BoundaryHolder.GetChild(3).position.x //right
                                        );

        m_TargetPosition = m_PlayerRB.position;

    }

	void FixedUpdate () {
        //MovePlayerForTablets();
        //FollowMouse();
        if (m_ControlsEnabled)
        {
            FollowPosition();

        }
    }

    public void FollowPosition()
    {

        Ray ray3 = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray3.origin, ray3.direction * 10, Color.yellow);

        RaycastHit hitGround;

        if (Physics.Raycast(ray3, out hitGround, m_RayLength, m_PlayGroundLayerMask)) //Si no ho fem en la layer mask, esta tot el rato xocant contra el jugador y te dona posicions incorrectes (pero aproximades)
        {
            float laX = Mathf.Clamp(hitGround.point.x, m_PlayerBoundary.Left, m_PlayerBoundary.Right); //Left y Right hacen de minimo y maximo, y si la primera variable supera alguno de estos dos, se devolvera el minimo o el maximo, sino la variable tal cual
            float laZ = Mathf.Clamp(hitGround.point.z, m_PlayerBoundary.Down, m_PlayerBoundary.Up); //Left y Right hacen de minimo y maximo, y si la primera variable supera alguno de estos dos, se devolvera el minimo o el maximo, sino la variable tal cual


            m_TargetPosition = new Vector3(laX, m_PlayerStriker.transform.position.y, laZ);

        }

        m_PlayerRB.MovePosition(Vector3.MoveTowards(m_PlayerRB.position, m_TargetPosition, m_MaxMoveSpeed * Time.fixedDeltaTime));



    }

    public void FollowMouse()
    {
        Ray ray2 = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray2.origin, ray2.direction * 10, Color.yellow);

        RaycastHit hitGround;

        if (Physics.Raycast(ray2, out hitGround, m_RayLength, m_PlayGroundLayerMask)) //Si no ho fem en la layer mask, esta tot el rato xocant contra el jugador y te dona posicions incorrectes (pero aproximades)
        {
            float laX = Mathf.Clamp(hitGround.point.x, m_PlayerBoundary.Left, m_PlayerBoundary.Right); //Left y Right hacen de minimo y maximo, y si la primera variable supera alguno de estos dos, se devolvera el minimo o el maximo, sino la variable tal cual
            float laZ = Mathf.Clamp(hitGround.point.z, m_PlayerBoundary.Down, m_PlayerBoundary.Up); //Left y Right hacen de minimo y maximo, y si la primera variable supera alguno de estos dos, se devolvera el minimo o el maximo, sino la variable tal cual


            m_PlayerRB.MovePosition(new Vector3(laX, m_PlayerStriker.transform.position.y, laZ));

            //m_PlayerRB.MovePosition(new Vector3(hitGround.point.x, m_PlayerStriker.transform.position.y, hitGround.point.z)); //aixina es sense el Clamp y xocant en boundaries si no es kinematic
            //m_PlayerStriker.transform.position = new Vector3(hitGround.point.x, m_PlayerStriker.transform.position.y, hitGround.point.z); //aixina no calcula be el colp
        }

    }
	public void MovePlayerForTablets(){


        //https://docs.unity3d.com/ScriptReference/Input-mousePosition.html

        if (Input.GetButton("Fire1")) //no se si fire vale tambien pra el tap de los mobiles
        {
            
            if (Input.GetButtonDown("Fire1")) //Si has clickat per primera vegada en el ratoli... (Aço no es get buton down?)
            {


                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);

                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, m_RayLength) && hit.transform.tag == "Striker") // si puntxem damunt del Striker
                {
                    //Instantiate(particle, hit.transform.position, hit.transform.rotation);
                    m_CanMove = true;
                }

                else //Si no esta damunt del striker...
                {
                    m_CanMove = false;
                }
            }

            Ray ray2 = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray2.origin, ray2.direction * 10, Color.yellow);

            RaycastHit hitGround;
   
            if (Physics.Raycast(ray2, out hitGround, m_RayLength, m_PlayGroundLayerMask) && m_CanMove) //si el tenim agafat, el menejem //Si no ho fem en la layer mask, esta tot el rato xocant contra el jugador y te dona posicions incorrectes (pero aproximades)
            {
                m_PlayerRB.MovePosition(new Vector3(hitGround.point.x, m_PlayerStriker.transform.position.y, hitGround.point.z)); 
            }
        }

        if (Input.GetButtonUp("Fire1")) //Si no esta apretat el ratoli / tap
        {
            m_CanMove = false;
        }


    }

    public void ControlsEnabled(bool boleano)
    {
        m_ControlsEnabled = boleano;
    }

}