using System.Collections.Generic;
using UnityEngine;

public class GhostTrailManager : MonoBehaviour
{
    // TODO: 
    //  - OPTIMIZATION: 
    //      Your pooling is ass, the ghosts remain after the object had died
    //      https://youtu.be/PO46vRZF7VI?si=OP-DvRpOJmGivoKv

    [SerializeField, Min(0)] int _count = 4;
    [SerializeField, Min(0)] float _fadeoutSpeed = 1f;
    [SerializeField, Min(1)] int _ghostsToCache = 16;
    
    [Space]
    [SerializeField] Transform _ghostsParent;

    List<GhostTrailUnit> _ghosts;

    public void InitializeTrail(Sprite sprite, Color color, Vector3 scale)
    {
        if (_ghosts != null && _ghosts.Count > 0)
        {
            foreach (GhostTrailUnit ghost in _ghosts)
                Destroy(ghost.gameObject);
        }

        _ghosts = new List<GhostTrailUnit>();
        for (int i = 0; i < _ghostsToCache; i++)
        {
            GhostTrailUnit unit = new GameObject("Ghost " + i).AddComponent<GhostTrailUnit>();
            SpriteRenderer renderer = unit.gameObject.AddComponent<SpriteRenderer>();
            renderer.sprite = sprite;
            color.a = 0;
            renderer.color = color;
            unit.SpriteRenderer = renderer;
            unit.FadeoutSpeed = _fadeoutSpeed; 
            unit.gameObject.transform.localScale = scale;
            unit.gameObject.transform.SetParent(_ghostsParent);
            _ghosts.Add(unit);
        }
    }

    public void SpawnTrail(Vector2 start, Vector2 end, bool includeExtraGhost = true)
    {
        int count = includeExtraGhost ? _count + 1 : _count; 
        List<GhostTrailUnit> units = GetInactiveUnits(_count);
        float distance = Vector2.Distance(start, end); 
        Vector2 direction = (end - start).normalized;

        float oppacityDifference = 1f / count;
        float distanceDifference = distance / count;
        
        count--;
        for (int i = 0; i <= count; i++)
        {
            GhostTrailUnit unit = units[i];
            if (unit == null)
                continue;
            
            unit.SetAlpha(1 - oppacityDifference * i);
            int distanceMultiplier = includeExtraGhost ? i + 1 : i;
            unit.transform.position = start + direction * distanceDifference * distanceMultiplier;
            unit.Fadeout();
        }
    }

    List<GhostTrailUnit> GetInactiveUnits(int count)
    {
        List<GhostTrailUnit> units = new List<GhostTrailUnit>();
        if (count < 0)
            return units;
        
        foreach (GhostTrailUnit unit in _ghosts)
        {
            if (!unit.IsActive)
                units.Add(unit);
        }

        return units;
    }
}
