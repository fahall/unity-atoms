namespace UnityAtoms
{
    public interface IAtomListener
    {
        void OnEventRaised();
    }

    public interface IAtomListener<T>
    {
        void HandleInput(T item);
    }
}
