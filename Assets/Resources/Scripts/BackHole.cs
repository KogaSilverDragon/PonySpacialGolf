using UnityEngine;
using System.Collections;

public class BackHole : MonoBehaviour {

public float AttractionPower = 900.0f;
	public float MinAttractDistance = 150.0f;
	private GameObject Comet;
	
	// Use this for initialization
	void Start () {
		Comet = GameObject.FindGameObjectWithTag("Comet");
	}
	
	// Update is called once per frame
	void Update () {
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
	
	 void OnDrawGizmos(){
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, transform.localScale.x + MinAttractDistance);
    }
}
