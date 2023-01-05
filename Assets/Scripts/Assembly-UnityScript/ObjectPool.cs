using System;
using UnityEngine;

[Serializable]
public class ObjectPool : MonoBehaviour
{
	public GameObject cachedObject;

	public int poolSize;

	private GameObject[] pool;

	private int pointer;

	private Transform thisTransform;

	private int temp;

	public virtual void Start()
	{
		thisTransform = transform;
		pool = new GameObject[poolSize];
		for (int i = 0; i < poolSize; i++)
		{
			pool[i] = (GameObject)UnityEngine.Object.Instantiate(cachedObject, Vector3.zero, Quaternion.identity);
			pool[i].transform.parent = thisTransform;
			ResetPoolObject(pool[i]);
		}
		pointer = 0;
	}

	public virtual GameObject GetObject(Vector3 pos, Quaternion rot)
	{
		pool[pointer].transform.position = pos;
		pool[pointer].transform.rotation = rot;
		pool[pointer].SetActive(true);
		temp = pointer;
		pointer = (pointer + 1) % poolSize;
		return pool[temp];
	}

	public virtual void ReturnObject(GameObject theObject)
	{
		ResetPoolObject(theObject);
	}

	private void ResetPoolObject(GameObject obj)
	{
		if ((bool)obj.GetComponent<Rigidbody>())
		{
			obj.GetComponent<Rigidbody>().velocity = Vector3.zero;
			obj.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
		}
		obj.SetActive(false);
	}

	public virtual void Main()
	{
	}
}
