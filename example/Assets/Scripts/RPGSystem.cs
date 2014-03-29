/**
 * 
 * RPGSystem - RPG core engine.
 * Classe che gestisce i calcoli RPG del gioco
 *
 */
 
using UnityEngine;
using System.Collections;

public class RPGSystem{

	// Costanti per il calcolo dei punti esperienza del livello, dell'HP, dell'AP e dei punti esperienza guadagnati
	// Costanti per la crescita dei punti livelli esperienza
	private const int fixedExpPoint = 500;
	private const int correctionExpPoint = 470;
	// Costanti per la crescita dell'HP
	private const int fixedHPPoint = 100;
	private const int correctionHPPoint = 70;
	// Costanti per la crescita dell'AP
	private const int fixedAPPoint = 150;
	private const int correctionAPPoint = 120;
	// Costanti per il guadagno dei punti esperienza dopo l'uccisione di un nemico
	private const int fixedEarnedExpPoint = 7;
	private const int correctionEarnedExpPoint = 4;
	private const float earnedExpPointRate = 1.4F;
	// Costanti per il guadagno dei punti esperienza dopo il completamento di una quest
	private const int fixedEarnedExpPointQuest = 25;
	private const int correctionEarnedExpPointQuest = 22;
	private const float earnedExpPointRateQuest = 1.4F;
	
	/**
	 * throwDice
	 * Lancia un dado a sei facce
	 * @return - un numero compreso fra 1 e 6 
	 */
	public static int throwDice(){
		return Random.Range(1, 7);
	}
	
	/**
	 * calculateNeededExpPoint
	 * Calcola i punti esperienza necessari per passare al livello successivo
	 * @param rate - tasso di crescita
	 * @param level - livello attuale
	 * @return neededExpPoint - punti esperienza necessari per passare al livello successivo
	 */
	public static int calculateNeededExpPoint(float rate, int level){
		int neededExpPoint = (int) (fixedExpPoint * (Mathf.Pow(rate, level)) - correctionExpPoint);
		return neededExpPoint;
	}
	
	/**
	 * calculateHPPoint
	 * Calcola i punti HP del personaggio al livello attuale
	 * @param rate - tasso di crescita
	 * @param level - livello attuale
	 * @return hpPoint - punti HP del personaggio al livello attuale
	 */
	public static int calculateHPPoint(float rate, int level){
		int hpPoint = (int) (fixedHPPoint * (Mathf.Pow(rate, level)) - correctionHPPoint);
		return hpPoint;
	}
	
	/**
	 * calculateAPPoint
	 * Calcola i punti AP del personaggio al livello attuale
	 * @param rate - tasso di crescita
	 * @param level - livello attuale
	 * @return hpPoint - punti AP del personaggio al livello attuale
	 */
	public static int calculateAPPoint(float rate, int level){
		int apPoint = (int) (fixedAPPoint * (Mathf.Pow(rate, level)) - correctionAPPoint);
		return apPoint;
	}
	
	/**
	 * calculateEarnedExpPoint
	 * Calcola i punti esperienza ottenuti dall'uccisione di un nemico
	 * @param level - il livello di esperienza del nemico ucciso
	 * @return earnedExpPoint - i punti esperienza guadagnati
	 */
	public static int calculateEarnedExpPoint(int level){
		int earnedExpPoint = (int) (fixedEarnedExpPoint * (Mathf.Pow(earnedExpPointRate, level)) - correctionEarnedExpPoint);
		return earnedExpPoint;
	}
	
	/**
	 * calculateEarnedExpPointQuest
	 * Calcola i punti esperienza ottenuti dal completamento di una quest
	 * @param level - il livello di difficoltà della quest
	 * @return earnedExpPointQuest - i punti esperienza guadagnati
	 */
	public static int calculateEarnedExpPointQuest(int level){
		int earnedExpPointQuest = (int) (fixedEarnedExpPointQuest * (Mathf.Pow(earnedExpPointRateQuest, level)) - correctionEarnedExpPointQuest);
		return earnedExpPointQuest;
	}
	
	/**
	 * calculateSkillPoint
	 * Calcola gli skill point acquisiti al passaggio di livello
	 * @return - un numero fra 0 e 4
	 */
	private static int calculateSkillPoint(){
		return Random.Range(0, 4);
	}
	/*
	public void assignSkillPoint(Player player){
		int skillPoint = calculateSkillPoint();
		if(skillPoint != 0){
			for(int i = 0; i < skillPoint; i++){
				int k = Random.Range(1, 4);
				switch(k){
					case 1:
						player.incrementStrenght();
						break;
					case 2:
						player.incrementDefense();
						break;
					case 3:
						player.incrementTechnical();
						break;
					case 4:
						player.incrementAgility();
						break;
				}
			}
		}
	}
	*/
	
	public static int calculateDamage(int strenght, int technical, int level){
		return (int) (throwDice() + strenght + level);
	}
	public static int calculateDefence(int defence, int agility, int level){
		return (int) (throwDice() + defence + level);
	}
}
