#if UNITY_ANDROID
namespace DeadMosquito.GoogleMapsView.Internal
{
	using System;
	using System.Collections.Generic;
	using UnityEngine;
	using Object = System.Object;

	internal class GoogleMapsSceneHelper : MonoBehaviour
	{
		static GoogleMapsSceneHelper _instance;
		static readonly object InitLock = new Object();
		readonly object _queueLock = new Object();
		readonly List<Action> _queuedActions = new List<Action>();
		readonly List<Action> _executingActions = new List<Action>();

		public static GoogleMapsSceneHelper Instance
		{
			get
			{
				if (_instance == null)
				{
					Init();
				}
				return _instance;
			}
		}

		internal static void Init()
		{
			lock (InitLock)
			{
				if (ReferenceEquals(_instance, null))
				{
					var instances = FindObjectsOfType<GoogleMapsSceneHelper>();

					if (instances.Length > 1)
					{
						Debug.LogError(typeof(GoogleMapsSceneHelper) + " Something went really wrong " +
						               " - there should never be more than 1 " + typeof(GoogleMapsSceneHelper) +
						               " Reopening the scene might fix it.");
					}
					else if (instances.Length == 0)
					{
						var singleton = new GameObject();
						_instance = singleton.AddComponent<GoogleMapsSceneHelper>();
						singleton.name = typeof(GoogleMapsSceneHelper).ToString();

						DontDestroyOnLoad(singleton);

						Debug.Log("[Singleton] An _instance of " + typeof(GoogleMapsSceneHelper) +
						          " is needed in the scene, so '" + singleton.name +
						          "' was created with DontDestroyOnLoad.");
					}
					else
					{
						Debug.Log("[Singleton] Using _instance already created: " + _instance.gameObject.name);
					}
				}
			}
		}

		GoogleMapsSceneHelper()
		{
		}

		internal static void Queue(Action action)
		{
			if (action == null)
			{
				Debug.LogWarning("Trying to queue null action");
				return;
			}

			lock (_instance._queueLock)
			{
				_instance._queuedActions.Add(action);
			}
		}

		void Update()
		{
			MoveQueuedActionsToExecuting();

			while (_executingActions.Count > 0)
			{
				Action action = _executingActions[0];
				_executingActions.RemoveAt(0);
				action();
			}
		}

		void MoveQueuedActionsToExecuting()
		{
			lock (_queueLock)
			{
				while (_queuedActions.Count > 0)
				{
					Action action = _queuedActions[0];
					_executingActions.Add(action);
					_queuedActions.RemoveAt(0);
				}
			}
		}
	}
}
#endif