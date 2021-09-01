package LPR;

import java.io.ByteArrayOutputStream;
import java.io.Closeable;

public interface ILPR extends Runnable, Closeable {
    public boolean Connect();

    public void Disconnect();

    public void Validate();

    public void Run(boolean isRun);

    public ByteArrayOutputStream Data();

    public boolean IsDataRecieved();

    public void ClearData();
}