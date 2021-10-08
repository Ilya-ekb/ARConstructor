using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DataScripts;
using HologramObject;
using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;
using Visualization;
using TouchEvent = HologramObject.TouchEvent;

namespace LogicScripts
{
    public class TestScenario : Scenario
    {
        private Logic logic;
        public TestScenario() : base()
        {
            logic = Logic.Instance;
        }

        public override void StartScenario(params object[] p)
        {
            if (scenarioState == 0)
            {
                scenarioState = 10;
                logic.SendCommand(CommandType.PlayAudio, 1);
                logic.SendCommand(CommandType.Outline, 0, "Резервуар с водой" ,
                    true,
                    ConnectorOrientType.OrientToCamera,
                    ConnectorFollowType.Position,
                    ConnectorPivotMode.Automatic,
                    ConnectorPivotDirection.Northwest,
                    .3f);

                logic.SendCommand(CommandType.Outline, 1, "Минимум воды", 
                    true, 
                    ConnectorOrientType.OrientToCamera, 
                    ConnectorFollowType.Position,
                    ConnectorPivotMode.Automatic,
                    ConnectorPivotDirection.Northeast,
                    .4f);
                logic.SendCommand(CommandType.Dialog, "Проверь наличие воды в резервуаре\n\nВыше минимума?",
                    "CheckCoffee", "Refuel"); 
            }
        }

        public override void CheckCoffee(params object[] p)
        {
            if (scenarioState == 10)
            {
                scenarioState = 20;
                var allHologram = Data.Instance.AllBaseHologramObjects.ToArray();
                SmoothAlphaVisualizer.Instance.SetInvisible(allHologram[0]);
                SmoothAlphaVisualizer.Instance.SetInvisible(allHologram[1]);

                logic.SendCommand(CommandType.PlayAudio, 2);
                logic.SendCommand(CommandType.Outline, 2, "Загляни под крышку");
                logic.SendCommand(CommandType.Dialog, "Проверь есть ли кофе?", "TakeCup", "LookInBox",
                    new ManipulatableObject[]
                    {
                        (ManipulatableObject)Data.Instance.AllBaseHologramObjects.ToArray()[2]
                    });
            }

        }

        public override void Refuel(params object[] p)
        {
            if (scenarioState == 10)
            {
                scenarioState = 0;
                logic.SendCommand(CommandType.PlayAudio, 9);
                logic.SendCommand(CommandType.InfoMessage, "Пока не реализовано)", "StartScenario");
                //scenarioState = 11;
                //var allHologram = Data.Instance.AllBaseHologramObjects.ToArray();
                //SmoothAlphaVisualizer.Instance.SetInvisible(allHologram[1]);
                //var touchEvent = allHologram[9].GameObject.GetComponent<TouchEvent>();
                //touchEvent.OnTouchEvent.AddListener(() => touchEvent.InvokeScenarioMethod("TakeContainer"));
            }
        }

        public override void TakeContainer(params object[] p)
        {
            if (scenarioState == 11)
            {
                scenarioState = 12;
                var allHologram = Data.Instance.AllBaseHologramObjects.ToArray();
                SmoothAlphaVisualizer.Instance.SetInvisible(allHologram[0]);
                logic.SendCommand(CommandType.InfoMessage, "Наберите воды в резервуар", "SetContainerBack");
                logic.SendCommand(CommandType.Outline, 10, "Наберите воды");

            }
        }
        public override void SetContainerBack(params object[] p)
        {
            if (scenarioState == 12)
            {
                scenarioState = 10;
                var allHologram = Data.Instance.AllBaseHologramObjects.ToArray();
                var touchEvent = allHologram[9].GameObject.GetComponent<TouchEvent>();
                touchEvent.OnTouchEvent.AddListener(() =>
                {
                    touchEvent.InvokeScenarioMethod("CheckCoffee");
                    var allHologram = Data.Instance.AllBaseHologramObjects.ToArray();
                    SmoothAlphaVisualizer.Instance.SetInvisible(allHologram[0]);
                });

                logic.SendCommand(CommandType.Outline, 0, "Установите резеруар обратно");
            }
        }

        public override void TakeCup(params object[] p)
        {
            if (scenarioState == 20)
            {
                scenarioState = 30;
                logic.SendCommand(CommandType.PlayAudio, 3);
                logic.SendCommand(CommandType.Outline, 3, "1\nВозьми стакан на верхней полке");
                logic.SendCommand(CommandType.Outline, 4, "2\nПоставь стакан сюда");
                logic.SendCommand(CommandType.InfoMessage, "Возьми бумажный стакан в шкафу и поставь в указанное место",
                    "ChooseCoffee",
                    new ManipulatableObject[]
                    {
                        (ManipulatableObject) Data.Instance.AllBaseHologramObjects.ToArray()[3],
                        (ManipulatableObject) Data.Instance.AllBaseHologramObjects.ToArray()[4]
                    });
            }
        }

        public override void LookInBox(params object[] p)
        {
            if (scenarioState == 20)
            {
                scenarioState = 0;
                logic.SendCommand(CommandType.PlayAudio, 9);
                logic.SendCommand(CommandType.InfoMessage, "Пока не реализовано)", "StartScenario");
            }
        }

        public override void ChooseCoffee(params object[] p)
        {
            if (scenarioState == 30)
            {
                scenarioState = 40;
                logic.SendCommand(CommandType.PlayAudio, 4);
                logic.SendCommand(CommandType.ChoosePanel, "Выбери какое кофе будешь:",
                    "Американо", "Americano",
                    "Американо х2", "DoubleAmericano",
                    "Эспрессо", "Espresso",
                    "Эспрессо x2", "DoubleEspresso");
            }
        }

        public override void Americano(params object[] p)
        {
            if (scenarioState == 40)
            {
                scenarioState = 50;
                logic.SendCommand(CommandType.PlayAudio, 5);
                logic.SendCommand(CommandType.Outline, 5, "Американо", 
                    true,
                    ConnectorOrientType.OrientToCamera,
                    ConnectorFollowType.Position,
                    ConnectorPivotMode.Automatic,
                    ConnectorPivotDirection.Northwest,
                    .3f,
                    "FinalScenario");

            }
        }

        public override void DoubleAmericano(params object[] p)
        {
            if (scenarioState == 40)
            {
                scenarioState = 50;
                logic.SendCommand(CommandType.PlayAudio, 6);
                logic.SendCommand(CommandType.Outline, 6, "Американо х2",
                    true,
                    ConnectorOrientType.OrientToCamera,
                    ConnectorFollowType.Position,
                    ConnectorPivotMode.Automatic,
                    ConnectorPivotDirection.Northwest,
                    .3f,
                    "FinalScenario");
            }
        }

        public override void Espresso(params object[] p)
        {
            if (scenarioState == 40)
            {
                scenarioState = 50;
                logic.SendCommand(CommandType.PlayAudio, 7);
                logic.SendCommand(CommandType.Outline, 7, "Эспрессо",
                    true,
                    ConnectorOrientType.OrientToCamera,
                    ConnectorFollowType.Position,
                    ConnectorPivotMode.Automatic,
                    ConnectorPivotDirection.Northwest,
                    .3f,
                    "FinalScenario");
            }
        }

        public override async void DoubleEspresso(params object[] p)
        {
            if (scenarioState == 40)
            {
                scenarioState = 50;
                logic.SendCommand(CommandType.PlayAudio, 8);
                logic.SendCommand(CommandType.Outline, 8, "Эспрессо x2",
                    true,
                    ConnectorOrientType.OrientToCamera,
                    ConnectorFollowType.Position,
                    ConnectorPivotMode.Automatic,
                    ConnectorPivotDirection.Northwest,
                    .3f,
                    "FinalScenario");
            }
        }

        public override void ButtonNo(params object[] p)
        {
            throw new System.NotImplementedException();
        }

        public override void ButtonYes(params object[] p)
        {
            throw new System.NotImplementedException();
        }

        public override void FinalScenario(params object[] p)
        {
            SmoothAlphaVisualizer.Instance.SetInvisibleAll();
        }
    }
}
