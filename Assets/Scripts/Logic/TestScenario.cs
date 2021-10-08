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
                logic.SendCommand(CommandType.Outline, 0, "��������� � �����" ,
                    true,
                    ConnectorOrientType.OrientToCamera,
                    ConnectorFollowType.Position,
                    ConnectorPivotMode.Automatic,
                    ConnectorPivotDirection.Northwest,
                    .3f);

                logic.SendCommand(CommandType.Outline, 1, "������� ����", 
                    true, 
                    ConnectorOrientType.OrientToCamera, 
                    ConnectorFollowType.Position,
                    ConnectorPivotMode.Automatic,
                    ConnectorPivotDirection.Northeast,
                    .4f);
                logic.SendCommand(CommandType.Dialog, "������� ������� ���� � ����������\n\n���� ��������?",
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
                logic.SendCommand(CommandType.Outline, 2, "������� ��� ������");
                logic.SendCommand(CommandType.Dialog, "������� ���� �� ����?", "TakeCup", "LookInBox",
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
                logic.SendCommand(CommandType.InfoMessage, "���� �� �����������)", "StartScenario");
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
                logic.SendCommand(CommandType.InfoMessage, "�������� ���� � ���������", "SetContainerBack");
                logic.SendCommand(CommandType.Outline, 10, "�������� ����");

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

                logic.SendCommand(CommandType.Outline, 0, "���������� �������� �������");
            }
        }

        public override void TakeCup(params object[] p)
        {
            if (scenarioState == 20)
            {
                scenarioState = 30;
                logic.SendCommand(CommandType.PlayAudio, 3);
                logic.SendCommand(CommandType.Outline, 3, "1\n������ ������ �� ������� �����");
                logic.SendCommand(CommandType.Outline, 4, "2\n������� ������ ����");
                logic.SendCommand(CommandType.InfoMessage, "������ �������� ������ � ����� � ������� � ��������� �����",
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
                logic.SendCommand(CommandType.InfoMessage, "���� �� �����������)", "StartScenario");
            }
        }

        public override void ChooseCoffee(params object[] p)
        {
            if (scenarioState == 30)
            {
                scenarioState = 40;
                logic.SendCommand(CommandType.PlayAudio, 4);
                logic.SendCommand(CommandType.ChoosePanel, "������ ����� ���� ������:",
                    "���������", "Americano",
                    "��������� �2", "DoubleAmericano",
                    "��������", "Espresso",
                    "�������� x2", "DoubleEspresso");
            }
        }

        public override void Americano(params object[] p)
        {
            if (scenarioState == 40)
            {
                scenarioState = 50;
                logic.SendCommand(CommandType.PlayAudio, 5);
                logic.SendCommand(CommandType.Outline, 5, "���������", 
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
                logic.SendCommand(CommandType.Outline, 6, "��������� �2",
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
                logic.SendCommand(CommandType.Outline, 7, "��������",
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
                logic.SendCommand(CommandType.Outline, 8, "�������� x2",
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
