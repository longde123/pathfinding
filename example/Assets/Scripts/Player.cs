using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	private int healthPoints = 200;
	private int level = 1;
	private float rate = 1.02F;
	private int strenght = 18;
	private int defence = 14;
	private int technical = 16;
	private int agility = 12;
	private int experience = 0;
	private int nextLevel = 0;
	private int experienceOffset = 0;
	
	// Use this for initialization
	void Start () {
		nextLevel = RPGSystem.calculateNeededExpPoint(rate, level);
	}
	
	// Update is called once per frame
	void Update () {
		GameObject.Find("cam").SendMessage("setHpPoints", healthPoints);
		GameObject.Find("cam").SendMessage("setLevel", level);
		GameObject.Find("cam").SendMessage("setExpPoints", experience);
		GameObject.Find("cam").SendMessage("setNextLevel", nextLevel);
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Application.Quit();
		}
	}
	
	public void getHit(int damage)
	{
		int defence = RPGSystem.calculateDefence(this.defence, agility, level);
		if(defence <= damage){
			this.healthPoints -= damage - defence;
			if(this.healthPoints <= 0)
			{
				Destroy(gameObject, 1);
			}
		}
	}
	
	public void addEarnedExpPoints(int exp)
	{
		experience += exp;
		
		if(experience >= nextLevel)
		{
			experienceOffset += nextLevel;
			level++;
			nextLevel = RPGSystem.calculateNeededExpPoint(rate, level) + experienceOffset;
			healthPoints += RPGSystem.calculateHPPoint(rate, level);
			strenght++;
			defence++;
			agility++;
			technical++;
			Debug.Log("Sono passato di livello!");
		}
	}
	
	void AttackEnemy(){
		GameObject enemy = gameObject.GetComponent<EnemyDetector>().getCurrentEnemy();
		if(enemy != null){
			int damage = RPGSystem.calculateDamage(strenght, technical, level);
			enemy.GetComponent<NPCControl>().getHit(damage);
		}
	}
	

}
