using UnityEngine;

public class GameInitial //: MonoBehaviour
{
	[RuntimeInitializeOnLoadMethod]
	static void OnRuntimeMethodLoad() {
		Screen.SetResolution(607, 1080, false, 60);

	}
}