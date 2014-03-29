using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class NPCControl : MonoBehaviour
{
	public GameObject player;
	private FSMSystem fsm;
	private CharacterController npcController;
	private float walkSpeed;
	private int healthPoints = 45;
	public int level = 1;
	private float rate = 1;
	public int strenght = 6;
	public int defence = 5;
	public int technical = 2;
	public int agility = 9;
	public bool attacking = false;
	public bool dead = false;
	

	public void getHit(int damage)
	{
		int defence = RPGSystem.calculateDefence(this.defence, agility, level);
		if(defence <= damage){
			this.healthPoints -= damage - defence;
			if(this.healthPoints <= 0 && !dead)
			{
				int experience = (int) RPGSystem.calculateEarnedExpPoint(level) + 10;
				GameObject.Find("RaycastSender").SendMessage("clearCurrentEnemy", experience);
				dead = true;
				Destroy(gameObject, 1);
			}
		}
	}
	
	public void SetTransition(Transition t)
	{
		fsm.PerformTransition(t);
	}
	
	public void performAttack()
	{
		if(!attacking)
			StartCoroutine(doAttack());
	}
	
	public IEnumerator doAttack()
	{
		attacking = true;
		int damage = RPGSystem.calculateDamage(strenght, technical, level);
		GameObject.Find("RaycastSender").SendMessage("getHit", damage);
		SendMessage("attack");
		yield return new WaitForSeconds(1);
		attacking = false;
	}

	public void Start ()
	{
		npcController = GetComponent<CharacterController>();
		walkSpeed = 2.5F;
		MakeFSM();
	}
	
	public void FixedUpdate ()
	{
		fsm.CurrentState.Reason(player, gameObject);
		fsm.CurrentState.Act(player, gameObject);
	}
	
	private void MakeFSM()
	{
		StayStillState stay = new StayStillState();
		stay.AddTransition(Transition.SawPlayer, StateID.ChasingPlayer);
		
		ChasePlayerState chase = new ChasePlayerState();
		chase.AddTransition(Transition.LostPlayer, StateID.FindPlayer);
		chase.AddTransition(Transition.ReachPlayer, StateID.AttackPlayer);
		
		AttackPlayerState attack = new AttackPlayerState();
		attack.AddTransition(Transition.LostPlayer, StateID.ChasingPlayer);
		
		//StayStillState stay = new StayStillState();
		
		fsm = new FSMSystem();
		fsm.AddState(stay);
		fsm.AddState(chase);
		fsm.AddState(attack);
		//fsm.AddState(stay);
		fsm.SetCurrentState(stay);
	}
		
	public void setWalkSpeed(float speed)
	{
		walkSpeed = speed;
	}
	
	public float getWalkSpeed()
	{
		return walkSpeed;
	}
}

/*
public class FindPlayerState : FSMState
{
	public FindPlayerState()
	{
		stateID = StateID.FindPlayer;
	}
	
	public override void Reason(GameObject player, GameObject npc)
	{
		if(Vector3.Distance(npc.transform.position, player.transform.position) <= 30)
		{
			npc.GetComponent<NPCControl>().SetTransition(Transition.SawPlayer);
			//Debug.Log("SawPlayer");
		}
	}
	
	public override void Act(GameObject player, GameObject npc)
	{
		
		
	}
}
*/

public class ChasePlayerState : FSMState
{
	public bool reached = false;
	
	public ChasePlayerState()
	{
		stateID = StateID.ChasingPlayer;
	}
	
	public override void DoBeforeEntering()
	{
		reached = false;
	}
	
	public override void Reason(GameObject player, GameObject npc)
	{
		if(Vector3.Distance(npc.transform.position, player.transform.position) >= 18)
		{
			npc.GetComponent<NPCControl>().SetTransition(Transition.LostPlayer);
			//Debug.Log("LostPlayer");
		}
		else if(Vector3.Distance(npc.transform.position, player.transform.position) < 5 || reached)
		{
			npc.GetComponent<NPCControl>().SetTransition(Transition.ReachPlayer);
			//Debug.Log("ReachPlayer");
		}
	}
	
	public override void Act(GameObject player, GameObject npc)
	{
		Vector3 moveDir = player.transform.position - npc.transform.position;
		//Quaternion wantedRotation = Quaternion.LookRotation(firstPosition - npc.transform.position);
		//npc.transform.rotation = Quaternion.Lerp(npc.transform.rotation, wantedRotation, Time.deltaTime * 2);
		
		if(Vector3.Distance(npc.transform.position, player.transform.position) > 2){
			// Move to next Position
			(npc.GetComponent<CharacterController>()).transform.LookAt(player.transform.position);
			npc.GetComponent<CharacterController>().Move(moveDir.normalized * Time.deltaTime * npc.GetComponent<NPCControl>().getWalkSpeed());
		}else{
			reached = true;
		}
	}
}

public class AttackPlayerState : FSMState
{
	public AttackPlayerState()
	{
		stateID = StateID.AttackPlayer;
	}
	
	public override void Reason(GameObject player, GameObject npc)
	{
		if(Vector3.Distance(npc.transform.position, player.transform.position) > 2)
		{
			npc.GetComponent<NPCControl>().SetTransition(Transition.LostPlayer);
			//Debug.Log("LostPlayer");
		}
	}
	
	public override void Act(GameObject player, GameObject npc)
	{
		npc.GetComponent<NPCControl>().performAttack();
	}
}

public class StayStillState : FSMState
{
	
	public StayStillState()
	{
		stateID = StateID.StayStill;
	}
	
	public void DoBeforeLeaving()
	{
		
	}
	
	public override void Reason(GameObject player, GameObject npc)
	{
		if(Vector3.Distance(npc.transform.position, player.transform.position) <= 30)
		{
			npc.GetComponent<NPCControl>().SetTransition(Transition.SawPlayer);
		}
	}
	
	public override void Act(GameObject player, GameObject npc)
	{
		// Do nothing
	}
}
