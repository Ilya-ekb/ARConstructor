using System.Collections.Generic;
using Storage;

public interface IBaseHologramObjectContainer
{
    string Id { get; }
    IEnumerable<string> ContainedIds { get; }
    IEnumerable<IBaseHologramObject> ContainedBaseHologramObjects { get; }
    IBaseHologramObject RestoreHologramObjectEvent(Memo memo);
}