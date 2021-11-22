using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoPool<T> where T : MonoBehaviour
{
	public T Prefab { get; }
	public bool IsAutoExpand { get; set; }
	public Transform Container { get; }
	private List<T> _pool;

	public MonoPool(T prefab, int count)
	{
		Prefab = prefab;
		Container = null;

		CreatePool(count: count);
	}

	public MonoPool(T prefab, int count, Transform container)
	{
		Prefab = prefab;
		Container = container;

		CreatePool(count: count);
	}

	private void CreatePool(int count)
	{
		_pool = new List<T>();

		for (int i = 0; i < count; i++)
		{
			CreateObject(isActiveByDefault: false);
		}
	}

	private T CreateObject(bool isActiveByDefault = false)
	{
		var createdObject = Object.Instantiate(Prefab, Container);
		createdObject.gameObject.SetActive(isActiveByDefault);
		_pool.Add(createdObject);
		return createdObject;

	}

	public bool HasFreeElement(out T element)
	{

		foreach (var mono in _pool)
		{
			if (!mono.gameObject.activeInHierarchy)
			{
				element = mono;
				mono.gameObject.SetActive(true);
				return true;
			}
		}
		element = null;
		return false;
	}

	public bool HasBusyElement(out T element)
	{
		foreach (var mono in _pool)
		{
			if (mono.gameObject.activeInHierarchy)
			{
				element = mono;
				//mono.gameObject.SetActive(false);
				return true;
			}
		}

		element = null;
		return false;
	}

	public T GetFreeElement()
	{
		if (HasFreeElement(element: out var element))
		{
			return element;
		}

		if (IsAutoExpand)
		{
			return CreateObject(true);
		}

		throw new System.Exception($"There is no free elements in pool of type {typeof(T)}");
	}
}
