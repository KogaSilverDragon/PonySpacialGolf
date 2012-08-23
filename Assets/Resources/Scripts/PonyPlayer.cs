using UnityEngine;
using System.Collections;

public class PonyPlayer : MonoBehaviour {
	
	public float Power = 10.0f;
	public float Thrust = 2.0f;
	public float Control = 10.0f;
	public TextMesh ReturnsGUI;
	public TextMesh FeedbackGUI;
	public Transform PowerBar;
	public AudioSource AudioSrc;
	public AudioClip SwingSound;
	public AudioClip OBSound;
	private bool Started = false;
	private bool ShotStarted = false;
	private float StartedTime;
	private float EndTime;
	private float CurrentLenght;
	private OTAnimatingSprite PonySprite;
	private GameObject Comet;
	private int ReturnsRemaining = 5;
	private bool IsActive = true;
	
	// Use this for initialization
	void Start() {
		PonySprite = (OTAnimatingSprite)gameObject.GetComponent("OTAnimatingSprite");
		Comet = GameObject.FindGameObjectWithTag("Comet");
	}
	
	// Update is called once per frame
	void Update() {
		if(Input.GetKeyUp(KeyCode.Space) && !ShotStarted && IsActive){
	    	if(!Started){
				Started = true;
				StartedTime = Time.deltaTime;
				CurrentLenght = Time.deltaTime;
				EndTime = StartedTime + Control * 0.4f;
			} else {
				Started = false;
				ShotStarted = true;
				StartCoroutine(Shot(0.2F, Mathf.Clamp(CurrentLenght / EndTime, 0, 1)));
			}
	    }
		
		if(CurrentLenght > EndTime){
			Started = false;
		} else {
			CurrentLenght += Time.deltaTime;
		}
		
		if(Started){
			PowerBar.localScale = new Vector3(400.0f * Mathf.Clamp(CurrentLenght / EndTime, 0, 1), 8.0f, 0.0f);
		} else {
			PowerBar.localScale = Vector3.zero;
		}
		
		ReturnsGUI.text = "Returns remaining: "+ReturnsRemaining;
	}
	
	IEnumerator Shot(float _delay, float _powerPercentage) {
		PonySprite.Play("Swing");
		yield return new WaitForSeconds(_delay);
		if(AudioSrc.clip != SwingSound) AudioSrc.clip = SwingSound; AudioSrc.Play();
		Vector3 force = new Vector3(Power * Thrust * Mathf.Clamp(CurrentLenght / EndTime, 0, 1), 0.0f, 0.0f);
				
		Comet.SendMessage("Run", force);
		yield return new WaitForSeconds(1.0f);
		GameObject[] Planets = GameObject.FindGameObjectsWithTag("Planet");
		foreach(GameObject _planet in Planets){
			_planet.SendMessage("EnableAttraction");
		}
	}
	
	void NextShot() { Debug.Log("ShotStarted: "+ShotStarted);
		if(ShotStarted && ReturnsRemaining > 0){
			PonySprite.Play("Idle");
			transform.position = Comet.transform.position;
			Started = false;
			ShotStarted = false;
		}
	}
	
	void UseReturnBall(){ 
		ReturnsRemaining--;
		if(ReturnsRemaining == 0){ FeedbackGUI.text = "You fail"; Desactivate(); }
		if(AudioSrc.clip != OBSound) AudioSrc.clip = OBSound; AudioSrc.Play();
	}
	
	void Desactivate(){ IsActive = false; }
	void Activate(){ IsActive = true; }
	void SetPosition(Vector3 _position){ transform.position = _position; }
}
