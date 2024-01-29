using System;

public class Bacteria : Enemy
{

	private const int BacteriaHealth = 20;

	private const int BacteriaMinDamage = 3;
	private const int BacteriaMaxDamage = 10;

	/*
	public int Damage
	{
		get
		{
			return BacteriaMaxDamage;
			//Random newRandom = new Random();
			//return newRandom.Next(BacteriaMinDamage, BacteriaMaxDamage + 1);
		} protected set
		{
			Damage = value;
		}
	}
	*/

	public Bacteria()
	{
		Health = BacteriaHealth;
		Damage = BacteriaMaxDamage;
	}


}
