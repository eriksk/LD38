using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour 
{
	public RobotClaws Claws;
	public List<GameObject> OnDeathPrefabs;

	private bool _alive;

	void Start () 
	{
		_alive = true;
	}
	
	public void Kill()
	{
		if(!_alive) return; // Already dead yo
		
		_alive = false;
		GetComponent<RobotController>().enabled = false;

		foreach(var prefab in OnDeathPrefabs)
		{
			Instantiate(prefab, transform.position, Quaternion.identity);
		}
	}
	
	void Update () 
	{
		
	}
}
