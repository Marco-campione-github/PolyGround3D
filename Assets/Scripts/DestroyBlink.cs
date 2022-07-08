using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
	public class DestroyBlink : MonoBehaviour
	{

		// Use this for initialization
		void Start()
		{
			Destroy(gameObject, 0.5f);
		}
	}
}