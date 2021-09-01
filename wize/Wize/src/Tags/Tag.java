package Tags;

public class Tag {
    private ITag _tag = null;

    public Tag(ITag tag) {
        _tag = tag;
    }

    public boolean Connect() {
        return _tag.Connect();
    }

    public void Run(Boolean isRunning) {
        _tag.Run(isRunning);
    }

    public void Disconnect() {
        _tag.Disconnect();
    }

    public String Data() {
        return _tag.Data();
    }

    public boolean IsDataRecieved() {
        return _tag.IsDataRecieved();
    }

    public void ClearData() {
        _tag.ClearData();
    }
}