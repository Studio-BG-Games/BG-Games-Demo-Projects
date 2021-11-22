using UnityEngine;
using Firebase;
using Firebase.Analytics;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager Instance;
    public virtual void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            if (Application.isPlaying)
                DontDestroyOnLoad(gameObject);//DontDestroyOnLoad() does not call Awake() or Start() when a new scene is loaded
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
            
            FirstOpen();
        });
    }

    private void FirstOpen()
    {
        print("push EventAppOpen");
        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventAppOpen);
    }

    public void PushPlayerOutOfFuelEvent(float distance)//check if distance int or float type
    {
        print("push PlayerOutOfFuel");
        FirebaseAnalytics.LogEvent("PlayerOutOfFuel", new Parameter("distance", distance));
    }

    public void PushPlayerCrashedEvent(float distance)
    {
        print("push PlayerCrashed");
        FirebaseAnalytics.LogEvent("PlayerCrashed", new Parameter("distance", distance));
    }

    public void PushPlayerShoppedEvent(string item)
    {
        print("push PlayerShopped");
        FirebaseAnalytics.LogEvent("PlayerShopped", new Parameter("item", item));
    }
}
