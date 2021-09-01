package Wize.Builders;

import Tags.Tag;
import Tags.TagArgs;
import Tags.TagModules;
import Tags.TagReader;
import Wize.Configurations.ModuleConfiguration;
import Wize.Modules.Module;
import Wize.Modules.TagModule;

public class TagBuilder extends ModuleBuilder {

    public TagBuilder(ModuleConfiguration config) {
        _config = config;
        _module = new TagModule(_config);
    }

    @Override
    public Tag BuildTag() {
        TagArgs args = new TagArgs();
        args.Com = _config.TagConfig().Com();
        args.BaudRate = 9600; //
        args.DataBits = 8;
        args.Dtr = false;
        args.Parity = 0;
        args.Rts = false;
        args.StopBits = 1;
        args.Module = TagModules.valueOf(_config.TagConfig().Type());
        return new Tag(new TagReader(args));
    }

    @Override
    public Module GetModule() {
        return _module;
    }

}