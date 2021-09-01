package Wize.Builders;

import Wize.WizeModules;
import Wize.Configurations.ModuleConfiguration;

public class ModuleBuilderFactory {
    public static ModuleBuilder Factory(ModuleConfiguration config) {
        WizeModules module = WizeModules.valueOf(config.Type());
        switch (module) {
            case OrAkiva:
                return new OrAkivaBuilder(config);
            case Binyamina:
                return new BinyaminaBuilder(config);
            case Caesarea:
                return new CaesareaBuilder(config);
            case Weight:
                return new WeightBuilder(config);
            case Tag:
                return new TagBuilder(config);
            case Controller:
                return new ControllerBuilder(config);
            case LPR:
                return new LPRBuilder(config);
            case Silo:
            case IO:

            default:
                return null;
        }
    }
}