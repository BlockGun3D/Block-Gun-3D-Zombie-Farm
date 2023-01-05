using System;
using UnityEngine;

[Serializable]
public class SetTutorialDone : MonoBehaviour
{
	public virtual void SetTutorialDoneVoid(bool activated)
	{
		if (activated)
		{
			Global.gm.SetTutorialDone(1);
		}
	}

	public virtual void Main()
	{
	}
}
