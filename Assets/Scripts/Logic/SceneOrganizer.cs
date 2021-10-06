using System;
using System.Collections;
using System.Collections.Generic;
using DataScripts;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;
using Serialization;

using Storage;

using UnityEngine;
using Visualization;

namespace LogicScripts
{
    public class SceneOrganizer : Singleton<SceneOrganizer>
    {

        private IPersistentDataController storageController;

        private TextMesh lastLabelPlacedText;
        
        private float probabilityThreshold = .2f;

        private Renderer quadRenderer;

        private Transform lastLabelPlaced;

        private ScrollList scrollList;

        private AudioSource audioSource;

        private void Awake()
        {
            gameObject.AddComponent<HologramController>();
            HologramController.Instance.HologramObjectsSmoothAlpha =
                gameObject.AddComponent<SmoothAlphaVisualizer>();
        }

        private void OnEnable()
        {
            StorageController.Instance.Load();
            HologramController.Instance.HologramObjectsSmoothAlpha?.SetInvisibleAll();
        }

        private void OnDisable()
        {
            StorageController.Instance.Save();
        }

        ///// <summary>
        ///// Инизализация объекта из сохраненных данных с диска
        ///// </summary>
        ///// <param name="savedData"></param>Десериализованные данные
        ///// <returns></returns>
        //public BaseObject CreateObject(Memo savedData)
        //{
        //    var defaultType = savedData.Type;
        //    GameObject resultObject;

        //    //Создание примитива 
        //    switch (defaultType)
        //    {
        //        case ObjectType.PanelToFloor:
        //        case ObjectType.Tooltip:
        //        default:
        //            resultObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
        //            break;
        //        case ObjectType.WorldAnchor:
        //        case ObjectType.Point:
        //            resultObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //            var box = resultObject.AddComponent<BoxCollider>();
        //            box.size = Vector3.one * 8.0f;
        //            break;
        //        case ObjectType.Capsule:
        //            resultObject = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        //            DestroyImmediate(resultObject.GetComponent<CapsuleCollider>());
        //            resultObject.AddComponent<BoxCollider>().size = Vector3.one * 8.0f;
        //            break;
        //        case ObjectType.Empty:
        //            resultObject = new GameObject();
        //            break;
        //    }


        //    resultObject.name = savedData.Name;

        //    resultObject.transform.localRotation = Quaternion.AngleAxis(savedData.RotX, Vector3.right) *
        //                                           Quaternion.AngleAxis(savedData.RotY, Vector3.up) *
        //                                           Quaternion.AngleAxis(savedData.RotZ, Vector3.forward);
        //    resultObject.transform.localPosition = new Vector3(savedData.PosX, savedData.PosY, savedData.PosZ);
        //    resultObject.transform.localScale = new Vector3(savedData.ScaleX, savedData.ScaleY, savedData.ScaleZ);

        //    var result = new BaseObject(resultObject, Instantiate(Data.Instance.arrow), defaultType);

        //    Data.InfoObjects.AddNew(result); //Добавление созданного объекта в словарь

        //    CreateChilderns(savedData, ref resultObject);

        //    //Установка материала и цвета
        //    var renderer = resultObject.GetComponent<Renderer>();
        //    if (renderer) renderer.material = new Material(Shader.Find("Sprites/Default"));
        //    if (renderer) renderer.material.color = new Color(savedData.ColorR, savedData.ColorG, savedData.ColorB, savedData.ColorA); ;

        //    return result;
        //}

        ///// <summary>
        ///// Создание нового объекта
        ///// </summary>
        ///// <param name="name">Имя создаваемого объекта</param>
        ///// <param name="defaultType">Тип создаваемого объекта</param>
        ///// <param name="material">Материал создаваемого объекта</param>
        ///// <param name="parent">Родительский объект для создаваемого объекта</param>
        ///// <returns></returns>
        //public IBaseHologramObject CreateObject(string name, ObjectType defaultType, Material material, Transform parent = null)
        //{
        //    Memo savedData = Serializator.Deserialization<Memo>(name);
        //    if (savedData != null) defaultType = savedData.Type != defaultType ? savedData.Type : defaultType;  // Уточнение типа создаваемого объекта

        //    Vector3 scale = Vector3.one;
        //    Vector3 centerPoint = new Vector3(cursor.transform.position.x, cursor.transform.position.y, cursor.transform.position.z);
        //    Quaternion rotation = Quaternion.AngleAxis(cursor.transform.rotation.eulerAngles.x, Vector3.right) *
        //                          Quaternion.AngleAxis(cursor.transform.rotation.eulerAngles.y, Vector3.up) *
        //                          Quaternion.AngleAxis(cursor.transform.rotation.eulerAngles.z, Vector3.forward);
        //    Color color = new Color(1f, 1f, 1f, .5f);

        //    GameObject resultObject;
        //    //Создание примитива 
        //    switch (defaultType)
        //    {
        //        case ObjectType.PanelToFloor:
        //        case ObjectType.Tooltip:
        //        default:
        //            resultObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
        //            break;
        //        case ObjectType.WorldAnchor:
        //        case ObjectType.Point:
        //            resultObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //            var box = resultObject.AddComponent<BoxCollider>();
        //            resultObject.AddComponent<ManipulatableObject>();
        //            box.size = Vector3.one * 8.0f;
        //            break;
        //        case ObjectType.Capsule:
        //            resultObject = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        //            DestroyImmediate(resultObject.GetComponent<CapsuleCollider>());
        //            resultObject.AddComponent<BoxCollider>().size = Vector3.one * 8.0f;
        //            break;
        //        case ObjectType.Empty:
        //            resultObject = new GameObject();
        //            break;
        //    }
        //    resultObject.name = name;



        //    //Настройка Transform
        //    GetTransform(defaultType, savedData, ref scale, ref centerPoint, ref rotation);
        //    resultObject.transform.localRotation = rotation;
        //    resultObject.transform.localPosition = centerPoint;
        //    resultObject.transform.localScale = scale;

        //    var result = new BaseObject(resultObject, Instantiate(Data.Instance.arrow), defaultType);
        //    Data.InfoObjects.AddNew(result); //Добавление созданного объекта в словарь

        //    CreateChilderns(savedData, ref resultObject);

        //    //Установка материала и цвета
        //    GetMaterial(savedData, ref color);
        //    var renderer = resultObject.GetComponent<Renderer>();
        //    if (renderer) renderer.material = material;
        //    if (renderer) renderer.material.color = color;

        //    return result;
        //}


        ///// <summary>
        ///// Создание дочерних объектов
        ///// </summary>
        ///// <param name="savedData"></param>
        ///// <param name="resutltObject"></param>
        //private void CreateChilderns(Memo savedData, ref GameObject resutltObject)
        //{
        //    if (savedData?.Childs?.Length > 0)
        //    {
        //        for (var i = 0; i < savedData.Childs.Length; i++)
        //        {
        //            var child = savedData.Childs[i];
        //            CreateObject(child, ObjectType.Empty, new Material(Shader.Find("Sprites/Default")));
        //        }
        //    }
        //}

        ///// <summary>
        ///// Установка Transform создаваемого объекта в зависимости от типа
        ///// </summary>
        ///// <param name="defaultType"></param>Тип объекта
        ///// <param name="label"></param>Точка, относительно которой устанавливается позиция
        ///// <param name="savedData"></param>Источник сохраненных данных о раннее заданном положении (если объект с таким именем уже создавался)
        ///// <param name="scale"></param>Изменяемое localScale значение 
        ///// <param name="centerPoint"></param> Изменяемое localPosition значение
        ///// <param name="rotation"></param>Изменяемое localRotation значение
        ///// <param name="parent"></param>Изменяемое localRotation значение
        //private void GetTransform(ObjectType defaultType, Memo savedData, ref Vector3 scale, ref Vector3 centerPoint, ref Quaternion rotation)
        //{
        //    switch (defaultType)
        //    {
        //        case ObjectType.PanelToFloor:
        //            if (Physics.Raycast(centerPoint, Vector3.down, out RaycastHit floor, 30.0f, spatialLayerMask))
        //            {
        //                rotation = savedData != null ? Quaternion.AngleAxis(savedData.PosX, Vector3.right) * Quaternion.AngleAxis(savedData.PosY, Vector3.up) * Quaternion.AngleAxis(savedData.PosZ, Vector3.forward) :
        //                                               Quaternion.identity;
        //                var scaleX = savedData != null ? savedData.ScaleX : 0.4f;
        //                var heigth = Vector3.Magnitude(centerPoint - floor.point);
        //                scale = new Vector3(scaleX, heigth, 1f);
        //                centerPoint = savedData != null ? new Vector3(savedData.PosX, savedData.PosY, savedData.PosZ) :
        //                                                  new Vector3(centerPoint.x - scaleX / 2, centerPoint.y - heigth / 2, centerPoint.z);
        //            }
        //            break;

        //        case ObjectType.WorldAnchor:
        //            if (savedData != null)
        //            {
        //                scale = new Vector3(savedData.ScaleX, savedData.ScaleY, savedData.ScaleZ);
        //                centerPoint = new Vector3(savedData.PosX, savedData.PosY, savedData.PosZ);
        //                rotation = Quaternion.AngleAxis(savedData.PosX, Vector3.right) *
        //                           Quaternion.AngleAxis(savedData.PosY, Vector3.up) *
        //                           Quaternion.AngleAxis(savedData.PosZ, Vector3.forward);
        //            }
        //            else
        //            {
        //                scale = Vector3.one * .005f;
        //            }
        //            break;

        //        case ObjectType.Capsule:
        //        case ObjectType.Tooltip:
        //            if (savedData != null)
        //            {
        //                scale = new Vector3(savedData.ScaleX, savedData.ScaleY, savedData.ScaleZ);
        //                centerPoint = new Vector3(savedData.PosX, savedData.PosY, savedData.PosZ);
        //                rotation = Quaternion.AngleAxis(savedData.PosX, Vector3.right) *
        //                           Quaternion.AngleAxis(savedData.PosY, Vector3.up) *
        //                           Quaternion.AngleAxis(savedData.PosZ, Vector3.forward);
        //            }
        //            else
        //            {
        //                scale = Vector3.one * .1f;
        //                scale[1] *= 2;
        //            }
        //            break;

        //        default:
        //            scale = Vector3.one;
        //            centerPoint = cursor.transform.position;
        //            rotation = cursor.transform.rotation;
        //            break;
        //    }
        //}

        ///// <summary>
        ///// Получения родителя для создаваемого объекта
        ///// </summary>
        ///// <param name="savedData"></param>
        ///// <param name="inner"></param>
        ///// <returns></returns>
        //public Transform GetParent(Memo savedData, ref Transform inner)
        //{
        //    var objDict = Data.InfoObjects;
        //    var parent = savedData != null ? savedData.Parent : string.Empty;
        //    var result = string.IsNullOrEmpty(parent) ? inner : objDict.ContainsKey(parent) ? objDict[savedData.Parent].GO.transform : inner;
        //    return result;
        //}


        public TextMesh CreateUI(Transform parent, string name, float scale, float yPos, float zPos, bool setActive)
        {
            var display = new GameObject(name, typeof(TextMesh));
            display.transform.parent = parent;
            display.transform.localPosition = new Vector3(.0f, yPos, zPos);
            display.SetActive(setActive);
            display.transform.localRotation = new Quaternion();
            display.transform.localScale = new Vector3(scale, scale, scale);
            var textMesh = display.GetComponent<TextMesh>();
            textMesh.anchor = TextAnchor.MiddleCenter;
            textMesh.alignment = TextAlignment.Center;
            return textMesh;
        }

        /// <summary>
        /// Вызов указателя к действию
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <param name="dialog"></param>
        public void Pointer(params object[] vs)
        {
            if (vs.Length > 0)
            {
                var offset = 1;
                string name = (PointType)vs[0 + offset] + vs[1 + offset].ToString();
                var obj = Data.Instance.GetBaseHologramObject(name);

                if ((CommandType)vs[0] == CommandType.Outline)
                {
                    HologramController.Instance.HologramObjectsSmoothAlpha.SetVisible(obj);
                }
                else if ((CommandType)vs[0] == CommandType.Outline_flash)
                {
                   // HologramController.Instance.AlphaSet(obj, Visibility.flashing, vs[2 + offset].ToString());
                }

                if (vs.Length > 3 + offset)
                {

                    var ind = 3 + offset;
                    var result = vs[ind].ToString();
                    if (Enum.TryParse(result, out CommandType type))
                    {
                        if (vs[vs.Length - 1] is IBaseHologramObject)
                        {
                            //obj.Parent = ((IBaseHologramObject)vs[vs.Length - 1]).HologramData.Id;
                        }
                    }
                    else
                    {
                        //obj.Parent = Data.InfoObjects[result].HologramData.Id;
                    }
                    var nextParam = new object[vs.Length - ind + offset];
                    for (var d = 0; d < nextParam.Length; d++)
                    {
                        if (d == nextParam.Length - 1)
                        {
                            nextParam[d] = obj;
                            continue;
                        }
                        nextParam[d] = vs[ind];
                        ind++;
                    }

                    Data.Logic.SendCommand(nextParam);
                }
            }
            else
            {
                Debug.LogError($"Incorrect parameters - min 2 parameters are needed, but {vs.Length} comes");
            }
        }

        /// <summary>
        /// Вызов панели выбора
        /// </summary>
        /// <param name="vs"></param>
        public void ChoosePanel(params object[] vs)
        {
            if (vs.Length > 0)
            {
                int offset = 1;
                scrollList = Instantiate(Data.Instance.scrollObject).GetComponent<ScrollList>();
                var radial = scrollList.gameObject.GetComponent<RadialView>();
                radial.MaxViewDegrees = 10.0f;
                scrollList.gameObject.transform.localScale = Vector3.zero;
                var parameters = new object[vs.Length - offset];
                for (int i = 0; i < parameters.Length; i++)
                {
                    parameters[i] = vs[i + offset];
                }
                scrollList.Activate(parameters);
            }
        }

        /// <summary>
        /// Вызов аудио подсказки
        /// </summary>
        /// <param name="vs"></param>
        public void PlayAudio(params object[] vs)
        {
            if (!audioSource) { audioSource = gameObject.AddComponent<AudioSource>(); }
            audioSource.clip = Data.Instance.audioClips[(int)vs[1]];
            audioSource.Play();
        }
    }
}
