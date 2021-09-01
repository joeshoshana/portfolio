package LPR;

import java.io.ByteArrayOutputStream;

public class LPR {
    private ILPR _lpr = null;

    public LPR(ILPR lpr) {
        _lpr = lpr;
    }

    public boolean Connect() {
        return _lpr.Connect();
    }

    public void Run(Boolean isRunning) {
        _lpr.Run(isRunning);
    }

    public void Disconnect() {
        _lpr.Disconnect();
    }

    public ByteArrayOutputStream Data() {
        return _lpr.Data();
    }

    public boolean IsDataRecieved() {
        return _lpr.IsDataRecieved();
    }

    public void ClearData() {
        _lpr.ClearData();
    }
}