using UnityEngine;
using System;
using System.Collections;

public class Node : IComparable{

	public Node(){}
		
	public Node(Vector3 position, bool walkable){
		this.position = position;
		this.walkable = walkable;
		f = 0;
		h = 0;
		g = 0;
		parent = null;
	}
	
	public Node(Node other){
		this.position = other.position;
		this.walkable = other.walkable;
		this.parent = other.parent;
		this.f = other.f;
		this.h = other.h;
		this.g = other.g;
	}
	
	public Vector3 getPosition(){
		return position;
	}
	
	public bool isWalkable(){
		return walkable;
	}
	
	public Node getParent(){
		return parent;
	}
	
	public float getG(){
		return g;
	}
	
	public float getH(){
		return h;
	}
	
	public float getF(){
		return f;
	}
	
	public void setPosition(Vector3 position){
		this.position = position;
	}
	
	public void setWalkable(bool walkable){
		this.walkable = walkable;
	}
	
	public void setParent(Node parent){
		this.parent = parent;
	}
	
	public void setG(float g){
		this.g = g;
	}
	
	public void setH(float h){
		this.h = h;
	}
	
	public void setF(float f){
		this.f = f;
	}
	
	private Vector3 position;
	private bool walkable = true;
	private Node parent;
	private float g;
	private float h;
	private float f;
	
	public int CompareTo(object Obj){
		if(Obj is Node){
			Node other = (Node) Obj;
			return f.CompareTo(other.f);
		}else
			throw new ArgumentException("Object is not a Node.");
	}
}
