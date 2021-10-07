using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataScripts;
using LogicScripts;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;
using TMPro;
using UnityEngine;
using Visualization;

public class Dialog : Singleton<Dialog>
{
    [SerializeField] private SpriteRenderer imageIn;
    [SerializeField] private SpriteRenderer imageOut;
    internal Transform Transform { get; set; }
    private Coroutine animationCor = null;
    private RadialView radial = null;
    private IBaseHologramObject parent = null;


    private Dictionary<ButtonType, Interactable> aButton;

    [SerializeField] private TextMeshPro contentText;
    [SerializeField] private TextMeshPro title;
    [SerializeField] private List<Interactable> buttons = new List<Interactable>();


    private string nameYesCommand;
    private string nameNoCommand;
    private object[] parameters = new object[1];

    /// <summary>
    /// Активация диалогового окна с кнопками
    /// </summary>
    /// <param name="objects[0]">Тип диалогового окна</param> 
    /// <param name="objects[1]">Информационное сообщение</param> 
    /// <param name="objects[2]">Функция в сценарии, вызываемая при положительном ответе</param> 
    /// <param name="objects[3]">Функция в сценарии, вызываемая при положительном ответе / Родительский объект (если InfoMessage)</param>  
    /// <param name="objects[4]">Родительский объект  / null (если InfoMessage)</param> 
    public void Activate(params object[] objects)
    {
        Vector3 scale = Vector3.one * .4f;
        imageIn.sprite = null;
        imageOut.sprite = null;
        if (objects.Length > 0)
        {
            //Общие настройки
            var type = (CommandType)objects[0];
            contentText.text = objects[1].ToString();
            nameYesCommand = objects[2].ToString();

            if (type == CommandType.Dialog)
            {
                Find(aButton, e => (ButtonType)Enum.Parse(typeof(ButtonType), e.name) == ButtonType.ButtonYes);
                Find(aButton, e => (ButtonType)Enum.Parse(typeof(ButtonType), e.name) == ButtonType.ButtonNo);
                nameNoCommand = objects[3].ToString();
            }
            else if (type == CommandType.InfoMessage)
            {
                Find(aButton, e => (ButtonType)Enum.Parse(typeof(ButtonType), e.name) == ButtonType.ButtonOk);
                imageIn.sprite = TryParseImageNo(3, objects);
            }

            TryParseScale(4, objects, ref scale);
            imageOut.sprite = TryParseImageNo(5, objects);

            try
            {
                parent = objects[objects.Length - 1] != default ? (IBaseHologramObject)objects[objects.Length - 1] : null;
            }
            catch
            {
            }


            if (!gameObject.activeSelf) gameObject.SetActive(true);
            StartAnimation(scale, true);
        }
        else
        {
            Debug.LogError($"None parameters in Activate() for {gameObject.name}");
        }
    }

    private Sprite TryParseImageNo(int offset, object[] objects)
    {
        if (objects.Length > offset)
        {
            if (int.TryParse(objects[offset].ToString(), out int imageNo))
            {
                if (imageNo > -1) return Data.Instance.images[imageNo];
            }
        }
        return null;
    }

    private void TryParseScale(int offset, object[] objects, ref Vector3 scale)
    {
        if (objects.Length > offset)
        {
            if (float.TryParse(objects[offset].ToString(), out float scaleMult))
            {
                scale = Vector3.one * scaleMult;
            }
        }
    }

    /// <summary>
    /// Событие при нажатие кнопки
    /// </summary>
    /// <param name="type"></param>В зависимости от типа вызывается соответствующее событие
    private void ButtonClickEvent(ButtonType type)
    {
        var command = (type == ButtonType.ButtonYes || type == ButtonType.ButtonOk) ? nameYesCommand : nameNoCommand;
        Deactivate();
        Logic.Instance.InvokeFunction(command, parameters);
    }

    /// <summary>
    /// Деактиванция диалогового окна
    /// </summary>
    private void Deactivate()
    {
        foreach (var button in aButton.Values) button.gameObject.SetActive(false);
        StartAnimation(Vector3.zero, false);
        //Если есть парент - запускаем анимацию исчезновения
        if (parent != null)
        {
            SmoothAlphaVisualizer.Instance.SetInvisible(parent);
        }
    }

    /// <summary>
    /// Запуск анимации включения/отключение окна
    /// </summary>
    /// <param name="targetScale"></param>
    /// <param name="active"></param>
    private void StartAnimation(Vector3 targetScale, bool active)
    {
        if (animationCor != null)
        {
            StartCoroutine(Waiting(targetScale, active));
        }
        else
        {
            animationCor = StartCoroutine(ActivateAnimation(targetScale, active));
        }
    }

    private IEnumerator Waiting(Vector3 target, bool active)
    {
        while (animationCor != null)
        {
            yield return null;
        }
        animationCor = StartCoroutine(ActivateAnimation(target, active));
    }

    private IEnumerator ActivateAnimation(Vector3 target, bool active)
    {
        while (true)
        {
            if (Vector3.Distance(gameObject.transform.localScale, target) > 0.01f)
            {
                if (!active)
                {
                    radial.enabled = false;
                }
                else
                {
                    radial.enabled = true;
                }
                gameObject.transform.localScale = Vector3.Lerp(gameObject.transform.localScale, target, .3f);
                yield return null;
            }
            else
            {
                if (!active)
                {
                    gameObject.transform.localScale = Vector3.zero;
                }
                else
                {
                    gameObject.transform.localScale = target;
                }
                animationCor = null;
                yield break;
            }
        }
    }

    private void Awake()
    {
        Transform = gameObject.transform;

        radial = GetComponent<RadialView>();
        title.text = "Сообщение";
        radial.MaxViewDegrees = 30.0f;

        aButton = new Dictionary<ButtonType, Interactable>();
        foreach (var type in Enum.GetValues(typeof(ButtonType)))
        {
            var button = buttons.Find(e => e.name == type.ToString());
            if (button)
            {
                aButton.Add((ButtonType)type, button);
                button.OnClick.AddListener(() => { ButtonClickEvent((ButtonType)type); });
                button.gameObject.SetActive(false);
            }
        }

        radial.enabled = false;
        gameObject.transform.localScale = Vector3.zero;
    }

    #region Полезные штуки
    delegate bool IsEqual(UnityEngine.Object x);
    private void Find(Dictionary<ButtonType, Interactable> dict, IsEqual func)
    {
        foreach (var i in dict.Values.ToArray())
        {
            if (func(i.gameObject)) { i.gameObject.SetActive(true); }
        }
    }
    #endregion

}
