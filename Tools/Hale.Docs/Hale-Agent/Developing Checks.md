## How it works ##

All checks in the Hale-Agent are loaded dynamically during runtime. Because of this, they need to behave a certain way to be usable. The following interface must be realized in all checks:


    public interface ICheck
    {
      string Author { get; }
      string Name { get; }
      string Platform { get; }
      decimal TargetApi { get; }
      Version Version { get; }
      Response Execute(string origin, long warn = 0, long crit = 0);
    }
    
Both the interface and the Response that Execute is expected to return are available in the Hale-Lib, which will be provided by the agent during runtime.
