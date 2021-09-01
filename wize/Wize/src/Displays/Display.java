package Displays;

import java.awt.Color;

public class Display {
    private IDisplay _display = null;
    private static final Color _warningColor = new Color(204, 35, 35);
    private static final Color _okColor = new Color(0, 173, 14);

    public Display(IDisplay display) {
        _display = display;
    }

    public void SetVisible(boolean isVisible) {
        _display.SetVisible(isVisible);
    }

    public void SuccessBackground(int timeoutMS) {
        _display.Background(_okColor, timeoutMS);
    }

    public void ErrorBackground(int timeoutMS) {
        _display.Background(_warningColor, timeoutMS);
    }

    public void Background(Color c, int timeoutMS) {
        _display.Background(c, timeoutMS);
    }

    public void Message(String msg, int timeoutMS) {
        _display.Message(msg, timeoutMS);
    }

    public void Data(String data, int timeoutMS) {
        _display.Data(data, timeoutMS);
    }

    public void Weight(String weight, int timeoutMS) {
        _display.Weight(weight, timeoutMS);
    }

    public String Data() {
        return _display.Data();
    }

    public boolean IsDataRecieved() {
        return _display.IsDataRecieved();
    }

    public void ClearData() {
        _display.ClearData();
    }
}