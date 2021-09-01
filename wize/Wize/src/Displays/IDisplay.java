package Displays;

import java.awt.Color;

public interface IDisplay {
    public void SetVisible(boolean isVisible);

    public void Background(Color c, int timeoutMS);

    public void Message(String msg, int timeoutMS);

    public void Data(String data, int timeoutMS);

    public void Weight(String weight, int timeoutMS);

    public String Data();

    public boolean IsDataRecieved();

    public void ClearData();
}