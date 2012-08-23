using UnityEngine;
using System.Collections;

public class Planet : MonoBehaviour {
	
	public float AttractionPower = 100.0f;
	public float MinAttractDistance = 50.0f;
	private GameObject Comet;
	private bool isActive = false;
	
	// Use this for initialization
	void Start () {
		Comet = GameObject.FindGameObjectWithTag("Comet");
	}
	
	// Update is called once per frame
	void Update () {
		if(isActive){
			Vector3 ToAttractor = transform.position - Comet.transform.position;
			float Distance = ToAttractor.magnitude;
			//Debug.Log("Distance: "+Distance);
			if(Distance <= transform.localScale.x + MinAttractDistance && transform.localScale.x * 0.5 < Distance && Comet.rigidbody){
				if(Distance <= 0) Distance = 1;
				Vector3 AttractionRay = (ToAttractor.normalized * AttractionPower * rigidbody.mass) / (Distance * Distance);
				//Comet.transform.position += AttractionRay;
				Comet.SendMessage("AddPower", AttractionRay);
				//Debug.Log("("+ToAttractor.normalized+" * "+AttractionPower+") / ("+Distance+" * "+Distance+")");
				//Debug.Log("AttractionRay: "+AttractionRay);
			}
		}
	}
	
	 void OnDrawGizmos(){
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, transform.localScale.x + MinAttractDistance);
    }
	
	void DisableAttraction(){ Debug.Log("Disabling -> isActive: "+isActive); isActive = false; }
	void EnableAttraction(){ Debug.Log("Enabling -> isActive: "+isActive); isActive = true; }
}
