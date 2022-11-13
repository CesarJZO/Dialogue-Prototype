namespace StatePattern
{
    public class State
    {
        // ReSharper disable Unity.PerformanceAnalysis
        public virtual void Start() { }
        // ReSharper disable Unity.PerformanceAnalysis
        public virtual void Exit() { }
        public virtual void Update() { }
        public virtual void FixedUpdate() { }
        public virtual void LateUpdate() { }
    }
}
