package Offline;

import java.util.ArrayList;

public class Offline {
    private IOffline _offline = null;

    public Offline(IOffline offline) {
        _offline = offline;
    }

    public void Save(ArrayList<String> records) {
        _offline.Save(records);
    }

    public void Save(String record) {
        _offline.Save(record);
    }

    public ArrayList<String> Load() {
        return _offline.Load();
    }

    public void Delete() {
        _offline.Delete();
    }

    public void Run(boolean isRun) {
        _offline.Run(isRun);
    }

    public void SetIsUpload(boolean isUpload) {
        _offline.SetIsUpload(isUpload);
    }

    public boolean IsUpload() {
        return _offline.IsUpload();
    }
}
