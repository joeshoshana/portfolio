package Wize;

public class Tag1 extends Engine {
	public Tag1() {
		super();
	}

	public void Start() {
		try {
			super.Start();

			while (m_isRunning) {
				if (m_tag.IsDataRecieved() && m_tag.Data().length() > 0) {
					super.sendTag(m_tag.Data());
					m_tag.ClearData();
				}
			}
		} catch (Exception ex) {
			ex.printStackTrace();
		}

	}
}
