package Siemens;

public class Controller {

    private IController _controller = null;

    public Controller(IController controller) {
        _controller = controller;
    }

    public void Send(SiemensCommands command) {
        _controller.Send(command);
    }

    public boolean Connect() {
        return _controller.Connect();
    }

    public void Run(Boolean isRunning) {
        _controller.Run(isRunning);
    }

    public void Disconnect() {
        _controller.Disconnect();
    }

    public String Data() {
        return _controller.Data();
    }

    public boolean IsDataRecieved() {
        return _controller.IsDataRecieved();
    }

    public void ClearData() {
        _controller.ClearData();
    }
}