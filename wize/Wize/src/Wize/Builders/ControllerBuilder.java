package Wize.Builders;

import Siemens.Controller;
import Wize.Modules.ControllerModule;
import Siemens.Siemens;
import Wize.Configurations.ModuleConfiguration;
import Wize.Modules.Module;

public class ControllerBuilder extends ModuleBuilder {

    public ControllerBuilder(ModuleConfiguration config) {
        _config = config;
        _module = new ControllerModule(_config);
    }

    public Controller BuildController() {
        return new Controller(new Siemens(_config.ControllerConfig()));
    }

    @Override
    public Module GetModule() {
        return _module;
    }

}