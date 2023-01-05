using System;
using UnityEngine;

[Serializable]
public class isPopup : MonoBehaviour
{
	public virtual void Start()
	{
	}

	public virtual void OnEnable()
	{
		Global.popupEnabled = true;
	}

	public virtual void Deactivate()
	{
		Global.popupEnabled = false;
		gameObject.SetActive(false);
	}

	public virtual void OnDisable()
	{
		Global.popupEnabled = false;
	}

	public virtual void Main()
	{
	}
}
