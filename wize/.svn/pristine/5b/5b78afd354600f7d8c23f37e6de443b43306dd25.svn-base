package Wize;

import java.awt.Color;

public class Weight extends Engine {
	public Weight() {
		super();
	}

	public void Start() {
		try {
			super.Start();

			while (m_isRunning) {
				if (m_sr.IsDataRecieved())
					super.sendWeight(m_sr.Data());
			}
		} catch (Exception ex) {
			ex.printStackTrace();
		}
	}

	public void Acknowledge(String data) {
		Response r = Response.fromJson(data);
		if (r != null) {
			if (Display() && !r.isSucceded) {
				m_disp.setMessage(r.msg, true);
				m_disp.ChangeBackground(Color.red);
			} else if (Display() && r.isSucceded) {
				m_disp.setMessage("", true);
			}
		} else
			System.out.println(data);
	}

}
