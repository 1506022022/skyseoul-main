using Unity.Behavior;

public abstract partial class When : Condition
{
    public abstract override bool IsTrue();
    public abstract override void OnStart();
    public abstract override void OnEnd();
}