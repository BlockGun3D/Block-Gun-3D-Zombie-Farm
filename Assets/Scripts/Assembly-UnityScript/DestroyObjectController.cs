using System;
using UnityEngine;

[Serializable]
public class DestroyObjectController : MonoBehaviour
{
	public ObjectiveManager objectiveMan;

	public virtual void Die()
	{
		objectiveMan.ObjectDestroyed();
	}

	public virtual void Main()
	{
	}
}
