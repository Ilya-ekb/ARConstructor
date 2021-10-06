using DataScripts;
using LogicScripts;

public class Scenario
{
    protected int scenarioState;

    public Scenario()
    {
        Data.Logic = new Logic();
        scenarioState = 0;
        Data.Logic.scenario = this;
    }

    public virtual void StartScenario(params object[] p) { }
    public virtual void GoToDasboard(params object[] p) { }
    public virtual void ShuntingLightNotOpen(params object[] p) { }
    public virtual void ShuntingLightM3(params object[] p) { }
    public virtual void WayPointer(params object[] p) { }
    public virtual void ButtonNo(params object[] p) { }
    public virtual void ButtonYes(params object[] p) { }
    public virtual void LightEndWay(params object[] p) { }
    public virtual void WhiteLine(params object[] p) { }
    public virtual void AllowTrafficLightDuplicate(params object[] p) { }
    public virtual void TakeMultimeter(params object[] p) { }
    public virtual void VoltageMeasure(params object[] p) { }
    public virtual void FindDashboard(params object[] p) { }
    public virtual void FindModuls(params object[] p) { }
    public virtual void Result(params object[] p) { }
}
