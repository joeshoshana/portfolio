package LPR;

import java.io.ByteArrayOutputStream;
import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.io.IOException;
import java.net.HttpURLConnection;
import java.net.InetAddress;
import java.net.NetworkInterface;
import java.net.ServerSocket;
import java.net.Socket;
import java.net.SocketException;
import java.net.URL;
import java.util.Base64;
import java.util.Enumeration;
import javax.xml.parsers.ParserConfigurationException;
import org.w3c.dom.Element;
import Utilities.UtilsXML;
import Wize.Configurations.LPRConfiguration;

public class HIKVision implements ILPR {
	private final String LPR_USERNAME = "admin";
	private final String LPR_PASSWORD = "Shkila6274411";
	protected ServerSocket m_socket = null;
	protected LPRConfiguration _config = null;
	private Boolean m_isRunning = false;
	private LPRListener OnReceive = null;
	protected DataInputStream m_input = null;
	private ByteArrayOutputStream m_data = null;
	private boolean m_isDataRecieved = false;

	public HIKVision(LPRConfiguration config) {
		_config = config;
		OnReceive = new LPRListener() {

			@Override
			public void Receive(ByteArrayOutputStream data) {
				m_data = data;
				m_isDataRecieved = true;
			}
		};
	}

	public boolean Connect() {
		try {
			if (_config == null)
				throw new Exception("No Arguments Was Initialized");

			Disconnect();
			Validate();

			if (isLprCameraOk(_config.Port(), _config.IP())) {
				m_socket = new ServerSocket(_config.Port());
			}

			return true;
		} catch (final Exception ex) {
			ex.printStackTrace();
			return false;
		}
	}

	private boolean isLprCameraOk(final int myPort, final String myLprCameraIp) {
		Boolean result = false;
		final String myFullPath = "http://" + myLprCameraIp + "/ISAPI/Event/notification/httpServers/1";

		try {
			final URL obj = new URL(myFullPath);
			final HttpURLConnection con = (HttpURLConnection) obj.openConnection();
			con.setRequestMethod("PUT");

			final String enccryptedUserPassword = Base64.getEncoder()
					.encodeToString((LPR_USERNAME + ":" + LPR_PASSWORD).getBytes());
			con.setRequestProperty("Authorization", "Basic " + enccryptedUserPassword); // YWRtaW46U2hraWxhNjI3NDQxMQ==

			con.setDoOutput(true);

			final DataOutputStream wr = new DataOutputStream(con.getOutputStream());
			final String bytesToWrite = getBytesToWrite(myPort);
			wr.writeBytes(bytesToWrite);
			wr.flush();
			wr.close();

			final int responseCode = con.getResponseCode();
			if (responseCode == 200)
				result = true;
		} catch (final Exception ex) {
			System.out.println(ex.getMessage());
		}

		return result;
	}

	private String getBytesToWrite(final int myPort) {
		String response;
		final String myIp = getCurrentIp();
		// if (myIp == "") {
		// throw new NullPointerException("Error - Cant get current machine IP");
		// }

		try {
			UtilsXML.makeDoc();
		} catch (final ParserConfigurationException e) {
			e.printStackTrace();
		}

		final Element httpServerList = UtilsXML.addRoot("HttpServerList");

		final Element httpServer = UtilsXML.addElement("HttpServer", httpServerList);

		UtilsXML.addNode(httpServer, "id", "1");
		UtilsXML.addNode(httpServer, "url", "test");
		UtilsXML.addNode(httpServer, "enabled", "true");
		UtilsXML.addNode(httpServer, "protocolType", "HTTP");
		UtilsXML.addNode(httpServer, "addressingFormatType", "ipaddress");
		UtilsXML.addNode(httpServer, "ipAddress", myIp);
		UtilsXML.addNode(httpServer, "portNo", String.valueOf(myPort));
		UtilsXML.addNode(httpServer, "userName", "");
		UtilsXML.addNode(httpServer, "httpAuthenticationMethod", "none");
		UtilsXML.addNode(httpServer, "uploadPicture", "true");
		UtilsXML.addNode(httpServer, "pictureType", "big");

		response = UtilsXML.getXML();

		return response;
	}

	public String getCurrentIp() {
		try {
			Enumeration<NetworkInterface> networkInterfaces = NetworkInterface.getNetworkInterfaces();
			while (networkInterfaces.hasMoreElements()) {
				NetworkInterface ni = (NetworkInterface) networkInterfaces.nextElement();
				Enumeration<InetAddress> nias = ni.getInetAddresses();
				while (nias.hasMoreElements()) {
					InetAddress ia = (InetAddress) nias.nextElement();
					if (!ia.isLinkLocalAddress() && !ia.isLoopbackAddress() && ia instanceof InetAddress) {
						System.out.println(ia);
						return ia.toString().replace("/", "");
					}
				}
			}
		} catch (SocketException e) {
			System.out.println("unable to get current IP " + e.getMessage());
		}
		return null;
	}

	public void Validate() {
		try {
			if (_config.IP() == null || _config.IP().trim().length() == 0) {
				throw new Exception("Missing IP");
			} else {
				if (!IsIPValid(_config.IP())) {
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

	public void Process() {
		RecieveProcess();
	}

	protected void RecieveProcess() {
		final Thread r = new Thread(() -> {
			while (m_isRunning) {
				try {
					final byte[] buffer = new byte[10000000];
					final Socket socket = m_socket.accept();
					Thread.sleep(160);
					m_input = new DataInputStream(socket.getInputStream());
					final ByteArrayOutputStream outputStream = new ByteArrayOutputStream();
					while (m_input.available() > 0) {
						Thread.sleep(160);
						final int len = m_input.read(buffer);
						outputStream.write(buffer, 0, len);
					}
					Receive(outputStream);
					outputStream.close();
					m_input.close();
					socket.close();
				} catch (final Exception e) {
					e.printStackTrace();
				}
			}
		});
		r.start();
	}

	private boolean IsIPValid(final String ipString) {
		try {
			if (ipString == null || ipString.contains(" ")) {
				return false;
			}

			final String[] splitValues = ipString.split("\\.");
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
		} catch (final Exception ex) {
			ex.printStackTrace();
			return false;
		}
	}

	public void Disconnect() {
		try {
			if (m_socket != null) {
				m_socket.close();
			}
			m_socket = null;
		} catch (final Exception ex) {
			ex.printStackTrace();
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

	protected void Receive(final ByteArrayOutputStream data) {
		if (OnReceive != null)
			OnReceive.Receive(data);
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
	public ByteArrayOutputStream Data() {
		return m_data;
	}

	@Override
	public boolean IsDataRecieved() {
		return m_isDataRecieved;
	}

	@Override
	public void ClearData() {
		m_data = null;
		m_isDataRecieved = false;
	}

}
