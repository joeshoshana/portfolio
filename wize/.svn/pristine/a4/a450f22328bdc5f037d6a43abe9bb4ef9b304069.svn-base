package Tags;

import java.io.IOException;
import java.io.InputStream;
import java.util.Timer;
import java.util.TimerTask;

import gnu.io.CommPort;
import gnu.io.CommPortIdentifier;
import gnu.io.SerialPort;

public class TagReader implements ITag {
	private SerialPort m_SerialPort = null;
	private TagArgs m_args = null;
	public TagListener OnTag = null;
	private String tag = "";
	private Timer m_timer = null;
	private Boolean m_isRunning = false;
	private String m_data = "";
	private boolean m_isDataRecieved = false;

	public TagReader(TagArgs args) {
		m_args = args;
		OnTag = new TagListener() {
			@Override
			public void Tag(String data, Boolean toProcess) {
				try {
					if (!toProcess) {
						/*
						 * if (Display()) m_disp.setData(data, false); System.out.println(data);
						 */
					}

					if (data != null && data.length() > 0 && toProcess) {
						System.out.println(data);
						m_data = data;
						m_isDataRecieved = true;
					}
				} catch (Exception ex) {
					ex.printStackTrace();
				}
			}
		};
	}

	@Override
	public boolean Connect() {
		try {
			if (m_args == null)
				throw new Exception("No Arguments Was Initialized");

			Disconnect();

			Validate();

			CommPortIdentifier portIdentifier = CommPortIdentifier.getPortIdentifier(m_args.Com);
			if (portIdentifier.isCurrentlyOwned()) {
				System.out.println("Error: Port is currently in use");
			} else {
				CommPort commPort = portIdentifier.open(this.getClass().getName(), 2000);

				if (commPort instanceof SerialPort) {
					m_SerialPort = (SerialPort) commPort;
					m_SerialPort.setSerialPortParams(m_args.BaudRate, m_args.DataBits, m_args.StopBits, m_args.Parity);
					m_SerialPort.setDTR(m_args.Dtr);
					m_SerialPort.setRTS(m_args.Rts);

				} else {
					System.out.println("Error: Only serial ports are handled by this example.");
				}
			}

			return true;
		} catch (Exception e) {
			e.printStackTrace();
			return false;
		}
	}

	@Override
	public void Validate() {
		try {
			if (m_args.Com == null || m_args.Com.trim().length() == 0) {
				throw new Exception("Missing Com");
			}
			if (m_args.DataBits < 5 || m_args.DataBits > 8) {
				throw new Exception("Missing Data Bits");
			}
			if (m_args.StopBits < 1 || m_args.StopBits > 3) {
				throw new Exception("Missing Stop Bits");
			}
			if (m_args.Parity < 0 || m_args.Parity > 4) {
				throw new Exception("Missing Parity");
			}
		} catch (Exception e) {
			e.printStackTrace();
		}

	}

	@Override
	public void Disconnect() {
		try {
			if (m_SerialPort != null) {
				m_SerialPort.close();
			}

			m_SerialPort = null;
		} catch (Exception e) {
			e.printStackTrace();
		}
	}

	private void Process() {
		try {
			InputStream in = m_SerialPort.getInputStream();
			while (m_isRunning) {

				try {
					byte[] buffer = new byte[1024];
					int len = -1;
					if (in.available() > 0) {
						Thread.sleep(160);
						len = in.read(buffer);
						String data = new String(buffer, 0, len);

						switch (m_args.Module) {
							case Tags: {
								if (data.length() >= 8) {
									tag += (data.replaceAll("(\\r|\\n)", ""));
									System.out.println(tag);
									Tag(tag, true);
								}
								break;
							}
							case Keys: {
								if (data.length() <= 2) {
									if (!data.contains("\n")) {
										if (data.contains("")) {
											if (tag != null && tag.length() > 0)
												tag = tag.substring(0, tag.length() - 1);
										} else
											tag += (data.getBytes()[0]);
										Tag(tag, false);
										if (m_timer != null) {
											System.out.println("Cancel Tag Countdown");
											m_timer.cancel();
											m_timer = null;
										}
										System.out.println("Create Tag Countdown");
										m_timer = new Timer("Tag Countdown");
										System.out.println("Start Tag Countdown");
										m_timer.schedule(new TimerTask() {
											public void run() {
												System.out.println("Erasing Tag: " + tag);
												tag = "";
												Tag(tag, false);
											}
										}, 10000);
										continue;
									} else {
										tag += data.replaceAll("(\\r|\\n)", "");
										System.out.println(tag);
										Tag(tag, true);
									}
								}
								break;
							}
							case Both: {
								if (!data.contains("\n")) {
									tag += (data.getBytes()[0]);
									Tag(tag, false);
									continue;
								} else {
									tag += data.replaceAll("(\\r|\\n)", "");
								}

								System.out.println(tag);
								Tag(tag, true);
								break;
							}
						}

						tag = "";
						Tag(tag, false);
					}
				} catch (IOException e) {
					e.printStackTrace();
				} catch (Exception e) {
					e.printStackTrace();
				}

			}
			in.close();
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

	protected void Tag(String msg, Boolean toProcess) {
		if (OnTag != null)
			OnTag.Tag(msg, toProcess);
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
