using System.Runtime.CompilerServices;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow Instance { get; private set; }
    [SerializeField] protected Transform m_anchor;
    [SerializeField] protected Transform m_background;
    [SerializeField] protected Transform m_foreground;
    [SerializeField, Range(0, 2)] protected float m_backgroundFollowingScaleX = 0.9f;
    [SerializeField, Range(0, 2)] protected float m_backgroundFollowingScaleY = 0.8f;
    [SerializeField, Range(0, 2)] protected float m_foregroundFollowingScaleX = 1.2f;
    [SerializeField, Range(0, 2)] protected float m_foregroundFollowingScaleY = 1.32f;
    [SerializeField] protected float m_followSpeed = 1f;

    public Transform Anchor
    {
        get => m_anchor;
        set
        {
            if (value is null)
            {
                // Changes values if field become NULL
                m_translationType = TranslationType.None;
                enabled = false;
            }
            else
            {
                // Changes values if field updates NOT NULL
                if (m_background != null)
                {
                    m_translationType = m_foreground != null ? TranslationType.AllTransforms : TranslationType.Background;
                }
                else m_translationType = m_foreground != null ? TranslationType.Foreground : TranslationType.OwnOnly;
                enabled = true;
            }
            m_anchor = value;
        }
    }
    public Transform Background
    {
        get => m_background;
        set
        {
            if (value is null)
            {
                // Changes values if field become NULL
                switch (m_translationType)
                {
                    case TranslationType.AllTransforms:
                        m_translationType = TranslationType.Foreground;
                        break;
                    case TranslationType.Background:
                        m_translationType = TranslationType.OwnOnly;
                        break;
                }
            }
            else
            {
                // Changes values if field become NOT NULL
                switch (m_translationType)
                {
                    case TranslationType.Foreground:
                        m_translationType = TranslationType.AllTransforms;
                        break;
                    case TranslationType.OwnOnly:
                        m_translationType = TranslationType.Background;
                        break;
                }
            }
            m_background = value;
        }
    }
    public Transform Foreground
    {
        get => m_foreground;
        set
        {
            if (value is null)
            {
                // Changes values if field become NULL
                switch (m_translationType)
                {
                    case TranslationType.AllTransforms:
                        m_translationType = TranslationType.Background;
                        break;
                    case TranslationType.Foreground:
                        m_translationType = TranslationType.OwnOnly;
                        break;
                }
            }
            else
            {
                // Changes values if field become NOT NULL
                switch (m_translationType)
                {
                    case TranslationType.Background:
                        m_translationType = TranslationType.AllTransforms;
                        break;
                    case TranslationType.OwnOnly:
                        m_translationType = TranslationType.Foreground;
                        break;
                }
            }
            m_foreground = value;
        }
    }

    TranslationType m_translationType = TranslationType.None;
    Vector3 initialPosition;
    private void Awake()
    {
        Instance = this;
        initialPosition = transform.position;
        DefineTransition();
    }
    private void DefineTransition()
    {
        if (m_anchor)
        {
            if (m_background != null)
            {
                m_translationType = m_foreground != null ? TranslationType.AllTransforms : TranslationType.Background;
            }
            else m_translationType = m_foreground != null ? TranslationType.Foreground : TranslationType.OwnOnly;
            enabled = true;
        }
        else
        {
            m_translationType = TranslationType.None;
            enabled = false;
        }
    }
    private void FixedUpdate()
    {
        switch (m_translationType)
        {
            case TranslationType.AllTransforms:
                OwnTransform();
                TransformBackground();
                TransformForeground();
                break;

            case TranslationType.Background:
                OwnTransform();
                TransformBackground();
                break;
            case TranslationType.Foreground:
                OwnTransform();
                TransformForeground();
                break;
            case TranslationType.OwnOnly:
                OwnTransform();
                break;
        }
    }
    private void OwnTransform()
        => transform.localPosition += new Vector3(m_anchor.position.x - transform.localPosition.x, m_anchor.position.y - transform.localPosition.y) * (m_followSpeed * Time.fixedDeltaTime);
    private void TransformBackground()
        => m_background.localPosition = new Vector3(x: (transform.localPosition.x - initialPosition.x) * m_backgroundFollowingScaleX,
                                                 y: (transform.localPosition.y - initialPosition.y) * m_backgroundFollowingScaleY,
                                                 z: m_background.localPosition.z);
    private void TransformForeground()
        => m_foreground.localPosition = new Vector3(x: (transform.localPosition.x - initialPosition.x) * m_foregroundFollowingScaleX,
                                                 y: (transform.localPosition.y - initialPosition.y) * m_foregroundFollowingScaleY,
                                                 z: m_foreground.localPosition.z);

    /// <summary>
    /// Stores info about what update method may be used for this time
    /// </summary>
    private enum TranslationType : byte
    {
        None,
        OwnOnly,
        AllTransforms,

        // Note that all other transforms does not works if main transforms will not updates.
        // That why this arguments is missing.
        Background,
        Foreground,
    }
}
