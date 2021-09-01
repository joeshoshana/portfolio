package Wize.Modules;

import Wize.Configurations.ModuleConfiguration;

public class LPRModule extends Module {
    public LPRModule(ModuleConfiguration config) {
        super(config);
    }

    @Override
    public void Run(boolean isRun) {
        try {
            super.Run(isRun);

            while (_isRunning) {
                if (_lpr.IsDataRecieved()) {
                    System.out.println(_lpr.Data().toString());
                    _lpr.ClearData();
                }
            }
        } catch (Exception ex) {
            ex.printStackTrace();
        }
    }
}