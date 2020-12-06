namespace UnityAtoms
{
    public interface IAtomAction
    {
        void Do();
    }

    public interface IAtomAction<in T>
    {
        void Do(T arg);
    }
}
