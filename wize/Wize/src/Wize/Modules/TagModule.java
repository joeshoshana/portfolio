package Wize.Modules;

import Wize.Configurations.ModuleConfiguration;

public class TagModule extends Module {
    public TagModule(ModuleConfiguration config) {
        super(config);
    }

    @Override
    public void Run(boolean isRun) {
        try {
            super.Run(isRun);

            while (_isRunning) {
                if (_tag.IsDataRecieved()) {
                    System.out.println(_tag.Data());
                }
            }
        } catch (Exception ex) {
            ex.printStackTrace();
        }
    }
}