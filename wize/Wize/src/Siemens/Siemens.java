package Siemens;

import java.io.DataInputStream;
import java.io.IOException;
import java.io.OutputStream;
import java.net.Socket;
import java.nio.charset.Charset;
import java.util.ArrayList;

import Wize.Configurations.ControllerConfiguration;

public class Siemens implements IController {
	private Socket m_socket = null;
	protected ControllerConfiguration _config = null;
	private DataInputStream m_input = null;
	private Boolean m_isRunning = false;
	public SendListener OnSend = null;
	public ReceiveListener OnReceive = null;
	private ArrayList<SiemensCommands> commandsToSend = new ArrayList<SiemensCommands>();
	private String m_data = "";
	private boolean m_isDataRecieved = false;

	public Siemens(ControllerConfiguration config) {
		_config = config;
		OnReceive = new ReceiveListener() {
			@Override
			public void Receive(String data) {
				m_data = data;
				m_isDataRecieved = true;
			}
		};

	}

	@Override
	public boolean Connect() {
		try {
			if (_config == null)
				throw new Exception("No Arguments Was Initialized");

			Disconnect();
			Validate();

			m_socket = new Socket(_config.IP(), _config.Port());
			m_input = new DataInputStream(m_socket.getInputStream());

			return true;
		} catch (Exception e) {
			e.printStackTrace();
			return false;
		}
	}

	@Override
	public void Validate() {
		try {
			if (_config.IP() == null || _config.IP().trim().length() == 0) {
				throw new Exception("Missing IP");
			} else {
				if (!IsIPValid()) {
					throw new Exception("Invalid IP");
				}
			}
			if (_config.Port() == 0) {
				throw new Exception("Missing Port");
			}
		} catch (Exception e) {
			e.printStackTrace();
		}
	}

	private boolean IsIPValid() {
		try {
			if (_config.IP() == null || _config.IP().contains(" ")) {
				return false;
			}

			final String[] splitValues = _config.IP().split("\\.");
			if (splitValues.length != 4) {
				return false;
			}

			for (int i = 0; i < splitValues.length; i++) {
				try {
					final int val = Integer.parseInt(splitValues[i]);
					if (val < 0 || val > 255)
						return false;
				} catch (final Exception ex) {
					return false;
				}
			}
			return true;
		} catch (Exception e) {
			e.printStackTrace();
			return false;
		}
	}

	@Override
	public void Disconnect() {
		try {
			if (m_socket != null) {
				m_socket.close();
			}
			m_socket = null;
		} catch (Exception e) {
			e.printStackTrace();
		}
	}

	private void Process() {

		RecieveProcess();
		SendProcess();
	}

	private void RecieveProcess() {
		final Thread r = new Thread(() -> {
			while (m_isRunning) {
				try {
					final byte[] buffer = new byte[1024];

					if (m_input.available() > 0) {
						Thread.sleep(160);
						m_input.read(buffer);
						final String data = new String(buffer, 2, buffer[1]);
						Receive(data);
					}

				} catch (Exception e) {
					e.printStackTrace();
				}
			}
		});
		r.start();
	}

	private void SendProcess() {
		final Thread r = new Thread(() -> {

			while (m_isRunning) {
				try {

					for (int i = 0; i < commandsToSend.size(); i++) {
						final SiemensCommands command = commandsToSend.get(i);
						if (command != null) {
							final String name = command.name();
							final OutputStream out = m_socket.getOutputStream();
							final byte[] a = { (byte) 254 };
							final byte[] b = new byte[1];
							b[0] = (byte) name.length();
							out.write(a, 0, 1);
							out.write(b, 0, 1);
							out.write(name.getBytes(Charset.forName("UTF-8")), 0, name.length());
							commandsToSend.remove(i--);
						}
					}

				} catch (IOException e) {
					e.printStackTrace();
				}
			}
		});
		r.start();
	}

	@Override
	public void Send(SiemensCommands command) {
		try {
			commandsToSend.add(command);
		} catch (Exception e) {
			e.printStackTrace();
		}
	}

	@Override
	public void run() {
		Process();
	}

	@Override
	public void close() throws IOException {
		Disconnect();
	}

	protected void Receive(String data) {
		if (OnReceive != null)
			OnReceive.Receive(data);
	}

	protected void Send(String data) {
		if (OnSend != null)
			OnSend.Send(data);
	}

	@Override
	public void Run(boolean isRun) {
		Thread sr = new Thread(() -> {
			m_isRunning = isRun;
			if (m_isRunning)
				run();
			else
				Disconnect();

		});
		sr.start();
	}

	@Override
	public String Data() {
		return m_data;
	}

	@Override
	public boolean IsDataRecieved() {
		return m_isDataRecieved;
	}

	@Override
	public void ClearData() {
		m_data = "";
		m_isDataRecieved = false;
	}

}
