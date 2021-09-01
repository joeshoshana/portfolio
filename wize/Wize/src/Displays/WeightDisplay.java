package Displays;

import javax.swing.JFrame;
import javax.swing.JPanel;
import javax.swing.JTextField;
import javax.swing.border.EmptyBorder;

import Wize.Configurations.DisplayConfiguration;

import javax.swing.JLabel;
import javax.swing.SwingConstants;
import javax.swing.Timer;

import java.awt.Font;
import java.awt.GridBagConstraints;
import java.awt.GridBagLayout;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.event.KeyEvent;
import java.awt.event.KeyListener;
import java.awt.Color;

public class WeightDisplay extends JFrame implements IDisplay {
	private static final Color mainColor = new Color(0, 62, 143);
	private static final long serialVersionUID = 42L;
	private JPanel contentPane;
	private JLabel lblWeight = new JLabel("- - -");
	private JLabel lblMessage = new JLabel("222");
	private JLabel lblData = new JLabel("111");
	private static JTextField txtFieldUserInput = new JTextField("", 10);
	private String FONT_FAMILY = "Impact";

	private DisplayConfiguration _config = null;
	private String _data = null;
	private boolean _isDataRecieved = false;

	javax.swing.Timer timerUserInput = new javax.swing.Timer(5000, new ActionListener() {
		public void actionPerformed(ActionEvent e) {
			txtFieldUserInput.setText("");
			ClearData();
		}
	});

	/*
	 * public static void main(String[] args) { EventQueue.invokeLater(new
	 * Runnable() { public void run() { try { WeightDisplay frame = new
	 * WeightDisplay(); frame.setVisible(true); } catch (Exception e) {
	 * e.printStackTrace(); } } }); }
	 */

	public void Background(Color c, int timeoutMS) {
		contentPane.setBackground(c);
		if (timeoutMS >= 0) {
			Timer t = new Timer(timeoutMS, new ActionListener() {

				@Override
				public void actionPerformed(ActionEvent e) {
					contentPane.setBackground(mainColor);
				}
			});
			t.setRepeats(false);
			t.start();
		}
	}

	/**
	 * Create the frame.
	 */
	public WeightDisplay(DisplayConfiguration config) {
		ArrangeLayout();

		_config = config;

		if (_config.IsShowUserInput()) {
			txtFieldUserInput.setVisible(true);
		}
	}

	@Override
	public void Message(String msg, int timeoutMS) {
		lblMessage.setText(msg);
		if (timeoutMS >= 0) {
			Timer t = new Timer(timeoutMS, new ActionListener() {

				@Override
				public void actionPerformed(ActionEvent e) {
					lblMessage.setText(null);
				}
			});
			t.setRepeats(false);
			t.start();
		}
	}

	@Override
	public void Data(String dt, int timeoutMS) {
		lblData.setText(dt);
		if (timeoutMS >= 0) {
			Timer t = new Timer(timeoutMS, new ActionListener() {

				@Override
				public void actionPerformed(ActionEvent e) {
					lblData.setText(null);
				}
			});
			t.setRepeats(false);
			t.start();
		}
	}

	@Override
	public void Weight(String weight, int timeoutMS) {
		lblWeight.setText(weight);
		/*
		 * if (timeoutMS >= 0) { Timer t = new Timer(timeoutMS, new ActionListener() {
		 * 
		 * @Override public void actionPerformed(ActionEvent e) {
		 * lblWeight.setText("- - -"); } }); t.setRepeats(false); t.start(); }
		 */
	}

	@Override
	public void SetVisible(boolean isVisible) {
		this.setVisible(isVisible);
	}

	public String Data() {
		return _data;
	}

	public boolean IsDataRecieved() {
		return _isDataRecieved;
	}

	public void ClearData() {
		_data = null;
		_isDataRecieved = false;
	}

	private void ArrangeLayout() {
		setBackground(mainColor);
		setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		setExtendedState(getExtendedState() | JFrame.MAXIMIZED_BOTH);
		setBounds(100, 100, 968, 595);
		contentPane = new JPanel();
		contentPane.setBackground(mainColor);
		contentPane.setBorder(new EmptyBorder(5, 5, 5, 5));
		setContentPane(contentPane);
		contentPane.setLayout(new GridBagLayout());

		GridBagConstraints gbc1 = new GridBagConstraints();
		gbc1.fill = GridBagConstraints.HORIZONTAL;
		gbc1.weighty = 1.0;
		gbc1.gridx = 1;
		gbc1.gridy = 0;

		GridBagConstraints gbc2 = new GridBagConstraints();
		gbc2.fill = GridBagConstraints.HORIZONTAL;
		gbc2.weighty = 0.1;
		gbc2.gridx = 1;
		gbc2.gridy = 1;

		GridBagConstraints gbc3 = new GridBagConstraints();
		gbc3.fill = GridBagConstraints.HORIZONTAL;
		gbc3.weighty = 0.2;
		gbc3.gridx = 1;
		gbc3.gridy = 2;

		GridBagConstraints gbc4 = new GridBagConstraints();
		gbc4.fill = GridBagConstraints.HORIZONTAL;
		gbc4.weighty = 1;
		gbc4.gridx = 1;
		gbc4.gridy = 3;

		lblWeight = new JLabel(" - - - ");
		// lblWeight.setBounds(10, 125, 2541, 253);
		lblWeight.setForeground(new Color(255, 255, 255));
		lblWeight.setFont(new Font(FONT_FAMILY, Font.BOLD, 80));
		lblWeight.setHorizontalAlignment(SwingConstants.CENTER);
		contentPane.add(lblWeight, gbc1);

		lblData = new JLabel("");
		// lblData.setBounds(10, 613, 2541, 123);
		lblData.setForeground(Color.WHITE);
		lblData.setFont(new Font(FONT_FAMILY, Font.BOLD, 48));
		lblData.setHorizontalAlignment(SwingConstants.CENTER);
		contentPane.add(lblData, gbc2);

		lblMessage = new JLabel("");
		// lblMessage.setBounds(10, 720, 1894, 156);
		lblMessage.setFont(new Font(FONT_FAMILY, Font.PLAIN, 48));
		lblMessage.setForeground(new Color(255, 255, 255));
		lblMessage.setHorizontalAlignment(SwingConstants.CENTER);
		contentPane.add(lblMessage, gbc3);

		txtFieldUserInput.setForeground(Color.BLACK);
		txtFieldUserInput.setFont(new Font(FONT_FAMILY, Font.BOLD, 100));
		txtFieldUserInput.setHorizontalAlignment(SwingConstants.CENTER);
		contentPane.add(txtFieldUserInput, gbc4);

		txtFieldUserInput.addKeyListener(new KeyListener() {

			@Override
			public void keyTyped(KeyEvent e) {
				if (e.getKeyChar() == '\n') {
					if (txtFieldUserInput.getText().length() > 0) {
						_data = txtFieldUserInput.getText();
						_isDataRecieved = true;
					}
				}
				timerUserInput.restart();
			}

			@Override
			public void keyPressed(KeyEvent e) {

			}

			@Override
			public void keyReleased(KeyEvent e) {

			}

		});
		/*
		 * txtFieldUserInput.addActionListener(new ActionListener() { public void
		 * actionPerformed(ActionEvent e) { if (txtFieldUserInput.getText().length() >
		 * 0) { _data = txtFieldUserInput.getText(); _isDataRecieved = true; }
		 * timerUserInput.restart(); } });
		 */
		txtFieldUserInput.setVisible(false);

	}

}