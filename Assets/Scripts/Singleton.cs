using UnityEngine;
using System.Collections;

public class Singleton<Instance> : MonoBehaviour where Instance : Singleton<Instance> {
	public static Instance instance;
	public bool isPersistant;
	
	public virtual void Awake() {
		if(isPersistant) {
			if(!instance) {
				instance = this as Instance;
				DontDestroyOnLoad(gameObject);
			}
			else {
				DestroyImmediate(gameObject);
			}
		}
		else {
            if (!instance)
            {
                instance = this as Instance;
            }
        }
	}
}