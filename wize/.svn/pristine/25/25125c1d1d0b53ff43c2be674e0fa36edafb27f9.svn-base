package Wize;

import java.awt.EventQueue;

import javax.swing.JFrame;
import javax.swing.JPanel;
import javax.swing.JTextField;
import javax.swing.border.EmptyBorder;
import javax.swing.JLabel;
import javax.swing.SwingConstants;
import javax.swing.Timer;

import java.awt.Font;
import java.awt.GridBagConstraints;
import java.awt.GridBagLayout;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.Color;

public class WeightDisplay extends JFrame {
	static final Color mainColor = new Color(0, 62, 143);
	static final Color warningColor = new Color(204, 35, 35);
	static final Color okColor = new Color(0, 173, 14);
	static final long serialVersionUID = 42L;
	private JPanel contentPane;
	public JLabel lblWeight = new JLabel("- - -");
	public JLabel lblError = new JLabel("");
	public JLabel lblOK = new JLabel("");
	public JLabel lblMessage = new JLabel("222");
	public JLabel lblData = new JLabel("111");
	static JTextField txtFieldUserInput = new JTextField("", 10);

	final String FONT_FAMILY = "Impact";

	javax.swing.Timer timerUserInput = new javax.swing.Timer(5000, new ActionListener() {
		public void actionPerformed(ActionEvent e) {
			txtFieldUserInput.setText("");
		}
	});

	public static void main(String[] args) {
		EventQueue.invokeLater(new Runnable() {
			public void run() {
				try {
					WeightDisplay frame = new WeightDisplay();
					frame.setVisible(true);
				} catch (Exception e) {
					e.printStackTrace();
				}
			}
		});
	}

	public void ChangeBackground(Color c) {
		contentPane.setBackground(c);
		Timer t = new Timer(5000, new ActionListener() {

			@Override
			public void actionPerformed(ActionEvent e) {
				contentPane.setBackground(mainColor);
			}
		});
		t.setRepeats(false);
		t.start();
	}

	/**
	 * Create the frame.
	 */
	public WeightDisplay() {
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

		if (Configuration.IsShowUserInput.equals("true")) {
			txtFieldUserInput.setForeground(Color.BLACK);
			txtFieldUserInput.setFont(new Font(FONT_FAMILY, Font.BOLD, 100));
			txtFieldUserInput.setHorizontalAlignment(SwingConstants.CENTER);
			contentPane.add(txtFieldUserInput, gbc4);
		}
	}

	public void setMessage(String msg, boolean activateTimer) {
		lblMessage.setText(msg);
		if (activateTimer) {
			Timer t = new Timer(5000, new ActionListener() {

				@Override
				public void actionPerformed(ActionEvent e) {
					lblMessage.setText(null);
				}
			});
			t.setRepeats(false);
			t.start();
		}
	}

	public void setData(String dt, boolean activateTimer) {
		lblData.setText(dt);
		if (activateTimer) {
			Timer t = new Timer(5000, new ActionListener() {

				@Override
				public void actionPerformed(ActionEvent e) {
					lblData.setText(null);
				}
			});
			t.setRepeats(false);
			t.start();
		}
	}

	public void initiateTxtFieldUserInputListener() {
		txtFieldUserInput.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent e) {
				timerUserInput.stop();
				timerUserInput.start();
			}
		});

		// txtFieldUserInput.getDocument().addDocumentListener(new
		// SimpleDocumentListener() {
		// @Override
		// public void update(DocumentEvent e) {
		// timerUserInput.stop();
		// timerUserInput.start();
		// }
		// });
	}

	public void setWeight(String msg, boolean activateTimer) {
		lblWeight.setText(msg);
		if (activateTimer) {
			Timer t = new Timer(5000, new ActionListener() {
				@Override
				public void actionPerformed(ActionEvent e) {
					lblWeight.setText("- - -");
				}
			});
			t.setRepeats(false);
			t.start();
		}
	}
}