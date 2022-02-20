using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Rnd = UnityEngine.Random;

public class SuspiciousWiresScript : MonoBehaviour
{
    public KMBombModule Module;
    public KMBombInfo BombInfo;
    public KMAudio Audio;

    public KMSelectable[] LeftSels;
    public KMSelectable[] RightSels;
    public GameObject[] WireObjs;

    private int _moduleId;
    private static int _moduleIdCounter = 1;
    private bool _moduleSolved;

    private Coroutine _wirePositioning;
    private bool[] _isHighlighting = new bool[4];
    private bool _isModFocused;

    public GameObject m_Cube;
    public float m_DistanceZ;

    private void Start()
    {
        _moduleId = _moduleIdCounter++;
        Module.GetComponent<KMSelectable>().OnFocus += () => _isModFocused = true;
        Module.GetComponent<KMSelectable>().OnDefocus += () => _isModFocused = false;
        for (int i = 0; i < LeftSels.Length; i++)
        {
            int j = i;
            LeftSels[i].OnInteract += delegate () { LeftPress(j); return false; };
            LeftSels[i].OnInteractEnded += delegate () { LeftRelease(j); };
            RightSels[i].OnHighlight += delegate () { RightHighlight(j); };
            RightSels[i].OnHighlight += delegate () { RightHighlightEnded(j); };
        }
    }

    private void Update()
    {
        if (_isModFocused && !_isHighlighting.Contains(true))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var enter = 0.0f;
            var allHit = Physics.RaycastAll(ray);
            var plane = new Plane(transform.up, -0.1f * transform.lossyScale.y);
            if (plane.Raycast(ray, out enter))
            {
                Vector3 hitPoint = ray.GetPoint(enter);
                m_Cube.transform.position = hitPoint;
            }
        }
    }

    private void LeftPress(int i)
    {
        WireObjs[i].SetActive(true);
        _wirePositioning = StartCoroutine(WirePositioning(i));
        Debug.LogFormat("[Sussy Wires #{0}] Pressed button {1}.", _moduleId, i);
    }

    private void LeftRelease(int i)
    {
        WireObjs[i].SetActive(false);
        if (_wirePositioning != null)
            StopCoroutine(_wirePositioning);
    }

    private void RightHighlight(int i)
    {
        _isHighlighting[i] = true;
    }

    private void RightHighlightEnded(int i)
    {
        _isHighlighting[i] = false;
    }

    private IEnumerator WirePositioning(int wire)
    {
        while (true)
        {
            yield return null;
        }
    }
}
