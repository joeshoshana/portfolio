package Wize.Modules;

import Siemens.SiemensCommands;
import Wize.Configurations.ModuleConfiguration;

public class ControllerModule extends Module {
    public ControllerModule(ModuleConfiguration config) {
        super(config);
    }

    @Override
    public void Run(boolean isRun) {
        try {
            super.Run(isRun);

            while (_isRunning) {
                if (_controller.IsDataRecieved()) {
                    System.out.println(_controller.Data());
                    switch (_controller.Data()) {
                        case "I001":
                            _controller.Send(SiemensCommands.Q001);
                            break;
                        case "I000":
                            _controller.Send(SiemensCommands.Q000);
                            break;
                        case "I011":
                            _controller.Send(SiemensCommands.Q011);
                            break;
                        case "I010":
                            _controller.Send(SiemensCommands.Q010);
                            break;
                    }
                    _controller.ClearData();
                }
            }
        } catch (Exception ex) {
            ex.printStackTrace();
        }
    }
}