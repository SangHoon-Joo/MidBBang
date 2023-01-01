using UnityEngine;

public static class TransformExtension
{
	public static Transform FindRecursively(this Transform transform, string name)
	{
		if(transform.name == name)
			return transform;

		foreach(Transform childTransform in transform)
		{
			Transform resultTransform = childTransform.FindRecursively(name);
			if(resultTransform != null)
				return resultTransform;
		}

		return null;
	}

	public static void ChangeLayerRecursively(this Transform transform, string layerName)
	{
		int layer = LayerMask.NameToLayer(layerName);
		transform.gameObject.layer = layer;
		foreach(Transform childTransform in transform)
		{
			childTransform.ChangeLayerRecursively(layer);
		}
	}

	public static void ChangeLayerRecursively(this Transform transform, int layer)
	{
		transform.gameObject.layer = layer;
		foreach(Transform childTransform in transform)
		{
			childTransform.ChangeLayerRecursively(layer);
		}
	}
}
