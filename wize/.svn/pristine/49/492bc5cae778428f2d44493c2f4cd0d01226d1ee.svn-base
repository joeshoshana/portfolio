package IO;

import java.io.IOException;
import java.util.ArrayList;

import com.pi4j.io.gpio.GpioController;
import com.pi4j.io.gpio.GpioFactory;
import com.pi4j.io.gpio.GpioPinDigitalInput;
import com.pi4j.io.gpio.GpioPinDigitalOutput;
import com.pi4j.io.gpio.Pin;
import com.pi4j.io.gpio.PinPullResistance;
import com.pi4j.io.gpio.PinState;
import com.pi4j.io.gpio.RaspiPin;
import com.pi4j.io.gpio.event.GpioPinDigitalStateChangeEvent;
import com.pi4j.io.gpio.event.GpioPinListenerDigital;

import Wize.Configurations.IOConfiguration;

public class RaspberryIO implements IIO {
	private IOConfiguration _config = null;
	private Boolean m_isRunning = false;
	private GpioController controller;
	private ArrayList<GpioPinDigitalOutput> outPins = null;
	private ArrayList<GpioPinDigitalInput> inPins = null;
	public IOListener onIO = null;
	private GPIO m_data = null;
	private boolean m_isDataRecieved = false;

	public RaspberryIO(IOConfiguration config) {
		_config = config;
		onIO = new IOListener() {

			@Override
			public void OnIO(GPIO io) {
				try {
					if (io != null) {
						m_data = io;
						m_isDataRecieved = true;
					}
				} catch (Exception ex) {
					ex.printStackTrace();
				}

			}
		};

	}

	public void ChangePinState(IOs io, State state) {
		GpioPinDigitalInput inPin = inPins.stream().filter(item -> item.getPin() == IO2Pin(io)).findAny().orElse(null);

		GpioPinDigitalOutput outPin = outPins.stream().filter(item -> item.getPin() == IO2Pin(io)).findAny()
				.orElse(null);

		if (inPin == null && outPin == null) {
			System.out.println("IO not Found");
			return;
		}

		if (outPin != null) {
			if (state == State.Low)
				outPin.low();
			else if (state == State.Low)
				outPin.high();
		}

		if (inPin != null) {
			if (state == State.Low)
				inPin.setPullResistance(PinPullResistance.PULL_DOWN);
			else if (state == State.Low)
				inPin.setPullResistance(PinPullResistance.PULL_UP);
		}
	}

	public boolean Connect() {
		try {
			if (_config.IO() == null)
				throw new Exception("No Gpios Were Initialized");

			if (controller != null)
				Disconnect();

			controller = GpioFactory.getInstance();

			for (int i = 0; i < _config.IO().size(); i++) {
				if (_config.IO().get(i).Transput == Transput.Input) {
					GpioPinDigitalInput pin = controller.provisionDigitalInputPin(IO2Pin(_config.IO().get(i).IO),
							State2PinPullResistance(_config.IO().get(i).State));
					pin.setShutdownOptions(true);
					inPins.add(pin);
				} else if (_config.IO().get(i).Transput == Transput.Output) {
					GpioPinDigitalOutput pin = controller.provisionDigitalOutputPin(IO2Pin(_config.IO().get(i).IO),
							State2PinState(_config.IO().get(i).State));
					pin.setShutdownOptions(true);
					outPins.add(pin);
				}
			}

			return true;
		} catch (Exception ex) {
			ex.printStackTrace();
			return false;
		}
	}

	public void Disconnect() {
		try {
			for (int i = 0; i < inPins.size(); i++) {
				inPins.get(i).removeAllListeners();
				inPins.get(i).setShutdownOptions(true, PinState.LOW, PinPullResistance.OFF);
			}

			for (int i = 0; i < outPins.size(); i++) {
				outPins.get(i).removeAllListeners();
				outPins.get(i).setShutdownOptions(true, PinState.LOW, PinPullResistance.OFF);
			}

			if (controller != null && !controller.isShutdown())
				controller.shutdown();

			controller = null;

		} catch (Exception ex) {
			ex.printStackTrace();
		}
	}

	private PinPullResistance State2PinPullResistance(State state) {
		switch (state) {
			case High:
				return PinPullResistance.PULL_UP;
			case Low:
				return PinPullResistance.PULL_DOWN;
			default:
				return null;
		}
	}

	private State PinPullResistance2State(PinPullResistance state) {
		switch (state) {
			case PULL_UP:
				return State.High;
			case PULL_DOWN:
				return State.Low;
			default:
				return null;
		}
	}

	private State PinState2State(PinState state) {
		switch (state) {
			case HIGH:
				return State.High;
			case LOW:
				return State.Low;
			default:
				return null;
		}
	}

	private PinState State2PinState(State state) {
		switch (state) {
			case High:
				return PinState.HIGH;
			case Low:
				return PinState.LOW;
			default:
				return null;
		}
	}

	private IOs Pin2IO(Pin pin) {
		switch (pin.getName()) {
			case "GPIO_00":
				return IOs.IO_00;
			case "GPIO_01":
				return IOs.IO_01;
			case "GPIO_02":
				return IOs.IO_02;
			case "GPIO_03":
				return IOs.IO_03;
			case "GPIO_04":
				return IOs.IO_04;
			case "GPIO_05":
				return IOs.IO_05;
			case "GPIO_06":
				return IOs.IO_06;
			case "GPIO_07":
				return IOs.IO_07;
			case "GPIO_08":
				return IOs.IO_08;
			case "GPIO_09":
				return IOs.IO_09;
			case "GPIO_10":
				return IOs.IO_10;
			case "GPIO_11":
				return IOs.IO_11;
			case "GPIO_12":
				return IOs.IO_12;
			case "GPIO_13":
				return IOs.IO_13;
			case "GPIO_14":
				return IOs.IO_14;
			case "GPIO_15":
				return IOs.IO_15;
			case "GPIO_16":
				return IOs.IO_16;
			case "GPIO_17":
				return IOs.IO_17;
			case "GPIO_18":
				return IOs.IO_18;
			case "GPIO_19":
				return IOs.IO_19;
			case "GPIO_20":
				return IOs.IO_20;
			case "GPIO_21":
				return IOs.IO_21;
			case "GPIO_22":
				return IOs.IO_22;
			case "GPIO_23":
				return IOs.IO_23;
			case "GPIO_24":
				return IOs.IO_24;
			case "GPIO_25":
				return IOs.IO_25;
			case "GPIO_26":
				return IOs.IO_26;
			default:
				return IOs.None;
		}
	}

	private Pin IO2Pin(IOs io) {
		switch (io) {
			case IO_00:
				return RaspiPin.GPIO_00;
			case IO_01:
				return RaspiPin.GPIO_01;
			case IO_02:
				return RaspiPin.GPIO_02;
			case IO_03:
				return RaspiPin.GPIO_03;
			case IO_04:
				return RaspiPin.GPIO_04;
			case IO_05:
				return RaspiPin.GPIO_05;
			case IO_06:
				return RaspiPin.GPIO_06;
			case IO_07:
				return RaspiPin.GPIO_07;
			case IO_08:
				return RaspiPin.GPIO_08;
			case IO_09:
				return RaspiPin.GPIO_09;
			case IO_10:
				return RaspiPin.GPIO_10;
			case IO_11:
				return RaspiPin.GPIO_11;
			case IO_12:
				return RaspiPin.GPIO_12;
			case IO_13:
				return RaspiPin.GPIO_13;
			case IO_14:
				return RaspiPin.GPIO_14;
			case IO_15:
				return RaspiPin.GPIO_15;
			case IO_16:
				return RaspiPin.GPIO_16;
			case IO_17:
				return RaspiPin.GPIO_17;
			case IO_18:
				return RaspiPin.GPIO_18;
			case IO_19:
				return RaspiPin.GPIO_19;
			case IO_20:
				return RaspiPin.GPIO_20;
			case IO_21:
				return RaspiPin.GPIO_21;
			case IO_22:
				return RaspiPin.GPIO_22;
			case IO_23:
				return RaspiPin.GPIO_23;
			case IO_24:
				return RaspiPin.GPIO_24;
			case IO_25:
				return RaspiPin.GPIO_25;
			case IO_26:
				return RaspiPin.GPIO_26;
			default:
				return null;
		}
	}

	public void Process() {
		for (int i = 0; i < inPins.size(); i++)
			inPins.get(i).addListener(new GpioPinListenerDigital() {

				@Override
				public void handleGpioPinDigitalStateChangeEvent(GpioPinDigitalStateChangeEvent event) {
					System.out.println(" --> GPIO PIN STATE CHANGE: " + event.getPin() + " = " + event.getState());
					GPIO gpio = new GPIO();
					gpio.IO = Pin2IO(event.getPin().getPin());
					gpio.State = PinState2State(event.getState());
					gpio.Transput = Transput.Input;
					IO(gpio);

				}

			});

		for (int i = 0; i < outPins.size(); i++)
			outPins.get(i).addListener(new GpioPinListenerDigital() {

				@Override
				public void handleGpioPinDigitalStateChangeEvent(GpioPinDigitalStateChangeEvent event) {
					System.out.println(" --> GPIO PIN STATE CHANGE: " + event.getPin() + " = " + event.getState());
					GPIO gpio = new GPIO();
					gpio.IO = Pin2IO(event.getPin().getPin());
					gpio.State = PinState2State(event.getState());
					gpio.Transput = Transput.Output;
					IO(gpio);
				}

			});
	}

	private void IO(GPIO io) {
		if (onIO != null)
			onIO.OnIO(io);
	}

	public void Run(boolean isRun) {
		m_isRunning = isRun;
		if (m_isRunning)
			run();
		else
			Disconnect();
	}

	@Override
	public void run() {
		Process();

	}

	@Override
	public void close() throws IOException {
		Disconnect();

	}

	@Override
	public GPIO Data() {
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
