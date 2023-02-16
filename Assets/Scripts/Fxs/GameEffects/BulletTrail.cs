using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class BulletTrail : MonoBehaviour
{
    public readonly List<BulletTrail> Trails = new();
    public bool IsDisabled => !gameObject.activeSelf;

    [SerializeField] LineRenderer m_lineRenderer;
    [SerializeField] float m_trailFadeoutSpeed = 10f;


    float time;
    /// <summary>
    /// Instantiates bullet prefab in real world, in given position and with goven parameters
    /// </summary>
    /// <param name="source">Point, relative to local space, from where bullet is moving</param>
    /// <param name="dest">Point, relative to local space, to where bullet will move</param>
    /// <param name="parent">Parent of created bullet</param>
    public void Create(Vector3 source, Vector3 dest, Color color, Transform parent = null)
    {
        foreach (BulletTrail bullet in Trails)
        {
            if (bullet is null) continue;
            if (bullet.IsDisabled)
            {
                bullet.Init(source, dest, color, parent);
                return;
            }
        }
        BulletTrail trail = Instantiate(gameObject, parent).GetComponent<BulletTrail>();
        Trails.Add(trail);
        trail.Init(source, dest, color, parent);
    }
    public void Init(Vector3 source, Vector3 dest, Color color, Transform parent)
    {
        transform.SetParent(parent, false);
        m_lineRenderer.SetPosition(0, source);
        m_lineRenderer.SetPosition(1, dest);
        m_lineRenderer.material.SetColor("_Color", color.Set_A(1f));

        time = 1;
        gameObject.SetActive(true);
    }
    void Update()
    {
        time -= Time.deltaTime * m_trailFadeoutSpeed;

        if (time > 0)
        {
            m_lineRenderer.material.SetColor("_Color", m_lineRenderer.material.color.Set_A(time));
        }
        else Disable();
    }
    private void Disable() => gameObject.SetActive(false);
}