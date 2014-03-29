using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using BenTools.Data;

public class Pathfinder
{
	// Lista Aperta
	private IPriorityQueue OpenList = new BinaryPriorityQueue();
	// Lista Chiusa
	private List<Node> CloseList = new List<Node>();
	// Nodo corrente
	private Node CurrentNode;
	// Flag di controllo
	public bool GoalReached = false;
	// Limite della ricerca
	private const int MaxTimeSearch = 1000;
	
	// Normalizza il vettore posizione in base alla rappresentazione della griglia
	private Vector3 Normalize(Vector3 position)
	{
		int Unit = GameObject.Find("Grid").GetComponent<Grid>().GetUnit();
		
		float x = (int) position.x / Unit;
		x = x * Unit + Unit / 2;
		
		float z = (int) position.z / Unit;
		z = z * Unit + Unit / 2;
		
		return new Vector3(x, position.y, z);
	}
	
	// Cerca il percorso
	// Algoritmo: A*
	public List<Vector3> FindPath(Vector3 Start, Vector3 Goal)
	{
		
		Vector3 StartNormalized = Normalize(Start);
		Vector3 GoalNormalized = Normalize(Goal);
		int Iterate = 0;
		List<Vector3> Path = new List<Vector3>();
		// Nodo di partenza
		Node root = new Node(StartNormalized, true);
		// Aggiungo il nodo alla lista aperta
		OpenList.Push(root);
		// ciclo finchè non trovo il percorso (oppure finchè non raggiungo il limite di iterate, oppure il percorso non può essere trovato (lista aperta vuota))
		while(!GoalReached && OpenList.Count != 0 && Iterate <= MaxTimeSearch)
		{
			Iterate++;
			// Prendo il nodo con minore costo F dalla lista aperta
			CurrentNode = new Node((Node) OpenList.Pop());
			// lo inserisco nella lista chiusa
			CloseList.Add(CurrentNode);
			// Se il nodo corrente è uguale al nodo Goal, ho trovato il percorso ed esco dal ciclo
			if(CurrentNode.getPosition() == GoalNormalized)
			{
				GoalReached = true;
				break;
			}
			// Prendo i nodi adiacenti al nodo corrente
			List<Node> NearbyNodes = GameObject.Find("Grid").GetComponent<Grid>().GetNearbyNodes(CurrentNode);
			// Per ognuno dei nodi adiacenti
			foreach(Node NearbyNode in NearbyNodes)
			{
				Node SuccessorNode = new Node(NearbyNode);
				// Se il nodo è percorribile
				if(SuccessorNode.isWalkable())
				{
					// Se non si trova nella lista chiusa
					if(!CloseList.Contains(SuccessorNode))
					{
						// Se non si trova nalla lista aperta
						if(!OpenList.Contains(SuccessorNode))
						{
							// Calcolo il costo G
							// E' il costo del movimento da un nodo a quello successivo
							// Sarà pari a 10 se lo spostamento è ortogonale, sarà pari a 14 se lo spostamento è diagonale
							float GCost = Mathf.Round(Mathf.Abs(Vector3.Distance(SuccessorNode.getPosition(), CurrentNode.getPosition())));
							SuccessorNode.setG(GCost);
							// Calcolo il costo H
							// E' la stima del costo del movimento da un nodo a quello finale
							// Calcolo il costo euristico tramite la distanza diagonale
							float HDiagonal = (Mathf.Min(Mathf.Abs(SuccessorNode.getPosition().x - GoalNormalized.x), Mathf.Abs(SuccessorNode.getPosition().z - GoalNormalized.z))) / 10;
							float HStraight = (Mathf.Abs(SuccessorNode.getPosition().x - GoalNormalized.x) + Mathf.Abs(SuccessorNode.getPosition().z - GoalNormalized.z)) / 10;
							float HCost = 14 * HDiagonal + 10 * (HStraight - 2 * HDiagonal);
							// Applico una tecnica di tie-breaking
							HCost *= (1 + 0.01F);
							SuccessorNode.setH(HCost);
							// Calcolo il costo F
							SuccessorNode.setF(GCost + HCost);
							// Imposto CurrentNode come padre di SuccessorNode
							SuccessorNode.setParent(CurrentNode);
							// Lo aggiungo alla lista aperta
							OpenList.Push(SuccessorNode);
						}
						// Se si trova nella lista aperta
						// controllo se conviene passare dal nodo in questo nuovo percorso
						else
						{
							// Prendo il costo G del nodo nel percorso precedente
							int index = OpenList.IndexOf(SuccessorNode);
							Node previous = new Node((Node) OpenList[index]);
							float PreviousGCost = previous.getG();
							// Calcolo il costo G del nodo nel nuovo percorso
							float GCost = Mathf.Round(Mathf.Abs(Vector3.Distance(previous.getPosition(), CurrentNode.getPosition())));
							GCost += CurrentNode.getG();
							// Se il nuovo costo G è migliore del precedente
							if(GCost < PreviousGCost)
							{
								// assegna al nodo presente nella OpenList CurrentNode come nodo genitore
								((Node) OpenList[index]).setParent(CurrentNode);
								// aggiorna il costo di G
								((Node) OpenList[index]).setG(GCost);
								// ricalcola F
								((Node) OpenList[index]).setF(GCost + previous.getH());
								// Aggiorna la lista
								OpenList.Update(index);
							}
						}
					}
				}
			}
		}
		// Ripercorre il percorso
		Node temp = new Node(CloseList[CloseList.Count - 1]);
		while(temp.getPosition() != StartNormalized)
		{
			Path.Add(temp.getPosition());
			temp = new Node(temp.getParent());
		}
		Path.Add(Start);
		Path[0] = Goal;
		
		// Inverte il percorso
		Path.Reverse();
		return Path;
	}
}
