using UnityEngine;
using System.Collections;

public class Comet : MonoBehaviour {
	
	public float OBDistance = 1200.0f;
	public TextMesh FeedbackGUI;
	public AudioSource AudioSrc;
	public AudioClip SurfaceHitSound;
	public AudioClip BlackHoleSound;
	private Vector3 Power;
	private Vector3 InitialPosition;
	
	// Use this for initialization
	void Start () {
		Power = Vector3.zero;
		OT.view.movementTarget = gameObject;
        OT.view.rotationTarget = gameObject;
		InitialPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		//gameObject.transform.position += Power * Time.deltaTime;
		gameObject.rigidbody.velocity = Power;
		//Debug.Log("Power: "+Power);
		GameObject BG = GameObject.FindGameObjectWithTag("BG");
		//Debug.Log("Distance: "+Vector3.Distance(BG.transform.position, transform.position));
		
		if(Vector3.Distance(BG.transform.position, transform.position) > OBDistance){
			GameObject[] Planets = GameObject.FindGameObjectsWithTag("Planet");
			foreach(GameObject _planet in Planets){
				_planet.SendMessage("DisableAttraction");
			}
			Power = Vector3.zero;
			transform.position = InitialPosition;
			GameObject.FindGameObjectWithTag("Player").SendMessage("NextShot");
			GameObject.FindGameObjectWithTag("Player").SendMessage("UseReturnBall");
			GameObject.FindGameObjectWithTag("Player").SendMessage("SetPosition", InitialPosition);
		}
	}
	
	void Run(Vector3 _force){
		Power = _force;	
	}
	
	void OnCollisionStay(Collision _object){
		if(_object.gameObject.tag == "Planet"){
			if(Power.magnitude > 1.0f){
				Power -= Power * 0.05f;
				if(AudioSrc.clip != SurfaceHitSound) AudioSrc.clip = SurfaceHitSound; AudioSrc.Play();
			} else {
				Power = Vector3.zero;
				GameObject[] Planets = GameObject.FindGameObjectsWithTag("Planet");
				foreach(GameObject _planet in Planets){
					_planet.SendMessage("DisableAttraction");
				}
				GameObject.FindGameObjectWithTag("Player").SendMessage("NextShot");
			}
		}
	}
	
	void OnTriggerEnter(Collider _object) {
        if(_object.gameObject.tag == "Hole"){
			if(AudioSrc.clip != BlackHoleSound) AudioSrc.clip = BlackHoleSound; AudioSrc.Play();
			Power = Vector3.zero;
			_object.transform.position = transform.position;
			FeedbackGUI.text = "You did it!";
			GameObject.FindGameObjectWithTag("Player").SendMessage("Desactivate");
		}
    }
	
	void AddPower(Vector3 _energy){ Power += _energy; }
}
