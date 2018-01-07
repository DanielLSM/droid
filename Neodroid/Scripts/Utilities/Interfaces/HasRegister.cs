namespace Neodroid.Utilities {
  public interface IHasRegister<in T> {
    void Register(T obj);

    void Register(T obj, string identifier);
  }
}
