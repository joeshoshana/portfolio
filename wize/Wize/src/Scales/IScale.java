package Scales;

import java.io.Closeable;

public interface IScale extends Runnable, Closeable {
    public boolean Connect();

    public void Disconnect();

    public void Validate();

    public void Run(boolean isRun);

    public String Data();

    public boolean IsDataRecieved();

    public void ClearData();
}