using System.Collections.Generic;
using UnityEngine;

namespace Game
{
	public class AssetLoader : MonoBehaviour
	{
		[SerializeField] private List<Config> _configs = new ();

		public Config LoadConfig(string configId)
		{
			var currentConfig = _configs.Find(x => x.Id.Equals(configId));
			if (currentConfig != null) return currentConfig;
			return null;
		}
	}
}