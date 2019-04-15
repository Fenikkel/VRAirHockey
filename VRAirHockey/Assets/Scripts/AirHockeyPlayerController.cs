using UnityEngine;

public class AirHockeyPlayerController : MonoBehaviour {

	private bool m_WasJustClicked = true;
	private bool m_CanMove;
    //Vector2 playerSize;
    public GameObject particle;
    public float m_RayLength = 200;
    public Transform m_PlayerStriker;
    public Rigidbody m_PlayerRB;

    // Bit shift the index of the layer (8) to get a bit mask
    private int m_PlayGroundLayerMask = 1 << 9;



    void Start () {

        //PONER LA Y SEGUN LA ALTURA DEL JUGADOR (preguntar antes de empezar el juego)

	}

	void FixedUpdate () {
        //MovePlayerForTablets();
        FollowMove();
    }

    public void FollowMove()
    {
        Ray ray2 = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray2.origin, ray2.direction * 10, Color.yellow);

        RaycastHit hitGround;

        if (Physics.Raycast(ray2, out hitGround, m_RayLength, m_PlayGroundLayerMask)) //Si no ho fem en la layer mask, esta tot el rato xocant contra el jugador y te dona posicions incorrectes (pero aproximades)
        {
            m_PlayerRB.MovePosition(new Vector3(hitGround.point.x, m_PlayerStriker.transform.position.y, hitGround.point.z));
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



}