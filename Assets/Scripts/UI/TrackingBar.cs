using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrackingBar : MonoBehaviour
{
    [field:SerializeField] public GameObject TrackPoint {get ; private set;}
    [field:SerializeField] public Slider FillBar {get ; private set;}
    
    private List<GameObject> _trackPoints = new List<GameObject>();
    
    public float FillBarWidth => FillBar.GetComponent<RectTransform>().rect.width;
    public void AddNewTrackPoint(Vector2 position)
    {
       GameObject newTrackPoint = Instantiate(TrackPoint.gameObject, transform);
       _trackPoints.Add(newTrackPoint);
       newTrackPoint.TryGetComponent(out RectTransform rectTransform);
       rectTransform.anchoredPosition += position;
    }
    
    public void ClearTrackPoints()
    {
        foreach (GameObject trackPoint in _trackPoints)
        {
            Destroy(trackPoint);
        }
        _trackPoints.Clear();
    }
    
} 
