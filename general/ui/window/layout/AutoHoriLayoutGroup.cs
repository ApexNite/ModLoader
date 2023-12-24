using UnityEngine;
using UnityEngine.UI;

namespace NeoModLoader.General.UI.Window.Layout;

public class AutoHoriLayoutGroup : AutoLayoutGroup<HorizontalLayoutGroup>
{
    public ContentSizeFitter fitter { get; private set; }
    public HorizontalLayoutGroup layout { get; private set; }
    protected override void Init()
    {
        if (Initialized) return;
        Initialized = true;
        fitter = gameObject.GetComponent<ContentSizeFitter>();
        layout = gameObject.GetComponent<HorizontalLayoutGroup>();
    }
    public void Setup(Vector2 pSize = default, TextAnchor pAlignment = TextAnchor.MiddleLeft, float pSpacing = 3, RectOffset pPadding = null)
    {
        Init();
        if (pSize == default)
        {
            fitter.enabled = true;
        }
        else
        {
            fitter.enabled = false;
            GetComponent<RectTransform>().sizeDelta = pSize;
        }
        layout.childAlignment = pAlignment;
        layout.spacing = pSpacing;
        layout.padding = pPadding ?? new RectOffset(3, 3, 3, 3);
    }
    internal static void _init()
    {
        GameObject game_object =
            new(nameof(AutoHoriLayoutGroup), typeof(HorizontalLayoutGroup), typeof(AutoHoriLayoutGroup), typeof(ContentSizeFitter));

        ContentSizeFitter fitter = game_object.GetComponent<ContentSizeFitter>();
        fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;

        var layout_group = game_object.GetComponent<HorizontalLayoutGroup>();
        layout_group.childAlignment = TextAnchor.MiddleLeft;
        layout_group.childControlHeight = false;
        layout_group.childControlWidth = false;
        layout_group.childForceExpandHeight = false;
        layout_group.childForceExpandWidth = false;
        layout_group.childScaleHeight = false;
        layout_group.childScaleWidth = false;
        layout_group.spacing = 3;
        layout_group.padding = new RectOffset(3, 3, 3, 3);

        Prefab = game_object.GetComponent<AutoHoriLayoutGroup>();
    }
}