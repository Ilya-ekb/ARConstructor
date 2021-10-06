using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataScripts;

using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities;
using TMPro;
using UnityEditor;
using UnityEngine;

public class ScrollList : MonoBehaviour, IFeatureInterface
{
    public bool setLabel = false;
    [SerializeField]
    private TextMeshPro description;

    public string DescriptionText
    {
        get { return description.text; }
        set { description.text = value; }
    }

    private string[] buttonNames;
    public string[] ButtonNames
    {
        get { return buttonNames; }
        set { buttonNames = value; }
    }

    private Coroutine animationCor = null;

    [SerializeField]
    [Tooltip("The ScrollingObjectCollection to populate, if left empty. the populator will create on your behalf.")]
    private ScrollingObjectCollection scrollView;

    /// <summary>
    /// The ScrollingObjectCollection to populate, if left empty. the populator will create on your behalf.
    /// </summary>
    public ScrollingObjectCollection ScrollView
    {
        get { return scrollView; }
        set { scrollView = value; }
    }

    [SerializeField]
    [Tooltip("Object to duplicate in ScrollCollection")]
    private GameObject dynamicItem;

    /// <summary>
    /// Object to duplicate in <see cref="ScrollView"/>. 
    /// </summary>
    public GameObject DynamicItem
    {
        get { return dynamicItem; }
        set { dynamicItem = value; }
    }

    [SerializeField]
    [Tooltip("Number of items to generate")]
    private int numItems;

    /// <summary>
    /// Number of items to generate
    /// </summary>
    public int NumItems
    {
        get { return numItems; }
        set { numItems = value; }
    }

    [SerializeField]
    [Tooltip("Demonstrate lazy loading")]
    private bool lazyLoad;

    /// <summary>
    /// Demonstrate lazy loading 
    /// </summary>
    public bool LazyLoad
    {
        get { return lazyLoad; }
        set { lazyLoad = value; }
    }

    [SerializeField]
    [Tooltip("Number of items to load each frame during lazy load")]
    private int itemsPerFrame = 3;

    /// <summary>
    /// Number of items to load each frame during lazy load 
    /// </summary>
    public int ItemsPerFrame
    {
        get { return itemsPerFrame; }
        set { itemsPerFrame = value; }
    }

    [SerializeField]
    [Tooltip("Indeterminate loader to hide / show for LazyLoad")]
    private GameObject loader;

    [SerializeField]
    private float cellWidth = 0.032f;

    [SerializeField]
    private float cellHeight = 0.032f;

    [SerializeField]
    private float cellDepth = 0.032f;

    [SerializeField]
    private int cellsPerTier = 3;

    [SerializeField]
    private int tiersPerPage = 5;

    [SerializeField]
    private Transform scrollPositionRef = null;

    private GridObjectCollection gridObjectCollection;

    private IBaseHologramObject parent;

    /// <summary>
    /// Indeterminate loader to hide / show for <see cref="LazyLoad"/> 
    /// </summary>
    public GameObject Loader
    {
        get => loader;
        set => loader = value;
    }

    private void OnEnable()
    {
        // Make sure we find a collection
        if (scrollView == null)
        {
            scrollView = GetComponentInChildren<ScrollingObjectCollection>();
        }
    }

    public void Setting(HologramEditor editor)
    {
        MakeScrollingList();
    }

    public void Dispose() { }

    public void MakeScrollingList()
    {
        if (buttonNames != null && buttonNames.Length > 0)
        {
            numItems = buttonNames.Length;
        }
        else
        {
            //numItems = Data.Scenario1Obj.Count;
            //buttonNames = new string[numItems + 1];
            //for (var i = 0; i < Data.Scenario1Obj.Count; i++)
            //{
            //    buttonNames[i] = Data.Scenario1Obj.ElementAt(i).Key;
            //}
        }
        if (scrollView == null)
        {
            GameObject newScroll = new GameObject("Scrolling Object Collection");
            newScroll.transform.parent = scrollPositionRef ? scrollPositionRef : transform;
            newScroll.transform.localPosition = Vector3.zero;
            newScroll.transform.localRotation = Quaternion.identity;
            newScroll.SetActive(false);
            scrollView = newScroll.AddComponent<ScrollingObjectCollection>();

            // Prevent the scrolling collection from running until we're done dynamically populating it.
            scrollView.CellWidth = cellWidth * 2;
            scrollView.CellHeight = cellHeight * 2;
            scrollView.CellDepth = cellDepth;
            scrollView.CellsPerTier = cellsPerTier;
            scrollView.TiersPerPage = tiersPerPage;
        }

        gridObjectCollection = scrollView.GetComponentInChildren<GridObjectCollection>();
        if (gridObjectCollection != null) Destroy(gridObjectCollection.gameObject);
        if (gridObjectCollection == null)
        {
            GameObject collectionGameObject = new GameObject("Grid Object Collection");
            collectionGameObject.transform.position = scrollView.transform.position;
            collectionGameObject.transform.rotation = scrollView.transform.rotation;

            gridObjectCollection = collectionGameObject.AddComponent<GridObjectCollection>();
            gridObjectCollection.CellWidth = cellWidth;
            gridObjectCollection.CellHeight = cellHeight;
            gridObjectCollection.SurfaceType = ObjectOrientationSurfaceType.Plane;
            gridObjectCollection.Layout = LayoutOrder.ColumnThenRow;
            gridObjectCollection.Columns = cellsPerTier;
            gridObjectCollection.Anchor = LayoutAnchor.UpperLeft;

            scrollView.AddContent(collectionGameObject);
        }

        if (!lazyLoad)
        {
            if (!setLabel)
            {
                for (int i = 0; i < numItems; i += 2)
                {
                    MakeItem(dynamicItem, buttonNames[i], buttonNames[i + 1]);
                }
            }
            else
            {
                for (int i = 0; i < numItems; i++)
                {
                    MakeItem(dynamicItem, buttonNames[i]);
                }
            }
            scrollView.gameObject.SetActive(true);
            gridObjectCollection.UpdateCollection();
        }
        else
        {
            if (loader != null)
            {
                loader.SetActive(true);
            }

            StartCoroutine(UpdateListOverTime(loader, itemsPerFrame));
        }
        //scrollView.gameObject.GetComponent<BoxCollider>().enabled = false;
    }

    private IEnumerator UpdateListOverTime(GameObject loaderViz, int instancesPerFrame)
    {
        for (int currItemCount = 0; currItemCount < numItems; currItemCount += 2)
        {
            for (int i = 0; i < instancesPerFrame; i++)
            {
                MakeItem(dynamicItem, buttonNames[currItemCount], buttonNames[currItemCount + 1]);
            }
            yield return null;
        }

        // Now that the list is populated, hide the loader and show the list
        loaderViz.SetActive(false);
        scrollView.gameObject.SetActive(true);

        // Finally, manually call UpdateCollection to set up the collection
        gridObjectCollection.UpdateCollection();
    }


    public void Activate(params object[] vs)
    {
        var offset = 1;
        DescriptionText = vs[0].ToString();
        if (vs.Length - offset % 2 == 0) { parent = Data.Instance.GetBaseHologramObject(vs[vs.Length - 1].ToString()); }
        var items = parent == null ? vs.Length - offset : vs.Length - offset - 1;
        buttonNames = new string[items];
        for (var i = 0; i < buttonNames.Length; i++)
        {
            buttonNames[i] = vs[i + offset].ToString();
        }
        if (animationCor != null)
        {
            StartCoroutine(Waiting(Vector3.one));
        }
        else
        {
            animationCor = StartCoroutine(ActivateAnimation(Vector3.one));
        }
    }

    public void Deactive()
    {
        animationCor = StartCoroutine(ActivateAnimation(Vector3.zero));
        if (parent == null)
        {
            return;
        }
        parent.ToolTip.gameObject.SetActive(false);
        HologramController.Instance.HologramObjectsSmoothAlpha.SetInvisible(parent);
    }
    private IEnumerator Waiting(Vector3 target)
    {
        while (animationCor != null)
        {
            yield return null;
        }
        animationCor = StartCoroutine(ActivateAnimation(target));
    }

    private IEnumerator ActivateAnimation(Vector3 target)
    {
        while (true)
        {
            if (Vector3.Distance(gameObject.transform.localScale, target) > 0.01f)
            {
                gameObject.transform.localScale = Vector3.Lerp(gameObject.transform.localScale, target, .3f);
                yield return null;
            }
            else
            {
                gameObject.transform.localScale = target;
                if (target.Equals(Vector3.one)) MakeScrollingList();
                else Destroy(this.gameObject);
                animationCor = null;
                yield break;
            }
        }
    }

    object[] objects = new object[1];
    private void MakeItem(GameObject item, string buttonName, string funcName = "")
    {
        GameObject itemInstance = Instantiate(item, gridObjectCollection.transform);
        //if (item.GetComponent<ID_Editor>() != null)
        //{

        //    var id_editor = item.GetComponent<ID_Editor>();
        //    id_editor.ID_Label = buttonName;
        //}
        //else
        //{
        //    var buttonHelper = itemInstance.GetComponent<ButtonConfigHelper>();
        //    buttonHelper.MainLabelText = buttonName;
        //    buttonHelper.OnClick.AddListener(() =>
        //    {
        //        Data.Logic.InvokeFunction(funcName, objects);
        //        Deactive();
        //    });
        //}
        //itemInstance.SetActive(true);
    }
}
