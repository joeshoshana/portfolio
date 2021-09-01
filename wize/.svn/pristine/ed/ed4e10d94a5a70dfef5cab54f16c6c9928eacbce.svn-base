package Offline;

import java.io.Closeable;
import java.util.ArrayList;

public interface IOffline extends Runnable, Closeable {
    public void Save(ArrayList<String> records);

    public void Save(String record);

    public ArrayList<String> Load();

    public void Delete();

    public void SetIsUpload(boolean isUpload);

    public boolean IsUpload();

    public void Run(boolean isRun);

}