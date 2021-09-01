package Tags;

import java.io.Closeable;

public interface ITag extends Runnable, Closeable {
    public boolean Connect();

    public void Disconnect();

    public void Validate();

    public void Run(boolean isRun);

    public String Data();

    public boolean IsDataRecieved();

    public void ClearData();
}