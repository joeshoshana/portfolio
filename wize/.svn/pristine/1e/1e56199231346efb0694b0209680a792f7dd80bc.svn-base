package Wize.Builders;

import LPR.HIKVision;
import LPR.LPR;
import Wize.Modules.LPRModule;
import Wize.Configurations.ModuleConfiguration;
import Wize.Modules.Module;

public class LPRBuilder extends ModuleBuilder {

    public LPRBuilder(ModuleConfiguration config) {
        _config = config;
        _module = new LPRModule(_config);
    }

    public LPR BuildLPR() {
        return new LPR(new HIKVision(_config.LPRConfig()));
    }

    @Override
    public Module GetModule() {
        return _module;
    }

}