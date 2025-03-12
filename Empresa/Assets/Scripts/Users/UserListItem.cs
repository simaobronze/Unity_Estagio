using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UserListItem : MonoBehaviour, IPointerClickHandler
{
    private Sprite PFPSprite;
    private User User { get; set; }
    private ulong _streamId;
    private UserListing _userListing;
    private Toggle _toggle;
    [SerializeField] private Image _pfp;
    [SerializeField] private TextMeshProUGUI _userName;
    [SerializeField] private Boolean _toggleSensors = false;
    [SerializeField] private Boolean _toggleStream = false;
    [SerializeField] private StreamsScriptableObject _streamsStore;

    // Start is called before the first frame update
    void Start()
    {
        _userListing = FindObjectOfType<UserListing>();
        if (!TryGetComponent(out _toggle))
        {
            Debug.LogError("Failed to get Toggle component");
            return;
        }
    }

    private void OnEnable()
    {
        _streamsStore.activeStreamUpdatedEvent.AddListener(HandleNewActiveStream);
    }

    private void OnDisable()
    {
        _streamsStore.activeStreamUpdatedEvent.RemoveListener(HandleNewActiveStream);
    }

    private void HandleNewActiveStream(ulong streamId)
    {
        _toggle.isOn = streamId == _streamId;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!_toggleSensors && !_toggleStream)
        {
            return;
        }
        if (_toggleStream)
        {
            _streamsStore.SetActiveStream(User, _streamId);
            //_streamsStore.AddStream(User, 999);
            return;
        }
        _userListing.ShowUser(User);
    }

    public void SetUser(User user)
    {
        User = user;
        _userName.text = User.name;
        if (user.streams.Count > 0)
        {
            _streamId = user.streams[0].stream_id;
        }
        string url = User?.data_configs?.pfp?.url;
        StartCoroutine(UsersController.FetchUserPFP(url, (sprite) =>
        {
            PFPSprite = sprite;
            _pfp.sprite = PFPSprite;
        }));
    }
}
