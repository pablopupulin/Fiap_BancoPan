namespace Application.Models;

public class Notifable<T> where T : class
{

    public Notifable(T data)
    {
        Data = data;
        Sucess = true;
    }

    public Notifable(string error)
    {
        Error = error;
        Sucess = false;
    }

    public T Data { get; set; }

    public bool Sucess { get; set; }

    public string Error { get; set; }
}