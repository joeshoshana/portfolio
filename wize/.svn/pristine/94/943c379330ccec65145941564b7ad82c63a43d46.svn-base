package IO;

public class IO {
    private IIO _io = null;

    public IO(IIO io) {
        _io = io;
    }

    public boolean Connect() {
        return _io.Connect();
    }

    public void Disconnect() {
        _io.Disconnect();
    }

    public void Run(boolean isRun) {
        _io.Run(isRun);
    }

    public GPIO Data() {
        return _io.Data();
    }

    public boolean IsDataRecieved() {
        return _io.IsDataRecieved();
    }

    public void ClearData() {
        _io.ClearData();
    }
}