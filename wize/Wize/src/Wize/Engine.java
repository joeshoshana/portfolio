package Wize;

import java.awt.Color;
import java.time.LocalDate;
import java.time.LocalTime;
import java.util.ArrayList;
import java.util.Timer;
import java.util.TimerTask;
import IO.IO;
import LPR.LPR;
import LPR.LPRArgs;
import Offline.Offline;
import Offline.OfflineArgs;
import Offline.OfflineHandler;
import ScaleReaders.ConnectionArgs;
import ScaleReaders.ConnectionType;
import ScaleReaders.ScaleHeaders;
import Scales.Scale;
import Scales.Shkila;
import Siemens.Controller;
import Siemens.SiemensArgs;
import Tags.Tag;
import Tags.TagArgs;
import Tags.TagModules;
import Tags.TagReader;
import Utilities.Network;
import Utilities.OS.Mac;
import Utilities.OS.Reboot;

import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;

public class Engine {
	private static Engine m_instance = null;
	private static Object m_lock = new Object();
	protected Controller m_controller = null;
	protected Scale m_sr = null;
	protected Tag m_tag = null;
	protected LPR m_lpr = null;
	protected Offline m_offline = null;
	protected IO m_io = null;

	private static String m_activeDirectory = "file:///" + System.getProperty("user.dir").replace("\\", "/");
	private static String m_configFile = m_activeDirectory + "/Configuration.xml";

	protected Boolean m_isDemo = true;
	protected Boolean m_toggle = false;
	protected String m_mac = null;

	protected Boolean m_isRunning = false;

	protected WeightDisplay m_disp = null;

	protected Engine() {
		m_mac = Mac.Get();
	}

	public static Engine getInstance(WizeModules type) {
		synchronized (m_lock) {
			if (m_instance == null)
				m_instance = WizeFactory.Factory(type);
		}
		return m_instance;
	}

	public static Boolean loadConfig() {

		if (!Configuration.LoadConfiguration(m_configFile)) {
			return false;
		}
		return true;
	}

	public boolean initOffline() {
		try {
			OfflineArgs args = new OfflineArgs();
			if (Configuration.OfflineInterval.length() > 0) {

				args.Interval = Integer.parseInt(Configuration.OfflineInterval);
				args.Task = new TimerTask() {
					public void run() {
						try {
							if (true/* IsPing() */) {
								ArrayList<String> records = m_offline.Load();
								for (int i = 0; i < records.size(); i++) {
									Request req = new Request();
									req.OnResponse = new ResponseListener() {

										@Override
										public void Reponse(String data) {
											try {
												Response r = Response.fromJson(data);
												if (r != null) {
													if (Display() && !r.isSucceded) {
														m_disp.setMessage(r.msg, true);
														m_disp.ChangeBackground(WeightDisplay.warningColor);
														m_offline.SetIsUpload(false);
													} else if (Display() && r.isSucceded) {
														m_offline.SetIsUpload(true);
													}
												} else {
													System.out.println(data);
													m_offline.SetIsUpload(false);
												}
											} catch (Exception ex) {
												if (Display()) {
													m_disp.setMessage(ex.getMessage(), true);
													m_disp.ChangeBackground(WeightDisplay.warningColor);
													;
												}
												System.out.println(ex.getMessage());
												m_offline.SetIsUpload(false);
											}

										}
									};

									req.fromJson(records.get(i));
									req.send(Configuration.Web);
									if (m_offline.IsUpload())
										records.remove(i--);

									m_offline.SetIsUpload(false);
								}

								m_offline.Delete();
								m_offline.Save(records);
							}
						} catch (Exception ex) {
							ex.printStackTrace();
						} finally {
							m_offline.SetIsUpload(false);
						}

					}
				};
			}
			m_offline = new Offline(new OfflineHandler(args));

			return true;
		} catch (Exception ex) {
			ex.printStackTrace();
			return false;
		}
	}

	public boolean initIO() {
		try {
			// m_io = new IO(new RaspberryIO(Configuration.GPIOs));
			return m_io.Connect();
		} catch (Exception ex) {
			ex.printStackTrace();
			return false;
		}
	}

	public boolean initTag() {
		try {
			TagArgs args = new TagArgs();
			args.Com = Configuration.TagCom;
			args.BaudRate = 9600; //
			args.DataBits = 8;
			args.Dtr = false;
			args.Parity = 0;
			args.Rts = false;
			args.StopBits = 1;
			args.Module = TagModules.valueOf(Configuration.TagModule);
			m_tag = new Tag(new TagReader(args));
			return m_tag.Connect();
		} catch (Exception ex) {
			ex.printStackTrace();
			return false;
		}
	}

	public boolean initSiemens() {
		try {
			SiemensArgs args = new SiemensArgs();
			args.IP = Configuration.SiemensIP;

			Integer port = 0;
			try {
				port = Integer.parseInt(Configuration.SiemensPort);
			} catch (NumberFormatException e) {
				throw e;
			}
			args.Port = port;

			// m_controller = new Controller(new Siemens(args));

			return m_controller.Connect();
		} catch (Exception ex) {
			ex.printStackTrace();
			return false;
		}

	}

	public boolean initLPR() {
		try {
			LPRArgs args = new LPRArgs();
			args.IP = Configuration.LPRIP;
			Integer port = 0;
			try {
				port = Integer.parseInt(Configuration.LPRPort);
			} catch (NumberFormatException e) {
				throw e;
			}
			args.Port = port;
			// m_lpr = new LPR(new HIKVision(args));

			return m_lpr.Connect();
		} catch (Exception ex) {
			ex.printStackTrace();
			return false;
		}
	}

	public boolean initScale() {
		try {
			ConnectionArgs connArgs = new ConnectionArgs();
			connArgs.BaudRate = 9600; //
			connArgs.Com = Configuration.Com;
			connArgs.DataBits = 8;
			connArgs.Dtr = false;
			connArgs.Parity = 0;
			connArgs.Rts = false;
			connArgs.StopBits = 1;
			connArgs.Type = Configuration.Com == "" || Configuration.Com.length() == 0 ? ConnectionType.Tcp
					: ConnectionType.Serial;
			m_sr = new Scale(new Shkila(ScaleHeaders.valueOf(Configuration.Scale), connArgs));

			if (m_sr == null)
				return false;
			return m_sr.Connect();
		} catch (Exception ex) {
			ex.printStackTrace();
			return false;
		}
	}

	private void selfWeighingListener() {
		WeightDisplay.txtFieldUserInput.addActionListener(new ActionListener() {
			@Override
			public void actionPerformed(ActionEvent myActionEvent) {
				String companyId = WeightDisplay.txtFieldUserInput.getText();
				sendSelfWeighToServer(companyId);

				// Validate company id and then show company name:
				// String companyName = getCompanyName(companyId);
				// if (companyName.toLowerCase().indexOf("wrong") > -1) {
				// String myMessage = "Wrong tag entered. Please try again.";
				// displayMessageToUser(myMessage, true, WeightDisplay.warningColor, "");
				// } else if (companyName.toLowerCase().indexOf("error") > -1) {
				// displayMessageToUser(companyName, true, WeightDisplay.warningColor, "");
				// } else {
				// displayMessageToUser(companyName, true, WeightDisplay.mainColor, "");
				// sendSelfWeighToServer(companyId);
				// }
			}
		});
	}

	private void sendSelfWeighToServer(String companyId) {
		try {
			Request req = new Request();

			req.OnResponse = new ResponseListener() {
				@Override
				public void Reponse(String data) {
					try {
						Acknowledge(data);
					} catch (Exception ex) {
						if (Display())
							m_disp.lblMessage.setText(ex.getMessage());
						System.out.println(ex.getMessage());
					}
				}
			};

			if (m_isDemo) {
				req.Weight("1234.5");
			} else {
				req.Weight(m_sr.Data());
			}

			req.Command("vehicle_self_weighing");
			req.Mac(m_mac);
			req.WeighingTime(LocalDate.now().toString() + " " + LocalTime.now().toString());
			req.Tag(companyId);

			if (m_lpr.Data() != null) {
				req.Pic(m_lpr.Data().toByteArray());
			}

			Validation myValidation = new Validation();
			myValidation = req.getStatus();
			if (myValidation.IsValid()) {
				req.send(Configuration.Web);
			} else {
				String myMessage = myValidation.Message();
				displayMessageToUser(myMessage, true, WeightDisplay.warningColor, "");
			}
		} catch (Exception ex) {
			ex.printStackTrace();
		}
	}

	protected void sendTag(String data) {
		try {
			Request req = new Request();
			req.OnResponse = new ResponseListener() {

				@Override
				public void Reponse(String data) {
					try {
						Acknowledge(data);
					} catch (Exception ex) {
						if (Display())
							m_disp.lblMessage.setText(ex.getMessage());
						System.out.println(ex.getMessage());
					}
				}
			};
			req.Command("vehicle_weight_modify");
			if (m_isDemo) {
				m_toggle = !m_toggle;
				if (m_toggle) {
					req.Mac("1");
				} else {
					req.Mac("demo");
				}
			} else
				req.Mac(m_mac);

			if (req.Mac() == null) {
				System.out.println("No mac address");
				return;
			}

			if (m_isDemo) {
				req.Tag("000001");
			} else
				req.Tag(data);

			if (!Network.IsPing()) {
				if (m_sr.Data().length() > 0) {
					req.Weight(m_sr.Data());
				} else {
					req.Weight(null);
				}
				m_offline.Save(req.toJson());
			} else
				req.send(Configuration.Web);
		} catch (Exception ex) {
			ex.printStackTrace();
		}
	}

	protected void sendWeight(String data) {
		try {
			Request req = new Request();

			req.OnResponse = new ResponseListener() {

				@Override
				public void Reponse(String data) {
					try {
						// Acknowledge(data);

					} catch (Exception ex) {
						if (Display()) {
							m_disp.setMessage(ex.getMessage(), true);
							m_disp.ChangeBackground(WeightDisplay.warningColor);
							;
						}
						System.out.println(ex.getMessage());
					}
				}
			};

			req.Command("update_weight");
			if (m_isDemo) {
				m_toggle = !m_toggle;
				if (m_toggle) {
					req.Mac("1");
				} else {
					req.Mac("demo");
				}
			} else
				req.Mac(m_mac);

			if (req.Mac() == null) {
				System.out.println("No mac address");
				return;
			}
			if (m_isDemo) {
				if (m_toggle) {
					req.Weight("15.32");
				} else {
					req.Weight("2258");
				}
			} else
				req.Weight(data);

			if (Display())
				m_disp.lblWeight.setText(data);

			req.send(Configuration.Web);
		} catch (Exception ex) {
			ex.printStackTrace();
		}
	}

	private void goodbyeMessageToUser() {
		javax.swing.Timer timerUserInput = new javax.swing.Timer(2000, new ActionListener() {
			public void actionPerformed(ActionEvent e) {
				displayMessageToUser("Have a great day! :-)", true, WeightDisplay.okColor, "");
			}
		});
		timerUserInput.start();
		timerUserInput.setRepeats(false);
	}

	private void displayMessageToUser(String message, Boolean isSetTimer, Color color, String txtFieldValue) {
		m_disp.setMessage(message, isSetTimer);
		m_disp.ChangeBackground(color);
		WeightDisplay.txtFieldUserInput.setText(txtFieldValue);
	}

	public void Start() {
		m_isRunning = true;

		MakeResetTimer();

		initOffline();
		// m_offline.Run();

		WizeModules type = WizeModules.valueOf(Configuration.Module);
		if (type == WizeModules.OrAkiva || type == WizeModules.Weight) {
			Thread sr = new Thread(() -> {
				if (Configuration.Scale.length() > 0) {
					initScale();
					m_sr.Run(m_isRunning);
				}
			});
			sr.start();
		}

		if (type == WizeModules.OrAkiva || type == WizeModules.Tag) {
			Thread tr = new Thread(() -> {
				initTag();
				m_tag.Run(m_isRunning);
			});
			tr.start();
		}

		if (type == WizeModules.OrAkiva || type == WizeModules.Controller) {
			Thread s = new Thread(() -> {
				initSiemens();
				m_controller.Run(m_isRunning);
			});
			s.start();
		}

		if (type == WizeModules.IO) {
			Thread s = new Thread(() -> {
				initIO();
				m_io.Run(m_isRunning);
			});
			s.start();
		}

		if (type == WizeModules.LPR) {
			Thread s = new Thread(() -> {
				initLPR();
				m_lpr.Run(m_isRunning);
			});
			s.start();
		}

		if (Display()) {
			m_disp = new WeightDisplay();
			m_disp.setVisible(true);

			if (Configuration.IsShowUserInput.equals("true")) {
				m_disp.initiateTxtFieldUserInputListener();

				selfWeighingListener();
			}
		}
	}

	public void MakeResetTimer() {
		int resetTime = 0;
		if (Configuration.ResetTime.length() > 0) {
			resetTime = Integer.parseInt(Configuration.ResetTime);
			TimerTask repeatedTask = new TimerTask() {
				public void run() {
					if (!Network.IsPing()) {
						Reboot.Start();
					}

				}
			};
			Timer timer = new Timer("Timer");

			long delay = 30000L;
			long period = resetTime * 60 * 1000;
			timer.scheduleAtFixedRate(repeatedTask, delay, period);
		}
	}

	public void Stop() {
		m_isRunning = false;

		WizeModules type = WizeModules.valueOf(Configuration.Module);
		if (type == WizeModules.OrAkiva || type == WizeModules.Weight) {
			Thread sr = new Thread(() -> {
				if (Configuration.Scale.length() > 0) {
					m_sr.Run(m_isRunning);
					m_sr.Disconnect();
					m_sr = null;
				}
			});
			sr.start();
		}

		if (type == WizeModules.OrAkiva || type == WizeModules.Tag) {
			Thread tr = new Thread(() -> {
				m_tag.Run(m_isRunning);
				m_tag.Disconnect();
				m_tag = null;
			});
			tr.start();
		}

		if (type == WizeModules.OrAkiva || type == WizeModules.Controller) {
			Thread s = new Thread(() -> {
				m_controller.Run(m_isRunning);
				m_controller.Disconnect();
				m_controller = null;
			});
			s.start();
		}

		if (type == WizeModules.IO) {
			Thread s = new Thread(() -> {
				m_io.Run(m_isRunning);
				m_io.Disconnect();
				m_io = null;
			});
			s.start();
		}

		if (Display()) {
			m_disp.setVisible(false);
			m_disp.dispose();
		}
	}

	public boolean Display() {
		if (Configuration.Display.length() > 0) {
			boolean val = Boolean.parseBoolean(Configuration.Display);
			return val;
		}
		return false;
	}

	public void Acknowledge(String data) {
		Response r = Response.fromJson(data);
		if (r != null) {
			if (Display() && !r.isSucceded) {
				m_disp.setMessage(r.msg, true);
				m_disp.ChangeBackground(WeightDisplay.warningColor);
			} else if (Display() && r.isSucceded) {
				m_disp.setWeight(m_sr.Data() + " kg", true);
				goodbyeMessageToUser();
				m_lpr.ClearData();
			}
		} else
			System.out.println(data);
	}
}
