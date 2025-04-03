using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class SessionManager : MonoBehaviour
{
    [SerializeField] string apiUrl = "https://mt-gateway.mobitrust.org/api";

    [SerializeField] UsersScriptableObject _usersStore;
    [SerializeField] MissionsScriptableObject _missionsStore;
    [SerializeField] StreamsScriptableObject _streamsStore;

    [SerializeField] GameObject map;



    UsersController _usersController;
    MissionsController _missionsController;
    StreamsController _streamsController;

    public string GetApiUrl() { return apiUrl; }

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        int numGameSessions = FindObjectsOfType<SessionManager>().Length;
        if (numGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!TryGetComponent(out _usersController) || !TryGetComponent(out _missionsController) || !TryGetComponent(out _streamsController))
        {
            Debug.LogError($"{MethodBase.GetCurrentMethod().Name} - Error getting controllers - users: {_usersController}, missions: {_missionsController}, streams: {_streamsController}");
            Application.Quit();
        }
        StartCoroutine(StartProcess());
    }

    IEnumerator StartProcess()
    {
        yield return StartCoroutine(_usersController.Login((isLoggedIn) =>
        {
            if (!isLoggedIn)
            {
                Debug.LogError("User could not login");
                return;
            }
            StartCoroutine(_usersController.FetchUsers((users) =>
            {
                _usersStore.FetchedUsers(users.ToDictionary(user => user.id, user => user));

            }));

            StartCoroutine(_missionsController.FetchMissions((missions) =>
            {
                _missionsStore.FetchedMissions(missions.ToDictionary(mission => mission.id, mission => mission));

            }));

            StartCoroutine(_streamsController.FetchStreams((streams) =>
            {
                _streamsStore.FetchedStreams(streams.ToDictionary(user => user.id, user => user));
            }));
        }));
    }

    public void UpdateUsers()
    {
        if (_usersStore.AuthenticatedUser == null)
        {
            Debug.LogError("User is not authenticated, couldn't update Users");
            return;
        }
        Debug.Log("USERS before coroutine");
        StartCoroutine(_usersController.FetchUsers((users) =>
        {
            _usersStore.FetchedUsers(users.ToDictionary(user => user.id, user => user));
        }
        ));
        Debug.Log("USERS debug -> users updated");
    }

    public void UpdateStreams()
    {
        if (_usersStore.AuthenticatedUser == null)
        {
            Debug.LogError("User is not authenticated, couldn't update Streams");
            return;
        }

        StartCoroutine(_streamsController.FetchStreams((streams) =>
        {
            _streamsStore.FetchedStreams(streams.ToDictionary(user => user.id, user => user));
        }
        ));
        Debug.Log("STREAMS debug -> Streams updated");
    }

    public void UpdateMissions()
    {
        if (_usersStore.AuthenticatedUser == null)
        {
            Debug.LogError("User is not authenticated, couldn't update Streams");
            return;
        }

        StartCoroutine(_missionsController.FetchMissions((missions) =>
        {
            _missionsStore.FetchedMissions(missions.ToDictionary(mission => mission.id, mission => mission));
        }
        ));

        Debug.Log("MISSIONS debug -> missions updated");
    }
}
