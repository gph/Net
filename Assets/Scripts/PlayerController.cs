using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

    public Rigidbody2D MyRigidbody2D;
    public float Thrust;
    public GameObject Bullet;
    [SyncVar]public float Health;
    public Transform HealthBar;
    private float startingScale;

	// Use this for initialization
	void Start () {
        startingScale = HealthBar.localScale.x;
        Health = 100;
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 HealthBarScale = new Vector3(startingScale * Health / 100, HealthBar.localScale.y, HealthBar.localScale.z);
        HealthBarScale.x = Mathf.Clamp(HealthBarScale.x, 0, startingScale);
        Health = Mathf.Clamp(Health, 0, 100f);
        HealthBar.localScale = HealthBarScale;

        if (!isLocalPlayer)
        {
            return;
        }

        // Camera follows player
        Camera.main.transform.position = new Vector3(transform.position.x, 
                                                     transform.position.y, 
                                                     Camera.main.transform.position.z);
        Camera.main.transform.rotation = transform.rotation;
        // INPUT
        if (Input.GetAxis("Vertical") > 0.1f && MyRigidbody2D.velocity.y < 2)
        {
            MyRigidbody2D.AddRelativeForce(new Vector2 (0,Input.GetAxis("Vertical") * Thrust));
        }

        MyRigidbody2D.AddTorque(-Input.GetAxis("Horizontal"));
        

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CmdFire();
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Bullet")
        {
            CmdDamage();
        }
    }
    [Command]
    void CmdDamage()
    {
        Health -= 25f;        
    }
    [Command]
    void CmdFire()
    {
        GameObject BulletClone = (GameObject)Instantiate(Bullet, transform.position + transform.up * 1.15f, transform.rotation);
        BulletClone.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(0,MyRigidbody2D.velocity.y + 7f), ForceMode2D.Impulse);
        NetworkServer.Spawn(BulletClone);
    }
}
