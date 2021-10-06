using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using DataScripts;
using UnityEngine;

namespace LogicScripts
{
    public class Logic : MonoBehaviour
    {
        internal Scenario scenario;

        public Logic()
        {
        }

        /// <summary>
        /// Вызов функции из сценария
        /// </summary>
        /// <param name="method"></param>Имя метода из сценария
        /// <param name="parameters"></param>Параметры для вызываемой функции
        public void InvokeFunction(string method, object[] parameters)
        {
            Type scenarioType = scenario.GetType();
            MethodInfo scenarioMethod = scenarioType.GetMethod(method);
            scenarioMethod.Invoke(scenario, parameters);
        }


        /// <summary>
        /// Отправка команды в сцену для соответствующей реализации
        /// Параметры:
        /// DIALOG: string text - сообщение, string nameYes - имя метода при положительном ответе, string nameNo - имя метода при отрицательном ответе 
        /// INFO MESSAGE:  string Text - сообщение, string nameOk - имя метода при нажатии ОК 
        /// OUTLINE: string name - имя точки в массиве ObjDictianory, string message - сообщение на точке, params string[] param
        /// </summary>
        /// <param name="messageType">Тип оправляемой команды</param>
        /// <param name="parameters"></param> 
        internal void SendCommand(params object[] parameters)
        {
            switch ((CommandType)parameters[0])
            {
                case CommandType.Dialog:
                case CommandType.InfoMessage:
                    Dialog.Instance.Activate(parameters);
                    break;

                case CommandType.Outline:
                case CommandType.Outline_flash:
                    SceneOrganizer.Instance.Pointer(parameters);
                    break;

                case CommandType.ChoosePanel:
                    SceneOrganizer.Instance.ChoosePanel(parameters);
                    break;
                case CommandType.PlayAudio:
                    SceneOrganizer.Instance.PlayAudio(parameters);
                    break;

            }
        }
    }
    public enum CommandType
    {
        InfoMessage,
        ChoosePanel,
        Dialog,
        Outline,
        Outline_flash,
        PlayAudio,
    }

}
