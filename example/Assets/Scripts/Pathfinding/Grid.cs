using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using BenTools.Data;

public class Grid : MonoBehaviour
{
	// Valori della griglia 2D
	// Questi valori vengono assegnati tramite l'Editor attaccato all'oggetto Grid
	// Unità di misura
	public int Unit = 6;
	// Larghezza
	public int GridWidth = 0;
	// Lunghezza
	public int GridLength = 0;
	// Altezza (solo per lanciare i RayCast)
	public int GridHeight = 100;
	// Coordinate del punto iniziale
	public int GridX = 0;
	public int GridY = 0;
	// Griglia 2D (Area di ricerca)
	private static Node[,] grid;
	// Flag di controllo della scansione della griglia
	private bool scanned = false;
	// Flag di controllo per mostrare i nodi non percorribili
	public bool ShowUnwalkables = false;
	// Elenco di nodi non percorribili (da mostrare)
	private List<Vector3> Unwalkables = null;
	
	// Inizializza e scansiona la griglia
	void Awake() 
	{
		// Inizializzo la griglia
		grid = new Node[GridWidth - GridX, GridLength - GridY];
		// Avvio la scansione
		Debug.Log("Avvio scansione mappa");
		for(int i = 0; i < GridWidth - GridX; i++)
		{
			for(int j = 0; j < GridLength - GridY; j++)
			{
				// Calcolo le coordinate del punto centrale del nodo attuale
				int x = (GridX + i) * Unit + Unit / 2;
				int y = (GridY + j) * Unit + Unit / 2;
				// Creo un vettore posizione che rappresenta il centro del nodo
				Vector3 CenterPosition = new Vector3(x, GridHeight, y);
				// Creo un vettore direzione (per dare la direzione al RayCast)
				Vector3 RayDirection = new Vector3(0, -1, 0);
				// Imposto walkable a true (il nodo è percorribile di default)
				bool Walkable = true;
				// Controllo che il nodo sia percorribile
				if(Physics.Raycast(CenterPosition, RayDirection, GridHeight)) Walkable = false;
				// Aggiungo il nodo alla griglia
				grid[i,j] = new Node(new Vector3(x, 0, y), Walkable);
			}
		}
		Debug.Log("Scansione mappa effettuata");
	}
	
	// Restituisce i nodi adiacenti ad un nodo data la sua posizione (Vector3)
	public List<Vector3> GetNearbyNodes(Vector3 position)
	{
		List<Vector3> NearbyNodes = new List<Vector3>();
		// Calcola la posizione nella griglia (matrice)
		int i = (int) (position.x - GridX) / Unit;
		int j = (int) (position.z - GridY) / Unit;
		// Estraggo dalla matrice le coordinate dei presunti nodi adiacenti
		// e le memorizzo nell'array couple
		for(int k = 1; k <= 8; k++)
		{
			int[] couple;
			
			switch(k)
			{
				case 1:
					couple = new int[2] {i - 1, j - 1};
				break;
				case 2:
					couple = new int[2] {i - 1, j};
				break;
				case 3:
					couple = new int[2] {i - 1, j + 1};
				break;
				case 4:
					couple = new int[2] {i, j - 1};
				break;
				case 5:
					couple = new int[2] {i, j + 1};
				break;
				case 6:
					couple = new int[2] {i + 1, j - 1};
				break;
				case 7:
					couple = new int[2] {i + 1, j};
				break;
				case 8:
					couple = new int[2] {i + 1, j + 1};
				break;
				default:
					throw new Exception("Ops");
				break;
			}
			// Se le coordinate sono all'interno della matrice, aggiungo il nodo alla lista dei nodi adiacenti
			if(couple[0] >= 0 && couple[0] < GridWidth && couple[1] >= 0 && couple[1] < GridLength)
				NearbyNodes.Add(grid[couple[0], couple[1]].getPosition());
		}
		return NearbyNodes;
	}
	
	// Restituisce i nodi adiacenti ad un nodo data la sua posizione (Vector3)
	public List<Node> GetNearbyNodes(Node node)
	{
		List<Node> NearbyNodes = new List<Node>();
		// Calcola la posizione nella griglia (matrice)
		int i = (int) (node.getPosition().x - GridX) / Unit;
		int j = (int) (node.getPosition().z - GridY) / Unit;
		// Estraggo dalla matrice le coordinate dei presunti nodi adiacenti
		// e le memorizzo nell'array couple
		for(int k = 1; k <= 8; k++)
		{
			int[] couple;
			
			switch(k)
			{
				case 1:
					couple = new int[2] {i - 1, j - 1};
				break;
				case 2:
					couple = new int[2] {i - 1, j};
				break;
				case 3:
					couple = new int[2] {i - 1, j + 1};
				break;
				case 4:
					couple = new int[2] {i, j - 1};
				break;
				case 5:
					couple = new int[2] {i, j + 1};
				break;
				case 6:
					couple = new int[2] {i + 1, j - 1};
				break;
				case 7:
					couple = new int[2] {i + 1, j};
				break;
				case 8:
					couple = new int[2] {i + 1, j + 1};
				break;
				default:
					throw new Exception("Ops");
				break;
			}
			// Se le coordinate sono all'interno della matrice, aggiungo il nodo alla lista dei nodi adiacenti
			if(couple[0] >= 0 && couple[0] < GridWidth && couple[1] >= 0 && couple[1] < GridLength)
				NearbyNodes.Add(new Node(grid[couple[0], couple[1]]));
		}
		return NearbyNodes;
	}
	
	// Restituisce l'unità base della griglia
	public int GetUnit()
	{
		return this.Unit;
	}
	
	// Mostra la griglia nell'editor del gioco
	public void OnDrawGizmos()
	{
		// Disegna il box che racchiude l'area di ricerca
		Gizmos.color = Color.white;
		// Calcolo le coordinate di origine della griglia
		int x = GridX * Unit;
		int z = GridY * Unit;
		// Calcolo le coordinate di fine della griglia
		int width = GridWidth * Unit + x;
		int height = GridLength * Unit + z;
		// Altezza della griglia
		int y = GridHeight;
		
		// Disegno il box
		Gizmos.DrawLine(new Vector3(x, 0, z), new Vector3(x + (width), 0, z));
		Gizmos.DrawLine(new Vector3(x, y, z), new Vector3(x + (width), y, z));
		Gizmos.DrawLine(new Vector3(x, 0, z), new Vector3(x, y, z));
		
		Gizmos.DrawLine(new Vector3(x + (width), 0, z), new Vector3(x + (width), 0, z + (height)));
		Gizmos.DrawLine(new Vector3(x + (width), y, z), new Vector3(x + (width), y, z + (height)));
		Gizmos.DrawLine(new Vector3(x + (width), 0, z), new Vector3(x + (width), y, z)); 
		
		Gizmos.DrawLine(new Vector3(x + (width), 0, z + (height)), new Vector3(x, 0, z + (height)));
		Gizmos.DrawLine(new Vector3(x + (width), y, z + (height)), new Vector3(x, y, z + (height)));
		Gizmos.DrawLine(new Vector3(x + (width), 0, z + (height)), new Vector3(x + (width), y, z + (height)));
		
		Gizmos.DrawLine(new Vector3(x, 0, z + (height)), new Vector3(x, 0, z));
		Gizmos.DrawLine(new Vector3(x, y, z + (height)), new Vector3(x, y, z));
		Gizmos.DrawLine(new Vector3(x, 0, z + (height)), new Vector3(x, y, z + (height)));
		
		// Disegno la griglia
		for(int i = x; i < width + x; i += Unit)
			Gizmos.DrawLine(new Vector3(i, 0, z), new Vector3(i, 0, height + z));
		
		for(int i = z; i < height + z; i += Unit)
			Gizmos.DrawLine(new Vector3(x, 0, i), new Vector3(width + x, 0, i));
		
		if(!scanned)
		{
			Unwalkables = new List<Vector3>();
			for(int i = 0; i < GridWidth - GridX; i++)
			{
				for(int j = 0; j < GridLength - GridY; j++)
				{
					// Calcolo le coordinate del punto centrale del nodo attuale
					x = (GridX + i) * Unit + Unit / 2;
					y = (GridY + j) * Unit + Unit / 2;
					// Creo un vettore posizione che rappresenta il centro del nodo
					Vector3 CenterPosition = new Vector3(x, GridHeight, y);
					// Creo un vettore direzione (per dare la direzione al RayCast)
					Vector3 RayDirection = new Vector3(0, -1, 0);
					// Controllo che il nodo sia percorribile
					if(Physics.Raycast(CenterPosition, RayDirection, GridHeight)) Unwalkables.Add(new Vector3(x, 0, y));
				}
			}
			scanned = true;
		}
		
		if(scanned && ShowUnwalkables)
		{
			Gizmos.color = Color.red;
			if(Unwalkables != null)
			{
				foreach(Vector3 Unwalkable in Unwalkables){
					Gizmos.DrawCube(Unwalkable, new Vector3(Unit, .2F, Unit));
				}
			}
		}
	}
	
	public void Scan()
	{
		scanned = false;
	}
}
