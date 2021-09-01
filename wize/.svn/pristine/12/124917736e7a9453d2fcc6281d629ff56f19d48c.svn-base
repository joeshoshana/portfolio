package Wize;

public class WizeFactory {

    public static Engine Factory(WizeModules type) {
        switch (type) {
            case OrAkiva:
                return new OrAkivaBridge();
            case Weight:
                return new Weight();
            case Tag:
                return new Tag1();
            case Silo:
            case Controller:
            case IO:
            case LPR:
                return new Engine();
            default:
                return null;
        }
    }
}