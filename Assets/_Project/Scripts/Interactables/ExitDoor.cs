using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    [SerializeField] private GameObject _leftDoor;
    [SerializeField] private GameObject _rightDoor;
    [SerializeField] private float _doorMoveDistance;
    [SerializeField] private float _doorMoveSpeed;
    [SerializeField] private bool _startOpen;
    [SerializeField] private GameObject[] _powerSources;
    private Vector3 _leftStart;
    private Vector3 _rightStart;
    private bool _doorOpening;
    private bool _doorClosing;

    // Start is called before the first frame update
    void Start()
    {
        _leftStart = _leftDoor.transform.position;
        _rightStart = _rightDoor.transform.position;
        _doorOpening = false;
        _doorClosing = false;

        if (_startOpen)
        {
            StartCoroutine(IOpenDoor());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_powerSources.Length > 0)
        {
            int num = 0;
            foreach (GameObject source in _powerSources)
            {
                if (source.GetComponent<OnOff>().GetStatus())
                {
                    num++;
                }
                else
                {
                    num = 0;
                }
            }

            if (num == _powerSources.Length && !_doorOpening)
            {
                if (_doorClosing)
                {
                    StopCoroutine(ICloseDoor());
                }
                StartCoroutine(IOpenDoor());
            }
            else
            {
                if (_doorOpening)
                {
                    StopCoroutine(IOpenDoor());
                }
                StartCoroutine(ICloseDoor());
            }
        }
    }

    private IEnumerator IOpenDoor()
    {
        Vector3 leftEnd = new Vector3(_leftDoor.transform.localPosition.x + _doorMoveDistance, _leftDoor.transform.localPosition.y, _leftDoor.transform.localPosition.z);
        Vector3 rightEnd = new Vector3(_rightDoor.transform.localPosition.x - _doorMoveDistance, _rightDoor.transform.localPosition.y, _rightDoor.transform.localPosition.z);

        _doorOpening = true;
        while (_leftDoor.transform.localPosition != leftEnd && _rightDoor.transform.localPosition != rightEnd)
        {
            Debug.Log("opening");
            _leftDoor.transform.localPosition = Vector3.MoveTowards(_leftDoor.transform.localPosition, leftEnd, Time.deltaTime * _doorMoveSpeed);
            _rightDoor.transform.localPosition = Vector3.MoveTowards(_rightDoor.transform.localPosition, rightEnd, Time.deltaTime * _doorMoveSpeed);
            yield return null;
        }
        _doorOpening = false;
    }

    private IEnumerator ICloseDoor()
    {
        _doorClosing = true;
        while (_leftDoor.transform.localPosition != _leftStart && _rightDoor.transform.localPosition != _rightStart)
        {
            _leftDoor.transform.localPosition = Vector3.MoveTowards(_leftDoor.transform.localPosition, _leftStart, Time.deltaTime * _doorMoveSpeed);
            _rightDoor.transform.localPosition = Vector3.MoveTowards(_rightDoor.transform.localPosition, _rightStart, Time.deltaTime * _doorMoveSpeed);
            yield return null;
        }
        _doorClosing = false;
    }
}
