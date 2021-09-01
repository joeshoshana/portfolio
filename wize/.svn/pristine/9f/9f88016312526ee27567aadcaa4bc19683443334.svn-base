package Siemens;

import java.io.Closeable;

public interface IController extends Runnable, Closeable {
	public boolean Connect();

	public void Disconnect();

	public void Validate();

	public void Run(boolean isRun);

	public void Send(SiemensCommands command);

	public String Data();

	public boolean IsDataRecieved();

	public void ClearData();

}
