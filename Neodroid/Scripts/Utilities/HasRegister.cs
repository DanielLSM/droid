namespace Neodroid.Utilities {
  public interface HasRegister<T> {
    void Register (T obj);

    void Register (T obj, string identifier);
  }
}
