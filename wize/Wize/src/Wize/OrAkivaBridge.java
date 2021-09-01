package Wize;

import java.awt.Color;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;

import javax.swing.Timer;

import Siemens.SiemensCommands;

public class OrAkivaBridge extends Engine {
	private Boolean IsFirstUp = false;
	private Boolean IsSecondUp = false;

	public OrAkivaBridge() {
		super();
	}

	public void Start() {
		try {
			super.Start();

			while (m_isRunning) {
				if (m_controller.IsDataRecieved()) {
					switch (m_controller.Data()) {
						case "I001":
							IsFirstUp = true;
							break;
						case "I000":
							IsFirstUp = false;
							break;
						case "I011":
							IsSecondUp = true;
							break;
						case "I010":
							IsSecondUp = false;
							break;
					}
					m_controller.ClearData();
				}

				// WriteToLog("Info", WeightData);
				if (m_sr.IsDataRecieved())
					super.sendWeight(m_sr.Data());

				if (IsFirstUp || IsSecondUp)
					m_tag.ClearData();
				/*
				 * if(IsFirstUp && !IsSecondUp && IsWeight) {
				 */
				if (m_tag.IsDataRecieved()) {
					super.sendTag(m_tag.Data());
					IsFirstUp = false;
					IsSecondUp = false;
					m_tag.ClearData();
				}
				// }

				// IsWeight = false;
			}
		} catch (Exception ex) {
			ex.printStackTrace();
		}
	}

	public void Acknowledge(String data) {

		super.Acknowledge(data);
		Response r = Response.fromJson(data);
		if (r != null) {
			if (Display() && !r.isSucceded) {
				m_disp.setMessage(r.msg, true);
				m_disp.ChangeBackground(Color.red);
			} else if (m_controller != null && r.isSucceded) {
				m_controller.Send(SiemensCommands.Q031);

				Timer t = new Timer(5000, new ActionListener() {

					@Override
					public void actionPerformed(ActionEvent e) {
						m_controller.Send(SiemensCommands.Q030);
					}
				});
				t.setRepeats(false);
				t.start();
			}
		} else
			System.out.println(data);
	}

}
