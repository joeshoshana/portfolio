package Wize.Builders;

import IO.IO;
import LPR.LPR;
import Offline.Offline;
import Scales.Scale;
import Siemens.Controller;
import Tags.Tag;
import Wize.Configurations.ModuleConfiguration;
import Displays.Display;
import Displays.WeightDisplay;
import Wize.Modules.Module;

public class ModuleBuilder implements IModuleBuilder {

    protected Module _module = null;
    protected ModuleConfiguration _config = null;

    public void Build() {
        _module.Controller(BuildController());
        _module.Tag(BuildTag());
        _module.LPR(BuildLPR());
        _module.IO(BuildIO());
        _module.Offline(BuildOffline());
        _module.Scale(BuildScale());
        _module.Display(BuildDisplay());
    }

    public Display BuildDisplay() {
        if (_config.DisplayConfig().IsDisplay())
            return new Display(new WeightDisplay(_config.DisplayConfig()));
        return null;
    }

    public IO BuildIO() {
        // return new IO(new RaspberryIO(_config.IOConfig()))
        return null;
    }

    public Controller BuildController() {
        return null;
    }

    public Scale BuildScale() {
        return null;
    }

    public Tag BuildTag() {
        return null;
    }

    public Offline BuildOffline() {
        return null;
    }

    public LPR BuildLPR() {
        // return new LPR(new HIKVision(_config.LPRConfig()));
        return null;
    }

    public Module GetModule() {
        return null;
    }
}