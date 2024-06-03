using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveAxis
{
    X,
    Y,
    Z
}

public class SingleDoor : MonoBehaviour
{
    [SerializeField] private GameObject _door;
    [SerializeField] private float _doorMoveDistance;
    [SerializeField] private float _doorMoveSpeed;
    [SerializeField] private GameObject[] _powerSources;
    [SerializeField] private MoveAxis moveAxis = new MoveAxis();
    private Vector3 _start;
    private bool _doorOpening;
    private bool _doorClosing;
    private bool _open;
    private bool _close;

    private void Start()
    {
        _start = _door.transform.position;
        _doorClosing = false;
        _doorOpening = false;
        _open = false;
        _close = true;

        if (_powerSources.Length == 0)
        {
            StartCoroutine(IOpenDoor());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (AllSourcesOn())
        {
            if (_doorClosing)
            {
                StopCoroutine(ICloseDoor());
            }
            else if (!_open && !_doorOpening)
            {
                StartCoroutine(IOpenDoor());
            }
        }
        else
        {
            if (_doorOpening)
            {
                StopCoroutine(IOpenDoor());
            }
            else if (!_close && !_doorClosing)
            {
                StartCoroutine(ICloseDoor());
            }
        }
    }

    private IEnumerator IOpenDoor()
    {
        _close = false;
        _doorClosing = false;
        _doorOpening = true;
        Vector3 end = CalcEndLoc();

        while (_door.transform.position != end)
        {
            _door.transform.position = Vector3.MoveTowards(_door.transform.position, end, Time.deltaTime * _doorMoveSpeed);
            yield return null;
        }
        _doorOpening = false;
        _open = true;
    }

    private IEnumerator ICloseDoor()
    {
        _open = false;
        _doorOpening = false;
        _doorClosing = true;
        while (_door.transform.position != _start)
        {
            _door.transform.position = Vector3.MoveTowards(_door.transform.position, _start, Time.deltaTime * _doorMoveSpeed);
            yield return null;
        }
        _doorClosing = false;
        _close = true;
    }

    private bool AllSourcesOn()
    {
        foreach (GameObject source in _powerSources)
        {
            if (!source.GetComponent<OnOff>().GetStatus())
            {
                return false;
            }
        }
        return true;
    }

    private Vector3 CalcEndLoc()
    {
        Vector3 final;
        switch (moveAxis)
        {
            case MoveAxis.X:
                final = new Vector3(_door.transform.position.x + _doorMoveDistance, _door.transform.position.y, _door.transform.position.z);
                return final;

            case MoveAxis.Y:
                final = new Vector3(_door.transform.position.x, _door.transform.position.y + _doorMoveDistance, _door.transform.position.z);
                return final;
            case MoveAxis.Z:
                final = new Vector3(_door.transform.position.x, _door.transform.position.y, _door.transform.position.z + _doorMoveDistance);
                return final;
            default:
                break;
        }
        return new Vector3(0,0,0);
    }
}
