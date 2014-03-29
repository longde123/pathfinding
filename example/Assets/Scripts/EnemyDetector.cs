using UnityEngine;
using System.Collections;

public class EnemyDetector : MonoBehaviour {

	private GameObject currentEnemy;
	public RaycastHit hit;
	// Use this for initialization
	void Start () {
	
	}
	
	void FixedUpdate(){
		Vector3 fwd = transform.TransformDirection(Vector3.forward);
		
		if (Physics.Raycast(transform.position, fwd, out hit, 7))
		{
			//Debug.Log("There is something in front of the object!");
			if(hit.collider.gameObject.tag == "Enemy"){
				if(currentEnemy == null || currentEnemy != hit.collider.gameObject)
				{
					currentEnemy = hit.collider.gameObject;
				}
			}
		}
			
	}
	
	public GameObject getCurrentEnemy(){
		return currentEnemy;
	}
	
	public void clearCurrentEnemy(int experience){
		// dare exp
		SendMessage("addEarnedExpPoints", experience);
		currentEnemy = null;
	}
}
